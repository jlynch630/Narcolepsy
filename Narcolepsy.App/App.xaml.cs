namespace Narcolepsy.App;

using Services;

public partial class App : Application {
    private readonly LifecycleManager Manager;

    internal App(LifecycleManager manager) {
        this.Manager = manager;
        this.InitializeComponent();

        this.MainPage = new MainPage();
    }

    protected override Window CreateWindow(IActivationState activationState) {
        Window Base = base.CreateWindow(activationState);
        Base.Title = "Narcolepsy";
        Base.Activated += (_, _) => this.Manager.InvokeActivated();
        Base.Deactivated += (_, _) => this.Manager.InvokeDeactivated();
        Base.Destroying += (_, _) => this.Manager.InvokeDestroying();
        Base.Stopped += (_, _) => this.Manager.InvokeStopped();
        return Base;
    }
}