using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Onbox.Analyzers.V7
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CommandTransactionDecorator : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "OBX3";

        private const string title = "Commands should have Transaction Mode Attribute.";
        private const string messageFormat = "{0} does not contain valid transaction attribute.";
        private const string description = "All Revit Commands should be decorated with Transaction Attribute.";
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
            var revitCommandInterface = namedTypeSymbol.AllInterfaces.FirstOrDefault(i => i.Name == "IExternalCommand");
            if (revitCommandInterface == null)
            {
                return;
            }

            var attributes = namedTypeSymbol.GetAttributes();
            var attribute = attributes.FirstOrDefault(a => a.AttributeClass.Name.Contains("Transaction"));
            if (attribute == null)
            {
                var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
                context.ReportDiagnostic(diagnostic);
            }

        }
    }
}
