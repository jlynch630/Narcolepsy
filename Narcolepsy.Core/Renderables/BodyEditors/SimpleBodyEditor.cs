namespace Narcolepsy.Core.Renderables.BodyEditors;

using Http;
using Microsoft.AspNetCore.Components;
using Platform.Rendering;

internal class SimpleBodyEditor<TComponent> : Renderable<TComponent, IHttpRequestContext>, IBodyEditor
    where TComponent : IComponent {
    public SimpleBodyEditor(string name, string id) {
        this.Name = name;
        this.Id = id;
    }

    public string Name { get; }

    public string Id { get; }
}