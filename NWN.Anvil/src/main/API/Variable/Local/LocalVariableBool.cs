using NWN.Core;

namespace Anvil.API
{
  public sealed class LocalVariableBool : LocalVariable<bool>
  {
    public override bool Value
    {
      get => NWScript.GetLocalInt(Object, Name).ToBool();
      set => NWScript.SetLocalInt(Object, Name, value.ToInt());
    }

    public override void Delete()
    {
      NWScript.DeleteLocalInt(Object, Name);
    }
  }
}
