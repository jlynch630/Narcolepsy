namespace Narcolepsy.Core; 
using Microsoft.Extensions.DependencyInjection;
using Narcolepsy.Core.Interop;
using Narcolepsy.Platform;

public class CorePluginServices : IPluginSetup {
    public void ConfigureServices(IServiceCollection services) {
        services.AddScoped<NarcolepsyJs>();
    }
}
