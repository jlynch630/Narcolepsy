namespace TestPlugin {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	using Narcolepsy.Platform;

	public class TestPlugin : IPlugin {
		public string FullName => "Test Plugin!";

		public string Description => "Test Description!";

		public PluginVersion Version => new PluginVersion(1, 0, 0);

		public async Task InitializeAsync(NarcolepsyContext context) {
			Console.WriteLine(context.AppVersion.ToString());
		}
	}
}
