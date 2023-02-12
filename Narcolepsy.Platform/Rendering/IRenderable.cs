namespace Narcolepsy.Platform.Rendering;

using Microsoft.AspNetCore.Components;

public interface IRenderable {
    public RenderFragment RenderWithContext<TContext>(TContext context);
}