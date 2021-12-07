global using CommonRoslynExtensionsLibrary;
namespace CommonRoslynExtensionsLibrary;
/// <summary>
/// this gives the information necessary to get the values of the property
/// </summary>
/// <param name="Name">Property Name</param>
/// <param name="Index">Index Of The Required Constructor (-1 means not required)</param>
public record struct AttributeProperty(string Name, int Index);