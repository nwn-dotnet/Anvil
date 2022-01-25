using System;

namespace Anvil.Services
{
  //! ## Examples
  //! @include XPReportService.cs

  /// <summary>
  /// Implement this interface to use the class as a deserialization target for <see cref="TwoDimArrayFactory.Get2DA{T}"/>.
  /// </summary>
  [Obsolete("Use the generic TwoDimArray<T> instead.")]
  public interface ITwoDimArray
  {
    void DeserializeRow(int rowIndex, TwoDimEntry twoDimEntry);
  }
}
