namespace CommonBasicLibraries.CollectionClasses;
//still needed listmodifiers because of custom dictionary for game package.
public interface IListModifiers<T>
{
    //this will allow the possibilities of having at least some shared behaviors.
    //this will be used for the game classes  the base class will use blank behaviors (if not the game package).
    void Add(T value);
    void AddRange(IEnumerable<T> thisList); //no need for collection stuff though.
    void LoadStartLists(IEnumerable<T> thisList);
    bool RemoveSpecificItem(T value);
    void ReplaceRange(IEnumerable<T> thisList);
    void ReplaceAllWithGivenItem(T value); //this means that the item that is sent in, the list will be set to the new item.
    void ReplaceItem(T oldItem, T newItem);
    void Clear();
}