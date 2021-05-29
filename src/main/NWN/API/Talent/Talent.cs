using System;
using NWN.Core;

namespace NWN.API
{
  public sealed partial class Talent
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

    public static implicit operator IntPtr(Talent talent)
    {
      return talent.handle;
    }

    public static implicit operator Talent(IntPtr intPtr)
    {
      return new Talent(intPtr);
    }
  }
}
