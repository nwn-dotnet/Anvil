using System.Text.Json;
using NWN.Core;

namespace Anvil.API
{
  public sealed class LocalVariableJson : LocalVariable<JsonElement>
  {
    public override JsonElement Value
    {
      get => HasValue ? JsonSerializer.Deserialize<JsonElement>(((Json)NWScript.GetLocalJson(Object, Name)).Dump()) : default;
      set => NWScript.SetLocalJson(Object, Name, Json.Parse(JsonSerializer.Serialize(value)));
    }

    public override void Delete()
    {
      NWScript.DeleteLocalJson(Object, Name);
    }
  }
}
