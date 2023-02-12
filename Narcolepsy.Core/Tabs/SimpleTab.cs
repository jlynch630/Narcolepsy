namespace Narcolepsy.Core.Tabs;

using Microsoft.AspNetCore.Components;
using Platform.Rendering;

public class SimpleTab<TComponent> : Renderable<TComponent>, ITab where TComponent : IComponent {
    public SimpleTab(string title) {
        this.Title = title;
    }

    public string Title { get; }

    public bool Enabled => true;
}