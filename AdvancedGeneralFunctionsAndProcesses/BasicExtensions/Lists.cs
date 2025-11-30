using CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.Misc;
using CommonBasicLibraries.BasicDataSettingsAndProcesses;
using CommonBasicLibraries.CollectionClasses;
using System.Collections;
using static CommonBasicLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
namespace CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
public static class Lists
{
    extension(BasicList<string> list)
    {
        public BasicList<int> CastStringListToIntegerList()
        {
            BasicList<int> output = [];
            foreach (string str in list)
            {
                if (int.TryParse(str, out int a) == false)
                {
                    return [];
                }
                output.Add(a);
            }
            return output;
        }
    }
    extension<T>(Dictionary<int, T> list)
    {
        public void Add(T item)
        {
            list.Add(list.Count + 1, item);
        }
    }
    extension<TKey, TValue>(IDictionary<TKey, TValue> list)
    {
        public TKey GetKey(TValue value)
        {
            foreach (KeyValuePair<TKey, TValue> thisPair in list)
            {
                if (value!.Equals(thisPair.Value) == true || ReferenceEquals(thisPair.Value, value) == true)
                {
                    return thisPair.Key;
                }
            }
            throw new CustomBasicException($"The Value Of {value} Was Not Found In The Dictionary");
        }
    }
    extension<T>(IEnumerable<T> list)
    {
        //was going to do as property but not since the ToList was method, i think makes sense to keep as method here too.
        public BasicList<T> ToBasicList() => [.. list];
        public int Count
        {
            get
            {
                if (list is IList c)
                {
                    return c.Count;
                }
                return list.Count();
            }
        }
        public bool Contains(T value) =>
            list switch
            {
                null => false,
                List<T> l => l.Contains(value),
                BasicList<T> bl => bl.Contains(value),
                _ => list.Any(item => EqualityComparer<T>.Default.Equals(item, value))
            };
        public async Task ForEachAsync(ActionAsync<T> action)
        {
            switch (list)
            {
                case BasicList<T> bl:
                    await bl.ForEachAsync(action);
                    break;
                default:
                    foreach (var item in list)
                    {
                        await action.Invoke(item);
                    }
                    break;
            }
        }
        public void ForEach(Action<T> action)
        {
            switch (list)
            {
                case List<T> l:
                    l.ForEach(action);
                    break;
                case BasicList<T> bl:
                    bl.ForEach(action);
                    break;
                default:
                    foreach (var item in list)
                    {
                        action(item);
                    }
                    break;
            }
        }
        
    }
    extension<T>(IList<T>)
    {
        public static IList<T> GetBlankList(int howMany)
        {
            BasicList<T> list = [];
            for (int i = 0; i < howMany; i++)
            {
                list.Add(default!);
            }
            return list;
        }
    }
    extension<T>(IList<T> list)
    {
        public void RemoveGivenList(IEnumerable<T> remove)
        {
            if (list is BasicList<T> b)
            {
                b.RemoveGivenList(remove);
                return;
            }
            var removeSet = new HashSet<T>(remove);
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (removeSet.Contains(list[i]))
                {
                    list.RemoveAt(i);
                }
            }
        }
        public void AddRange(IEnumerable<T> adds)
        {
            switch (list)
            {
                case BasicList<T> b:
                    b.AddRange(adds);
                    break;
                case List<T> l:
                    l.AddRange(adds);
                    break;
                default:
                    foreach (var item in adds)
                    {
                        list.Add(item);
                    }
                    break;
            }
        }
    }
    extension<T>(IEnumerable<string> list)
    {
        public async Task ReconcileStringsAsync(IList<T> saved, Func<T, string> match, Func<string, Task<T>> result)
        {
            BasicList<string> tempList = [];
            saved.ForEach(items => tempList.Add(match(items)));
            BasicList<T> removeList = [];
            BasicList<T> addList = [];
            tempList.ForEach(items =>
            {
                if (list.Contains(items) == false)
                {
                    removeList.Add(saved[tempList.IndexOf(items)]);
                }
            });
            await list.ForEachAsync(async items =>
            {
                if (tempList.Contains(items) == false)
                {
                    addList.Add(await result(items));
                }
            });
            saved.RemoveGivenList(removeList);
            saved.AddRange(addList);
        }
    }
    extension<T>(IEnumerable<object> list)
    {
        public BasicList<T> ToCastedList() => list.Cast<T>().ToBasicList();
    }
    extension<T>(IList<ConditionActionPair<T>> list)
    {
        public IList<ConditionActionPair<T>> Append(Predicate<T> match, Action<T, string> action, string value)
        {
            ConditionActionPair<T> item = new(match, action, value);
            list.Add(item);
            return list;
        }
    }
    extension<TSource, TKey>(IEnumerable<TSource> source)
    {
        public bool HasDuplicates(Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = [];
            foreach (var item in source)
            {
                if (seenKeys.Add(keySelector(item)) == false)
                {
                    return true;
                }
            }
            return false;
        }
        public bool HasOnlyOne(Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = [];
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
    }
    extension<TKey, TSource>(IEnumerable<TSource> source)
    {
        public IOrderedEnumerable<IGrouping<TKey, TSource>> GroupOrderDescending(Func<TSource, TKey> keySelector)
        {
            return source.GroupBy(keySelector).OrderByDescending(Items => Items.Count());
        }
        public IOrderedEnumerable<IGrouping<TKey, TSource>> GroupOrderAscending(Func<TSource, TKey> keySelector)
        {
            return source.GroupBy(keySelector).OrderBy(Items => Items.Count());
        }
    }
    extension<TSource, TKey>(IEnumerable<TSource> source)
    {
        public int MaximumDuplicates(Func<TSource, TKey> keySelector)
        {
            if (source.Any() == false)
            {
                throw new CustomBasicException("There has to be at least one item.  If I am wrong, rethink");
            }
            var firstList = source.GroupBy(keySelector).OrderByDescending(Items => Items.Count());
            return firstList.First().Count();
        }
        public bool DoesReconcile(IEnumerable<TSource> other, Func<TSource, TKey> keySelector)
        {
            if (source.Count() != other.Count())
            {
                return false; //because not even the same count.
            }
            HashSet<TKey> seenKeys = [];
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
            return true;
        }
        public int DistinctCount(Func<TSource, TKey> keySelector)
        {
            int count = 0;
            HashSet<TKey> seenKeys = [];
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    count++;
                }
            }
            return count;
        }
        public BasicList<TKey> DistinctItems(Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = [];
            BasicList<TKey> output = [];
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
    extension<TSource>(IEnumerable<TSource> source)
    {
        public int MaximumDuplicates
        {
            get
            {
                if (source.Any() == false)
                {
                    throw new CustomBasicException("There has to be at least one item.  If I am wrong, rethink");
                }
                var firstList = source.GroupBy(Items => Items).OrderByDescending(Items => Items.Count());
                return firstList.First().Count();
            }
        }
        public string Join(string delimiter)
        {
            StrCat cats = new();
            foreach (var item in source)
            {
                if (item is null)
                {
                    cats.AddToString("", delimiter);
                }
                else
                {
                    cats.AddToString(item.ToString()!, delimiter);
                }
            }
            return cats.GetInfo();
        }
        public BasicList<int?> ExtractIntegers(Func<TSource, int?> keySelector)
        {
            BasicList<int?> output = [];
            foreach (var item in source)
            {
                output.Add(keySelector(item));
            }
            return output;
        }
        public BasicList<int> ExtractIntegers(Func<TSource, int> keySelector)
        {
            BasicList<int> output = [];
            foreach (var item in source)
            {
                output.Add(keySelector(item));
            }
            return output;
        }
    }
    extension<TSource, TKey>(ICollection<TSource> source)
    {
        public BasicList<TSource> GetDuplicates(Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = [];
            BasicList<TSource> output = [];
            foreach (var item in source)
            {
                if (seenKeys.Add(keySelector(item)) == false)
                {
                    output.Add(item);
                }
            }
            return output;
        }
    }
    extension<TSource>(IBasicList<TSource> source)
    {
        public bool IsIntOrdered(Func<TSource, int?> keySelector, bool excludeUnknowns = true)
        {
            if (source.Count == 0)
            {
                return true; //just act like its in order because there was nothing.
            }
            BasicList<int?> list = source.ExtractIntegers(keySelector);
            int x; //starts at 1
            x = 0;
            if (excludeUnknowns == true)
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
    }
}