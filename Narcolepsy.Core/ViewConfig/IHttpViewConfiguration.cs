namespace Narcolepsy.Core.ViewConfig;

using Http;
using Microsoft.AspNetCore.Components;
using Platform.Rendering;
using Renderables.BodyEditors;
using Renderables.Tabs;

public interface IHttpViewConfiguration {
    public IHttpViewConfiguration AddHttpMethod(string name);

    public IHttpViewConfiguration AddRequestBodyEditor(IBodyEditor editor);

    public IHttpViewConfiguration AddRequestBodyEditor<TComponent>(string name, string id) where TComponent : IComponent;

    public IHttpViewConfiguration AddRequestTab<TComponent>(string name) where TComponent : IComponent;

    public IHttpViewConfiguration AddRequestTab(ITab<IHttpRequestContext> tab);

    public IHttpViewConfiguration AddResponseInfoBox(IRenderable<HttpResponse> infoBox);

    public IHttpViewConfiguration AddResponseInfoBox<TComponent>() where TComponent : IComponent;

    public IHttpViewConfiguration AddResponseTab<TComponent>(string name) where TComponent : IComponent;

    public IHttpViewConfiguration AddResponseTab(ITab<IHttpRequestContext> tab);
}