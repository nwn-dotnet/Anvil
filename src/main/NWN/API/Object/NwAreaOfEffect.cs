using NWN.API.Constants;
using NWNX.API.Constants;

namespace NWN.API
{
  [NativeObjectInfo(ObjectType.AreaOfEffect, InternalObjectType.AreaOfEffect)]
  public class NwAreaOfEffect : NwObject
  {
    internal NwAreaOfEffect(uint objectId) : base(objectId) {}
  }
}