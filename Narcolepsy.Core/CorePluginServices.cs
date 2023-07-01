namespace Narcolepsy.Core;

using Interop;
using Microsoft.Extensions.DependencyInjection;
using Platform;

public class CorePluginServices : IPluginSetup {
    public void ConfigureServices(IServiceCollection services) {
        services.AddScoped<NarcolepsyJs>();
    }
}