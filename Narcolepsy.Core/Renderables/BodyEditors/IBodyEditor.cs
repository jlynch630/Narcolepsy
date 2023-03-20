namespace Narcolepsy.Core.Renderables.BodyEditors; 
using Narcolepsy.Core.Http;
using Narcolepsy.Platform.Rendering;

public interface IBodyEditor : IRenderable<IHttpRequestContext> {
    public string Name { get; }
}
