namespace Narcolepsy.App;

using Narcolepsy.App.Plugins;
using Narcolepsy.Platform.Requests;

public static class MauiProgram {
	public static MauiApp CreateMauiApp() {
		MauiAppBuilder builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();
#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
#endif

        builder.Services.AddSingleton<RequestManager>();
        builder.Services.AddSingleton<AssetManager>();
        builder.Services.AddSingleton<PluginManager>();
		PluginManager.InitializePluginServices(builder.Services);

        return builder.Build();
    }
}
