namespace Narcolepsy.App.Services;

internal interface ILifecycleManager {
    public event EventHandler Activated;

    public event EventHandler Deactivated;

    public event EventHandler Destroying;

    public event EventHandler Stopped;
}