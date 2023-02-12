namespace Narcolepsy.Platform.Requests;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

public interface IViewBuilder {
	public Type ComponentType { get; }

	public void Configure(RenderTreeBuilder builder);
}