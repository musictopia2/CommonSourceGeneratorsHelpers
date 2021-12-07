namespace CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.RandomGenerator;
public class DateUtils //needs to be public because of unit testing
{
    /// <summary>
    /// Converts <see cref="DateTime"/> object to Unix timestamp.
    /// </summary>
    /// <param name="dateTime"><see cref="DateTime"/> to convert.</param>
    /// <returns>Returns Unix timestamp from given <see cref="DateTime"/> object.</returns>
    public static double DateTimeToUnixTimestamp(DateTime dateTime)
    {
        var unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        var unixTimeStampInTicks = (dateTime.ToUniversalTime() - unixStart).Ticks;
        return (double)unixTimeStampInTicks / TimeSpan.TicksPerSecond;
    }
    /// <summary>
    /// Converts Unix timestamp to the <see cref="DateTime"/> object.
    /// </summary>
    /// <param name="unixTime">Unix timestamp.</param>
    /// <returns>Returns Unix timestamp converted to the <see cref="DateTime"/> object.</returns>
    public static DateTime UnixTimestampToDateTime(double unixTime)
    {
        var unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        var unixTimeStampInTicks = (long)(unixTime * TimeSpan.TicksPerSecond);
        return new DateTime(unixStart.Ticks + unixTimeStampInTicks, DateTimeKind.Utc);
    }
}