using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
namespace CommonSourceGeneratorsHelpers.FluentHelpers;
internal struct CallInfo
{
    public InvocationExpressionSyntax Invocation;
    public IMethodSymbol MethodSymbol;
    public ArgumentListSyntax ArgumentList;
}