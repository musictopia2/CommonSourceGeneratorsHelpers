global using AddSourcesHelpers;
using Microsoft.CodeAnalysis;
namespace AddSourcesHelpers;
internal interface IAddSource
{
    void AddSource(string path, string text);
}
internal class IncrementalExecuteAddSource(SourceProductionContext context) : IAddSource
{
    void IAddSource.AddSource(string path, string text)
    {
        context.AddSource(path, text);
    }
}
internal class IncrementalInitAddSource(IncrementalGeneratorPostInitializationContext context) : IAddSource
{
    void IAddSource.AddSource(string path, string text)
    {
        context.AddSource(path, text);
    }
}
internal class OldGeneratorAddSource(GeneratorExecutionContext context) : IAddSource
{
    void IAddSource.AddSource(string path, string text)
    {
        context.AddSource(path, text);
    }
}
internal class OldGeneratorPostInitializationAddSource(GeneratorPostInitializationContext context) : IAddSource
{
    void IAddSource.AddSource(string path, string text)
    {
        context.AddSource(path, text);
    }
}
internal static class Extensions
{
    public static IAddSource CreateCustomSource(this SourceProductionContext source)
    {
        return new IncrementalExecuteAddSource(source);
    }
    public static IAddSource CreateCustomSource(this GeneratorExecutionContext source)
    {
        return new OldGeneratorAddSource(source);
    }
    public static IAddSource CreateCustomSource(this GeneratorPostInitializationContext source)
    {
        return new OldGeneratorPostInitializationAddSource(source);
    }
    public static IAddSource CreateCustomSource(this IncrementalGeneratorPostInitializationContext source)
    {
        return new IncrementalInitAddSource(source);
    }
}