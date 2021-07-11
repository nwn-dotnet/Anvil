using System;
using NWN.Core;

namespace Anvil.API
{
  internal sealed class Event : EngineStructure
  {
    internal Event(IntPtr handle) : base(handle) {}

    protected override int StructureId
    {
      get => NWScript.ENGINE_STRUCTURE_EVENT;
    }

    public static implicit operator Event(IntPtr intPtr)
    {
      return new Event(intPtr);
    }
  }
}
