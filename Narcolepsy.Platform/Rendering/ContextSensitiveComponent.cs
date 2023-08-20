namespace Narcolepsy.Platform.Rendering;

using Microsoft.AspNetCore.Components;
using Requests;

public abstract class ContextSensitiveComponent<TContext> : StateSensitiveComponent<TContext>
    where TContext : IRequestContext {
    [Parameter]
    public TContext? Context { get; set; }

    protected override TContext? StateProvider => this.Context;

    protected virtual Task OnContextChangedAsync() => Task.CompletedTask;

    protected sealed override async Task OnStateChangedAsync() {
        await base.OnStateChangedAsync();
        await this.OnContextChangedAsync();
    }
}