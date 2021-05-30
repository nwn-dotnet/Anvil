using System;
using NWN.Core;

namespace NWN.API
{
  [LocalVariableConverter(typeof(bool), typeof(Guid))]
  internal class ExtendedLocalVariableConverters : ILocalVariableConverter<bool>, ILocalVariableConverter<Guid>
  {
    bool ILocalVariableConverter<bool>.GetLocal(NwObject nwObject, string name)
    {
      return NWScript.GetLocalInt(nwObject, name).ToBool();
    }

    void ILocalVariableConverter<bool>.SetLocal(NwObject nwObject, string name, bool value)
    {
      NWScript.SetLocalInt(nwObject, name, value.ToInt());
    }

    void ILocalVariableConverter<bool>.ClearLocal(NwObject nwObject, string name)
    {
      NWScript.DeleteLocalInt(nwObject, name);
    }

    Guid ILocalVariableConverter<Guid>.GetLocal(NwObject nwObject, string name)
    {
      string stored = NWScript.GetLocalString(nwObject, name);
      return string.IsNullOrEmpty(stored) ? Guid.Empty : Guid.Parse(stored);
    }

    void ILocalVariableConverter<Guid>.SetLocal(NwObject nwObject, string name, Guid value)
    {
      NWScript.SetLocalString(nwObject, name, value.ToUUIDString());
    }

    void ILocalVariableConverter<Guid>.ClearLocal(NwObject nwObject, string name)
    {
      NWScript.DeleteLocalString(nwObject, name);
    }
  }
}
