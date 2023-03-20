namespace Narcolepsy.Core.ViewConfig;

using Microsoft.AspNetCore.Components;
using Narcolepsy.Core.Http;
using Narcolepsy.Core.Renderables.BodyEditors;
using Narcolepsy.Core.Renderables.Tabs;
using Narcolepsy.Platform.Rendering;
using Narcolepsy.Platform.State;

public class HttpViewConfiguration : IHttpViewConfiguration {
    private readonly List<string> HttpMethods = new();
    
    private readonly List<ITab<IHttpRequestContext>> RequestTabList = new();
    
    private readonly List<ITab<IHttpRequestContext>> ResponseTabList = new();
    
    private readonly List<IRenderable<HttpResponse>> ResponseInfoBoxList = new();

    private readonly List<IBodyEditor> RequestBodyEditorList = new();

    public IReadOnlyList<string> AvailableHttpMethods => this.HttpMethods;

    public IReadOnlyList<ITab<IHttpRequestContext>> RequestTabs => this.RequestTabList;

    public IReadOnlyList<ITab<IHttpRequestContext>> ResponseTabs => this.ResponseTabList;

    public IReadOnlyList<IRenderable<HttpResponse>> ResponseInfoBoxes => this.ResponseInfoBoxList;

    public IReadOnlyList<IBodyEditor> RequestBodyEditors => this.RequestBodyEditorList;

	public IHttpViewConfiguration AddHttpMethod(string name) {
        this.HttpMethods.Add(name);
        return this;
    }

    public IHttpViewConfiguration AddRequestTab<TComponent>(string name) where TComponent : IComponent =>
        this.AddRequestTab(new SimpleTab<TComponent, IHttpRequestContext>(name));

    public IHttpViewConfiguration AddRequestTab(ITab<IHttpRequestContext> tab) {
        this.RequestTabList.Add(tab);
        return this;
    }

    public IHttpViewConfiguration AddResponseTab<TComponent>(string name) where TComponent : IComponent => this.AddResponseTab(new SimpleTab<TComponent, IHttpRequestContext>(name));

    public IHttpViewConfiguration AddResponseTab(ITab<IHttpRequestContext> tab) {
        this.ResponseTabList.Add(tab);
        return this;
    }

    public IHttpViewConfiguration AddResponseInfoBox(IRenderable<HttpResponse> infoBox) {
        this.ResponseInfoBoxList.Add(infoBox);
        return this;
    }

    public IHttpViewConfiguration AddResponseInfoBox<TComponent>() where TComponent : IComponent => this.AddResponseInfoBox(new Renderable<TComponent, HttpResponse>());
    
    public IHttpViewConfiguration AddRequestBodyEditor(IBodyEditor editor) {
        this.RequestBodyEditorList.Add(editor);
        return this;
    }

    public IHttpViewConfiguration AddRequestBodyEditor<TComponent>(string name) where TComponent : IComponent
        => this.AddRequestBodyEditor(new SimpleBodyEditor<TComponent>(name));
}