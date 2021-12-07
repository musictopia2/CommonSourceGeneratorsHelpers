using CommonBasicLibraries.BasicDataSettingsAndProcesses;

namespace CommonBasicLibraries.CollectionClasses;
public interface IBasicList<T> : ISimpleList<T>, IListModifiers<T>
{
    //needs this still even with the special collection.  because dictionary needs to inherit from this for its interface.
    T this[int index]
    {
        get;
        set;
    }
    void RemoveAt(int index);
    void RemoveGivenList(IEnumerable<T> thisList); //this means if you have a list and anything on this list needs to be removed, it can be done.
    void RemoveRange(int index, int count);
    void ShuffleList(); //i think we can't go wrong with this.   whether i do lots of copy/paste, or find a routine, does not matter.
    void ShuffleList(int howMany); //this means you only keep a certain number
    T GetRandomItem();
    T GetRandomItem(bool removePrevious);
    void RemoveRandomItems(int howMany);
    IBasicList<T> GetRandomList(bool removePrevious, int howManyInList);
    IBasicList<T> GetRandomList();
    IBasicList<T> GetRandomList(bool removePrevious);
    IBasicList<T> GetConditionalItems(Predicate<T> match); //if you wanted to remove, there is another method that gives you the items and removes.
    void Sort(); //i do want all to have the ability to sort.
                 //that way i can decide whether a behavior does it or if the list does it.
                 //if there is something in common but not completely, then the isort.
                 //otherwise, the abstract class will do it.
    void Sort(Comparison<T> comparison);
    void Sort(int index, int count, IComparer<T> comparer);
    void Sort(IComparer<T> comparer);
    void InsertRange(int index, IEnumerable<T> items);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="match1">This is the criteria for doing all</param>
    /// <param name="AllAction">This will be performed for all matching criteria</param>
    /// <param name="match2">This is the second criteria</param>
    /// <param name="SpecificAction">This is the specific action.  The first match it finds, performs the actions and stops</param>
    void ComplexAction(Predicate<T> match1, Action<T> AllAction, Predicate<T> match2, Action<T> SpecificAction);
    bool ForSpecificItem(Predicate<T> match, Action<T> action, int HowManyToCheck = 0); //if 0, then it means that it won't check any
    int Capacity { get; set; }
    void TrimExcess();
    void InsertMiddle(int index, T value);
    void InsertBeginning(T value); //this means it adds to the beginning.  i like this idea.
    void RemoveOnlyOneAfterAction(Predicate<T> match, Action<T> action);
    void RemoveSeveralConditionalItems(BasicList<ConditionActionPair<T>> ThisList);
    void RemoveAllOnly(Predicate<T> match); //looks like i can't return anything after all.
    IBasicList<U> ConvertAll<U>(Converter<T, U> converter);
    IBasicList<T> RemoveAllAndObtain(Predicate<T> match); //this means whatever the condition is, it removes and you receive it.
    void KeepConditionalItems(Predicate<T> match);
    IBasicList<T> GetRange(int index, int count);
    void MoveItem(T item, int newIndex); //this will move an existing item to a new index.
    void Reverse();
    void RemoveFirstItem();
    void RemoveLastItem();
}