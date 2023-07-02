namespace Narcolepsy.App;

using LogConsole.Services;
using Platform.Serialization;
using Plugins;
using Services;
using Plugins.Requests;
using Narcolepsy.Platform.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

public static class MauiProgram {
    public static MauiApp CreateMauiApp() {
        LifecycleManager LifecycleManager = new();

        MauiAppBuilder Builder = MauiApp.CreateBuilder();
#pragma warning disable MCT001 // `.UseMauiCommunityToolkit()` Not Found on MauiAppBuilder
        Builder
            .UseMauiApp<App>(_ => new App(LifecycleManager));
#pragma warning restore MCT001 // `.UseMauiCommunityToolkit()` Not Found on MauiAppBuilder

        Builder.Services.AddMauiBlazorWebView();
#if DEBUG
        Builder.Services.AddBlazorWebViewDeveloperTools();
#endif
        Builder.Configuration.Sources.Add(new JsonConfigurationSource() { Path = "./appsettings.json"} );
        Builder.Logging.ClearProviders();
        Builder.Logging.AddDebug();
        //Builder.Logging.AddProvider(new NarcolepsyLoggerProvider());
        Builder.Services.AddSingleton<AssetManager>();
        Builder.Services.AddSingleton<PluginManager>();
        Builder.Services.AddSingleton<SerializationManager>();
        Builder.Services.AddSingleton<RequestManager>();
        Builder.Services.AddSingleton<DuplicateService>();
        Builder.Services.AddSingleton<IStorage, FileStorage>();
        Builder.Services.AddSingleton<ILifecycleManager>(LifecycleManager);
#if DEBUG
        LogService LogService = new();
        Logger.AddSink(LogService);
        Builder.Services.AddSingleton(LogService);
#endif
        PluginManager.InitializePluginServices(Builder.Services);

        return Builder.Build();
    }
}