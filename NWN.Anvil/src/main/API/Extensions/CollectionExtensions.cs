using System;
using System.Collections.Generic;
using System.Linq;

namespace Anvil.API
{
  /// <summary>
  /// Various class extensions for generic collections.
  /// </summary>
  public static class CollectionExtensions
  {
    /// <summary>
    /// Adds an element to a mutable lookup table<br/>
    /// (E.g. Dictionary&lt;Key,List&lt;Value&gt;&gt;)
    /// </summary>
    /// <param name="mutableLookup">The lookup to modify.</param>
    /// <param name="key">The key that the element should be added to.</param>
    /// <param name="value">The value that should be added.</param>
    public static void AddElement<TKey, TValue, TCollection>(this IDictionary<TKey, TCollection> mutableLookup, TKey key, TValue value) where TCollection : ICollection<TValue>, new()
    {
      if (!mutableLookup.TryGetValue(key, out TCollection? values))
      {
        values = new TCollection();
        mutableLookup[key] = values;
      }

      values.Add(value);
    }

    /// <summary>
    /// Queries if a certain value exists in a mutable lookup table<br/>
    /// (E.g. Dictionary&lt;Key,List&lt;Value&gt;&gt;)
    /// </summary>
    /// <param name="mutableLookup">The lookup to query.</param>
    /// <param name="key">The key to lookup.</param>
    /// <param name="value">The value to be searched.</param>
    public static bool ContainsElement<TKey, TValue, TCollection>(this IDictionary<TKey, TCollection> mutableLookup, TKey key, TValue value) where TCollection : ICollection<TValue>
    {
      return mutableLookup.TryGetValue(key, out TCollection? values) && values.Contains(value);
    }

    public static void DisposeAll(this IEnumerable<IDisposable?>? disposables)
    {
      if (disposables == null)
      {
        return;
      }

      foreach (IDisposable? disposable in disposables)
      {
        disposable?.Dispose();
      }
    }

    /// <summary>
    /// Inserts an item into an already sorted list.
    /// </summary>
    /// <param name="sortedList">The sorted list.</param>
    /// <param name="item">The item to insert.</param>
    /// <param name="comparer">A custom comparer to use when comparing the item against elements in the collection.</param>
    /// <typeparam name="T">The type of item to insert.</typeparam>
    public static void InsertOrdered<T>(this List<T> sortedList, T item, IComparer<T>? comparer = null)
    {
      int binaryIndex = sortedList.BinarySearch(item, comparer);
      int index = binaryIndex < 0 ? ~binaryIndex : binaryIndex;
      sortedList.Insert(index, item);
    }

    /// <summary>
    /// Removes an element from a mutable lookup table<br/>
    /// (E.g. Dictionary&lt;Key,List&lt;Value&gt;&gt;)
    /// </summary>
    /// <param name="mutableLookup">The lookup to modify.</param>
    /// <param name="key">The key that the element should be removed from.</param>
    /// <param name="value">The value that should be removed.</param>
    public static bool RemoveElement<TKey, TValue, TCollection>(this IDictionary<TKey, TCollection> mutableLookup, TKey key, TValue value) where TCollection : ICollection<TValue>, new()
    {
      bool retVal = false;

      if (mutableLookup.TryGetValue(key, out TCollection? values))
      {
        retVal = values.Remove(value);
        if (values.Any())
        {
          mutableLookup.Remove(key);
        }
      }

      return retVal;
    }

    public static TValue? SafeGet<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
    {
      return dictionary.TryGetValue(key, out TValue? retVal) ? retVal : default;
    }

    /// <summary>
    /// Wraps this object instance into an IEnumerable&lt;T&gt; consisting of a single item.<br/>
    /// If the item is null, returns an empty enumerable instead.
    /// </summary>
    /// <typeparam name="T">Type of the object.</typeparam>
    /// <param name="item">The instance that will be wrapped.</param>
    /// <returns>An IEnumerable&lt;T&gt; consisting of a single item. </returns>
    public static IEnumerable<T> SafeYield<T>(this T item)
    {
      return item is not null ? item.Yield() : Enumerable.Empty<T>();
    }

    /// <summary>
    /// Wraps this object instance into an IEnumerable&lt;T&gt; consisting of a single item.
    /// </summary>
    /// <typeparam name="T">Type of the object.</typeparam>
    /// <param name="item">The instance that will be wrapped.</param>
    /// <returns>An IEnumerable&lt;T&gt; consisting of a single item. </returns>
    public static IEnumerable<T> Yield<T>(this T item)
    {
      yield return item;
    }
  }
}
