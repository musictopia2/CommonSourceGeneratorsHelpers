using CommonBasicLibraries.CollectionClasses;

namespace CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.RandomGenerator;
//this is so others has the flexibility of custom data so i don't have to have as large list which makes file sizes much larger
/// <summary>
/// this allows the possibility of somebody providing their own data that would be used for the random functions.
/// </summary>
public interface IRandomData
{
    BasicList<string> FirstNamesMale { get; }
    BasicList<string> FirstNamesFemale { get; }
    BasicList<string> LastNames { get; }
    BasicList<string> ColorNames { get; }
    BasicList<FullNameClass> FullNames { get; } //in this case, has to populate the class.  will have suggested full names.  however, since there is one interface, you have unlimited flexibility of how you will implement this.
    BasicList<CityStateClass> Cities { get; } //this means the state has to match as well.
}