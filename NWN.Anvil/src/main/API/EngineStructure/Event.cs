using System;
using NWN.Core;

namespace Anvil.API
{
  internal sealed class Event : EngineStructure
  {
    internal Event(IntPtr handle, bool memoryOwn) : base(handle, memoryOwn) {}

    protected override int StructureId => NWScript.ENGINE_STRUCTURE_EVENT;

    public static implicit operator Event?(IntPtr intPtr)
    {
      return intPtr != IntPtr.Zero ? new Event(intPtr, true) : null;
    }
  }
}
