using System;
using NWN.Core;

namespace Anvil.API
{
  internal sealed class Json : EngineStructure
  {
    internal Json(IntPtr handle, bool memoryOwn) : base(handle, memoryOwn) {}

    protected override int StructureId => NWScript.ENGINE_STRUCTURE_JSON;

    public static implicit operator Json(IntPtr intPtr)
    {
      return new Json(intPtr, true);
    }

    public static Json Parse(string jsonString)
    {
      return NWScript.JsonParse(jsonString);
    }

    public string Dump()
    {
      return NWScript.JsonDump(this);
    }
  }
}
