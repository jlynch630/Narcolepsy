namespace Narcolepsy.Core.Renderables.BodyEditors;

using Http;
using Platform.Rendering;

public interface IBodyEditor : IRenderable<IHttpRequestContext> {
    public string Name { get; }
}