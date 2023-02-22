namespace Narcolepsy.Core.ViewConfig;

using Microsoft.AspNetCore.Components;
using Narcolepsy.Core.Http;
using Narcolepsy.Platform.Rendering;
using Tabs;

public interface IHttpViewConfiguration {
    public IHttpViewConfiguration AddHttpMethod(string name);

    public IHttpViewConfiguration AddRequestTab<TComponent>(string name) where TComponent : IComponent;

    public IHttpViewConfiguration AddRequestTab(ITab<IHttpRequestContext> tab);

    public IHttpViewConfiguration AddResponseTab<TComponent>(string name) where TComponent : IComponent;

    public IHttpViewConfiguration AddResponseTab(ITab<IHttpRequestContext> tab);

    public IHttpViewConfiguration AddResponseInfoBox(IRenderable<HttpResponse> infoBox);

    public IHttpViewConfiguration AddResponseInfoBox<TComponent>() where TComponent : IComponent;
}