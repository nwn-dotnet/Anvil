using NWN.Core;

namespace NWN.API
{
  public sealed class LocalVariableFloat : LocalVariable<float>
  {
    public override float Value
    {
      get => NWScript.GetLocalFloat(Object, Name);
      set => NWScript.SetLocalFloat(Object, Name, value);
    }

    public override void Delete()
    {
      NWScript.DeleteLocalFloat(Object, Name);
    }
  }
}
