namespace Narcolepsy.UiKit;

using Microsoft.AspNetCore.Components;

public class StyledHtmlComponent : ComponentBase {
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? Attributes { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; }

    protected string GetClassName(params string?[] classNames) =>
        $"{StyledHtmlComponent.ConcatenateClassNames(classNames)}{(this.Attributes?.ContainsKey("class") ?? false ? " " + this.Attributes["class"] : "")}";

    private static string ConcatenateClassNames(string?[] classes) =>
        String.Join(" ", classes.Where(c => c is not null));
}