using NWN.Core;

namespace Anvil.API
{
  /// <summary>
  /// A local variable generic structure.
  /// </summary>
  /// <remarks>
  /// This local variable type uses a JSON local variable to serialize/deserialize any C# type compatible with <see cref="System.Text.Json.JsonSerializer"/>.<br/>
  /// It is not very fast due to interop parsing. Although it's a very flexible API, do take caution in using it.
  /// </remarks>
  /// <typeparam name="T">The class type to serialize/deserialize.</typeparam>
  public sealed class LocalVariableStruct<T> : LocalVariable<T>
  {
    public override T? Value
    {
      get => HasValue ? JsonUtility.FromJson<T>(NWScript.GetLocalJson(Object, Name)) : default;
      set => NWScript.SetLocalJson(Object, Name, JsonUtility.ToJsonStructure(value));
    }

    public override void Delete()
    {
      NWScript.DeleteLocalJson(Object, Name);
    }
  }
}
