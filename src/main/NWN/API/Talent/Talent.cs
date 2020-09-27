using System;
using NWN.Core;

namespace NWN.API
{
  public partial class Talent
  {
    private readonly IntPtr handle;

    private Talent(IntPtr handle)
    {
      this.handle = handle;
    }

    ~Talent()
    {
      VM.FreeGameDefinedStructure(NWScript.ENGINE_STRUCTURE_TALENT, handle);
    }

    public static implicit operator IntPtr(Talent effect) => effect.handle;

    public static implicit operator Talent(IntPtr intPtr) => new Talent(intPtr);
  }
}
