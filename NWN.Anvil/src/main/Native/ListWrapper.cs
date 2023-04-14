using System;
using System.Collections;
using System.Collections.Generic;

namespace Anvil.Native
{
  internal sealed class ListWrapper<T1, T2> : IList<T2>
  {
    private readonly IList<T1> list;
    private readonly Func<T1, T2> get;
    private readonly Func<T2, T1> set;

    public int Count => list.Count;
    public bool IsReadOnly => list.IsReadOnly;

    public ListWrapper(IList<T1> list, Func<T1, T2> get, Func<T2, T1> set)
    {
      this.list = list;
      this.get = get;
      this.set = set;
    }

    public IEnumerator<T2> GetEnumerator()
    {
      foreach (T1 value in list)
      {
        yield return get(value);
      }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public void Add(T2 item)
    {
      T1 value = set(item);
      list.Add(value);
    }

    public void Clear()
    {
      list.Clear();
    }

    public bool Contains(T2 item)
    {
      T1 value = set(item);
      return list.Contains(value);
    }

    public void CopyTo(T2[] array, int arrayIndex)
    {
      if (array == null)
      {
        throw new NullReferenceException("array is null");
      }

      if (arrayIndex < 0)
      {
        throw new ArgumentOutOfRangeException(nameof(arrayIndex), "arrayIndex is less than 0.");
      }

      if (array.Length < Count - arrayIndex)
      {
        throw new ArgumentException("Copy would exceed size of target array.");
      }

      for (int i = arrayIndex; i < Count; i++)
      {
        array[i] = get(list[i]);
      }
    }

    public bool Remove(T2 item)
    {
      T1 value = set(item);
      return list.Remove(value);
    }

    public int IndexOf(T2 item)
    {
      T1 value = set(item);
      return list.IndexOf(value);
    }

    public void Insert(int index, T2 item)
    {
      T1 value = set(item);
      list.Insert(index, value);
    }

    public void RemoveAt(int index)
    {
      list.RemoveAt(index);
    }

    public T2 this[int index]
    {
      get => get(list[index]);
      set => list[index] = set(value);
    }
  }
}
