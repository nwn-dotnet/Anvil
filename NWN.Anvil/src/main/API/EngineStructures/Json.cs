using System;
using NWN.Core;

namespace Anvil.API
{
  public sealed class Json : EngineStructure
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

    public NwObject? ToNwObject(Location location, NwGameObject? owner = null, bool loadObjectState = true)
    {
      return NWScript.JsonToObject(this, location, owner, loadObjectState.ToInt()).ToNwObject();
    }

    public T? ToNwObject<T>(Location location, NwGameObject? owner = null, bool loadObjectState = true) where T : NwObject
    {
      return NWScript.JsonToObject(this, location, owner, loadObjectState.ToInt()).ToNwObject<T>();
    }
  }
}
