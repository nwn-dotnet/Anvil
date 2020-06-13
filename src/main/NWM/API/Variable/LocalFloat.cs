using NWN.Core;

namespace NWM.API
{
  public class LocalFloat : LocalVariable<float>
  {
    public LocalFloat(NwObject instance, string name) : base(instance, name) {}

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