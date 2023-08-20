namespace Narcolepsy.Core.ViewConfig;

using Http;
using Microsoft.AspNetCore.Components;
using Platform.Rendering;
using Renderables.BodyEditors;
using Renderables.Tabs;

public class HttpViewConfiguration : IHttpViewConfiguration {
    private readonly List<string> HttpMethods = new();

    private readonly List<IBodyEditor> RequestBodyEditorList = new();

    private readonly List<ITab<IHttpRequestContext>> RequestTabList = new();

    private readonly List<IRenderable<HttpResponse>> ResponseInfoBoxList = new();

    private readonly List<ITab<IHttpRequestContext>> ResponseTabList = new();

    public IReadOnlyList<string> AvailableHttpMethods => this.HttpMethods;

    public IReadOnlyList<IBodyEditor> RequestBodyEditors => this.RequestBodyEditorList;

    public IReadOnlyList<ITab<IHttpRequestContext>> RequestTabs => this.RequestTabList;

    public IReadOnlyList<IRenderable<HttpResponse>> ResponseInfoBoxes => this.ResponseInfoBoxList;

    public IReadOnlyList<ITab<IHttpRequestContext>> ResponseTabs => this.ResponseTabList;

    public IHttpViewConfiguration AddHttpMethod(string name) {
        this.HttpMethods.Add(name);
        return this;
    }

    public IHttpViewConfiguration AddRequestBodyEditor(IBodyEditor editor) {
        this.RequestBodyEditorList.Add(editor);
        return this;
    }

    public IHttpViewConfiguration AddRequestBodyEditor<TComponent>(string name, string id) where TComponent : IComponent
        => this.AddRequestBodyEditor(new SimpleBodyEditor<TComponent>(name, id));

    public IHttpViewConfiguration AddRequestTab<TComponent>(string name) where TComponent : IComponent =>
        this.AddRequestTab(new SimpleTab<TComponent, IHttpRequestContext>(name));

    public IHttpViewConfiguration AddRequestTab(ITab<IHttpRequestContext> tab) {
        this.RequestTabList.Add(tab);
        return this;
    }

    public IHttpViewConfiguration AddResponseInfoBox(IRenderable<HttpResponse> infoBox) {
        this.ResponseInfoBoxList.Add(infoBox);
        return this;
    }

    public IHttpViewConfiguration AddResponseInfoBox<TComponent>() where TComponent : IComponent =>
        this.AddResponseInfoBox(new Renderable<TComponent, HttpResponse>());

    public IHttpViewConfiguration AddResponseTab<TComponent>(string name) where TComponent : IComponent =>
        this.AddResponseTab(new SimpleTab<TComponent, IHttpRequestContext>(name));

    public IHttpViewConfiguration AddResponseTab(ITab<IHttpRequestContext> tab) {
        this.ResponseTabList.Add(tab);
        return this;
    }
}