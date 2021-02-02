using System;
using System.Collections.Generic;
using NWN.Native.API;
using ClassType = NWN.API.Constants.ClassType;

namespace NWN.API
{
  /// <summary>
  /// The faction info of a Creature/Player/GameObject.
  /// </summary>
  public sealed class NwFaction : IEquatable<NwFaction>
  {
    internal readonly CNWSFaction Faction;

    internal NwFaction(CNWSFaction faction)
    {
      this.Faction = faction;
    }

    /// <summary>
    /// Gets the most common type of class among the members of this faction/party.<br/>
    /// @note This can be a costly operation when used on large NPC factions.
    /// </summary>
    public ClassType MostFrequentClass
    {
      get => (ClassType)Faction.GetMostFrequentClass();
    }

    /// <summary>
    /// Gets the average level of members in this faction.<br/>
    /// @note This can be a costly operation when used on large NPC factions.
    /// </summary>
    public int AverageLevel
    {
      get => Faction.GetAverageLevel();
    }

    /// <summary>
    /// Gets the average amount of XP of members in this faction.<br/>
    /// @note This can be a costly operation when used on large NPC factions.
    /// </summary>
    public int AverageXP
    {
      get => Faction.GetAverageXP();
    }

    /// <summary>
    /// Gets the total amount of gold held by all members of this party.<br/>
    /// @note This can be a costly operation when used on large NPC factions.
    /// </summary>
    public int Gold
    {
      get => Faction.GetGold();
    }

    /// <summary>
    /// Gets the average Good/Evil alignment value of members in this faction.<br/>
    /// @note This can be a costly operation when used on large NPC factions.
    /// </summary>
    public int AverageGoodEvilAlignment
    {
      get => Faction.GetAverageGoodEvilAlignment();
    }

    /// <summary>
    /// Gets the average Law/Chaos alignment value of members in this faction.<br/>
    /// @note This can be a costly operation when used on large NPC factions.
    /// </summary>
    public int AverageLawChaosAlignment
    {
      get => Faction.GetAverageLawChaosAlignment();
    }

    /// <summary>
    /// Gets the leader of this player faction (party).<br/>
    /// </summary>
    public NwPlayer Leader
    {
      get => Faction.GetLeader().ToNwObject<NwPlayer>();
    }

    /// <summary>
    /// Gets the members in this faction.<br/>
    /// @note This can be a very costly operation when used on large NPC factions.
    /// </summary>
    /// <typeparam name="T">The type of members to get.</typeparam>
    /// <returns>All members in this faction of type T.</returns>
    public unsafe List<T> GetMembers<T>() where T : NwCreature
    {
      List<T> members = new List<T>();

      for (int i = 0; i < Faction.m_listFactionMembers.num; i++)
      {
        T member = (*Faction.m_listFactionMembers._OpIndex(i)).ToNwObjectSafe<T>();
        members.Add(member);
      }

      return members;
    }

    /// <summary>
    /// Gets an integer between 0 and 100 (inclusive) that represents how this faction feels about the specified target.<br/>
    ///  -> 0-10 means this faction is hostile to the target<br/>
    ///  -> 11-89 means this faction is neutral to the target<br/>
    ///  -> 90-100 means this faction is friendly to the target.<br/>
    /// </summary>
    /// <param name="target">The target object to check.</param>
    public int GetAverageReputation(NwGameObject target)
      => Faction.GetAverageReputation(target);

    /// <summary>
    /// Gets the member with the highest AC in this faction.
    /// </summary>
    /// <param name="referenceCreature">The reference creature. Bonuses and penalties against the reference creature will be considered when finding the best AC member.</param>
    /// <param name="visible">Highly recommended to set to "true" on large NPC factions. Includes only creatures visible to referenceCreature.</param>
    public NwCreature GetBestACMember(NwCreature referenceCreature = null, bool visible = false)
      => Faction.GetBestAC(referenceCreature, visible.ToInt()).ToNwObject<NwCreature>();

    /// <summary>
    /// Gets the member with the lowest AC in this faction that is visible from the specified object.
    /// </summary>
    /// <param name="referenceCreature">The reference creature. Bonuses and penalties against the reference creature will be considered when finding the worst AC member.</param>
    /// <param name="visible">Highly recommended to set to "true" on large NPC factions. Includes only creatures visible to referenceCreature.</param>
    public NwCreature GetWorstACMember(NwCreature referenceCreature = null, bool visible = false)
      => Faction.GetWorstAC(referenceCreature, visible.ToInt()).ToNwObject<NwCreature>();

    /// <summary>
    /// Gets the weakest member in this faction that is visible from the specified object.
    /// </summary>
    /// <param name="referenceCreature">The reference creature. Bonuses and penalties against the reference creature will be considered when finding the weakest member.</param>
    /// <param name="visible">Highly recommended to set to "true" on large NPC factions. Includes only creatures visible to referenceCreature.</param>
    public NwCreature GetWeakestMember(NwCreature referenceCreature = null, bool visible = false)
      => Faction.GetWeakestMember(referenceCreature, visible.ToInt()).ToNwObject<NwCreature>();

    /// <summary>
    /// Gets the strongest member in this faction that is visible from the specified object.
    /// </summary>
    /// <param name="referenceCreature">The reference creature. Bonuses and penalties against the reference creature will be considered when finding the strongest member.</param>
    /// <param name="visible">Highly recommended to set to "true" on large NPC factions. Includes only creatures visible to referenceCreature.</param>
    public NwCreature GetStrongestMember(NwCreature referenceCreature = null, bool visible = false)
      => Faction.GetStrongestMember(referenceCreature, visible.ToInt()).ToNwObject<NwCreature>();

    /// <summary>
    /// Gets the most damaged member in this faction that is visible from the specified object.
    /// </summary>
    /// <param name="referenceCreature">The reference creature, used to determine visibility.</param>
    /// <param name="visible">Highly recommended to set to "true" on large NPC factions. Includes only creatures visible to referenceCreature.</param>
    public NwCreature GetMostDamagedMember(NwCreature referenceCreature = null, bool visible = false)
      => Faction.GetMostDamagedMember(referenceCreature, visible.ToInt()).ToNwObject<NwCreature>();

    /// <summary>
    /// Gets the least damaged member in this faction that is visible from the specified object.
    /// </summary>
    /// <param name="referenceCreature">The reference creature, used to determine visibility.</param>
    /// <param name="visible">Highly recommended to set to "true" on large NPC factions. Includes only creatures visible to referenceCreature.</param>
    public NwCreature GetLeastDamagedMember(NwCreature referenceCreature = null, bool visible = false)
      => Faction.GetLeastDamagedMember(referenceCreature, visible.ToInt()).ToNwObject<NwCreature>();

    /// <summary>
    /// Adjusts how this faction feels about the specified creature.
    /// </summary>
    /// <param name="creature">The target creature for the reputation change.</param>
    /// <param name="adjustment">The adjustment in reputation to make.</param>
    public void AdjustReputation(NwCreature creature, int adjustment)
      => creature.Creature.AdjustReputation(Faction.m_nFactionId, adjustment);

    internal void AddMember(NwCreature creature)
    {
      Faction.AddMember(creature);
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

      return Faction.Equals(other.Faction);
    }

    public override bool Equals(object obj)
    {
      return ReferenceEquals(this, obj) || obj is NwFaction other && Equals(other);
    }

    public override int GetHashCode()
    {
      return Faction.GetHashCode();
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
