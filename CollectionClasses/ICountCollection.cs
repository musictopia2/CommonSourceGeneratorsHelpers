namespace CommonBasicLibraries.CollectionClasses;
//this is needed so the contact manager can cast something for getting count alone.
public interface ICountCollection
{
    int Count { get; }
}