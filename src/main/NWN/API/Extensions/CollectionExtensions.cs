using System.Collections.Generic;

namespace NWN.API
{
  internal static class CollectionExtensions
  {
    public static void InsertOrdered<T>(this List<T> sortedList, T item, IComparer<T> comparer = null)
    {
      int binaryIndex = sortedList.BinarySearch(item, comparer);
      int index = binaryIndex < 0 ? ~binaryIndex : binaryIndex;
      sortedList.Insert(index, item);
    }

    public static void AddElement<TKey, TValue, TCollection>(this IDictionary<TKey, TCollection> mutableLookup, TKey key, TValue value) where TCollection : ICollection<TValue>, new()
    {
      if (!mutableLookup.TryGetValue(key, out TCollection values))
      {
        values = new TCollection();
        mutableLookup[key] = values;
      }

      values.Add(value);
    }

    public static bool ContainsElement<TKey, TValue, TCollection>(this IDictionary<TKey, TCollection> mutableLookup, TKey key, TValue value) where TCollection : ICollection<TValue>
    {
      return mutableLookup.TryGetValue(key, out TCollection values) && values.Contains(value);
    }

    public static TValue SafeGet<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
    {
      return dictionary.TryGetValue(key, out TValue retVal) ? retVal : default;
    }
  }
}
