namespace Narcolepsy.Core;

using Components.Http;
using Components.Http.InfoBoxes;
using Components.Http.RequestTabs;
using Components.Http.ResponseTabs;
using Http;
using Platform;
using Platform.Extensions;
using Renderables.BodyEditors;
using System.Net.Mime;
using System.Xml.Linq;
using ViewConfig;

public class CorePlugin : IPlugin {
    public string Description => "Provides core functionality essential to Narcolepsy";

    public string FullName => "Narcolepsy Core Plugin";

    public async Task InitializeAsync(NarcolepsyContext context) {
        HttpViewConfiguration Config = new();
        context.Requests
               .RegisterType<IHttpRequestContext, HttpRequestContextSnapshot, HttpView, IHttpViewConfiguration>(
                   "HTTP",
                   snapshot => new HttpRequestContext(snapshot),
                   Config)
               .ConfigureIcon("language");

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
                .AddRequestBodyEditor<EmptyBodyView>("No Body", "Narcolepsy.Core-Empty")
                .AddRequestBodyEditor<UrlEncodedFormBodyView>("Url Encoded Form", "Narcolepsy.Core-form")
                .AddCodeBodyEditor("JSON", "Narcolepsy.Core-Json","json", "application/json; charset=utf-8")
                .AddCodeBodyEditor("XML", "Narcolepsy.Core-XML", "xml", "application/xml; charset=utf-8")
                .AddCodeBodyEditor("Plain Text", "Narcolepsy.Core-Text", "plaintext", "text/plain; charset=utf-8");
        });
    }

    public PluginVersion Version => new(0, 0, 1);
}