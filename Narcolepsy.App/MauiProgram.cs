namespace Narcolepsy.App;

using Narcolepsy.App.Plugins;
using Narcolepsy.App.Services;
using Narcolepsy.Platform.Requests;
using Narcolepsy.Platform.Serialization;

public static class MauiProgram {
	public static MauiApp CreateMauiApp() {
		MauiAppBuilder builder = MauiApp.CreateBuilder();
#pragma warning disable MCT001 // `.UseMauiCommunityToolkit()` Not Found on MauiAppBuilder
        builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});
#pragma warning restore MCT001 // `.UseMauiCommunityToolkit()` Not Found on MauiAppBuilder

        builder.Services.AddMauiBlazorWebView();
#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
#endif

        builder.Services.AddSingleton<AssetManager>();
        builder.Services.AddSingleton<PluginManager>();
        builder.Services.AddSingleton<SerializationManager>();
        builder.Services.AddSingleton<RequestManager>();
        builder.Services.AddSingleton<RequestStorage>();
        PluginManager.InitializePluginServices(builder.Services);

        return builder.Build();
    }
}
