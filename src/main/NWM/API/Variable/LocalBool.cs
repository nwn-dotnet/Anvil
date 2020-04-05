using NWN;

namespace NWM.API
{
  public class LocalBool : LocalVariable<bool>
  {
    public LocalBool(NwObject instance, string name) : base(instance, name) {}

    public override bool Value
    {
      get => NWScript.GetLocalInt(Object, Name).ToBool();
      set
      {
        if (value)
        {
          NWScript.SetLocalInt(Object, Name, value.ToInt());
        }
        else
        {
          Delete();
        }
      }
    }

    public override void Delete()
    {
      NWScript.DeleteLocalInt(Object, Name);
    }
  }
}