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
using Microsoft.Maui.LifecycleEvents;
using Microsoft.Maui.Platform;

public static class MauiProgram {
    public static MauiApp CreateMauiApp() {
        LifecycleManager LifecycleManager = new();

        MauiAppBuilder Builder = MauiApp.CreateBuilder();
#pragma warning disable MCT001 // `.UseMauiCommunityToolkit()` Not Found on MauiAppBuilder
        Builder
            .UseMauiApp<App>(_ => new App(LifecycleManager));
#pragma warning restore MCT001 // `.UseMauiCommunityToolkit()` Not Found on MauiAppBuilder

        Builder.Services.AddMauiBlazorWebView();
        Builder.Logging.ClearProviders();
#if DEBUG
        Builder.Configuration.Sources.Add(new JsonConfigurationSource() { Path = "./appsettings.json"} );
        Builder.Services.AddBlazorWebViewDeveloperTools();
        Builder.Logging.AddDebug();
#endif
        Builder.ConfigureLifecycleEvents(l => {
#if WINDOWS
            l.AddWindows(w => {
                w.OnWindowCreated(window => {
                    window.ExtendsContentIntoTitleBar = true;
                });
            });
#endif
        });

        Builder.Services.AddSingleton<AssetManager>();
        Builder.Services.AddSingleton<PluginManager>();
        Builder.Services.AddSingleton<SerializationManager>();
        Builder.Services.AddSingleton<RequestManager>();
        Builder.Services.AddSingleton<DuplicateService>();
        Builder.Services.AddSingleton<IStorage, FileStorage>();
        Builder.Services.AddSingleton<ILifecycleManager>(LifecycleManager);
        LogService LogService = new();
        Logger.AddSink(LogService);
        Builder.Services.AddSingleton(LogService);
        PluginManager.InitializePluginServices(Builder.Services);

        return Builder.Build();
    }
}