namespace Narcolepsy.Core;

using Components;
using Components.Http;
using Http;
using Interop;
using Microsoft.Extensions.DependencyInjection;
using Narcolepsy.Core.Components.Http.InfoBoxes;
using Narcolepsy.Core.Components.Http.RequestTabs;
using Narcolepsy.Core.Components.Http.ResponseTabs;
using Narcolepsy.Core.Renderables.BodyEditors;
using Narcolepsy.Platform.Serialization;
using Platform;
using Platform.Extensions;
using System;
using ViewConfig;

public class CorePlugin : IPlugin {
    public string FullName => "Narcolepsy Core Plugin";

    public string Description => "Provides core functionality essential to Narcolepsy";

    public PluginVersion Version => new(1, 0, 0);

    public async Task InitializeAsync(NarcolepsyContext context) {
        HttpViewConfiguration Config = new();
        context.Requests
            .RegisterType<IHttpRequestContext, HttpRequestContextSnapshot, HttpView, IHttpViewConfiguration>(
                "HTTP",
                (snapshot) => new HttpRequestContext(snapshot),
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
                .AddRequestTab<RequestBodyTab>("Body")
                .AddRequestTab<RequestHeaderTab>("Headers");

            config
                .AddResponseTab<ResponseBodyTab>("Body")
                .AddResponseTab<ResponseHeaderTab>("Headers");

            config
                .AddResponseInfoBox<StatusCodeInfo>()
                .AddResponseInfoBox<TimeInfo>()
                .AddResponseInfoBox<ResponseSizeInfo>();

            config
                .AddRequestBodyEditor<EmptyBodyView>("No Body")
                .AddCodeBodyEditor("JSON", "json", "application/json; charset=utf-8")
                .AddCodeBodyEditor("XML", "xml", "application/xml; charset=utf-8")
                .AddCodeBodyEditor("Plain Text", "plaintext", "text/plain; charset=utf-8");
        });

        context.Assets.InjectScript("script/index.js");
    }
}
