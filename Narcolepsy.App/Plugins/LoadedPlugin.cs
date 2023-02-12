namespace Narcolepsy.App.Plugins {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	using Narcolepsy.Platform;

	internal record LoadedPlugin(IPlugin Plugin, PluginSource Source) {
	}
}
