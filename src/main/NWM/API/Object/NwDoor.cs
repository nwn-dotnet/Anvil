using NWM.API.Constants;
using NWMX.API.Constants;

namespace NWM.API
{
  [NativeObjectInfo(ObjectType.Door, InternalObjectType.Door)]
  public sealed class NwDoor : NwStationary
  {
    internal NwDoor(uint objectId) : base(objectId) {}
  }
}