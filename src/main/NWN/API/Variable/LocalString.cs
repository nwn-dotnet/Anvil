using NWN.Core;

namespace NWN.API
{
  public class LocalString : LocalVariable<string>
  {
    public LocalString(NwObject instance, string name) : base(instance, name) {}

    public override string Value
    {
      get => NWScript.GetLocalString(Object, Name);
      set
      {
        if (value != null)
        {
          NWScript.SetLocalString(Object, Name, value);
        }
        else
        {
          Delete();
        }
      }
    }

    public override void Delete()
    {
      NWScript.DeleteLocalString(Object, Name);
    }
  }
}