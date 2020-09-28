using NWN.API.Constants;
using NWN.Core;
using NWNX.API.Constants;

namespace NWN.API
{
  [NativeObjectInfo(ObjectType.AreaOfEffect, InternalObjectType.AreaOfEffect)]
  public class NwAreaOfEffect : NwObject
  {
    internal NwAreaOfEffect(uint objectId) : base(objectId) {}

    /// <summary>
    /// Gets the creator of this Area of Effect.
    /// </summary>
    public NwGameObject Creator
    {
      get => NWScript.GetAreaOfEffectCreator(this).ToNwObject<NwGameObject>();
    }
  }
}
