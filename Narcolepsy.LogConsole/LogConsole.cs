namespace Narcolepsy.LogConsole; 
public static class LogConsole {
    public static void Create() {
        if (Application.Current is null) return;
        Application.Current.OpenWindow(new Window() {
                                         Page = new LogPage(),
                                         Height = 600,
                                         Width = 1000,
                                         Title = "Log Console"
                                     });
    }
}
