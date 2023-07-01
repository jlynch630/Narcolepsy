namespace Narcolepsy.LogConsole {
    using Microsoft.Extensions.Logging;
    using Narcolepsy.LogConsole.Data;
    using Narcolepsy.LogConsole.Services;

    public static class MauiProgram {
        public static MauiApp CreateMauiApp() {
            var Builder = MauiApp.CreateBuilder();
            Builder
                .UseMauiApp<App>();

            Builder.Services.AddMauiBlazorWebView();
#if DEBUG
		Builder.Services.AddBlazorWebViewDeveloperTools();
		Builder.Logging.AddDebug();
#endif
            Builder.Services.AddSingleton<LogService>();
            return Builder.Build();
        }
    }
}