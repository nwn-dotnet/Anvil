using System;
using NWN.Core;

namespace Anvil.API
{
  public sealed class CampaignVariableObject<T> : CampaignVariable<T> where T : NwGameObject
  {
    public override T Value
    {
      get => throw new NotSupportedException("Value get is not supported for object variables as objects require a spawn location/owner. Use GetValue instead.");
      set => NWScript.StoreCampaignObject(Campaign, Name, value, Player?.ControlledCreature);
    }

    /// <summary>
    /// Gets the object from the campaign database, and spawns it at the specified location.
    /// </summary>
    /// <param name="location">The location for the object to be spawned at.</param>
    /// <returns>The created object.</returns>
    public T? GetValue(Location location)
    {
      return NWScript.RetrieveCampaignObject(Campaign, Name, location, NwObject.Invalid, Player?.ControlledCreature).ToNwObject<T>();
    }

    /// <summary>
    /// Gets the object from the campaign database, and attempts to spawn it in the inventory of the specified object.<br/>
    /// If the owner cannot carry the object, it will be spawned at the owner's current location.
    /// </summary>
    /// <param name="owner">The owner who should receive the spawned object.</param>
    /// <returns>The created object.</returns>
    public T? GetValue(NwGameObject owner)
    {
      Location? location = owner.Location;
      if (location == null)
      {
        return null;
      }

      return NWScript.RetrieveCampaignObject(Campaign, Name, location, owner, Player?.ControlledCreature).ToNwObject<T>();
    }
  }
}
