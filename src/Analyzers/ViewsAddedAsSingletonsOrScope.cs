using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace Onbox.Analyzers.V7
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ViewsAddedAsSingletonsOrScope : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "OBX4";

        private const string title = "Views should not be added as Singleton or Scoped dependencies.";
        private const string message = "WPF View added as Singleton or Scope.";
        private const string description = "WPF will throw an exception when trying to show this view more than once. Please replace by the AddTransient<> approach.";
        private const string category = "Usage";
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, title, message, category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxNodeAction(AnalyzeContainer, SyntaxKind.ExpressionStatement);
        }

        private void AnalyzeContainer(SyntaxNodeAnalysisContext context)
        {
            var syntax = context.Node as ExpressionStatementSyntax;
            if (syntax != null)
            {
                var symbolInfo = context.SemanticModel.GetSymbolInfo(syntax.Expression);
                var symbol = symbolInfo.Symbol;
                if (symbol != null
                    && symbol.Kind == SymbolKind.Method
                    && symbol.ContainingNamespace.ToString().Contains("Onbox.Abstractions.")
                    && symbol.Name.ToString() == "AddSingleton" || symbol.Name.ToString() == "AddScoped")
                {
                    if (symbol is IMethodSymbol methodSymbol)
                    {
                        if (!methodSymbol.TypeArguments.IsEmpty)
                        {
                            foreach (var argumentSymbol in methodSymbol.TypeArguments)
                            {
                                var currentSymbol = argumentSymbol;
                                while (currentSymbol.BaseType != null)
                                {
                                    if (currentSymbol.Name == "Window" && currentSymbol.OriginalDefinition.ToString() == "System.Windows.Window")
                                    {
                                        var diagnostic = Diagnostic.Create(Rule, syntax.GetLocation());
                                        context.ReportDiagnostic(diagnostic);

                                        return;
                                    }
                                    currentSymbol = currentSymbol.BaseType;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
