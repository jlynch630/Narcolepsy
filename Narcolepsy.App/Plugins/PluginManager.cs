namespace Narcolepsy.App.Plugins;

using System.Reflection;
using System.Runtime.InteropServices;
using Core;
using Narcolepsy.Platform.Logging;
using Platform;
using Platform.Requests;
using Platform.Serialization;
using Requests;
using Thrift;

internal class PluginManager {
    private static readonly Lazy<Assembly[]> PluginAssemblies = new(PluginManager.FindPluginAssemblies);
    private static readonly IPluginSetup[] DefaultPluginSetups = { new CorePluginServices() };

    private readonly AssetManager AssetManager;

    private readonly RequestManager RequestManager;
    private readonly SerializationManager SerializationManager;
    private bool HasInitialized;

    private LoadedPlugin[] LoadedPluginList;

    public PluginManager(RequestManager requestManager, AssetManager assetManager,
                         SerializationManager serializationManager) {
        this.RequestManager = requestManager;
        this.AssetManager = assetManager;
        this.SerializationManager = serializationManager;
    }

    public static void InitializePluginServices(IServiceCollection services) {
        // todo: violate srp? refactor?
        // called during app startup
        IPluginSetup[] PluginSetups = PluginManager.PluginAssemblies.Value.SelectMany(
                                                       a => a.ExportedTypes.Where(type => typeof(IPluginSetup).IsAssignableFrom(type))
                                                             .Select(type => Activator.CreateInstance(type) as IPluginSetup)
                                                             .Where(plugin => plugin is not null))
                                                   .ToArray();

        foreach (IPluginSetup Setup in PluginSetups.Concat(PluginManager.DefaultPluginSetups)) {
            Setup.ConfigureServices(services);
            Logger.Verbose("Configured services for {Plugin}", Setup.GetType().FullName);
        }
    }

    public async Task InitializePluginsAsync() {
        if (this.HasInitialized) {
            Logger.Warning("Refusing to initialize plugins again, already initialized");
            return;
        }
        if (this.LoadedPluginList is null)
            this.LoadPlugins();

        NarcolepsyContext Context = this.CreateContext();
        // then initialize them all
        foreach (LoadedPlugin Plugin in this.LoadedPluginList) {
            await Plugin.Plugin.InitializeAsync(Context);
            Logger.Information("Initialized {Source} plugin {Plugin}",
                Plugin.Source is PluginSource.BuiltIn ? "built-in" : "dynamic", Plugin.Plugin.FullName);
        }

        this.HasInitialized = true;
    }

    public void LoadPlugins() {
        // first load all our plugins: default + dynamic
        IEnumerable<LoadedPlugin> Default = PluginManager.LoadDefaultPlugins()
                                                         .Select(plugin =>
                                                             new LoadedPlugin(plugin, PluginSource.BuiltIn));

        IEnumerable<LoadedPlugin> Dynamic = PluginManager.LoadDynamicPlugins()
                                                         .Select(plugin =>
                                                             new LoadedPlugin(plugin, PluginSource.Dynamic));

        this.LoadedPluginList = Default.Concat(Dynamic).ToArray();
        Logger.Debug("Loaded {Count} total plugin(s)", this.LoadedPluginList.Length);
    }

    private static Assembly[] FindPluginAssemblies() {
        string PluginsDir = PluginManager.GetPluginsDirectory();

        // find all DLL files in the plugins dir and make them assemblies
        return Directory.EnumerateFiles(PluginsDir, "*.dll")
                        .Select(file => new PluginLoadContext(file).LoadPluginAssembly()).ToArray();
    }

    private static string GetPluginsDirectory() {
        // plugins are stored in AppData/Narcolepsy/Plugins
        string AppDataRoot = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string PluginFolder = Path.Combine(AppDataRoot, "Narcolepsy", "Plugins");
        Directory.CreateDirectory(PluginFolder);
        Logger.Debug("Using plugins directory: {Directory}", PluginFolder);
        return PluginFolder;
    }

    private static IPlugin[] LoadDefaultPlugins() => new IPlugin[] { new CorePlugin(), new ThriftPlugin() };

    private static IPlugin[] LoadDynamicPlugins() {
        // find all the plugins our assemblies have
        IPlugin[] Plugins = PluginManager.PluginAssemblies.Value.SelectMany(
                                             a => a.ExportedTypes.Where(type => typeof(IPlugin).IsAssignableFrom(type))
                                                   .Select(type => Activator.CreateInstance(type) as IPlugin)
                                                   .Where(plugin => plugin is not null))
                                         .ToArray();

        Logger.Verbose("Found {Count} dynamic plugin(s)", Plugins.Length);
        return Plugins;
    }

    private NarcolepsyContext CreateContext() {
        OSPlatform Platform =
            new[] { OSPlatform.Windows, OSPlatform.Linux, OSPlatform.OSX, OSPlatform.FreeBSD }.FirstOrDefault(
                RuntimeInformation.IsOSPlatform);

        Version Version = Assembly.GetCallingAssembly().GetName().Version ?? new Version("0.0.0.0");

        Logger.Verbose("Initializing plugin context for platform {Platform}, version {Version}", Platform, Version);
        return new NarcolepsyContext(
            Version,
            Platform,
            this.RequestManager,
            this.AssetManager,
            this.SerializationManager);
    }
}