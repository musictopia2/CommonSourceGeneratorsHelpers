namespace CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.HtmlParserClasses;
public class MissingTags : Exception
{
    public EnumLocation Location { get; }
    public override string Message
    {
        get
        {
            if (Location == EnumLocation.Beginning)
            {
                return "Must have the start tag filled out first";
            }
            if (Location == EnumLocation.Ending)
            {
                return "Must have the end tag filled out first";
            }
            throw new Exception("Can't figure out the message");
        }
    }
    public MissingTags(EnumLocation tagLocation)
    {
        Location = tagLocation;
    }
}