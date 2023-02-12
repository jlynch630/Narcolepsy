namespace Narcolepsy.Core.Tabs;

using Platform.Rendering;

public interface ITab : IRenderable {
    public string Title { get; }

    public bool Enabled { get; }
}