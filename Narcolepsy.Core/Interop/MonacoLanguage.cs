namespace Narcolepsy.Core.Interop {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;

    public record MonacoLanguage(
        string Id,
        string[] Extensions,
        string[] Aliases,
        string[] MimeTypes) {
        private static readonly MonacoLanguage Plaintext =
            new MonacoLanguage("plaintext", new string[] { ".txt" }, new string[] { "Plain Text", "text" }, new string[] { "text/plain" });

        // based on monaco editor's monaco.languages.getLanguages()
        public static readonly MonacoLanguage[] AllLanguages = new MonacoLanguage[] {
            MonacoLanguage.Plaintext,
            new MonacoLanguage("apex", new string[] { ".cls" }, new string[] { "Apex", "apex" }, new string[] { "text/x-apex-source", "text/x-apex" }),
            new MonacoLanguage("coffeescript", new string[] { ".coffee" }, new string[] { "CoffeeScript", "coffeescript", "coffee" }, new string[] { "text/x-coffeescript", "text/coffeescript" }),
            new MonacoLanguage("css", new string[] { ".css" }, new string[] { "CSS", "css" }, new string[] { "text/css" }),
            new MonacoLanguage("dart", new string[] { ".dart" }, new string[] { "Dart", "dart" }, new string[] { "text/x-dart-source", "text/x-dart" }),
            new MonacoLanguage("graphql", new string[] { ".graphql", ".gql" }, new string[] { "GraphQL", "graphql", "gql" }, new string[] { "application/graphql" }),
            new MonacoLanguage("handlebars", new string[] { ".handlebars", ".hbs" }, new string[] { "Handlebars", "handlebars", "hbs" }, new string[] { "text/x-handlebars-template" }),
            new MonacoLanguage("html", new string[] { ".html", ".htm", ".shtml", ".xhtml", ".mdoc", ".jsp", ".asp", ".aspx", ".jshtm" }, new string[] { "HTML", "htm", "html", "xhtml" }, new string[] { "text/html", "text/x-jshtm", "text/template", "text/ng-template" }),
            new MonacoLanguage("java", new string[] { ".java", ".jav" }, new string[] { "Java", "java" }, new string[] { "text/x-java-source", "text/x-java" }),
            new MonacoLanguage("javascript", new string[] { ".js", ".es6", ".jsx", ".mjs", ".cjs" }, new string[] { "JavaScript", "javascript", "js" }, new string[] { "text/javascript" }),
            new MonacoLanguage("kotlin", new string[] { ".kt", ".kts" }, new string[] { "Kotlin", "kotlin" }, new string[] { "text/x-kotlin-source", "text/x-kotlin" }),
            new MonacoLanguage("less", new string[] { ".less" }, new string[] { "Less", "less" }, new string[] { "text/x-less", "text/less" }),
            new MonacoLanguage("liquid", new string[] { ".liquid", ".html.liquid" }, new string[] { "Liquid", "liquid" }, new string[] { "application/liquid" }),
            new MonacoLanguage("mips", new string[] { ".s" }, new string[] { "MIPS", "MIPS-V" }, new string[] { "text/x-mips", "text/mips", "text/plaintext" }),
            new MonacoLanguage("pascal", new string[] { ".pas", ".p", ".pp" }, new string[] { "Pascal", "pas" }, new string[] { "text/x-pascal-source", "text/x-pascal" }),
            new MonacoLanguage("php", new string[] { ".php", ".php4", ".php5", ".phtml", ".ctp" }, new string[] { "PHP", "php" }, new string[] { "application/x-php" }),
            new MonacoLanguage("razor", new string[] { ".cshtml" }, new string[] { "Razor", "razor" }, new string[] { "text/x-cshtml" }),
            new MonacoLanguage("scala", new string[] { ".scala", ".sc", ".sbt" }, new string[] { "Scala", "scala", "SBT", "Sbt", "sbt", "Dotty", "dotty" }, new string[] { "text/x-scala-source", "text/x-scala", "text/x-sbt", "text/x-dotty" }),
            new MonacoLanguage("scss", new string[] { ".scss" }, new string[] { "Sass", "sass", "scss" }, new string[] { "text/x-scss", "text/scss" }),
            new MonacoLanguage("swift", new string[] { ".swift" }, new string[] { "Swift", "swift" }, new string[] { "text/swift" }),
            new MonacoLanguage("twig", new string[] { ".twig" }, new string[] { "Twig", "twig" }, new string[] { "text/x-twig" }),
            new MonacoLanguage("typescript", new string[] { ".ts", ".tsx" }, new string[] { "TypeScript", "ts", "typescript" }, new string[] { "text/typescript" }),
            new MonacoLanguage("xml", new string[] { ".xml", ".dtd", ".ascx", ".csproj", ".config", ".props", ".targets", ".wxi", ".wxl", ".wxs", ".xaml", ".svg", ".svgz", ".opf", ".xsl" }, new string[] { "XML", "xml" }, new string[] { "text/xml", "application/xml", "application/xaml+xml", "application/xml-dtd" }),
            new MonacoLanguage("yaml", new string[] { ".yaml", ".yml" }, new string[] { "YAML", "yaml", "YML", "yml" }, new string[] { "application/x-yaml", "text/x-yaml" }),
            new MonacoLanguage("json", new string[] { ".json", ".bowerrc", ".jshintrc", ".jscsrc", ".eslintrc", ".babelrc", ".har" }, new string[] { "JSON", "json" }, new string[] { "application/json" })
         };

        public string PrimaryExtension => this.Extensions[0];

        public string PrimaryAlias => this.Aliases[0];

        public static MonacoLanguage GetByContentTypeHeader(string? headerValue) =>
            MonacoLanguage.GetByMimeType(headerValue?.Split(';')[0].Trim());

        public static MonacoLanguage GetByMimeType(string? mimeType) => MonacoLanguage.AllLanguages.FirstOrDefault(l => l.MimeTypes.Any(t => t.Equals(mimeType, StringComparison.OrdinalIgnoreCase))) ?? MonacoLanguage.Plaintext;
    }
}
