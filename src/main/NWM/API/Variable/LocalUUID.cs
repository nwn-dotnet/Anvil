using System;
using NWN.Core;

namespace NWM.API
{
  public class LocalUUID : LocalVariable<Guid>
  {
    public LocalUUID(NwObject instance, string name) : base(instance, name) {}

    public bool HasValue => !string.IsNullOrEmpty(NWScript.GetLocalString(Object, Name));
    public bool HasNothing => !HasValue;

    public override Guid Value
    {
      get
      {
        if (!HasValue)
        {
          throw new InvalidOperationException("The variable does not have a value");
        }

        return Guid.Parse(NWScript.GetLocalString(Object, Name));
      }
      set => NWScript.SetLocalString(Object, Name, value.ToUUIDString());
    }

    public override void Delete()
    {
      NWScript.DeleteLocalString(Object, Name);
    }
  }
}