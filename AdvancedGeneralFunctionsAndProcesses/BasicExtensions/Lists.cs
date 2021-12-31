using CommonBasicLibraries.BasicDataSettingsAndProcesses;
using CommonBasicLibraries.CollectionClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
public static class Lists
{
    public static void Add<T>(this Dictionary<int, T> thisDict, T thisItem)
    {
        thisDict.Add(thisDict.Count + 1, thisItem); //this is used in cases where we do a dictionary just for the purpose of one based items
    }
    public static T GetRandomItem<T>(this Dictionary<int, T> thisList)
    {
        return thisList.Values.ToBasicList().GetRandomItem();
    }
#if NETSTANDARD2_0
    public static HashSet<T> ToHashSet<T>(this IEnumerable<T> TempList)
    {
        return new HashSet<T>(TempList);
    }
#endif
    public static BasicList<T> ToBasicList<T>(this IEnumerable<T> tempList)
    {
        return new BasicList<T>(tempList);
    }
    public static BasicList<T> ToCastedList<T>(this IEnumerable<object> tempList) //in this case, you get another list.
    {
        return tempList.Cast<T>().ToBasicList();
    }
    public static TKey GetKey<TKey, TValue>(this IDictionary<TKey, TValue> thisDict, TValue thisValue)
    {
        if (thisDict == null)
        {
            throw new ArgumentNullException(nameof(thisDict));
        }
        foreach (KeyValuePair<TKey, TValue> thisPair in thisDict)
        {
            if (thisValue!.Equals(thisPair.Value) == true || ReferenceEquals(thisPair.Value, thisValue) == true)
            {
                return thisPair.Key;
            }
        }
        throw new CustomBasicException($"The Value Of {thisValue} Was Not Found In The Dictionary");
    }
    public static void PopulateBlankList(this BasicList<string> thisList, int howMany)
    {
        for (int i = 0; i < howMany; i++)
        {
            thisList.Add("");
        }
    }
    public static BasicList<string> CastIntegerListToStringList(this BasicList<int> thisList)
    {
        BasicList<string> newList = new();
        thisList.ForEach(x => newList.Add(x.ToString()));
        return newList;
    }
    public static BasicList<int> CastStringListToIntegerList(this BasicList<string> list)
    {
        BasicList<int> output = new();
        foreach (string str in list)
        {
            if (int.TryParse(str, out int a) == false)
            {
                return new();
            }
            output.Add(a);
        }
        return output;
    }
    
    public static async Task ReconcileStrings<T>(this BasicList<string> sentList, BasicList<T> savedList, Func<T, string> match, Func<string, Task<T>> result)
    {
        BasicList<string> tempList = new();
        savedList.ForEach(items => tempList.Add(match(items)));
        BasicList<T> removeList = new();
        BasicList<T> addList = new();
        tempList.ForEach(items =>
        {
            if (sentList.Contains(items) == false)
            {
                removeList.Add(savedList[tempList.IndexOf(items)]);
            }
        });
        await sentList.ForEachAsync(async items =>
        {
            if (tempList.Contains(items) == false)
            {
                addList.Add(await result(items));
            }
        });
        savedList.RemoveGivenList(removeList);
        savedList.AddRange(addList);
    }
    public static BasicList<ConditionActionPair<T>> Append<T>(this BasicList<ConditionActionPair<T>> tempList, Predicate<T> match, Action<T, string> action, string value = "") //if it needs to be something else. rethink
    {
        ConditionActionPair<T> ThisC = new(match, action, value);
        tempList.Add(ThisC);
        return tempList;
    }
    public static void WriteString<T>(this BasicList<T> thisList)
    {
        thisList.ForEach(items => Console.WriteLine(items!.ToString()));
    }
    public static bool HasDuplicates<TSource, TKey>(this IBasicList<TSource> source, Func<TSource, TKey> keySelector)
    {
        HashSet<TKey> seenKeys = new();
        foreach (var item in source)
        {
            if (seenKeys.Add(keySelector(item)) == false)
            {
                return true;
            }
        }
        return false;
    }
    public static bool HasOnlyOne<TSource, TKey>(this IBasicList<TSource> source, Func<TSource, TKey> keySelector)
    {
        HashSet<TKey> seenKeys = new();
        if (source.Count == 0)
        {
            return false; //because there are none.
        }
        foreach (var item in source)
        {
            seenKeys.Add(keySelector(item));
            if (seenKeys.Count > 1)
            {
                return false;
            }
        }
        return true;
    }
    public static IOrderedEnumerable<IGrouping<TKey, TSource>> GroupOrderDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        return source.GroupBy(keySelector).OrderByDescending(Items => Items.Count());
    }
    public static IOrderedEnumerable<IGrouping<TKey, TSource>> GroupOrderAscending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        return source.GroupBy(keySelector).OrderBy(Items => Items.Count());
    }
    public static int MaximumDuplicates<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        if (source.Any() == false)
        {
            throw new CustomBasicException("There has to be at least one item.  If I am wrong, rethink");
        }
        var firstList = source.GroupBy(keySelector).OrderByDescending(Items => Items.Count());
        return firstList.First().Count();
    }
    public static int MaximumDuplicates<TSource>(this IEnumerable<TSource> source)
    {
        if (source.Any() == false)
        {
            throw new CustomBasicException("There has to be at least one item.  If I am wrong, rethink");
        }
        var firstList = source.GroupBy(Items => Items).OrderByDescending(Items => Items.Count());
        return firstList.First().Count();
    }
    public static BasicList<TSource> GetDuplicates<TSource, TKey>(this IBasicList<TSource> source, Func<TSource, TKey> keySelector)
    {
        HashSet<TKey> seenKeys = new();
        BasicList<TSource> output = new();
        foreach (var item in source)
        {
            if (seenKeys.Add(keySelector(item)) == false)
            {
                output.Add(item);
            }
        }
        return output;
    }
    public static bool DoesReconcile<TSource, TKey>(this IEnumerable<TSource> source, IEnumerable<TSource> other, Func<TSource, TKey> keySelector)
    {
        if (source.Count() != other.Count())
            return false; //because not even the same count.
        HashSet<TKey> seenKeys = new();
        foreach (var item in source)
        {
            seenKeys.Add(keySelector(item));
        }
        foreach (var item in other)
        {
            if (seenKeys.Add(keySelector(item)))
            {
                return false;
            }
        }
        return true; //may need to test this idea.
                     //its case sensitive.  i think its okay since its intended for anything.
    }
    public static bool IsIntOrdered<TSource>(this IBasicList<TSource> source, Func<TSource, int?> keySelector, bool ExcludeUnknowns = true)
    {
        if (source.Count == 0)
        {
            return true; //just act like its in order because there was nothing.
        }
        BasicList<int?> list = source.ExtractIntegers(keySelector);
        int x; //starts at 1
        x = 0;
        if (ExcludeUnknowns == true)
        {
            list.RemoveAllOnly(Items => Items.HasValue == false);
        }
        list.Sort();
        if (list.First() != 1)
        {
            return false; //if the first item is not in order, then its not in order.
        }
        foreach (var item in list)
        {
            x++;
            if (item != x)
            {
                return false;
            }
        }
        return true;
    }
    //decided to use ienumerable now because sometimes it does not quite implement the custom list but is still needed.
    public static BasicList<int?> ExtractIntegers<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> keySelector)
    {
        BasicList<int?> output = new();
        foreach (var item in source)
        {
            output.Add(keySelector(item));
        }
        return output;
    }
    public static BasicList<int> ExtractIntegers<TSource>(this IEnumerable<TSource> source, Func<TSource, int> keySelector)
    {
        BasicList<int> output = new();
        foreach (var item in source)
        {
            output.Add(keySelector(item));
        }
        return output;
    }
    public static int DistinctCount<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        int count = 0;
        HashSet<TKey> seenKeys = new();
        foreach (TSource element in source)
        {
            if (seenKeys.Add(keySelector(element)))
            {
                count++;
            }
        }
        return count;
    }

    public static IEnumerable<TSource> DistinctBy<TSource, TKey> //2 choices
(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        HashSet<TKey> seenKeys = new();
        foreach (TSource element in source)
        {
            if (seenKeys.Add(keySelector(element)))
            {
                yield return element;
            }
        }
    }
    public static BasicList<TKey> DistinctItems<TSource, TKey>
        (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        HashSet<TKey> seenKeys = new();
        BasicList<TKey> output = new();
        foreach (TSource element in source)
        {
            if (seenKeys.Add(keySelector(element)))
            {
                output.Add(keySelector(element));
            }
        }
        output.Sort();
        return output;
    }
}