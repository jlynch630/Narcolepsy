namespace Narcolepsy.GraphQL;

using Interop;
using Microsoft.Extensions.DependencyInjection;
using Platform;

public class GraphQLServices : IPluginSetup {
    public void ConfigureServices(IServiceCollection services) {
        services.AddScoped<GraphQLInterop>();
    }
}