﻿namespace Narcolepsy.Platform.Requests;

using Microsoft.AspNetCore.Components;

public class Request {
	private readonly IViewBuilder ViewBuilder;

	public Request(string type, IRequestContext context, IViewBuilder viewBuilder) {
		this.Type = type;
		this.Context = context;
		this.ViewBuilder = viewBuilder;
	}

	public IRequestContext Context { get; }

    public string Type { get; }

    public RenderFragment Render() {
		Type ComponentType = this.ViewBuilder.ComponentType;
		return builder =>
		{
			builder.OpenComponent(0, ComponentType);
			builder.AddAttribute(0, "Context", this.Context);
			this.ViewBuilder.Configure(builder);
			builder.CloseComponent();
		};
	}
}