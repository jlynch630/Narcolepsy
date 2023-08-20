namespace Narcolepsy.Core.Renderables.BodyEditors;

using ViewConfig;

public static class BodyEditorExtensions {
    public static IHttpViewConfiguration AddCodeBodyEditor(this IHttpViewConfiguration configuration,
                                                           string name,
                                                           string id,
                                                           string languageId,
                                                           string contentType)
        => configuration.AddRequestBodyEditor(new CodeBodyEditor(name, id, languageId, contentType));
}