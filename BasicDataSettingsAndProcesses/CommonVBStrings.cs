namespace CommonBasicLibraries.BasicDataSettingsAndProcesses;
public static class VBCompat
{
    public static int AscW(string str)
    {
        return Convert.ToInt32(str[0]);
    }
    public static int AscW(char character)
    {
        return Convert.ToInt32(character);
    }
    public static string ChrW(int key)
    {
        return Convert.ToChar(key).ToString();
    }
}