using System;
using NWN.Core;

namespace NWN.API.Events
{
  internal sealed class Event
  {
    public IntPtr Handle;

    public Event(IntPtr handle)
    {
      Handle = handle;
    }

    ~Event()
    {
      VM.FreeGameDefinedStructure(NWScript.ENGINE_STRUCTURE_EVENT, Handle);
    }

    public static implicit operator IntPtr(Event effect)
    {
      return effect.Handle;
    }

    public static implicit operator Event(IntPtr intPtr)
    {
      return new Event(intPtr);
    }
  }
}
