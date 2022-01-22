namespace Anvil.API
{
  public sealed class IPRPCostTableEntry : ITwoDimArrayEntry
  {
    void ITwoDimArrayEntry.InterpretEntry(TwoDimArrayEntry entry)
    {
      entry["test"].ParseInt();
    }
  }
}
