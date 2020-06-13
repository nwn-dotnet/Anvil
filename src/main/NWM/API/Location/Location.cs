using System;
using NWN.Core;

namespace NWM.API
{
  public partial class Location
  {
    public IntPtr Handle;
    public Location(IntPtr handle) { Handle = handle; }
    ~Location() { Internal.NativeFunctions.FreeLocation(Handle); }

    public static implicit operator IntPtr(Location effect) => effect.Handle;
    public static implicit operator Location(IntPtr intPtr) => new Location(intPtr);
  }
}