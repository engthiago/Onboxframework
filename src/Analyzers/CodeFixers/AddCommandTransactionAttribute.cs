using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace Onbox.Analyzers.V7.CodeFixers
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AddAppContainerProviderAttribute)), Shared]
    public class AddCommandTransactionAttribute : CodeFixProvider
    {
        private const string title = "Add Transaction Attribute";

        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(CommandTransactionDecorator.DiagnosticId); }
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

            var rewriter = new TransactionRewriter();
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


        private class TransactionRewriter : CSharpSyntaxRewriter
        {
            public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
            {
                var declaration = (TypeDeclarationSyntax)base.VisitClassDeclaration(node);

                var transactionAttribute = SyntaxFactory
                    .AttributeList
                    (
                        SyntaxFactory.SingletonSeparatedList
                        (
                            SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("Transaction"))
                            .WithArgumentList
                            (
                                SyntaxFactory.AttributeArgumentList
                                (
                                    SyntaxFactory.SingletonSeparatedList
                                    (
                                        SyntaxFactory.AttributeArgument
                                        (
                                            SyntaxFactory.MemberAccessExpression
                                            (
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName("TransactionMode"),
                                                SyntaxFactory.IdentifierName("Manual")
                                            )
                                        )
                                    )
                                )
                            )
                        )
                    )
                    .NormalizeWhitespace();

                var newDeclaration = declaration.AddAttributeLists(transactionAttribute);
                return newDeclaration;
            }
        }
    }
}
