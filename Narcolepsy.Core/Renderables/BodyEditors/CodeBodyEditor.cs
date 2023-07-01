namespace Narcolepsy.Core.Renderables.BodyEditors;

using Components.Http.RequestTabs;
using Http;
using Microsoft.AspNetCore.Components;

public class CodeBodyEditor : IBodyEditor {
    public CodeBodyEditor(string name, string languageId, string contentType) {
        this.Name = name;
        this.LanguageId = languageId;
        this.ContentType = contentType;
    }

    public string ContentType { get; }

    public string LanguageId { get; }

    public string Name { get; }

    public RenderFragment RenderWithContext(IHttpRequestContext context) {
        return builder => {
            builder.OpenComponent<CodeBodyEditorView>(0);
            builder.AddAttribute(0, "Context", context);
            builder.AddAttribute(0, "ContentType", this.ContentType);
            builder.AddAttribute(0, "LanguageId", this.LanguageId);
            builder.CloseComponent();
        };
    }
}