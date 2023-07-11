#nullable enable
namespace Narcolepsy.App.Plugins.Requests;

using Microsoft.AspNetCore.Components;
using Microsoft.Maui.Controls;
using Narcolepsy.Platform.Logging;
using Platform.Requests;
using Platform.Serialization;
using System.Reflection;

internal delegate Task<Request> RequestFactory(string? name, byte[]? serialized, object? saveState);

internal class RequestManager : IRequestManager {
    private readonly Dictionary<string, RequestDefinition> RequestFactories = new();

    private readonly Dictionary<string, object> ViewConfigurations = new();

    private readonly SerializationManager SerializationManager;

    public IEnumerable<RequestDefinition> RequestDefinitions => this.RequestFactories.Values;

    public RequestManager(SerializationManager serializationManager) =>
        this.SerializationManager = serializationManager;

    public void Configure<TViewConfiguration>(string name, Action<TViewConfiguration> buildDelegate) {
#if DEBUG
        string? CallingAssembly = Assembly.GetCallingAssembly().GetName().Name;
#endif
        if (!this.ViewConfigurations.TryGetValue(name, out object? Config)) {
#if DEBUG
            throw new RequestConfigurationException(
                $"A plugin from assembly {CallingAssembly} tried to configure a request type ({name}) that didn't exist. The available request types are: {String.Join(", ", this.RequestDefinitions.Select(x => x.Name))}");
#else
            Logger.Warning("A plugin tried to configure a request type ({Type}) that didn't exist. This issue would throw an exception in debug but silently fails in release mode", name);
            return;
#endif
        }

        if (Config is not TViewConfiguration ViewConfig) {
#if DEBUG
            throw new RequestConfigurationException(
                $"A plugin from assembly {CallingAssembly} tried to configure a request type ({name}) with an improper TViewConfiguration type {typeof(TViewConfiguration).Name}. The expected view configuration type is {Config.GetType().Name}");
#else
            Logger.Warning(
                "A plugin tried to configure a request type ({Type}) with an improper TViewConfiguration type. This issue would throw an exception in debug but silently fails in release mode",
                name);
            return;
#endif
        }

#if DEBUG
        Logger.Verbose(
            "Plugin from assembly {SourceAssembly} configured request type {RequestType}",
            CallingAssembly, 
            name);
#endif
        buildDelegate(ViewConfig);
    }

    public IRequestDefinitionBuilder RegisterType<TContext, TSaveState, TView, TViewConfiguration>(
        string name, Func<TSaveState?, TContext> contextFactory, TViewConfiguration viewConfig)
        where TContext : IRequestContext
        where TSaveState : class
        where TView : IComponent
        where TViewConfiguration : notnull {
        if (this.RequestFactories.ContainsKey(name))
            throw new RequestConfigurationException(
                $"Request type ({name}) already exists.");

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

    public Task<Request> CreateRequestAsync(string typeName, string name) => this.CreateRequestAsync(typeName, name, null, null);

    public Task<Request> CreateRequestAsync(RequestSnapshot snapshot) => this.CreateRequestAsync(snapshot.RequestType, null, snapshot.SaveState, null);

    public Task<Request> CreateRequestAsync(string name, string type, object saveState) => this.CreateRequestAsync(type, name, null, saveState);

    private Task<Request> CreateRequestAsync(string type, string? name, byte[]? serialized, object? saveState) =>
        !this.RequestFactories.TryGetValue(type, out RequestDefinition? Definition)
            ? throw new RequestConfigurationException($"Failed to create request. Request type {type} does not exist")
            : Definition.RequestFactory(name, serialized, saveState);
}