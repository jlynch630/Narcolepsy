namespace Narcolepsy.Platform.Requests;

using Microsoft.AspNetCore.Components;
using Narcolepsy.Platform.Serialization;

internal delegate Task<Request> RequestFactory(string? name, byte[]? serialized);

public class RequestManager : IRequestManager {
	private readonly Dictionary<string, RequestFactory> RequestFactories = new();

	private readonly Dictionary<string, object> ViewConfigurations = new();

	private SerializationManager SerializationManager;

	public RequestManager(SerializationManager serializationManager) {
		this.SerializationManager = serializationManager;
	}

    public void RegisterType<TContext, TSaveState, TView, TViewConfiguration>(string name, Func<TSaveState?, TContext> contextFactory, TViewConfiguration viewConfig)
		where TContext : IRequestContext
        where TSaveState : class
        where TView : IComponent
		where TViewConfiguration : notnull {
        IViewBuilder ViewBuilder = ViewBuilder<TViewConfiguration>.Create<TView>(viewConfig);
        async Task<Request> MakeRequestAsync(string? reqName, byte[]? serialized) {
            TContext RequestContext = serialized is null
				? contextFactory(null)
				: contextFactory(await this.SerializationManager.DeserializeAsync<TSaveState>(serialized));

			if (reqName is not null)
				RequestContext.Name.Value = reqName;

            return new Request(name, RequestContext, ViewBuilder);
        }

        this.ViewConfigurations.Add(name, viewConfig);
        this.RequestFactories.Add(name, MakeRequestAsync);
    }

    public void Configure<TViewConfiguration>(string name, Action<TViewConfiguration> buildDelegate) {
		Type ViewType = typeof(TViewConfiguration);
		if (!this.ViewConfigurations.TryGetValue(name, out object? Config))
			throw new NotImplementedException();

		if (Config is not TViewConfiguration ViewConfig)
			throw new NotImplementedException();

		buildDelegate(ViewConfig);
	}

	public Task<Request> CreateRequestAsync(string typeName, string name) {
		if (!this.RequestFactories.TryGetValue(typeName, out RequestFactory? Factory))
			throw new NotImplementedException();

		return Factory(name, null);
	}

    public Task<Request> CreateRequestAsync(RequestSnapshot snapshot) {
        if (!this.RequestFactories.TryGetValue(snapshot.RequestType, out RequestFactory? Factory))
            throw new NotImplementedException();

        return Factory(null, snapshot.SaveState);
    }
}