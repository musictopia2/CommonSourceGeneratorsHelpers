namespace CommonSourceGeneratorsHelpers;
public interface ICustomProperty
{
    EnumSimpleTypeCategory VariableCustomCategory { get; set; }
    bool Nullable { get; set; }
    string PropertyName { get; set; }
    string ContainingNameSpace { get; set; }
    string UnderlyingSymbolName { get; set; }
}