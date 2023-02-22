namespace Narcolepsy.Core;

using Components;
using Components.Http;
using Http;
using Interop;
using Microsoft.Extensions.DependencyInjection;
using Narcolepsy.Core.Components.Http.InfoBoxes;
using Narcolepsy.Core.Components.Http.RequestTabs;
using Narcolepsy.Core.Components.Http.ResponseTabs;
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
            .RegisterType<IHttpRequestContext, HttpView, IHttpViewConfiguration>(
                "HTTP",
                () => new HttpRequestContext(),
                Config);

        context.Requests.ConfigureHttp(config => {
            config
                .AddHttpMethod("GET")
                .AddHttpMethod("POST")
                .AddHttpMethod("PUT")
                .AddHttpMethod("PATCH")
                .AddHttpMethod("DELETE")
                .AddHttpMethod("OPTIONS")
                .AddHttpMethod("HEAD");

            config
                .AddRequestTab<RequestHeaderTab>("Headers");

            config
                .AddResponseTab<ResponseBodyTab>("Body")
                .AddResponseTab<ResponseHeaderTab>("Headers");

            config.AddResponseInfoBox<StatusCodeInfo>();
            config.AddResponseInfoBox<TimeInfo>();
            config.AddResponseInfoBox<ResponseSizeInfo>();
        });

        context.Assets.InjectScript("script/index.js");
    }
}
