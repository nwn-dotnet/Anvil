namespace Anvil.API
{
  //! ## Examples
  //! @include XPReportService.cs

  /// <summary>
  /// Implement to decode <see cref="TwoDimArrayEntry"/> into a type using <see cref="TwoDimArray{T}(string)"/>.
  /// </summary>
  public interface ITwoDimArrayEntry
  {
    int RowIndex { get; init; }

    void InterpretEntry(TwoDimArrayEntry entry);
  }
}
