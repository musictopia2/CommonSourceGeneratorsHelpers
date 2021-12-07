namespace CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.RandomGenerator;
public static class CreditCardUtils
{
    /// <summary>
    /// Calculates checksum for credit card number using Luhn Algorithm.
    /// </summary>
    /// <param name="num">Credit card number.</param>
    /// <returns>Returns a number representing credit card number checksum.</returns>
    public static int LuhnCalcualte(long num)
    {
        var digits = num.ToString().ToCharArray().Reverse().ToList();
        var sum = 0;
        for (int i = 0, l = digits.Count; l > i; ++i)
        {
            var digit = int.Parse(digits[i].ToString());
            if (i % 2 == 0)
            {
                digit *= 2;
                if (digit > 9)
                {
                    digit -= 9;
                }
            }

            sum += digit;
        }

        return sum * 9 % 10;
    }
    /// <summary>
    /// Checks whether the given credit card number is valid.
    /// </summary>
    /// <param name="num">Credit card number.</param>
    /// <returns>Returns "true" if credit card number is valid; False otherwise.</returns>
    public static bool LuhnCheck(long num)
    {
        var str = num.ToString();
        var checkDigit = int.Parse(str[str.Length - 1].ToString());
        return checkDigit == LuhnCalcualte(long.Parse(str.Substring(0, str.Length - 1)));
    }
}