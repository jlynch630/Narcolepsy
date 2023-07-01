namespace Narcolepsy.Core.Renderables.BodyEditors;

using Http;
using Microsoft.AspNetCore.Components;
using Platform.Rendering;

internal class SimpleBodyEditor<TComponent> : Renderable<TComponent, IHttpRequestContext>, IBodyEditor
    where TComponent : IComponent {
    public SimpleBodyEditor(string name) => this.Name = name;

    public string Name { get; }
}