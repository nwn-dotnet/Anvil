using System;
using NWN.Core;

namespace NWN.API
{
  public partial class ItemProperty
  {
    public IntPtr Handle;
    public ItemProperty(IntPtr handle) { Handle = handle; }
    ~ItemProperty() { Internal.NativeFunctions.FreeItemProperty(Handle); }

    public static implicit operator IntPtr(ItemProperty effect) => effect.Handle;
    public static implicit operator ItemProperty(IntPtr intPtr) => new ItemProperty(intPtr);
  }
}