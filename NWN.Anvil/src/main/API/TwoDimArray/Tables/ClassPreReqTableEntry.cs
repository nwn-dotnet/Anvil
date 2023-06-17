using System;

namespace Anvil.API
{
  public class ClassPreReqTableEntry : ITwoDimArrayEntry
  {
    public string? Label { get; private set; }

    public ClassPreReqType? ReqType { get; private set; }

    public string? ReqParam1 { get; private set; }

    public string? ReqParam2 { get; private set; }

    public int RowIndex { get; init; }

    void ITwoDimArrayEntry.InterpretEntry(TwoDimArrayEntry entry)
    {
      Label = entry.GetString("LABEL");
      if (Enum.TryParse(entry.GetString("ReqType"), true, out ClassPreReqType reqType))
      {
        ReqType = reqType;
      }

      ReqParam1 = entry.GetString("ReqParam1");
      ReqParam2 = entry.GetString("ReqParam2");
    }
  }
}
