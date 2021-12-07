namespace CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.HtmlParserClasses;
public class ParserException : Exception
{
    public string OriginalBody { get; set; } = ""; // this is the html text.  there is link.  but that is not correct because its not a link
    public string FirstTag { get; set; } = "";
    public string SecondTag { get; set; } = "";
    public string RemainingHtml { get; set; } = "";
    public EnumMethod Method { get; }
    public ParserException(string message, EnumMethod method) : base(message)
    {
        Method = method;
    }
}