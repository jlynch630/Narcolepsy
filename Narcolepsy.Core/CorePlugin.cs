namespace Narcolepsy.Core;

using Components;
using Http;
using Platform;
using Platform.Extensions;
using ViewConfig;

public class CorePlugin : IPlugin {
    public string FullName => "Narcolepsy Core Plugin";

    public string Description => "Provides core functionality essential to Narcolepsy";

    public PluginVersion Version => new(1, 0, 0);

    public async Task InitializeAsync(NarcolepsyContext context) {
        HttpViewConfiguration Config = new();
        context.Requests
            .RegisterType<IHttpRequestContext, HttpView, IHttpViewConfiguration>("HTTP", () => new HttpRequestContext(), Config);
        
        context.Requests.ConfigureHttp(config => {
            config
                .AddHttpMethod("GET")
                .AddHttpMethod("POST")
                .AddHttpMethod("PUT")
                .AddHttpMethod("PATCH")
                .AddHttpMethod("DELETE")
                .AddHttpMethod("OPTIONS")
                .AddHttpMethod("HEAD");

            config.AddTab<TestTab>("My Tab");
        });
    }
}