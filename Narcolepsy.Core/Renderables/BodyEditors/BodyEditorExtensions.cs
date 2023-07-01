namespace Narcolepsy.Core.Renderables.BodyEditors;

using ViewConfig;

internal static class BodyEditorExtensions {
    public static IHttpViewConfiguration AddCodeBodyEditor(this IHttpViewConfiguration configuration, string name,
                                                           string languageId, string contentType)
        => configuration.AddRequestBodyEditor(new CodeBodyEditor(name, languageId, contentType));
}