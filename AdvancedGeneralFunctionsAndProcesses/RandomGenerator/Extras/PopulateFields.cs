using CommonBasicLibraries.CollectionClasses;
namespace CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.RandomGenerator;
public partial class RandomGenerator
{
    public static bool UseFullName { get; set; } //if true, then will make sure you get real names.
    public BasicList<ITestPerson> GetTestPeopleList<T>(int HowMany, EnumAgeRanges DefaultAge = EnumAgeRanges.Adult) where T : ITestPerson, new()
    {
        BasicList<ITestPerson> output = new();
        for (int i = 0; i < HowMany; i++)
        {
            output.Add(GetTestSinglePerson<T>(DefaultAge));
        }
        return output;
    }
    public ITestPerson GetTestSinglePerson<T>(EnumAgeRanges defaultAge = EnumAgeRanges.Adult) where T : ITestPerson, new()
    {
        ITestPerson output = new T();

        if (UseFullName)
        {
            FullNameClass full = NextFullName();
            output.FirstName = full.FirstName;
            output.LastName = full.LastName;
        }
        else
        {
            output.FirstName = NextAnyName();
            output.LastName = NextLastName();
        }
        BasicList<CityStateClass> cities = _data.Cities;
        CityStateClass chosen = cities.GetRandomItem();
        output.City = chosen.City;
        output.State = chosen.StateAbb;
        output.PostalCode = NextZipCode();
        output.Address = NextAddress();
        output.IsActive = NextBool(70); //wants to lean towards active
        output.CreditCardNumber = NextCreditCardNumber();
        output.Age = NextAge(defaultAge);
        output.SSN = NextSSN();
        output.EmailAddress = NextEmail();
        return output;
    }
}