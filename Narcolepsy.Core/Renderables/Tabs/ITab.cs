namespace Narcolepsy.Core.Renderables.Tabs;

using Platform.Rendering;

public interface ITab<in TContext> : IRenderable<TContext>
{
    public string Title { get; }

    public bool Enabled { get; }
}