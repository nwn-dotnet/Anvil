using System;
using NWN;

namespace NWM.API
{
  public partial class Effect
  {
    public IntPtr Handle;
    public Effect(IntPtr handle) { Handle = handle; }
    ~Effect() { Internal.NativeFunctions.FreeEffect(Handle); }

    public static implicit operator IntPtr(Effect effect) => effect.Handle;
    public static implicit operator Effect(IntPtr intPtr) => new Effect(intPtr);
  }
}