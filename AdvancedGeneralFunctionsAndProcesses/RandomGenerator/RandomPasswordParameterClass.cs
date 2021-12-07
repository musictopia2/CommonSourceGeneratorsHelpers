using CommonBasicLibraries.CollectionClasses;

namespace CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.RandomGenerator;
public class RandomPasswordParameterClass
{
    public int Digits { get; set; } = 10;
    public int UpperCases { get; set; } = 3;
    public int LowerCases { get; set; } = 3;
    public int HowManyNumbers { get; set; } = 3;
    public int HowManySymbols { get; set; } = 1;
    public BasicList<string> SymbolList { get; set; } = new BasicList<string>() { "!", "\"", "#", "@", "%", "&", "*", "(", ")", ",", ";", ".", "$", "*" };
    public bool EliminateSimiliarCharacters { get; set; } = true; // sometimes needs to not have that because too confusing otherwise. first thought it was 0s but also looks like 1, i and l are the same thing
    public bool IsValid()
    {
        if (UpperCases + LowerCases + HowManyNumbers + HowManySymbols != Digits)
        {
            return false;// because must be equal
        }
        if (HowManySymbols > 0 && SymbolList.Count == 0)
        {
            return false;// because if you require symbols, needs to send symbol list
        }
        if (UpperCases < 0 || LowerCases < 0 || HowManyNumbers < 0 || HowManySymbols < 0)
        {
            return false;
        }
        if (SymbolList.Count > 0 && HowManySymbols == 0)
        {
            return false;
        }
        return true;
    }
}