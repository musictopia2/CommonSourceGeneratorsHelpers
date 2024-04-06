namespace CommonSourceGeneratorsHelpers;
public interface ICustomResult
{
    //decided that there are many common things that all common results should have.
    //they suggest instead of capturing the symbol for processing capturing common information.
    string ClassName { get; set; }
    string Namespace { get; set; }
}