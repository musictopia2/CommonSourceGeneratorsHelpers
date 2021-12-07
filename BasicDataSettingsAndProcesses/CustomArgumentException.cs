namespace CommonBasicLibraries.BasicDataSettingsAndProcesses;
public class CustomArgumentException : CustomBasicException
{
    public CustomArgumentException() : base() { }
    public CustomArgumentException(string message) : base(message) { }
    public CustomArgumentException(string message, Exception innerexception) : base(message, innerexception) { }
    public virtual string ParamName { get; } = "";
    public CustomArgumentException(string paramName, string message) : base(message)
    {
        ParamName = paramName;
    }
    public CustomArgumentException(string paramName, string message, Exception innerexception) : base(message, innerexception)
    {
        ParamName = paramName;
    }
    public override string Message => $"Custom Argument Exception.  Message Is {base.Message}";
}