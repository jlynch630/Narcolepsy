namespace Narcolepsy.Core.Renderables.Tabs;

using Platform.Rendering;

public interface ITab<in TContext> : IRenderable<TContext> {
    public bool Enabled { get; }

    public string Title { get; }
}