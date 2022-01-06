using Microsoft.CodeAnalysis;
namespace CommonSourceGeneratorsHelpers.FluentHelpers;
internal sealed class ParseContext
{
    public Compilation Compilation { get; }
    public SemanticModel SemanticModel { get; }
    public ParseContext(Compilation compilation, SyntaxNode syntax)
    {
        Compilation = compilation;
        SemanticModel = syntax.GetSemanticModel(compilation);
    }
}