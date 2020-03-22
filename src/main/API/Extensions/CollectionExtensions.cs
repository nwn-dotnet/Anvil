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
  }
}