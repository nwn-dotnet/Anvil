namespace Anvil.API
{
  public interface ITwoDimArrayEntry
  {
    int RowIndex { get; init; }

    void InterpretEntry(TwoDimArrayEntry entry);
  }
}
