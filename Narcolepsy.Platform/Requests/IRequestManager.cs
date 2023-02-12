namespace Narcolepsy.Platform.Requests;

using Microsoft.AspNetCore.Components;

public interface IRequestManager {
	void RegisterType<TContext, TImplementation, TView, TViewConfiguration>(string name)
		where TContext : IRequestContext
		where TImplementation : TContext, new()
		where TView : IComponent
        where TViewConfiguration : notnull, new();

    void RegisterType<TContext, TView, TViewConfiguration>(string name, Func<TContext> contextFactory, TViewConfiguration viewConfig)
        where TContext : IRequestContext
        where TView : IComponent
        where TViewConfiguration : notnull;

    void Configure<TViewConfiguration>(string name, Action<TViewConfiguration> buildDelegate);
}