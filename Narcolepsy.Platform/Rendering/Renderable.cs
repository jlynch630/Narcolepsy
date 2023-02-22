namespace Narcolepsy.Platform.Rendering;

using Microsoft.AspNetCore.Components;

public class Renderable<TComponent, TContext> : IRenderable<TContext> where TComponent : IComponent {
    public RenderFragment RenderWithContext(TContext context) {
        return builder => {
            builder.OpenComponent<TComponent>(0);
            builder.AddAttribute(0, "Context", context);
            builder.CloseComponent();
        };
    }
}