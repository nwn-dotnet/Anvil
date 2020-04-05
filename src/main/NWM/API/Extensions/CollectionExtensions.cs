using System.Collections.Generic;

namespace NWM.API
{
  public static class CollectionExtensions
  {
    public static void InsertOrdered<T>(this List<T> sortedList, T item, IComparer<T> comparer = null)
    {
      int binaryIndex = sortedList.BinarySearch(item, comparer);
      int index = binaryIndex < 0 ? ~binaryIndex : binaryIndex;
      sortedList.Insert(index, item);
    }

    public static void AddElement<TKey, TValue>(this IDictionary<TKey, IList<TValue>> mutableLookup, TKey key, TValue value)
    {
      if(!mutableLookup.TryGetValue(key, out IList<TValue> values))
      {
        values = new List<TValue>();
        mutableLookup[key] = values;
      }

      values.Add(value);
    }
  }
}