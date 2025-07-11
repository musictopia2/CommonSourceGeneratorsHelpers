using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;

namespace CommonSourceGeneratorsHelpers;
internal static class IncrementalGeneratorInitializationContextExtensions
{
    public static IncrementalValueProvider<ImmutableArray<TResult>> RunFilteredGenerator<TResult>(
       this IncrementalGeneratorInitializationContext context,
       string baseClassFullName,
       string boolPropertyName,
       bool defaultBoolValue,
       Func<ImmutableHashSet<ClassDeclarationSyntax>, Compilation, ImmutableHashSet<TResult>> processClasses)
    {

        // 1. Find candidate classes by syntax (quick syntax filter)
        IncrementalValuesProvider<ClassDeclarationSyntax> candidates = context.SyntaxProvider.CreateSyntaxProvider(
            predicate: (node, _) => IsCandidateSyntax(node, baseClassFullName),
            transform: (ctx, _) => ctx.Node as ClassDeclarationSyntax)
            .Where(cls => cls != null)!;

        // 2. Combine compilation and collected candidates
        var combined = context.CompilationProvider.Combine(candidates.Collect());

        // 3. Filter classes based on bool property, then run processClasses delegate
        var filteredResults = combined.SelectMany((compAndClasses, _) =>
        {
            Compilation compilation = compAndClasses.Left;
            ImmutableArray<ClassDeclarationSyntax> classesArray = compAndClasses.Right;
            var classesSet = classesArray.ToImmutableHashSet();

            var filteredClasses = FilterByBoolProperty(classesSet, compilation, boolPropertyName, defaultBoolValue);

            return processClasses(filteredClasses, compilation);
        });

        // 4. Collect all results as ImmutableArray<TResult>
        return filteredResults.Collect();
    }
    private static bool IsCandidateSyntax(SyntaxNode node, string baseClassFullName)
    {
        if (node is ClassDeclarationSyntax cls && cls.BaseList != null)
        {
            // Simple textual check in base list
            return cls.BaseList.ToString().Contains(baseClassFullName);
        }
        return false;
    }

    private static ImmutableHashSet<ClassDeclarationSyntax> FilterByBoolProperty(
        ImmutableHashSet<ClassDeclarationSyntax> classes,
        Compilation compilation,
        string boolPropertyName,
        bool defaultBoolValue)
    {
        var builder = ImmutableHashSet.CreateBuilder<ClassDeclarationSyntax>();

        foreach (var cls in classes)
        {
            var model = compilation.GetSemanticModel(cls.SyntaxTree);
            if (model.GetDeclaredSymbol(cls) is not INamedTypeSymbol symbol)
            {
                continue;
            }

            var boolProp = symbol.GetMembers()
                .OfType<IPropertySymbol>()
                .FirstOrDefault(p => p.Name == boolPropertyName);

            bool propValue;

            if (boolProp == null)
            {
                propValue = defaultBoolValue;
            }
            else
            {
                bool? val = TryGetBoolPropertyLiteralValue(cls, boolProp);
                propValue = val ?? defaultBoolValue;
            }

            if (propValue == defaultBoolValue)
            {
                builder.Add(cls);
            }
        }

        return builder.ToImmutable();
    }

    private static bool? TryGetBoolPropertyLiteralValue(
        ClassDeclarationSyntax classDecl,
        IPropertySymbol propSymbol)
    {
        var propDecl = classDecl.Members
            .OfType<PropertyDeclarationSyntax>()
            .FirstOrDefault(p => p.Identifier.Text == propSymbol.Name);

        if (propDecl?.ExpressionBody != null)
        {
            var expr = propDecl.ExpressionBody.Expression;
            if (expr is LiteralExpressionSyntax literal)
            {
                if (literal.IsKind(SyntaxKind.TrueLiteralExpression))
                {
                    return true;
                }

                if (literal.IsKind(SyntaxKind.FalseLiteralExpression))
                {
                    return false;
                }
            }
        }
        else if (propDecl?.AccessorList != null)
        {
            var getter = propDecl.AccessorList.Accessors
                .FirstOrDefault(a => a.Kind() == SyntaxKind.GetAccessorDeclaration);

            if (getter?.Body != null)
            {
                var retStmt = getter.Body.Statements
                    .OfType<ReturnStatementSyntax>()
                    .FirstOrDefault();

                if (retStmt?.Expression is LiteralExpressionSyntax lit)
                {
                    if (lit.IsKind(SyntaxKind.TrueLiteralExpression))
                    {
                        return true;
                    }

                    if (lit.IsKind(SyntaxKind.FalseLiteralExpression))
                    {
                        return false;
                    }
                }
            }
        }

        return null;
    }
}