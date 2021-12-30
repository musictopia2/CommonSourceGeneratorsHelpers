namespace CommonSourceGeneratorsHelpers;
public static class SourceCodeBuilderExtensions
{
    //i like the idea of doing as extensions and not requiring new members for the interface.
    public static IWriter CustomExceptionLine(this IWriter w, Action<IWriter> message)
    {
        w.Write("throw new ")
        .BasicProcessesWrite()
            .Write("CustomBasicException(")
            .AppendDoubleQuote(message)
            .Write(");");
        return w;
    }
    public static IWriter BasicProcessesWrite(this IWriter w)
    {
        w.GlobalWrite()
            .Write("CommonBasicLibraries.BasicDataSettingsAndProcesses.");
        return w;
    }
    public static IWriter SingleCollectionInfoWrite(this IWriter w, string genericType)
    {
        w.GlobalWrite()
            .Write("CommonBasicLibraries.CollectionClasses.BasicList")
            .SingleGenericWrite(genericType);
        return w;
    }
    public static IWriter SingleGenericWrite(this IWriter w, string genericType)
    {
        w.Write("<")
            .Write(genericType)
            .Write(">");
        return w;
    }
    public static IWriter SingleGenericWrite(this IWriter w, Action<IWriter> action) //this means you could have extras.
    {
        w.Write("<");
        action.Invoke(w);
        w.Write(">");
        return w;
    }
    //this means this can be as complex as you want (i think).
    public static IWriter CustomGenericWrite(this IWriter w, Action<IWriter> action) //this means you could have extras.
    {
        w.SingleGenericWrite(action);
        return w; //give me a choice.
    }
    public static IWriter GlobalWrite(this IWriter w)
    {
        w.Write("global::");
        return w;
    }
    public static IWriter SystemWrite(this IWriter w)
    {
        w.GlobalWrite()
            .Write("System.");
        return w;
    }
    public static IWriter EmptyEqualsEndString(this IWriter w)
    {
        w.Write(" = ")
            .AppendDoubleQuote(w => w.Write(""))
            .Write(";");
        return w;
    }
}
