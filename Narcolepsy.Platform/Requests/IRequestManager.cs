namespace Narcolepsy.Platform.Requests;

using Microsoft.AspNetCore.Components;

public interface IRequestManager {
    void RegisterType<TContext, TSaveState, TView, TViewConfiguration>(string name, Func<TSaveState?, TContext> contextFactory, TViewConfiguration viewConfig)
        where TContext : IRequestContext
        where TSaveState : class
        where TView : IComponent
        where TViewConfiguration : notnull;

    void Configure<TViewConfiguration>(string name, Action<TViewConfiguration> buildDelegate);
}