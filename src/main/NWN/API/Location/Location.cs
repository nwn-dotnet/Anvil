using System;
using NWN.Core;

namespace NWN.API
{
  public partial class Location
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

    public static implicit operator IntPtr(Location effect) => effect.handle;
    public static implicit operator Location(IntPtr intPtr) => new Location(intPtr);
  }
}