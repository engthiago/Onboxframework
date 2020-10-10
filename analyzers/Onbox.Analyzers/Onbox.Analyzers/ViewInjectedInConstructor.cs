using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;

namespace Onbox.Analyzers.V7
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ViewInjectedInConstructor : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "OBX5";

        private const string title = "Views should not be injected in the constructor.";
        private const string message = "Views should not be injected in the constructor.";
        private const string description = "WPF will throw an exception if it tries to show this view more than once. Please resolve the view inside a method.";
        private const string category = "Usage";
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, title, message, category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxNodeAction(AnalyserConstructor, SyntaxKind.ConstructorDeclaration);
        }

        private void AnalyserConstructor(SyntaxNodeAnalysisContext context)
        {
            var syntax = context.Node as ConstructorDeclarationSyntax;
            if (syntax != null)
            {
                foreach (var parameter in syntax.ParameterList.Parameters)
                {
                    var currentSymbol = context.SemanticModel.GetSymbolInfo(parameter.Type).Symbol as INamedTypeSymbol;
                    if (currentSymbol != null)
                    {
                        var mvcViewInterface = currentSymbol.AllInterfaces
                            .FirstOrDefault(i => i.Name == "IMvcView" && i.ContainingNamespace.Name.ToString().Contains("Onbox.Mvc"));

                        if (mvcViewInterface != null)
                        {
                            var diagnostic = Diagnostic.Create(Rule, parameter.GetLocation());
                            context.ReportDiagnostic(diagnostic);

                            return;
                        }

                        while (currentSymbol.BaseType != null)
                        {
                            if (currentSymbol.Name == "Window" && currentSymbol.OriginalDefinition.ToString() == "System.Windows.Window")
                            {
                                var diagnostic = Diagnostic.Create(Rule, parameter.GetLocation());
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
