using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Onbox.Analyzers.V7
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CircularClassReferenceAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "OBX2";

        private const string title = "Circular class reference detected";
        private const string messageFormat = "Circular class Reference on: {0}{1}";
        private const string description = "To fix this issue, please remove the argument from the high level class constructor";
        private const string category = "Usage";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, title, messageFormat, category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.EnableConcurrentExecution();

            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
        }

        public class SymbolData
        {
            public ISymbol Argument { get; set; }
            public ISymbol ArgumentType { get; set; }
        }

        private void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var types = new List<SymbolData>();
            var symbol = (INamedTypeSymbol)context.Symbol;

            if (symbol.TypeKind != TypeKind.Class)
            {
                return;
            }

            var constructor = symbol.Constructors.FirstOrDefault();
            if (constructor == null)
            {
                return;
            }

            foreach (var arg in constructor.Parameters.Select(a => a.OriginalDefinition))
            {
                if (ConstructorContains(symbol, arg, context, types))
                {
                    ReportError(symbol, context, types);
                }
            }

        }

        private void ReportError(INamedTypeSymbol target, SymbolAnalysisContext context, List<SymbolData> types)
        {
            var errorPath = types.Select(s => $"->{s.ArgumentType.Name}").Aggregate((i, j) => i + j);
            var diagnostic = Diagnostic.Create(Rule, types[0].Argument.Locations[0], target.Name, errorPath);
            context.ReportDiagnostic(diagnostic);
        }

        private bool ConstructorContains(INamedTypeSymbol target, ISymbol symbol, SymbolAnalysisContext context, List<SymbolData> types)
        {
            if ((symbol as IParameterSymbol)?.Type.Kind != SymbolKind.NamedType)
            {
                types.Clear();
                return false;
            }

            var namedSymbol = (symbol as IParameterSymbol).Type as INamedTypeSymbol;
            types.Add(new SymbolData { Argument = symbol, ArgumentType = namedSymbol });

            if (target.ContainingNamespace.Name == namedSymbol.ContainingNamespace.Name)
            {
                if (target.Name == namedSymbol.Name)
                {
                    return true;
                }
            }

            var constructor = namedSymbol.Constructors.FirstOrDefault();
            if (constructor == null)
            {
                types.Clear();
                return false;
            }

            foreach (var arg in constructor.Parameters.Select(a => a.OriginalDefinition))
            {
                if (ConstructorContains(target, arg, context, types))
                {
                    return true;
                }
            }

            types.Clear();
            return false;
        }
    }
}
