using System.Collections.Generic;

namespace Anvil
{
  /// <summary>
  /// Represents a collection of elements that can be accessed and modified by index, but may not be resized.
  /// </summary>
  /// <typeparam name="T">The type of elements in the array.</typeparam>
  public interface IArray<T> : IReadOnlyList<T>
  {
    new T this[int index] { get; set; }
  }
}
