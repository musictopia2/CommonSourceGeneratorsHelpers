using Microsoft.CodeAnalysis;
//can't use global namespaces.   otherwise, the getclasssymbol does not work properly.
namespace HelpersExperiment.HelpersGeneratorLibrary;
[Generator] //this is important so it knows this class is a generator which will generate code for a class using it.
public class MySourceGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        context.BuildSourceCode();

    }
    public void Initialize(GeneratorInitializationContext context)
    {
    }
}