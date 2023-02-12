namespace Narcolepsy.Platform.Requests;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

internal class ViewBuilder<TConfiguration> : IViewBuilder {
	private ViewBuilder(Type componentType, TConfiguration configuration) {
		this.ComponentType = componentType;
		this.Configuration = configuration;
	}

	public Type ComponentType { get; }

	public TConfiguration Configuration { get; }

	public void Configure(RenderTreeBuilder builder) {
		builder.AddAttribute(0, nameof(this.Configuration), this.Configuration);
	}

	public static ViewBuilder<TConfiguration> Create<TComponent>(TConfiguration config) where TComponent : IComponent =>
		new(typeof(TComponent), config);
}