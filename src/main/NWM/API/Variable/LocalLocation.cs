using NWN.Core;

namespace NWM.API
{
  public class LocalLocation : LocalVariable<Location>
  {
    public LocalLocation(NwObject instance, string name) : base(instance, name) {}

    public override Location Value
    {
      get => NWScript.GetLocalLocation(Object, Name);
      set => NWScript.SetLocalLocation(Object, Name, value);
    }

    public override void Delete()
    {
      NWScript.DeleteLocalLocation(Object, Name);
    }
  }
}