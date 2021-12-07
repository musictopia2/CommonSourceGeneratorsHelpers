using CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using CommonBasicLibraries.CollectionClasses;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
namespace CommonRoslynExtensionsLibrary;
public static class RoslynExtensions
{
    public static INamedTypeSymbol GetClassSymbol(this Compilation compilation, ClassDeclarationSyntax clazz)
    {
        var model = compilation.GetSemanticModel(clazz.SyntaxTree);
        var classSymbol = model.GetDeclaredSymbol(clazz)!;
        return classSymbol;
    }
    public static INamedTypeSymbol GetRecordSymbol(this Compilation compilation, RecordDeclarationSyntax record)
    {
        var model = compilation.GetSemanticModel(record.SyntaxTree);
        var recordSymbol = model.GetDeclaredSymbol(record)!;
        return recordSymbol;
    }
    public static INamedTypeSymbol GetStructSymbol(this Compilation compilation, StructDeclarationSyntax struz)
    {
        var model = compilation.GetSemanticModel(struz.SyntaxTree);
        var structSymbol = model.GetDeclaredSymbol(struz)!;
        return structSymbol;
    }
    public static string GetAccessModifier(this INamedTypeSymbol symbol)
    {
        return symbol.DeclaredAccessibility.ToString().ToLowerInvariant();
    }
    public static bool IsPartial(this ClassDeclarationSyntax classDeclaration)
    {
        return classDeclaration.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword));
    }
    public static bool IsPartial(this RecordDeclarationSyntax recordDeclaration)
    {
        return recordDeclaration.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword));
    }
    public static bool IsPartial(this StructDeclarationSyntax structDeclaration)
    {
        return structDeclaration.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword));
    }
    public static bool IsMappable(this ClassDeclarationSyntax source) => source.Implements("IMappable");
    public static bool Implements(this ClassDeclarationSyntax source, string interfaceName)
    {
        if (source.BaseList is null)
        {
            return false;
        }
        IEnumerable<BaseTypeSyntax> baseTypes = source.BaseList.Types.Select(baseType => baseType);
        return baseTypes.Any(baseType => baseType.ToString() == interfaceName);
    }
    public static bool IsMappable(this RecordDeclarationSyntax source) => source.Implements("IMappable");
    public static bool Implements(this RecordDeclarationSyntax source, string interfaceName)
    {
        if (source.BaseList is null)
        {
            return false;
        }
        IEnumerable<BaseTypeSyntax> baseTypes = source.BaseList.Types.Select(baseType => baseType);
        return baseTypes.Any(baseType => baseType.ToString() == interfaceName);
    }
    public static bool IsMappable(this StructDeclarationSyntax source) => source.Implements("IMappable");
    public static bool Implements(this StructDeclarationSyntax source, string interfaceName)
    {
        if (source.BaseList is null)
        {
            return false;
        }
        IEnumerable<BaseTypeSyntax> baseTypes = source.BaseList.Types.Select(baseType => baseType);
        return baseTypes.Any(baseType => baseType.ToString() == interfaceName);
    }
    public static bool IsMappable(this ITypeSymbol symbol) => symbol.Implements("IMappable");
    public static bool Implements(this ITypeSymbol symbol, string interfaceName)
    {
        var firsts = symbol.AllInterfaces;
        return firsts.Any(xx => xx.Name == interfaceName);
    }
    public static bool IsCollection(this ITypeSymbol symbol)
    {
        bool rets = symbol.Implements("ICollection");
        return rets;
    }
    public static bool IsCollection(this IPropertySymbol symbol)
    {
        return symbol.Type.IsCollection();
    }
    public static ITypeSymbol? GetCollectionSingleGenericTypeUsed(this ITypeSymbol symbol)
    {
        INamedTypeSymbol? others = symbol as INamedTypeSymbol;
        if (others is null)
        {
            return null;
        }
        if (others.TypeArguments.Count() is not 1)
        {
            return null;
        }
        return others.TypeArguments[0];
    }
    public static ITypeSymbol? GetCollectionSingleGenericTypeUsed(this IPropertySymbol symbol)
    {
        return symbol.Type.GetCollectionSingleGenericTypeUsed();
    }
    public static bool IsSimpleType(this ITypeSymbol symbol)
    {
        if (symbol.Name == "String")
        {
            return true;
        }
        if (symbol.Name == "Nullable")
        {
            return true;
        }
        if (symbol.TypeKind == TypeKind.Enum)
        {
            return true;
        }
        if (symbol.TypeKind == TypeKind.Struct)
        {
            return true;
        }
        return false;
    }
    public static bool IsSimpleType(this IPropertySymbol symbol)
    {
        return symbol.Type.IsSimpleType();
    }
    public static BasicList<IPropertySymbol> GetProperties(this INamedTypeSymbol symbol) => symbol.GetMembers().OfType<IPropertySymbol>().ToBasicList();
    public static BasicList<IPropertySymbol> GetProperties(this INamedTypeSymbol symbol, Func<IPropertySymbol, bool> predicate) => symbol.GetMembers().OfType<IPropertySymbol>().Where(predicate).ToBasicList();
    public static bool TryGetAttribute(this ISymbol symbol, string attributeName, out IEnumerable<AttributeData> attributes)
    {
        string otherName = attributeName.Replace("Attribute", "");
        attributes = symbol.GetAttributes()
            .Where(a => a.AttributeClass is not null && (a.AttributeClass.Name == attributeName || a.AttributeClass.Name == otherName));
        return attributes.Any();
    }
    public static bool TryGetAttribute(this ISymbol symbol, INamedTypeSymbol attributeType, out IEnumerable<AttributeData> attributes)
    {
        attributes = symbol.GetAttributes()
            .Where(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, attributeType));
        return attributes.Any();
    }
    public static bool IsRequiredAttributeUsed(this ISymbol symbol)
    {
        return symbol.HasAttribute("Required");
    }
    public static bool HasAttribute(this ISymbol symbol, INamedTypeSymbol attributeType)
    {
        return symbol.GetAttributes().Any(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, attributeType));
    }
    public static bool HasAttribute(this ISymbol symbol, string attributeName)
    {
        string otherName = attributeName.Replace("Attribute", "");
        return symbol.GetAttributes()
            .Any(a => a.AttributeClass is not null && (a.AttributeClass.Name == attributeName || a.AttributeClass.Name == otherName));
    }
    /// <summary>
    /// This gets the data of the property.  null means not found.
    /// Can be bool, etc.
    /// </summary>
    /// <typeparam name="T">This is the type being returned</typeparam>
    /// <param name="attributes"></param>
    /// <param name="property">All the information about the property so the data can be found.</param>
    /// <returns></returns>
    public static T? AttributePropertyValue<T>(this IEnumerable<AttributeData> attributes, AttributeProperty property)
    {
        AttributeData attribute = attributes.Single();
        return attribute.AttributePropertyValue<T>(property);
        //if there is more than one, can't do this way.
    }
    /// <summary>
    /// This gets the data of a single attribute.  null means not found.
    /// Can be bool, etc.
    /// </summary>
    /// <typeparam name="T">This is the type being returned</typeparam>
    /// <param name="attribute"></param>
    /// <param name="property">All the information about the property so the data can be found.</param>
    /// <returns></returns>
    public static T? AttributePropertyValue<T>(this AttributeData attribute, AttributeProperty property)
    {
        T? output;
        if (property.Index == -1)
        {
            //this means nothing found.  so if not in named arguments, then not found.
            output = (T?)attribute.NamedArguments.FirstOrDefault(xx => xx.Key.Equals(property.Name)).Value.Value;
            if (output is null)
            {
                return default;
            }
            return output;
        }
        output = (T?)attribute.ConstructorArguments[property.Index].Value;
        return output;
    }
    /// <summary>
    /// this is used in order to make the string compatible with c#.  since this is intended to be used for source generation.
    /// often times, will put this data into the code file itself
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public static string GetCSharpString(this string content)
    {
        content = content.Replace("\"", "\"\"");
        return content;
    }
    public static string ChangeCasingForVariable(this string value, EnumVariableCategory category)
    {
        string firsts = value.Substring(0, 1);
        if (firsts == "_" && category == EnumVariableCategory.PrivateFieldParameter)
        {
            return value;
        }
        string rests = value.Substring(1);
        return category switch
        {
            EnumVariableCategory.PrivateFieldParameter => $"_{firsts.ToLower()}{rests}",
            EnumVariableCategory.ParameterCamelCase => $"{firsts.ToLower()}{rests}",
            EnumVariableCategory.PublicPascalCase => $"{firsts.ToUpper()}{rests}",
            _ => throw new Exception("No category chosen"),
        };
    }
    public static string RemoveAttribute(this string content, string attributeName)
    {
        content = content.Replace($"    [{attributeName}]{Environment.NewLine}", "");
        return content;
    }
    public static void ReportPartialClassRequired(this GeneratorExecutionContext context, ClassDeclarationSyntax clazz, string id = "PartialCode")
    {
        string message = $"Partial class was required.  The class name was {clazz.Identifier.Value}";
        context.ReportDiagnostic(Diagnostic.Create(message.ReportError(id), Location.None));
    }
#pragma warning disable RS2008 // Enable analyzer release tracking
    private static DiagnosticDescriptor ReportError(this string errorMessage, string id) => new(id,
#pragma warning restore RS2008 // Enable analyzer release tracking
        "Could not create source generation",
        errorMessage,
        "Error",
        DiagnosticSeverity.Error,
        true
        );

    public static void ReportError(this GeneratorExecutionContext context, string errorMessgae, string id)
    {
        context.ReportDiagnostic(Diagnostic.Create(errorMessgae.ReportError(id), Location.None));
    }
    public static string GetFileNameForCopy(this SyntaxTree tree) //better be safe than sorry.
    {
        var temps = tree.GetCompilationUnitRoot();
        var nexts = temps.Members.OfType<FileScopedNamespaceDeclarationSyntax>().First().Name.ToString();
        string path = tree.FilePath;
        string fileName = Path.GetFileNameWithoutExtension(path);
        string finalName = $"{nexts}.{fileName}.g";
        return finalName;
    }
}