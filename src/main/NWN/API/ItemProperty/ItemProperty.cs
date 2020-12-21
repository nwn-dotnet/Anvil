using System;
using NWN.Core;

namespace NWN.API
{
  public partial class ItemProperty
  {
    private readonly IntPtr handle;

    private ItemProperty(IntPtr handle)
    {
      this.handle = handle;
    }

    ~ItemProperty()
    {
      VM.FreeGameDefinedStructure(NWScript.ENGINE_STRUCTURE_ITEMPROPERTY, handle);
    }

    public static implicit operator IntPtr(ItemProperty effect) => effect.handle;

    public static implicit operator ItemProperty(IntPtr intPtr) => new ItemProperty(intPtr);
  }
}
