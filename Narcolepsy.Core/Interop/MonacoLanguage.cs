namespace Narcolepsy.Core.Interop;

public record MonacoLanguage(
    string Id,
    string[] Extensions,
    string[] Aliases,
    string[] MimeTypes) {
    // based on monaco editor's monaco.languages.getLanguages()
    private static readonly MonacoLanguage Plaintext =
        new("plaintext", new[] { ".txt" }, new[] { "Plain Text", "text" }, new[] { "text/plain" });

    public static readonly MonacoLanguage[] AllLanguages = {
                                                               MonacoLanguage.Plaintext,
                                                               new("apex", new[] { ".cls" }, new[] { "Apex", "apex" },
                                                                   new[] { "text/x-apex-source", "text/x-apex" }),
                                                               new("coffeescript", new[] { ".coffee" },
                                                                   new[] { "CoffeeScript", "coffeescript", "coffee" },
                                                                   new[] {
                                                                             "text/x-coffeescript", "text/coffeescript"
                                                                         }),
                                                               new("css", new[] { ".css" }, new[] { "CSS", "css" },
                                                                   new[] { "text/css" }),
                                                               new("dart", new[] { ".dart" }, new[] { "Dart", "dart" },
                                                                   new[] { "text/x-dart-source", "text/x-dart" }),
                                                               new("graphql", new[] { ".graphql", ".gql" },
                                                                   new[] { "GraphQL", "graphql", "gql" },
                                                                   new[] { "application/graphql" }),
                                                               new("handlebars", new[] { ".handlebars", ".hbs" },
                                                                   new[] { "Handlebars", "handlebars", "hbs" },
                                                                   new[] { "text/x-handlebars-template" }),
                                                               new("html",
                                                                   new[] {
                                                                             ".html", ".htm", ".shtml", ".xhtml",
                                                                             ".mdoc", ".jsp", ".asp", ".aspx", ".jshtm"
                                                                         }, new[] { "HTML", "htm", "html", "xhtml" },
                                                                   new[] {
                                                                             "text/html", "text/x-jshtm",
                                                                             "text/template", "text/ng-template"
                                                                         }),
                                                               new("java", new[] { ".java", ".jav" },
                                                                   new[] { "Java", "java" },
                                                                   new[] { "text/x-java-source", "text/x-java" }),
                                                               new("javascript",
                                                                   new[] { ".js", ".es6", ".jsx", ".mjs", ".cjs" },
                                                                   new[] { "JavaScript", "javascript", "js" },
                                                                   new[] { "text/javascript" }),
                                                               new("kotlin", new[] { ".kt", ".kts" },
                                                                   new[] { "Kotlin", "kotlin" },
                                                                   new[] { "text/x-kotlin-source", "text/x-kotlin" }),
                                                               new("less", new[] { ".less" }, new[] { "Less", "less" },
                                                                   new[] { "text/x-less", "text/less" }),
                                                               new("liquid", new[] { ".liquid", ".html.liquid" },
                                                                   new[] { "Liquid", "liquid" },
                                                                   new[] { "application/liquid" }),
                                                               new("mips", new[] { ".s" }, new[] { "MIPS", "MIPS-V" },
                                                                   new[] {
                                                                             "text/x-mips", "text/mips",
                                                                             "text/plaintext"
                                                                         }),
                                                               new("pascal", new[] { ".pas", ".p", ".pp" },
                                                                   new[] { "Pascal", "pas" },
                                                                   new[] { "text/x-pascal-source", "text/x-pascal" }),
                                                               new("php",
                                                                   new[] { ".php", ".php4", ".php5", ".phtml", ".ctp" },
                                                                   new[] { "PHP", "php" },
                                                                   new[] { "application/x-php" }),
                                                               new("razor", new[] { ".cshtml" },
                                                                   new[] { "Razor", "razor" },
                                                                   new[] { "text/x-cshtml" }),
                                                               new("scala", new[] { ".scala", ".sc", ".sbt" },
                                                                   new[] {
                                                                             "Scala", "scala", "SBT", "Sbt", "sbt",
                                                                             "Dotty", "dotty"
                                                                         },
                                                                   new[] {
                                                                             "text/x-scala-source", "text/x-scala",
                                                                             "text/x-sbt", "text/x-dotty"
                                                                         }),
                                                               new("scss", new[] { ".scss" },
                                                                   new[] { "Sass", "sass", "scss" },
                                                                   new[] { "text/x-scss", "text/scss" }),
                                                               new("swift", new[] { ".swift" },
                                                                   new[] { "Swift", "swift" }, new[] { "text/swift" }),
                                                               new("twig", new[] { ".twig" }, new[] { "Twig", "twig" },
                                                                   new[] { "text/x-twig" }),
                                                               new("typescript", new[] { ".ts", ".tsx" },
                                                                   new[] { "TypeScript", "ts", "typescript" },
                                                                   new[] { "text/typescript" }),
                                                               new("xml",
                                                                   new[] {
                                                                             ".xml", ".dtd", ".ascx", ".csproj",
                                                                             ".config", ".props", ".targets", ".wxi",
                                                                             ".wxl", ".wxs", ".xaml", ".svg", ".svgz",
                                                                             ".opf", ".xsl"
                                                                         }, new[] { "XML", "xml" },
                                                                   new[] {
                                                                             "text/xml", "application/xml",
                                                                             "application/xaml+xml",
                                                                             "application/xml-dtd"
                                                                         }),
                                                               new("yaml", new[] { ".yaml", ".yml" },
                                                                   new[] { "YAML", "yaml", "YML", "yml" },
                                                                   new[] { "application/x-yaml", "text/x-yaml" }),
                                                               new("json",
                                                                   new[] {
                                                                             ".json", ".bowerrc", ".jshintrc",
                                                                             ".jscsrc", ".eslintrc", ".babelrc", ".har"
                                                                         }, new[] { "JSON", "json" },
                                                                   new[] { "application/json" })
                                                           };

    public string PrimaryAlias => this.Aliases[0];

    public string PrimaryExtension => this.Extensions[0];

    public static MonacoLanguage GetByContentTypeHeader(string? headerValue) =>
        MonacoLanguage.GetByMimeType(headerValue?.Split(';')[0].Trim());

    public static MonacoLanguage GetByMimeType(string? mimeType) =>
        MonacoLanguage.AllLanguages.FirstOrDefault(l =>
            l.MimeTypes.Any(t => t.Equals(mimeType, StringComparison.OrdinalIgnoreCase))) ?? MonacoLanguage.Plaintext;
}