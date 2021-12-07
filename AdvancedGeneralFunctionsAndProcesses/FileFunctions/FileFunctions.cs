namespace CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.FileFunctions;
public static class FileFunctions
{ 
    public static string DirectoryName(string directoryPath)
    {
        var thisItem = new DirectoryInfo(directoryPath);
        return thisItem.Name;
    }
    public static string FullFile(string filePath)
    {
        var thisItem = new FileInfo(filePath);
        return thisItem.Name;
    }
    public static string FileName(string filePath)
    {
        var thisItem = new FileInfo(filePath);
        var thisName = thisItem.Name;
        return thisName.Substring(0, thisName.Length - thisItem.Extension.Length);
    }
}