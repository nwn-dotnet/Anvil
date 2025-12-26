namespace Anvil.API
{
  /// <summary>
  /// Implement to decode <see cref="TwoDimArrayEntry"/> into a type using <see cref="NwGameTables.GetTable{T}(string, bool, bool)"/>.
  /// </summary>
  /// <example>[!code-csharp[](~/../NWN.Anvil.Samples/src/main/Services/XPReportService.cs)]</example>
  public interface ITwoDimArrayEntry
  {
    int RowIndex { get; init; }

    void InterpretEntry(TwoDimArrayEntry entry);
  }
}
