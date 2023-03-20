namespace Narcolepsy.Core.Renderables.BodyEditors;

using Microsoft.AspNetCore.Components;
using Narcolepsy.Core.Components.Http.RequestTabs;
using Narcolepsy.Core.Http;

public class CodeBodyEditor : IBodyEditor {
    public string ContentType { get; }

    public string Name { get; }

    public string LanguageId { get; }

    public CodeBodyEditor(string name, string languageId, string contentType) {
        this.Name = name;
        this.LanguageId = languageId;
        this.ContentType = contentType;
    }

    public RenderFragment RenderWithContext(IHttpRequestContext context) {
        return builder => {
            builder.OpenComponent<CodeBodyEditorView>(0);
            builder.AddAttribute(0, "Context", context);
            builder.AddAttribute(0, "ContentType", ContentType);
            builder.AddAttribute(0, "LanguageId", LanguageId);
            builder.CloseComponent();
        };
    }
}
