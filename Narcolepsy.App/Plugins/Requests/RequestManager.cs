#nullable enable
namespace Narcolepsy.App.Plugins.Requests;

using Microsoft.AspNetCore.Components;
using Platform.Requests;
using Platform.Serialization;

internal delegate Task<Request> RequestFactory(string? name, byte[]? serialized, object? saveState);

internal class RequestManager : IRequestManager {
    private readonly Dictionary<string, RequestDefinition> RequestFactories = new();

    private readonly Dictionary<string, object> ViewConfigurations = new();

    private readonly SerializationManager SerializationManager;

    public IEnumerable<RequestDefinition> RequestDefinitions => this.RequestFactories.Values;

    public RequestManager(SerializationManager serializationManager) =>
        this.SerializationManager = serializationManager;

    public void Configure<TViewConfiguration>(string name, Action<TViewConfiguration> buildDelegate) {
        if (!this.ViewConfigurations.TryGetValue(name, out object? Config))
            throw new NotImplementedException();

        if (Config is not TViewConfiguration ViewConfig)
            throw new NotImplementedException();

        buildDelegate(ViewConfig);
    }

    public IRequestDefinitionBuilder RegisterType<TContext, TSaveState, TView, TViewConfiguration>(
        string name, Func<TSaveState?, TContext> contextFactory, TViewConfiguration viewConfig)
        where TContext : IRequestContext
        where TSaveState : class
        where TView : IComponent
        where TViewConfiguration : notnull {
        IViewBuilder ViewBuilder = ViewBuilder<TViewConfiguration>.Create<TView>(viewConfig);

        async Task<Request> MakeRequestAsync(string? reqName, byte[]? serialized, object? saveState) {
            TSaveState? SaveToUse = saveState as TSaveState;
            SaveToUse ??= serialized is null
                ? null
                : await this.SerializationManager.DeserializeAsync<TSaveState>(serialized);
            TContext RequestContext = contextFactory(SaveToUse);
            
            if (reqName is not null)
                RequestContext.Name.Value = reqName;

            return new Request(name, RequestContext, ViewBuilder);
        }

        this.ViewConfigurations.Add(name, viewConfig);

        RequestDefinition Definition = new(name, MakeRequestAsync);
        this.RequestFactories.Add(name, Definition);

        return new RequestDefinition.RequestDefinitionBuilder(Definition);
    }

    public Task<Request> CreateRequestAsync(string typeName, string name) {
        if (!this.RequestFactories.TryGetValue(typeName, out RequestDefinition? Definition))
            throw new NotImplementedException();

        return Definition.RequestFactory(name, null, null);
    }

    public Task<Request> CreateRequestAsync(RequestSnapshot snapshot) {
        if (!this.RequestFactories.TryGetValue(snapshot.RequestType, out RequestDefinition? Definition))
            throw new NotImplementedException();

        return Definition.RequestFactory(null, snapshot.SaveState, null);
    }
    public Task<Request> CreateRequestAsync(string name, string type, object saveState) {
        if (!this.RequestFactories.TryGetValue(type, out RequestDefinition? Definition))
            throw new NotImplementedException();

        return Definition.RequestFactory(name, null, saveState);
    }
}