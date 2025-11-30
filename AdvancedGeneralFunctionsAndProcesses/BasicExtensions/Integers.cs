using CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.Misc;
using CommonBasicLibraries.BasicDataSettingsAndProcesses;
using CommonBasicLibraries.CollectionClasses;
namespace CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
public static class Integers
{
    extension(BasicList<int> list)
    {
        public string Join(string delimiter)
        {
            StrCat cats = new();
            list.ForEach(x => cats.AddToString(x.ToString(), delimiter));
            return cats.GetInfo();
        }
    }
    extension(int payload)
    {
        public (int Batches, int LeftOver) GetRemainderInfo(int batchSize)
        {
            int x = 0;
            int b = 0;
            for (int i = 1; i < payload; i++)
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
        public string ConvertToSpecificStrings(int desiredDigits)
        {
            string temps = payload.ToString();
            if (temps.Length > desiredDigits)
            {
                throw new CustomBasicException($"The Integer Of {payload} has more digits than the desired digits of {desiredDigits}");
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
        public void Times(Action action)
        {
            for (var i = 0; i < payload; i++)
            {
                action?.Invoke();
            }
        }
        public void Times(Action<int> action)
        {
            for (var i = 0; i < payload; i++)
            {
                action?.Invoke(i + 1);
            }
        }
    }   
}