using System.Collections.Generic;

namespace NWM.Core
{
  public static class CollectionExtensions
  {
    public static void InsertOrdered<T>(this List<T> list, T item, IComparer<T> comparer = null)
    {
      int binaryIndex = list.BinarySearch(item, comparer);

      //The value will be a negative integer if the list already
      //contains an item equal to the one searched for above
      if (binaryIndex < 0)
      {
        list.Insert(~binaryIndex, item);
      }
      else
      {
        list.Insert(binaryIndex, item);
      }
    }
  }
}