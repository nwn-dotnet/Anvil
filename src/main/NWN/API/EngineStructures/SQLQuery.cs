using System;

namespace NWN.API
{
  public class SQLQuery : EngineStructure
  {
    internal SQLQuery(IntPtr handle) : base(handle) {}
    protected override int StructureId { get; }
  }
}
