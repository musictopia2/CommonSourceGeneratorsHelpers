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
        //has to be this way now for classes so it can implement generic interfaces.
        if (source.BaseList is null)
        {
            return false;
        }
        IEnumerable<BaseTypeSyntax> baseTypes = source.BaseList.Types.Select(baseType => baseType);
        foreach (BaseTypeSyntax baseType in baseTypes)
        {
            if (baseType.Type is GenericNameSyntax gg && gg.Identifier.ValueText == interfaceName)
            {
                return true;
            }
            else if (baseType.Type is IdentifierNameSyntax identifierName && identifierName.Identifier.ValueText == interfaceName)
            {
                return true;
            }
        }
        return false;
    }
    public static BasicList<INamedTypeSymbol> GetGenericSymbolsImplemented(this INamedTypeSymbol symbol, string interfaceName)
    {
        var list = symbol.AllInterfaces;
        BasicList<INamedTypeSymbol> output = new();
        foreach (var item in list)
        {
            if (item.IsGenericType && item.TypeArguments.Count() == 1 && item.Name == interfaceName)
            {
                output.Add((INamedTypeSymbol)item.TypeArguments.Single());
            }
        }
        return output;
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
    //had to rename because there can be cases like nullable where we need to get the generic type even if not a collection type.
    public static ITypeSymbol? GetSingleGenericTypeUsed(this ITypeSymbol symbol)
    {
        if (symbol is not INamedTypeSymbol others)
        {
            return null;
        }
        if (others.TypeArguments.Count() is not 1)
        {
            return null;
        }
        return others.TypeArguments[0];
    }
    public static ITypeSymbol? GetSingleGenericTypeUsed(this IPropertySymbol symbol)
    {
        return symbol.Type.GetSingleGenericTypeUsed();
    }
    public static bool IsNullable(this IPropertySymbol symbol) => symbol.Type.IsNullable();
    public static bool IsNullable(this ITypeSymbol symbol) => symbol.Name == "Nullable";
    public static bool IsKnownType(this IPropertySymbol symbol) => symbol.Type.IsKnownType();
    public static bool IsKnownType(this ITypeSymbol symbol)
    {
        if (symbol.TypeKind == TypeKind.Enum)
        {
            return true;
        }
        if (symbol.Name.StartsWith("Enum"))
        {
            return true; //act like its known (no need to scan through to support my custom stuff
        }
        HashSet<string> names = new()
        {
            "String", "Nullable", "Decimal", "Byte",
            "Double", "Int16", "Int32", "Int64",
            "SByte", "Single", "UInt16", "UInt32",
            "UInt64", "Boolean", "Char", "Object",
            "Guid","DateTime", "DateTimeOffset",
            "DateOnly", "TimeOnly"
        };
        return names.Contains(symbol.Name);
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
    public static BasicList<IPropertySymbol> GetRequiredProperties(this ITypeSymbol classSymbol)
    {
        var output = classSymbol.GetMembers().OfType<IPropertySymbol>().Where(xx => xx.IsRequiredAttributeUsed()).ToBasicList();
        return output;
    }
    public static BasicList<IPropertySymbol> GetPropertiesWithAttribute(this ITypeSymbol classSymbol, string attributeName)
    {
        var output = classSymbol.GetMembers().OfType<IPropertySymbol>().Where(xx => xx.HasAttribute(attributeName)).ToBasicList();
        return output;
    }
    public static BasicList<IPropertySymbol> GetRequiredProperties(this INamedTypeSymbol classSymbol)
    {
        var output = classSymbol.GetMembers().OfType<IPropertySymbol>().Where(xx => xx.IsRequiredAttributeUsed()).ToBasicList();
        return output;
    }
    public static BasicList<IPropertySymbol> GetPropertiesWithAttribute(this INamedTypeSymbol classSymbol, string attributeName)
    {
        var output = classSymbol.GetMembers().OfType<IPropertySymbol>().Where(xx => xx.HasAttribute(attributeName)).ToBasicList();
        return output;
    }
    public static BasicList<IPropertySymbol> GetProperties(this ITypeSymbol symbol) => symbol.GetMembers().OfType<IPropertySymbol>().ToBasicList();
    public static BasicList<IPropertySymbol> GetProperties(this ITypeSymbol symbol, Func<IPropertySymbol, bool> predicate) => symbol.GetMembers().OfType<IPropertySymbol>().Where(predicate).ToBasicList();

    public static BasicList<IPropertySymbol> GetProperties(this INamedTypeSymbol symbol) => symbol.GetMembers().OfType<IPropertySymbol>().ToBasicList();
    public static BasicList<IPropertySymbol> GetProperties(this INamedTypeSymbol symbol, Func<IPropertySymbol, bool> predicate) => symbol.GetMembers().OfType<IPropertySymbol>().Where(predicate).ToBasicList();
    public static bool TryGetAttribute(this ISymbol symbol, string attributeName, out IEnumerable<AttributeData> attributes)
    {
        if (attributeName.EndsWith("Attribute") == false)
        {
            attributeName = $"{attributeName}Attribute";
        }
        BasicList<AttributeData> output = new();
        foreach (var attribute in symbol.GetAttributes())
        {
            if (attribute.AttributeClass is null)
            {
                continue;
            }
            if(attribute.AttributeClass.Name == attributeName)
            {
                output.Add(attribute);
            }
        }
        //trying a faster method.
        attributes = output;
        return output.Count > 0;
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
        if (attributeName.EndsWith("Attribute") == false)
        {
            attributeName = $"{attributeName}Attribute";
        }
        foreach (var attribute in symbol.GetAttributes())
        {
            if (attribute.AttributeClass is null)
            {
                continue;
            }
            if (attribute.AttributeClass.Name == attributeName)
            {
                return true;
            }
        }
        //trying a faster method.  because this has to be as fast as possible.
        return false;
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
            if (attribute.NamedArguments.Count() == 0)
            {
                return default;
            }
            //this means nothing found.  so if not in named arguments, then not found.
            output = (T?)attribute.NamedArguments.FirstOrDefault(xx => xx.Key.Equals(property.Name)).Value.Value;
            if (output is null)
            {
                return default;
            }
            return output;
        }
        if (attribute.ConstructorArguments.Count() - 1 < property.Index)
        {
            //i think best to do this way so if there are more than one required, won't also cause problems.
            return default;
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
    private static DiagnosticDescriptor ReportError(this string errorMessage, string id) => new(id,
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
    public static BasicList<ClassDeclarationSyntax> GetClasses(this SyntaxTree tree)
    {
        var unit = tree.GetCompilationUnitRoot();
        var firsts = unit.Members.OfType<FileScopedNamespaceDeclarationSyntax>().ToList();
        if (firsts.Count == 0)
        {
            return new();
        }
        if (firsts.Count > 1)
        {
            return new();
        }
        return firsts.Single().Members.OfType<ClassDeclarationSyntax>().ToBasicList();
    }
    /// <summary>
    /// This gets all the members of a certain type from the syntax tree after the filescope namespace.  
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tree"></param>
    /// <returns></returns>
    public static BasicList<T> GetMembersOfSpecificType<T>(this SyntaxTree tree)
    {
        var unit = tree.GetCompilationUnitRoot();
        var firsts = unit.Members.OfType<FileScopedNamespaceDeclarationSyntax>().ToList();
        if (firsts.Count == 0)
        {
            return new();
        }
        if (firsts.Count > 1)
        {
            return new();
        }
        return firsts.Single().Members.OfType<T>().ToBasicList();
    }
    /// <summary>
    /// this returns all members after the filescoped namespace
    /// </summary>
    /// <param name="tree"></param>
    /// <returns></returns>
    public static BasicList<MemberDeclarationSyntax> GetAllMembers(this SyntaxTree tree)
    {
        var unit = tree.GetCompilationUnitRoot();
        var firsts = unit.Members.OfType<FileScopedNamespaceDeclarationSyntax>().ToList();
        if (firsts.Count == 0)
        {
            return new();
        }
        if (firsts.Count > 1)
        {
            return new();
        }
        return firsts.Single().Members.ToBasicList();
    }
    public static string AppendCodeText(this ClassDeclarationSyntax ourClass, string syntaxText, string additionalText)
    {
        string firsts = ourClass.ToString();
        string extras = additionalText;
        int startAt = firsts.IndexOf("{");
        int endAt = firsts.LastIndexOf("}");
        string toReplace = firsts.Substring(startAt, endAt - startAt - 1);
        string newText = $"{toReplace}{extras}";
        string results = firsts.Replace(toReplace, newText);
        syntaxText = syntaxText.Replace(firsts, results);
        return syntaxText;
    }
    //extra extensions to help with incremental source generators.
    public static INamedTypeSymbol GetClassSymbol(this GeneratorSyntaxContext context, ClassDeclarationSyntax clazz)
    {
        return context.SemanticModel.GetDeclaredSymbol(clazz)!;
    }
    public static INamedTypeSymbol GetRecordSymbol(this GeneratorSyntaxContext context, RecordDeclarationSyntax record)
    {
        return context.SemanticModel.GetDeclaredSymbol(record)!;
    }
    public static ClassDeclarationSyntax GetClassNode(this GeneratorSyntaxContext context)
    {
        return (ClassDeclarationSyntax)context.Node;
    }
    public static RecordDeclarationSyntax GetRecordNode(this GeneratorSyntaxContext context)
    {
        return (RecordDeclarationSyntax)context.Node;
    }
    public static StructDeclarationSyntax GetStructNode(this GeneratorSyntaxContext context)
    {
        return (StructDeclarationSyntax)context.Node;
    }
    public static bool IsPublic(this ClassDeclarationSyntax clazz)
    {
        foreach (var m in clazz.Modifiers)
        {
            if (m.IsKind(SyntaxKind.PublicKeyword))
            {
                return true;
            }
        }
        return false;
    }
    public static bool IsPublic(this RecordDeclarationSyntax record)
    {
        foreach (var m in record.Modifiers)
        {
            if (m.IsKind(SyntaxKind.PublicKeyword))
            {
                return true;
            }
        }
        return false;
    }
    public static bool IsPublic(this StructDeclarationSyntax strucz)
    {
        foreach (var m in strucz.Modifiers)
        {
            if (m.IsKind(SyntaxKind.PublicKeyword))
            {
                return true;
            }
        }
        return false;
    }
    public static bool IsReadOnly(this RecordDeclarationSyntax recordDeclaration)
    {
        if (recordDeclaration.IsKind(SyntaxKind.ReadOnlyKeyword))
        {
            return true;
        }
        foreach (var m in recordDeclaration.Modifiers)
        {
            if (m.IsKind(SyntaxKind.ReadOnlyKeyword))
            {
                return true;
            }
        }
        return false;
    }
    public static bool IsReadOnly(this StructDeclarationSyntax s)
    {
        if (s.IsKind(SyntaxKind.ReadOnlyKeyword))
        {
            return true;
        }
        foreach (var m in s.Modifiers)
        {
            if (m.IsKind(SyntaxKind.ReadOnlyKeyword))
            {
                return true;
            }
        }
        return false;
    }
    public static bool IsRecordStruct(this RecordDeclarationSyntax record)
    {
        if (record.IsKind(SyntaxKind.RecordStructDeclaration))
        {
            return true;
        }
        foreach (var m in record.Modifiers)
        {
            if (m.IsKind(SyntaxKind.RecordStructDeclaration))
            {
                return true;
            }    
        }
        return false;
    }
    //new extensions.  this helps with getting all public properties and methods.
    private static IEnumerable<INamedTypeSymbol> GetAllParents(this INamedTypeSymbol symbol)
    {
        BasicList<INamedTypeSymbol> output = new();
        output.Add(symbol);
        INamedTypeSymbol temps = symbol;
        do
        {
            if (temps.BaseType is null)
            {
                return output;
            }
            temps = temps.BaseType;
            if (temps.Name == "Object")
            {
                return output;
            }
            output.Add(temps);
        } while (true);
    }
    public static BasicList<IPropertySymbol> GetAllPublicProperties(this ITypeSymbol symbol)
    {
        INamedTypeSymbol other = (INamedTypeSymbol)symbol;
        return other.GetAllPublicProperties();
    }
    public static BasicList<IPropertySymbol> GetAllPublicProperties(this INamedTypeSymbol symbol)
    {
        //since everything goes back to object, once something inherits from object, then that is all of them.
        //also, since the inheritance is only single inheritance, then should be okay.
        var symbols = symbol.GetAllParents();
        BasicList<IPropertySymbol> output = new();
        foreach (var s in symbols)
        {
            var list = s.GetMembers().OfType<IPropertySymbol>().Where(xx => xx.DeclaredAccessibility == Accessibility.Public);
            output.AddRange(list);
        }
        return output;
    }
    public static BasicList<IPropertySymbol> GetAllPublicProperties(this ITypeSymbol symbol, string attributeName)
    {
        INamedTypeSymbol other = (INamedTypeSymbol)symbol;
        return other.GetAllPublicProperties(attributeName);
    }
    public static BasicList<IPropertySymbol> GetAllPublicProperties(this INamedTypeSymbol classSymbol, string attributeName) //this means if i have attribute, hopefully its obvious i only want properties with specific attribute.
    {
        var output = classSymbol.GetAllPublicProperties().Where(xx => xx.HasAttribute(attributeName)).ToBasicList();
        return output;
    }
    public static BasicList<IMethodSymbol> GetAllPublicMethods(this ITypeSymbol symbol, string attributeName)
    {
        INamedTypeSymbol other = (INamedTypeSymbol)symbol;
        return other.GetAllPublicMethods(attributeName);
    }
    public static BasicList<IMethodSymbol> GetAllPublicMethods(this INamedTypeSymbol symbol)
    {
        var symbols = symbol.GetAllParents();
        BasicList<IMethodSymbol> output = new();
        foreach (var s in symbols)
        {
            var list = s.GetMembers().OfType<IMethodSymbol>().Where(xx => xx.DeclaredAccessibility == Accessibility.Public && xx.MethodKind == MethodKind.Ordinary);
            output.AddRange(list);
        }
        return output;
    }
    //decided to add support for getting sematicmodel for the class its going through.  since microsoft thought it was fine to use do it that way, try that way.
    public static SemanticModel GetSemanticModel(this SyntaxNode node, Compilation compilation)
    {
        var firsts = node.FirstAncestorOrSelf<CompilationUnitSyntax>();
        return compilation.GetSemanticModel(firsts!.SyntaxTree);
    }
    public static string GetStringFromList(this IEnumerable<string> list) => string.Join(Environment.NewLine, list);
}