using System;
using NWN.Core;

namespace NWN.API
{
  public sealed class LocalVariableGuid : LocalVariable<Guid>
  {
    public override Guid Value
    {
      get
      {
        string stored = NWScript.GetLocalString(Object, Name);
        return string.IsNullOrEmpty(stored) ? Guid.Empty : Guid.Parse(stored);
      }
      set => NWScript.SetLocalString(Object, Name, value.ToUUIDString());
    }

    public override void Delete()
    {
      NWScript.DeleteLocalString(Object, Name);
    }
  }
}
