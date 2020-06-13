using NWN.Core;

namespace NWM.API
{
  public class LocalInt : LocalVariable<int>
  {
    public LocalInt(NwObject instance, string name) : base(instance, name) {}
    
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