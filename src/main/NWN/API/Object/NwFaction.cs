using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NWN.API.Constants;
using NWN.Core;

namespace NWN.API
{
  /// <summary>
  /// The faction info of a Creature/Player/GameObject
  /// </summary>
  public sealed class NwFaction : IEquatable<NwFaction>
  {
    internal readonly NwGameObject GameObject;

    internal NwFaction(NwGameObject gameObject)
    {
      this.GameObject = gameObject;
    }

    /// <summary>
    /// Gets the most common type of class among the members of this faction/party.<br/>
    /// @note This can be a costly operation when used on large NPC factions.
    /// </summary>
    public ClassType MostFrequentClass
    {
      get => (ClassType) NWScript.GetFactionMostFrequentClass(GameObject);
    }

    /// <summary>
    /// Gets the average level of members in this faction.<br/>
    /// @note This can be a costly operation when used on large NPC factions.
    /// </summary>
    public int AverageLevel
    {
      get => NWScript.GetFactionAverageLevel(GameObject);
    }

    /// <summary>
    /// Gets the average amount of XP of members in this faction.<br/>
    /// @note This can be a costly operation when used on large NPC factions.
    /// </summary>
    public int AverageXP
    {
      get => NWScript.GetFactionAverageXP(GameObject);
    }

    /// <summary>
    /// Get the total amount of gold held by all members of this party.<br/>
    /// @note This can be a costly operation when used on large NPC factions.
    /// </summary>
    public int Gold
    {
      get => NWScript.GetFactionGold(GameObject);
    }

    /// <summary>
    /// Gets the average Good/Evil alignment value of members in this faction.<br/>
    /// @note This can be a costly operation when used on large NPC factions.
    /// </summary>
    public int AverageGoodEvilAlignment
    {
      get => NWScript.GetFactionAverageGoodEvilAlignment(GameObject);
    }

    /// <summary>
    /// Gets the average Law/Chaos alignment value of members in this faction.<br/>
    /// @note This can be a costly operation when used on large NPC factions.
    /// </summary>
    public int AverageLawChaosAlignment
    {
      get => NWScript.GetFactionAverageLawChaosAlignment(GameObject);
    }

    /// <summary>
    /// Gets the member with the highest AC in this faction.<br/>
    /// @note This can be a costly operation when used on large NPC factions. Consider using <see cref="VisibleHighestACMember"/> instead.
    /// </summary>
    public NwGameObject HighestACMember
    {
      get => NWScript.GetFactionBestAC(GameObject, false.ToInt()).ToNwObject<NwGameObject>();
    }

    /// <summary>
    /// Gets the member with the lowest AC in this faction.<br/>
    /// @note This can be a costly operation when used on large NPC factions. Consider using <see cref="VisibleLowestACMember"/> instead.
    /// </summary>
    public NwGameObject LowestACMember
    {
      get => NWScript.GetFactionWorstAC(GameObject, false.ToInt()).ToNwObject<NwGameObject>();
    }

    /// <summary>
    /// Gets the weakest member in this faction.<br/>
    /// @note This can be a costly operation when used on large NPC factions. Consider using <see cref="VisibleWeakestMember"/> instead.
    /// </summary>
    public NwGameObject WeakestMember
    {
      get => NWScript.GetFactionWeakestMember(GameObject, false.ToInt()).ToNwObject<NwGameObject>();
    }

    /// <summary>
    /// Gets the strongest member in this faction.<br/>
    /// @note This can be a costly operation when used on large NPC factions. Consider using <see cref="VisibleStrongestMember"/> instead.
    /// </summary>
    public NwGameObject StrongestMember
    {
      get => NWScript.GetFactionStrongestMember(GameObject, false.ToInt()).ToNwObject<NwGameObject>();
    }

    /// <summary>
    /// Gets the most damaged member in this faction.<br/>
    /// @note This can be a costly operation when used on large NPC factions. Consider using <see cref="VisibleMostDamagedMember"/> instead.
    /// </summary>
    public NwGameObject MostDamagedMember
    {
      get => NWScript.GetFactionMostDamagedMember(GameObject, false.ToInt()).ToNwObject<NwGameObject>();
    }

    /// <summary>
    /// Gets the least damaged member in this faction.<br/>
    /// @note This can be a costly operation when used on large NPC factions. Consider using <see cref="VisibleLeastDamagedMember"/> instead.
    /// </summary>
    public NwGameObject LeastDamagedMember
    {
      get => NWScript.GetFactionLeastDamagedMember(GameObject, false.ToInt()).ToNwObject<NwGameObject>();
    }

    /// <summary>
    /// Gets the leader of this player faction (party)<br/>
    /// </summary>
    public NwPlayer Leader
    {
      get => NWScript.GetFactionLeader(GameObject).ToNwObject<NwPlayer>();
    }

    /// <summary>
    /// Gets the members in this faction.<br/>
    /// @note This can be a very costly operation when used on large NPC factions.
    /// </summary>
    /// <typeparam name="T">The type of members to get.</typeparam>
    /// <returns>All members in this faction of type T.</returns>
    public IEnumerable<T> GetMembers<T>() where T : NwGameObject
    {
      int pcOnly = (typeof(T) == typeof(NwPlayer)).ToInt();

      for (uint obj = NWScript.GetFirstFactionMember(GameObject, pcOnly);
        obj != NWScript.OBJECT_INVALID;
        obj = NWScript.GetNextFactionMember(GameObject, pcOnly))
      {
        T next = obj.ToNwObject<T>();
        if (next != null)
        {
          yield return next;
        }
      }
    }

    /// <summary>
    /// Gets an integer between 0 and 100 (inclusive) that represents how this faction feels about the specified target.<br/>
    ///  -> 0-10 means this faction is hostile to the target<br/>
    ///  -> 11-89 means this faction is neutral to the target<br/>
    ///  -> 90-100 means this faction is friendly to the target<br/>
    /// </summary>
    /// <param name="target">The target object to check.</param>
    /// <returns></returns>
    public int GetAverageReputation(NwGameObject target)
      => NWScript.GetFactionAverageReputation(GameObject, target);

    /// <summary>
    /// Gets the member with the highest AC in this faction that is visible from the specified object.
    /// </summary>
    public async Task<NwGameObject> VisibleHighestACMember(NwGameObject visibleFrom)
    {
      await visibleFrom.WaitForObjectContext();
      return NWScript.GetFactionBestAC(GameObject, true.ToInt()).ToNwObject<NwGameObject>();
    }

    /// <summary>
    /// Gets the member with the lowest AC in this faction that is visible from the specified object.
    /// </summary>
    public async Task<NwGameObject> VisibleLowestACMember(NwGameObject visibleFrom)
    {
      await visibleFrom.WaitForObjectContext();
      return NWScript.GetFactionWorstAC(GameObject, true.ToInt()).ToNwObject<NwGameObject>();
    }

    /// <summary>
    /// Gets the weakest member in this faction that is visible from the specified object.
    /// </summary>
    public async Task<NwGameObject> VisibleWeakestMember(NwGameObject visibleFrom)
    {
      await visibleFrom.WaitForObjectContext();
      return NWScript.GetFactionWeakestMember(GameObject, true.ToInt()).ToNwObject<NwGameObject>();
    }

    /// <summary>
    /// Gets the strongest member in this faction that is visible from the specified object.
    /// </summary>
    public async Task<NwGameObject> VisibleStrongestMember(NwGameObject visibleFrom)
    {
      await visibleFrom.WaitForObjectContext();
      return NWScript.GetFactionStrongestMember(GameObject, true.ToInt()).ToNwObject<NwGameObject>();
    }

    /// <summary>
    /// Gets the most damaged member in this faction that is visible from the specified object.
    /// </summary>
    public async Task<NwGameObject> VisibleMostDamagedMember(NwGameObject visibleFrom)
    {
      await visibleFrom.WaitForObjectContext();
      return NWScript.GetFactionMostDamagedMember(GameObject, true.ToInt()).ToNwObject<NwGameObject>();
    }

    /// <summary>
    /// Gets the least damaged member in this faction that is visible from the specified object.
    /// </summary>
    public async Task<NwGameObject> VisibleLeastDamagedMember(NwGameObject visibleFrom)
    {
      await visibleFrom.WaitForObjectContext();
      return NWScript.GetFactionLeastDamagedMember(GameObject, true.ToInt()).ToNwObject<NwGameObject>();
    }

    public bool Equals(NwFaction other)
    {
      if (ReferenceEquals(null, other))
      {
        return false;
      }

      if (ReferenceEquals(this, other))
      {
        return true;
      }

      return NWScript.GetFactionEqual(GameObject, other.GameObject).ToBool();
    }

    public override bool Equals(object obj)
    {
      return ReferenceEquals(this, obj) || obj is NwFaction other && Equals(other);
    }

    public override int GetHashCode()
    {
      return (GameObject != null ? GameObject.GetHashCode() : 0);
    }

    public static bool operator ==(NwFaction left, NwFaction right)
    {
      return Equals(left, right);
    }

    public static bool operator !=(NwFaction left, NwFaction right)
    {
      return !Equals(left, right);
    }
  }
}