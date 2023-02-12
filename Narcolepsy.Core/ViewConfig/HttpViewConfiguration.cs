namespace Narcolepsy.Core.ViewConfig;

using Microsoft.AspNetCore.Components;
using Tabs;

public class HttpViewConfiguration : IHttpViewConfiguration {
    private readonly List<string> HttpMethods = new();
    private readonly List<ITab> TabList = new();

    public IReadOnlyList<string> AvailableHttpMethods => this.HttpMethods;

    public IReadOnlyList<ITab> Tabs => this.TabList;

    public IHttpViewConfiguration AddHttpMethod(string name) {
        this.HttpMethods.Add(name);
        return this;
    }

    public IHttpViewConfiguration AddTab<TComponent>(string name) where TComponent : IComponent {
        this.TabList.Add(new SimpleTab<TComponent>(name));
        return this;
    }

    public IHttpViewConfiguration AddTab(ITab tab) {
        this.TabList.Add(tab);
        return this;
    }
}