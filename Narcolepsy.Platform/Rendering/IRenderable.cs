namespace Narcolepsy.Platform.Rendering;

using Microsoft.AspNetCore.Components;

public interface IRenderable<in TContext> {
    public RenderFragment RenderWithContext(TContext context);
}