using System;
using NWN.Core;

namespace NWN.API
{
  public partial class Effect
  {
    private IntPtr handle;

    private Effect(IntPtr handle)
    {
      this.handle = handle;
    }

    ~Effect()
    {
      Internal.NativeFunctions.FreeGameDefinedStructure(NWScript.ENGINE_STRUCTURE_EFFECT, handle);
    }

    public static implicit operator IntPtr(Effect effect) => effect.handle;
    public static implicit operator Effect(IntPtr intPtr) => new Effect(intPtr);
  }
}