namespace Narcolepsy.Platform.Requests;

using Microsoft.AspNetCore.Components;

internal delegate Request RequestFactory(string name);

public class RequestManager : IRequestManager {
	private readonly Dictionary<string, RequestFactory> RequestFactories = new();

	private readonly Dictionary<string, object> ViewConfigurations = new();

	public void RegisterType<TContext, TImplementation, TView, TViewConfiguration>(string name)
		where TContext : IRequestContext
		where TImplementation : TContext, new()
		where TView : IComponent
		where TViewConfiguration : notnull, new() {
        this.RegisterType<TContext, TView, TViewConfiguration>(name, () => new TImplementation(), new TViewConfiguration());
    }

    public void RegisterType<TContext, TView, TViewConfiguration>(string name, Func<TContext> contextFactory, TViewConfiguration viewConfig) where TContext : IRequestContext where TView : IComponent where TViewConfiguration : notnull {
        IViewBuilder ViewBuilder = ViewBuilder<TViewConfiguration>.Create<TView>(viewConfig);
        Request MakeRequest(string reqName) {
            TContext RequestContext = contextFactory();
			RequestContext.Name.Value = reqName;

            return new Request(RequestContext, ViewBuilder);
        }

        this.ViewConfigurations.Add(name, viewConfig);
        this.RequestFactories.Add(name, MakeRequest);
    }

    public void Configure<TViewConfiguration>(string name, Action<TViewConfiguration> buildDelegate) {
		Type ViewType = typeof(TViewConfiguration);
		if (!this.ViewConfigurations.TryGetValue(name, out object? Config))
			throw new NotImplementedException();

		if (Config is not TViewConfiguration ViewConfig)
			throw new NotImplementedException();

		buildDelegate(ViewConfig);
	}

	public Request CreateRequest(string typeName, string name) {
		if (!this.RequestFactories.TryGetValue(typeName, out RequestFactory? Factory))
			throw new NotImplementedException();

		return Factory(name);
	}
}