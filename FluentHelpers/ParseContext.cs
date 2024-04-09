using Microsoft.CodeAnalysis;
namespace CommonSourceGeneratorsHelpers.FluentHelpers;
internal sealed class ParseContext(Compilation compilation, SyntaxNode syntax)
{
    public Compilation Compilation { get; } = compilation;
    public SemanticModel SemanticModel { get; } = syntax.GetSemanticModel(compilation);
}