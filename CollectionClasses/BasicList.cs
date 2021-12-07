using CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.RandomGenerator;
using CommonBasicLibraries.BasicDataSettingsAndProcesses;
using System.Collections;
using static CommonBasicLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;

namespace CommonBasicLibraries.CollectionClasses;
//has to implement the ilist after all now.
public class BasicList<T> : IEnumerable<T>, IListModifiers<T>, ICountCollection, ISimpleList<T>, IBasicList<T>, IList<T> //needs inheritance still because game package needs it.
{
    protected List<T> PrivateList;
    public BasicList() //try this to make it supported by the system.text.json serializer system.
    {
        PrivateList = new(5);
        LoadBehavior();
    }
    public BasicList(int initCapacity = 5)
    {

        PrivateList = new(initCapacity);
        LoadBehavior();
    }
    public BasicList(IEnumerable<T> list)
    {
        if (list == null)
        {
            throw new CustomArgumentException();
        }
        PrivateList = new List<T>(list.Count()); //telling them we know what to start with if sending a new list.
        CopyFrom(list);
        LoadBehavior();
        Behavior!.LoadStartLists(list);
    }
    protected bool IsStart = true;
    protected IListModifiers<T>? Behavior;
    protected virtual void LoadBehavior()
    {
        Behavior = new BlankListBehavior<T>(); //so inherited version can load a different behavior.
    }
    protected void CopyFrom(IEnumerable<T> collection)
    {
        IList<T> items = PrivateList;
        if (collection != null && items != null)
        {
            using IEnumerator<T> enumerator = collection.GetEnumerator();
            while (enumerator.MoveNext())
            {
                items.Add(enumerator.Current);
            }
        }
    }
    public int Capacity { get => PrivateList.Capacity; set => PrivateList.Capacity = value; }
    public void TrimExcess()
    {
        PrivateList.TrimExcess();
    }
    public T this[int index]
    {
        get { return PrivateList[index]; }
        set
        {
            if (index < 0 || index >= PrivateList.Count)
            {
                throw new CustomArgumentException("Index", "When setting custom collection, out of range");
            }
            PrivateList[index] = value;
        }
    }
    public int Count => PrivateList.Count;
    bool ICollection<T>.IsReadOnly { get; }
    internal IRandomGenerator? _rs;
    public void Add(T value)
    {
        PrivateList.Add(value);
        Behavior!.Add(value);
    }
    public void AddRange(IEnumerable<T> thisList)
    {
        PrivateList.AddRange(thisList);
        Behavior!.AddRange(thisList);
    }
    public void Clear()
    {
        PrivateList.Clear();
        Behavior!.Clear();
    }
    public bool Contains(T item)
    {
        return PrivateList.Contains(item);
    }
    public bool Exists(Predicate<T> match)
    {
        return PrivateList.Exists(match);
    }
    public T? Find(Predicate<T> match)
    {
        return PrivateList.Find(match);
    }
    public IBasicList<T> FindAll(Predicate<T> match)
    {
        BasicList<T> output = new();
        foreach (T thisItem in PrivateList)
        {
            if (match(thisItem) == true)
            {
                output.Add(thisItem);
            }
        }
        return output;
    }
    public T FindOnlyOne(Predicate<T> match)
    {
        var thisList = FindAll(match);
        if (thisList.Count > 1)
        {
            throw new CustomBasicException("Found more than one item using FindOnlyOne");
        }
        if (thisList.Count == 0)
        {
            throw new CustomBasicException("Did not find any items using FindOnlyOne");
        }
        return thisList.Single();
    }
    public int FindFirstIndex(Predicate<T> match)
    {
        return PrivateList.FindIndex(match);
    }
    public int FindFirstIndex(int startIndex, Predicate<T> match)
    {
        return PrivateList.FindIndex(startIndex, match);
    }
    public int FindFirstIndex(int startIndex, int count, Predicate<T> match)
    {
        return PrivateList.FindIndex(startIndex, count, match);
    }
    public T? FindLast(Predicate<T> match)
    {
        return PrivateList.FindLast(match);
    }
    public int FindLastIndex(Predicate<T> match)
    {
        return PrivateList.FindLastIndex(match);
    }
    public int FindLastIndex(int startIndex, Predicate<T> match)
    {
        return PrivateList.FindLastIndex(startIndex, match);
    }
    public int FindLastIndex(int startIndex, int count, Predicate<T> match)
    {
        return PrivateList.FindLastIndex(startIndex, count, match);
    }
    public int LastIndexOf(T thisItem)
    {
        return PrivateList.LastIndexOf(thisItem);
    }
    public void ForEach(Action<T> action)
    {
        PrivateList.ForEach(action);
    }
    public bool ForSpecificItem(Predicate<T> match, Action<T> action, int howManyToCheck = 0)
    {
        int privateCheck;
        if (howManyToCheck == 0)
        {
            privateCheck = PrivateList.Count;
        }
        else
        {
            privateCheck = howManyToCheck;
        }
        int privateCount = 0;
        foreach (T thisItem in PrivateList)
        {
            privateCount++;
            if (privateCount > privateCheck)
            {
                break;
            }
            if (match.Invoke(thisItem) == true)
            {
                action.Invoke(thisItem);
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="match1">This is the criteria for doing all</param>
    /// <param name="allAction">This will be performed for all matching criteria</param>
    /// <param name="match2">This is the second criteria</param>
    /// <param name="specificAction">This is the specific action.  The first match it finds, performs the actions and stops</param>
    public void ComplexAction(Predicate<T> match1, Action<T> allAction, Predicate<T> match2, Action<T> specificAction)
    {
        foreach (T thisItem in PrivateList)
        {
            if (match1.Invoke(thisItem) == true)
                allAction.Invoke(thisItem);
        }
        foreach (T thisItem in PrivateList)
        {
            if (match1.Invoke(thisItem) == true && match2.Invoke(thisItem) == true)
            {
                specificAction.Invoke(thisItem);
                return;
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="match">Condition that must be matched in order to do something</param>
    /// <param name="action">Action to perform on the conditional items</param>
    public void ForConditionalItems(Predicate<T> match, Action<T> action)
    {
        foreach (T thisItem in PrivateList)
        {
            if (match.Invoke(thisItem) == true)
                action.Invoke(thisItem);
        }
    }
    public async Task ForConditionalItemsAsync(Predicate<T> match, ActionAsync<T> action)
    {
        foreach (T thisItem in PrivateList)
        {
            if (match.Invoke(thisItem) == true)
                await action.Invoke(thisItem);
        }
    }
    public async Task ForEachAsync(ActionAsync<T> action)
    {
        foreach (T thisItem in PrivateList)
        {
            await action.Invoke(thisItem);
        }
    }
    public IEnumerator<T> GetEnumerator()
    {
        return PrivateList.GetEnumerator();
    }
    public T GetRandomItem()
    {
        return GetRandomItem(false);
    }
    public T GetRandomItem(bool removePrevious)
    {
        _rs = RandomHelpers.GetRandomGenerator();
        int ask1 = _rs.GetRandomNumber(PrivateList.Count);
        T output = PrivateList[ask1 - 1];
        if (removePrevious)
        {
            RemoveItem(ask1 - 1);
        }
        return output;
    }
    public int GetSeed()
    {
        _rs = RandomHelpers.GetRandomGenerator();
        return _rs!.GetSeed();
    }
    public IBasicList<T> GetRandomList(bool removePrevious, int howManyInList)
    {
        _rs = RandomHelpers.GetRandomGenerator();
        BasicList<int> rList = _rs.GenerateRandomList(PrivateList.Count, howManyInList);
        BasicList<T> output = new();
        foreach (var index in rList)
        {
            output.Add(PrivateList[index - 1]);
        }
        if (removePrevious == false)
        {
            return output;
        }
        RemoveGivenList(output);
        return output;
    }
    public void RemoveRandomItems(int howMany)
    {
        _rs = RandomHelpers.GetRandomGenerator();
        BasicList<int> rList = _rs.GenerateRandomList(PrivateList.Count, howMany);
        List<T> list = new();
        foreach (int index in rList)
        {
            list.Add(PrivateList[index - 1]);
        }
        RemoveGivenList(list);
    }
    public IBasicList<T> GetConditionalItems(Predicate<T> match)
    {
        BasicList<T> output = new();
        foreach (var item in PrivateList)
        {
            if (match.Invoke(item))
            {
                output.Add(item);
            }
        }
        return output;
    }
    public IBasicList<T> GetRandomList()
    {
        return GetRandomList(false, PrivateList.Count);
    }
    public IBasicList<T> GetRandomList(bool removePrevious)
    {
        return GetRandomList(removePrevious, PrivateList.Count);
    }
    public IBasicList<T> GetRange(int index, int count)
    {
        BasicList<T> output = new();
        for (int i = index; i < count; i++)
        {
            output.Add(PrivateList[i]);
        }
        return output;
    }
    public int IndexOf(T value)
    {
        return PrivateList.IndexOf(value);
    }
    public int IndexOf(T value, int index)
    {
        return PrivateList.IndexOf(value, index);
    }
    public int IndexOf(T value, int index, int count)
    {
        return PrivateList.IndexOf(value, index, count);
    }
    public void InsertBeginning(T value)
    {
        InsertItem(0, value);
    }
    public void InsertMiddle(int index, T value)
    {
        InsertItem(index, value);
    }
    private void InsertItem(int index, T value)
    {
        PrivateList.Insert(index, value);
        Behavior!.Add(value);
    }
    public IBasicList<T> RemoveAllAndObtain(Predicate<T> match)
    {
        BasicList<T> output = new();
        foreach (var item in PrivateList)
        {
            if (match(item))
            {
                output.Add(item);
            }
        }
        if (output.Count > 0)
        {
            RemoveGivenList(output);
        }
        return output;
    }
    public void RemoveAllOnly(Predicate<T> match)
    {
        List<T> tempList = new();
        foreach (T item in PrivateList)
        {
            if (match(item) == true)
            {
                tempList.Add(item);
            }
        }
        if (tempList.Count > 0)
        {
            RemoveGivenList(tempList);
        }
    }
    public void KeepConditionalItems(Predicate<T> match)
    {
        List<T> tempList = new();
        foreach (T item in PrivateList)
        {
            if (match(item) == false)
            {
                tempList.Add(item);
            }
        }
        if (tempList.Count > 0)
        {
            RemoveGivenList(tempList);
        }
    }
    public void RemoveAt(int index)
    {
        RemoveItem(index);
    }
    public void RemoveFirstItem()
    {
        RemoveItem(0);
    }
    private void RemoveItem(int index)
    {
        T oldItem = PrivateList[index];
        PrivateList.RemoveAt(index);
        Behavior!.RemoveSpecificItem(oldItem);
    }
    public void RemoveGivenList(IEnumerable<T> list)
    {
        if (list == null)
        {
            throw new ArgumentNullException(nameof(list));
        }
        foreach (var item in list)
        {
            PrivateList.Remove(item);
            Behavior!.RemoveSpecificItem(item);
        }
    }
    public void RemoveLastItem()
    {
        RemoveItem(PrivateList.Count - 1);
    }
    public void RemoveRange(int index, int count)
    {
        List<T> newList = PrivateList.GetRange(index, count);
        RemoveGivenList(newList);
    }
    public bool RemoveSpecificItem(T value)
    {
        int index = PrivateList.IndexOf(value);
        if (index == -1)
        {
            return false;
        }
        RemoveItem(index);
        return true;
    }
    public void ReplaceAllWithGivenItem(T value)
    {
        PrivateList.Clear();
        Behavior!.Clear();
        Add(value);
    }
    public void ReplaceItem(T oldItem, T newItem)
    {
        int index = PrivateList.IndexOf(oldItem);
        RemoveItem(index);
        InsertMiddle(index, newItem);
    }
    public void Reverse()
    {
        PrivateList.Reverse();
    }
    public void ShuffleList()
    {
        if (Count == 0)
        {
            return;
        }
        _rs = RandomHelpers.GetRandomGenerator();
        BasicList<int> thisList = _rs.GenerateRandomList(PrivateList.Count);
        List<T> rList = new();
        foreach (int index in thisList)
        {
            rList.Add(PrivateList[index - 1]); //because 0 based.
        }
        PrivateList.Clear();
        PrivateList.AddRange(rList);
    }
    public void ShuffleList(int howMany)
    {
        _rs = RandomHelpers.GetRandomGenerator();
        BasicList<int> thisList = _rs.GenerateRandomList(PrivateList.Count, howMany);
        List<T> rList = new();
        foreach (int index in thisList)
        {
            rList.Add(PrivateList[index - 1]);
        }
        PrivateList.Clear();
        InsertRange(0, rList);
    }
    public void Sort()
    {
        PrivateList.Sort();
    }
    public void Sort(Comparison<T> comparison)
    {
        PrivateList.Sort(comparison);
    }
    public void Sort(int index, int count, IComparer<T> comparer)
    {
        PrivateList.Sort(index, count, comparer);
    }
    //for icomparer, 1 means greater than.  -1 means less than.  0 means equal.
    public void Sort(IComparer<T> comparer)
    {
        PrivateList.Sort(comparer);
    }
    public bool TrueForAll(Predicate<T> match)
    {
        return PrivateList.TrueForAll(match);
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return PrivateList.GetEnumerator();
    }
    public void LoadStartLists(IEnumerable<T> thisList)
    {
        CopyFrom(thisList);
    }
    public void InsertRange(int index, IEnumerable<T> items)
    {
        if (items == null)
        {
            throw new ArgumentNullException(nameof(items));
        }
        PrivateList.InsertRange(index, items);
        Behavior!.AddRange(items);
    }
    public int HowMany(Predicate<T> match)
    {
        int y = 0;
        PrivateList.ForEach(items =>
        {
            if (match.Invoke(items) == true)
            {
                y++;
            }
        }
        );
        return y;
    }
    public void ReplaceRange(IEnumerable<T> thisList)
    {
        if (thisList == null)
        {
            throw new CustomArgumentException("Collection Cannot Be Nothing When Replacing Range");
        }
        PrivateList.Clear();
        PrivateList.AddRange(thisList);
        Behavior!.ReplaceRange(thisList);
    }
    public void RemoveOnlyOneAfterAction(Predicate<T> match, Action<T> action)
    {
        if (Exists(match) == false)
        {
            return;
        }
        T thisItem;
        try
        {
            thisItem = FindOnlyOne(match);
        }
        catch (CustomBasicException)
        {
            throw new CustomBasicException("RemoveOnlyOneAction Had An Error.  Most Likely, The Condition Had More Than Element Satisfying It");
        }
        catch (Exception ex)
        {
            throw new CustomBasicException($"Other Exception Was Thrown.  The Error Was {ex.Message}");
        }
        action.Invoke(thisItem);
        RemoveSpecificItem(thisItem);
    }
    public void RemoveSeveralConditionalItems(BasicList<ConditionActionPair<T>> thisList)
    {
        BasicList<T> rList = new();
        thisList.ForEach(firstItem =>
        {
            if (Exists(firstItem.Predicate) == true)
            {
                T thisItem;
                try
                {
                    thisItem = FindOnlyOne(firstItem.Predicate);
                }
                catch (CustomBasicException)
                {
                    throw new CustomBasicException("RemoveSeveralConditionalItems Had An Error.  Most Likely, The Condition Had More Than Element Satisfying One Of The Condition Lists");
                }
                catch (Exception ex)
                {
                    throw new CustomBasicException($"Other Exception Was Thrown.  The Error Was {ex.Message}");
                }
                firstItem.Action.Invoke(thisItem, firstItem.Value);
                rList.Add(thisItem);
            }
        });
        RemoveGivenList(rList);
    }
    public IBasicList<U> ConvertAll<U>(Converter<T, U> converter)
    {
        BasicList<U> output = new();
        output.Capacity = PrivateList.Count;
        for (int i = 0; i < PrivateList.Count; i++)
        {
            output.Add(converter(PrivateList[i]));
        }
        return output;
    }
    public void MoveItem(T item, int newIndex)
    {
        int oldIndex = PrivateList.IndexOf(item);
        PrivateList.RemoveAt(oldIndex);
        PrivateList.Insert(newIndex, item);
    }
    void IList<T>.Insert(int index, T item)
    {
        PrivateList.Insert(index, item);
    }
    void ICollection<T>.CopyTo(T[] array, int arrayIndex)
    {
        PrivateList.CopyTo(array, arrayIndex); //go ahead and allow.  a person can only use if specifically using the interface.
    }
    bool ICollection<T>.Remove(T item)
    {
        return PrivateList.Remove(item);
    }
}