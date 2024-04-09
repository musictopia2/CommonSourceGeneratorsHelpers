using Microsoft.CodeAnalysis;
using SourceGeneratorsAttributesLibrary;
//can't use global namespaces.   otherwise, the getclasssymbol does not work properly.
namespace HelpersExperiment.HelpersGeneratorLibrary;
[Generator] //this is important so it knows this class is a generator which will generate code for a class using it.
public class MySourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(c =>
        {
            c.CreateCustomSource().BuildSourceCode();
        });
    }
}