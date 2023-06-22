namespace Narcolepsy.Platform.Rendering; 
using Microsoft.AspNetCore.Components;
using Narcolepsy.Platform.Requests;
using Narcolepsy.Platform.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

public abstract class ContextSensitiveComponent<TContext> : ComponentBase, IDisposable where TContext : IRequestContext {
    [Parameter]
    public TContext Context { get; set; }

    private List<Action> RemoveEventHandlerDelegates = new();

    private List<Action<TContext>> AttachEventHandlerDelegates = new();

    public override async Task SetParametersAsync(ParameterView parameters) {
        TContext? NewContextValue = parameters.GetValueOrDefault<TContext>(nameof(this.Context));
        bool ContextChanged = !(this.Context?.Equals(NewContextValue) ?? false);

        await base.SetParametersAsync(parameters);

        if (ContextChanged) await this.OnContextChangedAsync();
    }

    protected void UpdateOnChange<T>(Func<TContext, IReadOnlyState<T>> stateGetter) {
        if (this.Context is not null)
            this.UpdateOnChange(stateGetter(this.Context));
        AttachEventHandlerDelegates.Add(ctx => this.UpdateOnChange(stateGetter(ctx)));
    }

    protected void UpdateOnChange<T>(IReadOnlyState<T> state) {
        state.ValueChanged += this.StateValueChanged;
        this.RemoveEventHandlerDelegates.Add(() => state.ValueChanged -= this.StateValueChanged);
    }

    private void StateValueChanged(object? sender, EventArgs e) {
        this.InvokeAsync(this.StateHasChanged);
    }

    protected virtual Task OnContextChangedAsync() {
        this.RemoveAllEventHandlers();

        if (this.Context is not null) {
            foreach (Action<TContext> AttachEventHandler in this.AttachEventHandlerDelegates)
                AttachEventHandler(this.Context);
        }
        this.StateHasChanged();
        return Task.CompletedTask;
    }

    private void RemoveAllEventHandlers() {
        foreach (Action RemoveEventHandler in this.RemoveEventHandlerDelegates)
            RemoveEventHandler();
    }

    public void Dispose() {
        this.RemoveAllEventHandlers();
    }
}
