namespace CommonBasicLibraries.BasicDataSettingsAndProcesses;
public class CustomBasicException : Exception
{
    public CustomBasicException() : base() { }
    public CustomBasicException(string message) : base(message) { }

    public CustomBasicException(string message, Exception innerexception) : base(message, innerexception) { }  //i think it needs to be flexible enough to not just do my custom exception.

    public override string Message => $"Custom Basic Exception.  Message Is {base.Message}";
}