using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Onbox.Analyzers.V7
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AppContainerProvider : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "OBX1";

        private const string title = "Applications must have a Container Provide Attribute";
        private const string messageFormat = "{0} does not contain valid Container Attribute";
        private const string description = "All Onbox Applications should be decorated with ContainerProvider Attribute";
        private const string category = "Usage";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, title, messageFormat, category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.EnableConcurrentExecution();

            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

            if (namedTypeSymbol.TypeKind != TypeKind.Class)
            {
                return;
            }

            if (namedTypeSymbol.BaseType == null)
            {
                return;
            }

            // Check if it inherits from IRevitExternallApp interface
            var revitAppInterface = namedTypeSymbol.AllInterfaces.FirstOrDefault(i => i.Name == "IRevitExternalApp");
            if (revitAppInterface == null)
            {
                return;
            }

            var attributes = namedTypeSymbol.GetAttributes();
            var attribute = attributes.FirstOrDefault(a => a.AttributeClass.Name.Contains("ContainerProvider"));
            if (attribute == null)
            {
                var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
                context.ReportDiagnostic(diagnostic);
            }

        }
    }
}
