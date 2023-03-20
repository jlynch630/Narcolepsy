namespace Narcolepsy.Core.Renderables.Tabs;

using Microsoft.AspNetCore.Components;
using Platform.Rendering;

public class SimpleTab<TComponent, TContext> : Renderable<TComponent, TContext>, ITab<TContext> where TComponent : IComponent {
    public SimpleTab(string title) {
        this.Title = title;
    }

    public string Title { get; }

    public bool Enabled => true;
}