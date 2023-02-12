namespace Narcolepsy.App.Plugins {
	using System.Reflection;
	using System.Runtime.InteropServices;

	using Narcolepsy.Core;
	using Narcolepsy.Platform;
    using Narcolepsy.Platform.Requests;

    internal class PluginManager {
		private LoadedPlugin[] LoadedPluginList;
        private RequestManager RequestManager;

        public PluginManager(RequestManager requestManager) {
	        this.RequestManager = requestManager;
        }

        public async Task InitializePluginsAsync() {
			// first load all our plugins: default + dynamic
			IEnumerable<LoadedPlugin> Default = PluginManager.LoadDefaultPlugins()
				.Select(plugin => new LoadedPlugin(plugin, PluginSource.BuiltIn));

			IEnumerable<LoadedPlugin> Dynamic = PluginManager.LoadDynamicPlugins()
				.Select(plugin => new LoadedPlugin(plugin, PluginSource.Dynamic));

			this.LoadedPluginList = Default.Concat(Dynamic).ToArray();

			NarcolepsyContext Context = this.CreateContext();
			// then initialize them all
			foreach (LoadedPlugin Plugin in this.LoadedPluginList)
				await Plugin.Plugin.InitializeAsync(Context);
		}

		private NarcolepsyContext CreateContext() {
			OSPlatform Platform =
				new[] { OSPlatform.Windows, OSPlatform.Linux, OSPlatform.FreeBSD, OSPlatform.OSX }.FirstOrDefault(
					RuntimeInformation.IsOSPlatform);

			return new NarcolepsyContext(
				Assembly.GetCallingAssembly().GetName().Version ?? new Version("0.0.0.0"),
				Platform,
				this.RequestManager);
		}

		private static IPlugin[] LoadDefaultPlugins() => new IPlugin[] { new CorePlugin() };

		private static IPlugin[] LoadDynamicPlugins() {
			// first get all the assemblies we could find plugins in
			Assembly[] PluginAssemblies = PluginManager.FindPluginAssemblies();

			// then find all the plugins they have
			IPlugin[] Plugins = PluginAssemblies.SelectMany(
					a => a.ExportedTypes.Where(type => typeof(IPlugin).IsAssignableFrom(type))
						.Select(type => Activator.CreateInstance(type) as IPlugin).Where(plugin => plugin is not null))
				.ToArray();

			return Plugins;
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