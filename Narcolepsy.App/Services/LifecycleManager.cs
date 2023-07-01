namespace Narcolepsy.App.Services;

internal class LifecycleManager : ILifecycleManager {
    public event EventHandler Activated;

    public event EventHandler Deactivated;

    public event EventHandler Destroying;

    public event EventHandler Stopped;

    public void InvokeActivated() => this.Activated?.Invoke(this, EventArgs.Empty);

    public void InvokeDeactivated() => this.Deactivated?.Invoke(this, EventArgs.Empty);

    public void InvokeDestroying() => this.Destroying?.Invoke(this, EventArgs.Empty);

    public void InvokeStopped() => this.Stopped?.Invoke(this, EventArgs.Empty);
}