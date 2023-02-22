namespace Narcolepsy.Platform; 
using Microsoft.Extensions.DependencyInjection;

public interface IPluginSetup {
    public void ConfigureServices(IServiceCollection services);
}
