namespace Narcolepsy.UiKit {
    using Microsoft.AspNetCore.Components;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class StyledHtmlComponent : ComponentBase {

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object>? Attributes { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        protected string GetClassName(string className) => $"{className}{((Attributes?.ContainsKey("class") ?? false) ? " " + Attributes["class"] : "")}";
    }
}
