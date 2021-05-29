using NWN.Core;

namespace NWN.API
{
  [LocalVariableConverter(typeof(string), typeof(float), typeof(int), typeof(Location), typeof(NwObject))]
  internal class NativeLocalVariableConverters : ILocalVariableConverter<string>, ILocalVariableConverter<float>, ILocalVariableConverter<int>, ILocalVariableConverter<Location>, ILocalVariableConverter<NwObject>, ILocalVariableConverter<Cassowary>
  {
    string ILocalVariableConverter<string>.GetLocal(NwObject nwObject, string name)
    {
      return NWScript.GetLocalString(nwObject, name);
    }

    void ILocalVariableConverter<string>.SetLocal(NwObject nwObject, string name, string value)
    {
      NWScript.SetLocalString(nwObject, name, value);
    }

    void ILocalVariableConverter<string>.ClearLocal(NwObject nwObject, string name)
    {
      NWScript.DeleteLocalString(nwObject, name);
    }

    float ILocalVariableConverter<float>.GetLocal(NwObject nwObject, string name)
    {
      return NWScript.GetLocalFloat(nwObject, name);
    }

    void ILocalVariableConverter<float>.SetLocal(NwObject nwObject, string name, float value)
    {
      NWScript.SetLocalFloat(nwObject, name, value);
    }

    void ILocalVariableConverter<float>.ClearLocal(NwObject nwObject, string name)
    {
      NWScript.DeleteLocalFloat(nwObject, name);
    }

    int ILocalVariableConverter<int>.GetLocal(NwObject nwObject, string name)
    {
      return NWScript.GetLocalInt(nwObject, name);
    }

    void ILocalVariableConverter<int>.SetLocal(NwObject nwObject, string name, int value)
    {
      NWScript.SetLocalInt(nwObject, name, value);
    }

    void ILocalVariableConverter<int>.ClearLocal(NwObject nwObject, string name)
    {
      NWScript.DeleteLocalInt(nwObject, name);
    }

    Location ILocalVariableConverter<Location>.GetLocal(NwObject nwObject, string name)
    {
      return NWScript.GetLocalLocation(nwObject, name);
    }

    void ILocalVariableConverter<Location>.SetLocal(NwObject nwObject, string name, Location value)
    {
      NWScript.SetLocalLocation(nwObject, name, value);
    }

    void ILocalVariableConverter<Location>.ClearLocal(NwObject nwObject, string name)
    {
      NWScript.DeleteLocalLocation(nwObject, name);
    }

    NwObject ILocalVariableConverter<NwObject>.GetLocal(NwObject nwObject, string name)
    {
      return NWScript.GetLocalObject(nwObject, name).ToNwObject();
    }

    void ILocalVariableConverter<NwObject>.SetLocal(NwObject nwObject, string name, NwObject value)
    {
      NWScript.SetLocalObject(nwObject, name, value);
    }

    void ILocalVariableConverter<NwObject>.ClearLocal(NwObject nwObject, string name)
    {
      NWScript.DeleteLocalObject(nwObject, name);
    }

    Cassowary ILocalVariableConverter<Cassowary>.GetLocal(NwObject nwObject, string name)
    {
      return NWScript.GetLocalCassowary(nwObject, name);
    }

    void ILocalVariableConverter<Cassowary>.SetLocal(NwObject nwObject, string name, Cassowary value)
    {
      NWScript.SetLocalCassowary(nwObject, name, value);
    }

    void ILocalVariableConverter<Cassowary>.ClearLocal(NwObject nwObject, string name)
    {
      NWScript.DeleteLocalCassowary(nwObject, name);
    }
  }
}
