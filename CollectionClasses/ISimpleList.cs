using static CommonBasicLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions; //this is still needed though because its completely static.
namespace CommonBasicLibraries.CollectionClasses;
//there are some processes that still need this interface (sometimes game package needs to do functions on top of it.
public interface ISimpleList<T> : IEnumerable<T>, ICountCollection
{
    Task ForEachAsync(ActionAsync<T> action);
    //its possible for there to be an await.
    void ForEach(Action<T> action);
    void ForConditionalItems(Predicate<T> match, Action<T> action);
    Task ForConditionalItemsAsync(Predicate<T> match, ActionAsync<T> action);
    bool Exists(Predicate<T> match);
    bool Contains(T item);
    T? Find(Predicate<T> match); //the first one it finds, will be this one.
    T FindOnlyOne(Predicate<T> match); //this means it must find only one item.
                                       //this can be iffy.
    IBasicList<T> FindAll(Predicate<T> match);
    int FindFirstIndex(Predicate<T> match);
    int FindFirstIndex(int startIndex, Predicate<T> match);
    int FindFirstIndex(int startIndex, int count, Predicate<T> match);
    T? FindLast(Predicate<T> match);
    int FindLastIndex(Predicate<T> match);
    int FindLastIndex(int startIndex, Predicate<T> match);
    int FindLastIndex(int startIndex, int count, Predicate<T> match);
    int HowMany(Predicate<T> match);
    int IndexOf(T value);
    int IndexOf(T value, int Index);
    int IndexOf(T value, int Index, int Count);
    bool TrueForAll(Predicate<T> match); //i like this idea
}