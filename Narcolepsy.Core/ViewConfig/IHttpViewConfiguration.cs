namespace Narcolepsy.Core.ViewConfig;

using Microsoft.AspNetCore.Components;
using Tabs;

public interface IHttpViewConfiguration {
    public IHttpViewConfiguration AddHttpMethod(string name);

    public IHttpViewConfiguration AddTab<TComponent>(string name) where TComponent : IComponent;

    public IHttpViewConfiguration AddTab(ITab tab);
}