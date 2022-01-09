using CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
namespace CommonSourceGeneratorsHelpers.FluentHelpers;
internal class ParseUtils
{
    private static readonly List<CallInfo> EmptyCallsList = new();
    internal static IReadOnlyList<CallInfo> FindCallsOfMethodWithName(
        ParseContext context, SyntaxNode node, string methodName)
    {
        // Oh man getting all fluent calls of a method is really not that easy... try to use the symbol to find them
        // The invocation expression often span multiple calls when using fluent syntax...
        var callInfos = new List<CallInfo>();
        var identifierNodes = node.DescendantNodes()
            .OfType<SimpleNameSyntax>()
            .Where(sns => sns.Identifier.ValueText == methodName);
        foreach (var ins in identifierNodes)
        {
            var ci = CreateCallInfo(context, ins);

            if (ci.HasValue)
            {
                callInfos.Add(ci.Value);
            }
        }

        return callInfos;
    }
    private static CallInfo? CreateCallInfo(ParseContext context, SimpleNameSyntax nameSyntax)
    {
        if (nameSyntax.Parent is null)
        {
            return null;
        }
        if (nameSyntax.Parent.Parent is null)
        {
            return null;
        }
        if (nameSyntax.Parent.Parent is not InvocationExpressionSyntax invocation)
        {
            return null;
        }
        try
        {
            var method = TryGetMethodSymbolInfo<IMethodSymbol>(context, nameSyntax);
            var argList = invocation.ArgumentList;

            if (method == null)
            {
                return null;
            }
            return new CallInfo()
            {
                Invocation = invocation,
                MethodSymbol = method,
                ArgumentList = argList,
            };
        }
        catch
        {
            return null;
        }
    }
    internal static IReadOnlyList<CallInfo> FindCallsOfMethodInConfigLambda(
        ParseContext context,
        CallInfo call,
        string name,
        bool optional = false,
        int argumentIndex = 0 //to support more than one argument.
        )
    {
        if (optional && call.ArgumentList.Arguments.Count <= 0)
        {
            return EmptyCallsList;
        }
        int toUse;
        if (call.ArgumentList.Arguments.Count() < argumentIndex + 1)
        {
            toUse = call.ArgumentList.Arguments.Count - 1;
        }
        else
        {
            toUse = argumentIndex;
        }
        var configLambda = call.ArgumentList.Arguments[toUse];
        return FindCallsOfMethodWithName(context, configLambda, name);
}
    internal static string GetStringContent(IReadOnlyList<CallInfo> list, int index = 0) //to get the string content as well since this is how its done.
    {
        var a = list.Single().ArgumentList;
        int toUse;
        if (a.Arguments.Count < index + 1)
        {
            toUse = a.Arguments.Count - 1;
        }
        else
        {
            toUse = index;
        }
        var b = a.Arguments[toUse];
        var w = b.DescendantNodes().OfType<LiteralExpressionSyntax>().SingleOrDefault();
        if (w is null)
        {
            return "";
        }
        else
        {
            return w.Token.ValueText;
        }
        //this means i can get any information i want.  can parse to something else if i want.
    }
    static T? TryGetMethodSymbolInfo<T>(ParseContext context, SyntaxNode n) where T : class, ISymbol
    {
        try
        {
            var item = context.SemanticModel.GetSymbolInfo(n).Symbol;
            return context.SemanticModel.GetSymbolInfo(n).Symbol as T;
        }
        catch
        {
            return default;
        }
    }
}