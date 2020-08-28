using NWN.Core;

namespace NWN.API
{
  [LocalVariableConverter(typeof(string), typeof(float), typeof(int), typeof(Location), typeof(NwObject))]
  internal class NativeLocalVariableConverters : ILocalVariableConverter<string>, ILocalVariableConverter<float>, ILocalVariableConverter<int>, ILocalVariableConverter<Location>, ILocalVariableConverter<NwObject>
  {
    string ILocalVariableConverter<string>.GetLocal(NwObject nwObject, string name)
      => NWScript.GetLocalString(nwObject, name);

    void ILocalVariableConverter<string>.SetLocal(NwObject nwObject, string name, string value)
      => NWScript.SetLocalString(nwObject, name, value);

    void ILocalVariableConverter<string>.ClearLocal(NwObject nwObject, string name)
      => NWScript.DeleteLocalString(nwObject, name);

    float ILocalVariableConverter<float>.GetLocal(NwObject nwObject, string name)
      => NWScript.GetLocalFloat(nwObject, name);

    void ILocalVariableConverter<float>.SetLocal(NwObject nwObject, string name, float value)
      => NWScript.SetLocalFloat(nwObject, name, value);

    void ILocalVariableConverter<float>.ClearLocal(NwObject nwObject, string name)
      => NWScript.DeleteLocalFloat(nwObject, name);

    int ILocalVariableConverter<int>.GetLocal(NwObject nwObject, string name)
      => NWScript.GetLocalInt(nwObject, name);

    void ILocalVariableConverter<int>.SetLocal(NwObject nwObject, string name, int value)
      => NWScript.SetLocalInt(nwObject, name, value);

    void ILocalVariableConverter<int>.ClearLocal(NwObject nwObject, string name)
      => NWScript.DeleteLocalInt(nwObject, name);

    Location ILocalVariableConverter<Location>.GetLocal(NwObject nwObject, string name)
      => NWScript.GetLocalLocation(nwObject, name);

    void ILocalVariableConverter<Location>.SetLocal(NwObject nwObject, string name, Location value)
      => NWScript.SetLocalLocation(nwObject, name, value);

    void ILocalVariableConverter<Location>.ClearLocal(NwObject nwObject, string name)
      => NWScript.DeleteLocalLocation(nwObject, name);

    NwObject ILocalVariableConverter<NwObject>.GetLocal(NwObject nwObject, string name)
      => NWScript.GetLocalObject(nwObject, name).ToNwObject();

    void ILocalVariableConverter<NwObject>.SetLocal(NwObject nwObject, string name, NwObject value)
      => NWScript.SetLocalObject(nwObject, name, value);

    void ILocalVariableConverter<NwObject>.ClearLocal(NwObject nwObject, string name)
      => NWScript.DeleteLocalObject(nwObject, name);
  }
}