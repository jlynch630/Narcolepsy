namespace Narcolepsy.App;

using Services;
using System.Diagnostics;

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

    protected override void OnStart() {
        base.OnStart();

#if !DEBUG
        // in a release build, only show the log console when NARCOLEPSY_SHOW_LOG_CONSOLE is defined
        if (Environment.GetEnvironmentVariable("NARCOLEPSY_SHOW_LOG_CONSOLE") is null) return;
#endif

        LogConsole.LogConsole.Create();
    }
}