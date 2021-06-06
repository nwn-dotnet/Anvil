using JetBrains.Annotations;

namespace NWN.API
{
  public static class ObjectExtensions
  {
    /// <summary>
    /// Gets if this object is currently being controlled by a player/DM.<br/>
    /// If this creature is a possessed familiar or is DM possessed, this will return true.<br/>
    /// If this creature is a player creature (the creature a played logged in with), but the player is possessing another creature, this returns false.<br/>
    /// If no player is controlling this creature, this returns false.
    /// </summary>
    public static bool IsPlayerControlled([CanBeNull] this NwObject gameObject, out NwPlayer player)
    {
      player = gameObject is NwCreature creature ? creature.ControllingPlayer : null;
      return player != null;
    }

    /// <summary>
    /// Gets if this object is a player character/DM avatar.<br/>
    /// If this creature is a NPC or familiar, regardless of possession, this will return false.
    /// </summary>
    public static bool IsLoginPlayerCharacter([CanBeNull] this NwObject gameObject, out NwPlayer player)
    {
      player = gameObject is NwCreature creature ? creature.LoginPlayer : null;
      return player != null;
    }
  }
}
