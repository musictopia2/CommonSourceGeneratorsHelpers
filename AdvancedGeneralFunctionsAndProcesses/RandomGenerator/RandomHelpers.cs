using CommonBasicLibraries.BasicDataSettingsAndProcesses;
namespace CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.RandomGenerator;
public static class RandomHelpers
{
    private static IRandomGenerator? _rs;
    private static IRandomData? _data;
    public static void SetUpRandom(IRandomGenerator random)
    {
        _rs = random;
    }
    public static void SetUpData(IRandomData data)
    {
        _data = data; //it should be okay to be static.
    }
    public static IRandomGenerator GetRandomGenerator()
    {
        if (_rs == null)
        {
            if (_data is null)
            {
                throw new CustomBasicException("You must figure out the irandomdata");
            }
            _rs = new RandomGenerator(_data);
        }
        return _rs;
    }
    
}