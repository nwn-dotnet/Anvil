using System;
using NWN.API.EngineStructures;
using NWN.Core;

namespace NWN.API.Events
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
