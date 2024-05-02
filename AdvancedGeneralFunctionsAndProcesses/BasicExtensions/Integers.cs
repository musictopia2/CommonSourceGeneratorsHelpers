using CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.Misc;
using CommonBasicLibraries.BasicDataSettingsAndProcesses;
using CommonBasicLibraries.CollectionClasses;
namespace CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
public static class Integers
{
    public static string Join(this BasicList<int> thisList, string delimiter)
    {
        StrCat cats = new();
        thisList.ForEach(x => cats.AddToString(x.ToString(), delimiter));
        return cats.GetInfo();
    }
    public static (int Batches, int LeftOver) GetRemainderInfo(this int thisInt, int batchSize)
    {
        int x = 0;
        int b = 0;
        for (int i = 1; i < thisInt; i++)
        {
            x += 1;
            if (x == batchSize)
            {
                x = 0;
                b++;
            }
        }
        return (b, x);
    }
    public static string ConvertToSpecificStrings(this int thisInt, int desiredDigits)
    {
        string temps = thisInt.ToString();
        if (temps.Length > desiredDigits)
        {
            throw new CustomBasicException($"The Integer Of {thisInt} has more digits than the desired digits of {desiredDigits}");
        }
        if (temps.Length == desiredDigits)
        {
            return temps;
        }
        int padding = desiredDigits - temps.Length;
        StrCat cats = new();
        for (int i = 0; i < padding; i++)
        {
            cats.AddToString("0");
        }
        cats.AddToString(temps);
        return cats.GetInfo();
    }
    public static void Times(this int @this, Action action)
    {
        for (var i = 0; i < @this; i++)
        {
            action?.Invoke();
        }
    }
    public static void Times(this int @this, Action<int> action)
    {
        for (var i = 0; i < @this; i++)
        {
            action?.Invoke(i + 1);
        }
    }
}