namespace CommonBasicLibraries.CollectionClasses;
public class BlankListBehavior<T> : IListModifiers<T>
{
    public void Add(T value) { }
    public void AddRange(IEnumerable<T> thisList)
    {
        //i am violating some princples.  but if i don't this time, its going to be too complex.
    }
    public void Clear() { }
    public void LoadStartLists(IEnumerable<T> thisList) { }
    public bool RemoveSpecificItem(T value)
    {
        return true;
    }
    public void ReplaceAllWithGivenItem(T value) { }
    public void ReplaceItem(T oldItem, T newItem) { }
    public void ReplaceRange(IEnumerable<T> thisList) { }
}