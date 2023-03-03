using System;
using System.Collections;
using System.Collections.Generic;

namespace Anvil.API
{
  /// <summary>
  /// Represents an array of <see cref="Effect"/> parameters.
  /// </summary>
  /// <typeparam name="T">The parameter type.</typeparam>
  public sealed class EffectParams<T> : IReadOnlyList<T?>
  {
    private readonly Func<int, T?> get;
    private readonly Action<int, T?> set;

    internal EffectParams(int count, Func<int, T?> get, Action<int, T?> set)
    {
      Count = count;
      this.get = get;
      this.set = set;
    }

    /// <summary>
    /// Gets the number of parameters.
    /// </summary>
    public int Count { get; }

    public T? this[int index]
    {
      get
      {
        if (index < 0 || index >= Count)
        {
          throw new IndexOutOfRangeException("Index was out of range. Must be non-negative and less than the size of the array.");
        }

        return get(index);
      }
      set
      {
        if (index < 0 || index >= Count)
        {
          throw new IndexOutOfRangeException("Index was out of range. Must be non-negative and less than the size of the array.");
        }

        set(index, value);
      }
    }

    public IEnumerator<T?> GetEnumerator()
    {
      for (int i = 0; i < Count; i++)
      {
        yield return this[i];
      }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}
