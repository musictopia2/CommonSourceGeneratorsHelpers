namespace CommonBasicLibraries.BasicDataSettingsAndProcesses;
public static class BasicDataFunctions
{
    public delegate Task ActionAsync<in T>(T obj); //this is used so if there is a looping, then it can await it if needed.
    
}