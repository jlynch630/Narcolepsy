namespace Narcolepsy.App.Plugins {
	using System.Reflection;
    using System.Runtime.InteropServices;

	using Narcolepsy.Core;
	using Narcolepsy.Platform;
    using Narcolepsy.Platform.Requests;

    internal class PluginManager {
		private static readonly Lazy<Assembly[]> PluginAssemblies = new(PluginManager.FindPluginAssemblies);
		private static readonly IPluginSetup[] DefaultPluginSetups = new[] { new CorePluginServices() };

        private readonly RequestManager RequestManager;
        private readonly AssetManager AssetManager;

        private LoadedPlugin[] LoadedPluginList;
        private bool HasInitialized = false;

        public PluginManager(RequestManager requestManager, AssetManager assetManager) {
	        this.RequestManager = requestManager;
            this.AssetManager = assetManager;
        }

        public async Task InitializePluginsAsync() {
			if (this.HasInitialized) return;
            if (this.LoadedPluginList is null)
                this.LoadPlugins();
			
            NarcolepsyContext Context = this.CreateContext();
			// then initialize them all
			foreach (LoadedPlugin Plugin in this.LoadedPluginList)
				await Plugin.Plugin.InitializeAsync(Context);
            this.HasInitialized = true;
        }

        public void LoadPlugins() {
            // first load all our plugins: default + dynamic
            IEnumerable<LoadedPlugin> Default = PluginManager.LoadDefaultPlugins()
                .Select(plugin => new LoadedPlugin(plugin, PluginSource.BuiltIn));

            IEnumerable<LoadedPlugin> Dynamic = PluginManager.LoadDynamicPlugins()
                .Select(plugin => new LoadedPlugin(plugin, PluginSource.Dynamic));

            this.LoadedPluginList = Default.Concat(Dynamic).ToArray();
        }

        private NarcolepsyContext CreateContext() {
			OSPlatform Platform =
				new[] { OSPlatform.Windows, OSPlatform.Linux, OSPlatform.OSX, OSPlatform.FreeBSD }.FirstOrDefault(
					RuntimeInformation.IsOSPlatform);

			return new NarcolepsyContext(
				Assembly.GetCallingAssembly().GetName().Version ?? new Version("0.0.0.0"),
				Platform,
				this.RequestManager,
                this.AssetManager);
		}

		private static IPlugin[] LoadDefaultPlugins() => new IPlugin[] { new CorePlugin() };

		private static IPlugin[] LoadDynamicPlugins() {
			// find all the plugins our assemblies have
			IPlugin[] Plugins = PluginManager.PluginAssemblies.Value.SelectMany(
					a => a.ExportedTypes.Where(type => typeof(IPlugin).IsAssignableFrom(type))
						.Select(type => Activator.CreateInstance(type) as IPlugin).Where(plugin => plugin is not null))
				.ToArray();

			return Plugins;
		}

		public static void InitializePluginServices(IServiceCollection services) {
            // todo: violate srp? refactor?
            // called during app startup
            IPluginSetup[] PluginSetups = PluginManager.PluginAssemblies.Value.SelectMany(
                    a => a.ExportedTypes.Where(type => typeof(IPluginSetup).IsAssignableFrom(type))
                        .Select(type => Activator.CreateInstance(type) as IPluginSetup).Where(plugin => plugin is not null))
                .ToArray();

			foreach (IPluginSetup Setup in PluginSetups.Concat(PluginManager.DefaultPluginSetups))
				Setup.ConfigureServices(services);
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

			return PluginFolder;
		}
	}
}