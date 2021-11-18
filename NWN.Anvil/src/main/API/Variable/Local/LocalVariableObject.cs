using NWN.Core;

namespace Anvil.API
{
  public sealed class LocalVariableObject<T> : LocalVariable<T> where T : NwObject
  {
    public override T Value
    {
      get => NWScript.GetLocalObject(Object, Name).ToNwObject<T>();
      set => NWScript.SetLocalObject(Object, Name, value);
    }

    public override void Delete()
    {
      NWScript.DeleteLocalObject(Object, Name);
    }
  }
}
