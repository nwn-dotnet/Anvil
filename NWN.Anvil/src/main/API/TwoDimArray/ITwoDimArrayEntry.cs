namespace Anvil.API
{
  /// <summary>
  /// Implement to decode <see cref="TwoDimArrayEntry"/> into a type using <see cref="TwoDimArray{T}(string)"/>.<br/>
  /// See <see cref="AppearanceTableEntry"/> for an example.
  /// </summary>
  public interface ITwoDimArrayEntry
  {
    int RowIndex { get; init; }

    void InterpretEntry(TwoDimArrayEntry entry);
  }
}
