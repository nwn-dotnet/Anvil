using NWN.API.Constants;
using NWNX.API.Constants;

namespace NWN.API
{
  [NativeObjectInfo(ObjectType.Trigger, InternalObjectType.Trigger)]
  public sealed class NwTrigger : NwTrappable
  {
    internal NwTrigger(uint objectId) : base(objectId) {}
  }
}