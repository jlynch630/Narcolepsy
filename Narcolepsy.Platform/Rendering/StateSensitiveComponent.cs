namespace Narcolepsy.Platform.Rendering;

using Microsoft.AspNetCore.Components;
using State;

public abstract class StateSensitiveComponent<TStateProvider> : ComponentBase, IDisposable {
    private readonly List<Action<TStateProvider>> AttachEventHandlerDelegates = new();

    private readonly List<Action> RemoveEventHandlerDelegates = new();

    protected abstract TStateProvider? StateProvider { get; }

    public virtual void Dispose() {
        this.RemoveAllEventHandlers();
        GC.SuppressFinalize(this);
    }

    public override async Task SetParametersAsync(ParameterView parameters) {
        TStateProvider? NewContextValue = parameters.GetValueOrDefault<TStateProvider>(nameof(this.StateProvider));
        bool ContextChanged = !(this.StateProvider?.Equals(NewContextValue) ?? false);

        await base.SetParametersAsync(parameters);

        if (ContextChanged) await this.OnStateChangedAsync();
    }

    protected virtual Task OnStateChangedAsync() {
        this.RemoveAllEventHandlers();

        if (this.StateProvider is not null) {
            foreach (Action<TStateProvider> AttachEventHandler in this.AttachEventHandlerDelegates)
                AttachEventHandler(this.StateProvider);
        }

        this.StateHasChanged();
        return Task.CompletedTask;
    }

    protected void UpdateOnChange<T>(Func<TStateProvider, IReadOnlyState<T>> stateGetter) {
        if (this.StateProvider is not null)
            this.UpdateOnChange(stateGetter(this.StateProvider));
        this.AttachEventHandlerDelegates.Add(ctx => this.UpdateOnChange(stateGetter(ctx)));
    }

    protected void UpdateOnChange<T>(IReadOnlyState<T> state) {
        state.ValueChanged += this.StateValueChanged;
        this.RemoveEventHandlerDelegates.Add(() => state.ValueChanged -= this.StateValueChanged);
    }

    private void RemoveAllEventHandlers() {
        foreach (Action RemoveEventHandler in this.RemoveEventHandlerDelegates)
            RemoveEventHandler();
    }

    private void StateValueChanged(object? sender, EventArgs e) {
        this.InvokeAsync(this.StateHasChanged);
    }
}