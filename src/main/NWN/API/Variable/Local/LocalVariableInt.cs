using NWN.Core;

namespace NWN.API
{
  public sealed class LocalVariableInt : LocalVariable<int>
  {
    public override int Value
    {
      get => NWScript.GetLocalInt(Object, Name);
      set => NWScript.SetLocalInt(Object, Name, value);
    }

    public override void Delete()
    {
      NWScript.DeleteLocalInt(Object, Name);
    }
  }
}
