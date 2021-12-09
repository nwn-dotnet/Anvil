using System;
using NWN.Core;

namespace Anvil.API
{
  /// <summary>
  /// Extension methods for getting and setting options of standard factions.
  /// </summary>
  public static class StandardFactionExtensions
  {
    /// <summary>
    /// Gets an integer between 0 and 100 (inclusive) that represents how this faction feels about the specified target.<br/>
    ///  -> 0-10 means this faction is hostile to the target<br/>
    ///  -> 11-89 means this faction is neutral to the target<br/>
    ///  -> 90-100 means this faction is friendly to the target.
    /// </summary>
    /// <param name="faction">The source faction.</param>
    /// <param name="target">The target object.</param>
    /// <returns>0-100 (inclusive) based on the standing of the target within this standard faction.</returns>
    public static int GetReputation(this StandardFaction faction, NwGameObject target)
    {
      return NWScript.GetStandardFactionReputation((int)faction, target);
    }

    /// <summary>
    /// Sets how this standard faction feels about the specified creature.<br/>
    ///  -> 0-10 means this faction is hostile to the target.<br/>
    ///  -> 11-89 means this faction is neutral to the target.<br/>
    ///  -> 90-100 means this faction is friendly to the target.
    /// </summary>
    /// <param name="faction">The source faction.</param>
    /// <param name="target">The target object.</param>
    /// <param name="newReputation">A value between 0-100 (inclusive).</param>
    public static void SetReputation(this StandardFaction faction, NwGameObject target, int newReputation)
    {
      NWScript.SetStandardFactionReputation((int)faction, newReputation, target);
    }
  }
}
