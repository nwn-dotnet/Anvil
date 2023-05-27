using System;
using System.Collections;
using System.Collections.Generic;
using NWN.Native.API;

namespace Anvil.Native
{
  internal sealed class ArrayWrapper<T1, T2> : IArray<T2> where T1 : unmanaged
  {
    private readonly NativeArray<T1> nativeArray;
    private readonly Func<T1, T2> get;
    private readonly Func<T2, T1> set;

    public int Count => nativeArray.Length;

    public ArrayWrapper(NativeArray<T1> nativeArray, Func<T1, T2> get, Func<T2, T1> set)
    {
      this.nativeArray = nativeArray;
      this.get = get;
      this.set = set;
    }

    public IEnumerator<T2> GetEnumerator()
    {
      foreach (T1 value in nativeArray)
      {
        yield return get(value);
      }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public void CopyTo(T2[] array, int arrayIndex)
    {
      if (array == null)
      {
        throw new ArgumentNullException(nameof(array));
      }

      if (arrayIndex < 0)
      {
        throw new ArgumentOutOfRangeException(nameof(arrayIndex));
      }

      if (array.Length - arrayIndex < Count)
      {
        throw new ArgumentException("Not enough elements after arrayIndex in the destination array.");
      }

      for (int i = 0; i < Count; i++)
      {
        array[i + arrayIndex] = this[i];
      }
    }

    public T2 this[int index]
    {
      get => get(nativeArray[index]);
      set => nativeArray[index] = set(value);
    }
  }
}
