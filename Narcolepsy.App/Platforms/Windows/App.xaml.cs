﻿// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Narcolepsy.App.WinUI;

using System.Diagnostics;

/// <summary>
///     Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : MauiWinUIApplication {
    public static Stopwatch Timer;
    public int AppTitleBarHeight { get; set; } = 48;
    /// <summary>
    ///     Initializes the singleton application object.  This is the first line of authored code
    ///     executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App() {
        App.Timer = Stopwatch.StartNew();
        this.InitializeComponent();
        
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}