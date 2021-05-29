using System;
using NWN.Core;

namespace NWN.API
{
  public sealed partial class Location
  {
    private readonly IntPtr handle;

    private Location(IntPtr handle)
    {
      this.handle = handle;
    }

    ~Location()
    {
      VM.FreeGameDefinedStructure(NWScript.ENGINE_STRUCTURE_LOCATION, handle);
    }

    public static implicit operator IntPtr(Location effect)
    {
      return effect.handle;
    }

    public static implicit operator Location(IntPtr intPtr)
    {
      return new Location(intPtr);
    }
  }
}
