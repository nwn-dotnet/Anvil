using NWN.Core;

namespace NWM.API
{
  public class LocalObject : LocalVariable<NwObject>
  {
    public LocalObject(NwObject instance, string name) : base(instance, name) {}

    public override NwObject Value
    {
      get => NWScript.GetLocalObject(Object, Name).ToNwObject();
      set => NWScript.SetLocalObject(Object, Name, value);
    }

    public override void Delete()
    {
      NWScript.DeleteLocalLocation(Object, Name);
    }
  }
}