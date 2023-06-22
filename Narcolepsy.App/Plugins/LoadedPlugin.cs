namespace Narcolepsy.App.Plugins;

using Platform;

internal record LoadedPlugin(IPlugin Plugin, PluginSource Source) { }