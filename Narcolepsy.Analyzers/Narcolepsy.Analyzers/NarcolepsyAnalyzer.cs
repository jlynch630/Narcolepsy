namespace Narcolepsy.Analyzers {
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Threading;

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class NarcolepsyAnalyzer : DiagnosticAnalyzer {
        public const string DiagnosticId = "NLY0001";

        private const string Category = "Serialization";

        private static readonly DiagnosticDescriptor SerializationRule = new DiagnosticDescriptor(
            DiagnosticId, 
            "IRequestContext Save function must call IContextStore.Put", 
            "{0}.Save must call IContextStore.Put", 
            Category, 
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true, 
            description: "The Save function of every request context must call IContextStore.Put in order to store save data");

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(NarcolepsyAnalyzer.SerializationRule);

        public override void Initialize(AnalysisContext context) {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            // TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
            // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context) {
            IMethodSymbol Method = (IMethodSymbol)context.Symbol;

            if (false) {
                // For all such symbols, produce a diagnostic.
                var diagnostic = Diagnostic.Create(NarcolepsyAnalyzer.SerializationRule, Method.Locations[0], Method.Name);

                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
