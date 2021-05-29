using JetBrains.Annotations;

namespace NWN.API
{
  public static class ObjectExtensions
  {
    /// <summary>
    /// Gets if this creature is currently being controlled by a player/DM.<br/>
    /// If this creature is a possessed familiar or is DM possessed, this will return true.<br/>
    /// If this creature is a player creature (the creature a played logged in with), but the player is possessing another creature, this returns false.<br/>
    /// If no player is controlling this creature, this returns false.
    /// </summary>
    public static bool IsPlayerControlled([CanBeNull] this NwCreature creature, out NwPlayer player)
    {
      player = creature?.ControllingPlayer;
      return player != null;
    }

    /// <summary>
    /// Gets if this creature is a player character/DM avatar.<br/>
    /// If this creature is a NPC or familiar, regardless of possession, this will return false.
    /// </summary>
    public static bool IsLoginPlayerCharacter([CanBeNull] this NwCreature creature, out NwPlayer player)
    {
      player = creature?.LoginPlayer;
      return player != null;
    }
  }
}
