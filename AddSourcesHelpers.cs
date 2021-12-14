global using AddSourcesHelpers;
using Microsoft.CodeAnalysis;
namespace AddSourcesHelpers;
internal interface IAddSource
{
    void AddSource(string path, string text);
}
internal class IncrementalExecuteAddSource : IAddSource
{
    private readonly SourceProductionContext _context;
    public IncrementalExecuteAddSource(SourceProductionContext context)
    {
        _context = context;
    }
    void IAddSource.AddSource(string path, string text)
    {
        _context.AddSource(path, text);
    }
}
internal class IncrementalInitAddSource : IAddSource
{
    private readonly IncrementalGeneratorPostInitializationContext _context;
    public IncrementalInitAddSource(IncrementalGeneratorPostInitializationContext context)
    {
        _context = context;
    }
    void IAddSource.AddSource(string path, string text)
    {
        _context.AddSource(path, text);
    }
}
internal class OldGeneratorAddSource : IAddSource
{
    private readonly GeneratorExecutionContext _context;
    public OldGeneratorAddSource(GeneratorExecutionContext context)
    {
        _context = context;
    }
    void IAddSource.AddSource(string path, string text)
    {
        _context.AddSource(path, text);
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
    public static IAddSource CreateCustomSource(this IncrementalGeneratorPostInitializationContext source)
    {
        return new IncrementalInitAddSource(source);
    }
}