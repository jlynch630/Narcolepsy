namespace Narcolepsy.Platform {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public interface IPlugin {
		public string FullName { get; }

		public string Description { get; }

		public PluginVersion Version { get; }

		public Task InitializeAsync(NarcolepsyContext context);
	}
}