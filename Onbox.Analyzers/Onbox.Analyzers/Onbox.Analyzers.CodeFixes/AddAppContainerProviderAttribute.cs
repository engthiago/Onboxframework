using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using Microsoft.CodeAnalysis.Formatting;

namespace Onbox.Analyzers.V7.CodeFixers
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AddAppContainerProviderAttribute)), Shared]
    public class AddAppContainerProviderAttribute : CodeFixProvider
    {
        private const string title = "Add Container Provider";

        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(AppContainerProvider.DiagnosticId); }
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            // Find the type declaration identified by the diagnostic.
            var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<TypeDeclarationSyntax>().First();

            var rewriter = new ContainerRewriter();
            var newNode = rewriter.Visit(declaration);

            newNode = root.ReplaceNode(declaration, newNode);
            newNode = Formatter.Format(newNode, new AdhocWorkspace());

            var document = context.Document.WithSyntaxRoot(newNode);

            context.RegisterCodeFix(
                CodeAction.Create(
                    title,
                    createChangedDocument: c => Task.FromResult(document),
                    equivalenceKey: title),
                    diagnostic);
        }

        private class ContainerRewriter : CSharpSyntaxRewriter
        {
            public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
            {
                var declaration = (TypeDeclarationSyntax)base.VisitClassDeclaration(node);

                var guid = Guid.NewGuid().ToString();

                var containerProviderAttribute = SyntaxFactory
                    .AttributeList
                    (
                        SyntaxFactory.SingletonSeparatedList
                        (
                            SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("ContainerProvider"))
                            .WithArgumentList
                            (
                                SyntaxFactory.AttributeArgumentList
                                (
                                    SyntaxFactory.SingletonSeparatedList
                                    (
                                        SyntaxFactory.AttributeArgument
                                        (
                                            SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(guid))
                                        )
                                    )
                                )
                            )
                        )
                    )
                    .NormalizeWhitespace();

                var newDeclaration = declaration.AddAttributeLists(containerProviderAttribute);
                return newDeclaration;
            }
        }
    }
}
