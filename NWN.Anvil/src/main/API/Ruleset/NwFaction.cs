using System;
using System.Collections.Generic;
using Anvil.Internal;
using NWN.Core;
using NWN.Native.API;

namespace Anvil.API
{
  /// <summary>
  /// The faction info of a Creature/Player/GameObject.
  /// </summary>
  public sealed class NwFaction : IEquatable<NwFaction>
  {
    private static readonly CFactionManager FactionManager = LowLevel.ServerExoApp.m_pcExoAppInternal.m_pFactionManager;

    /// <summary>
    /// Gets a list of all active factions.
    /// </summary>
    public static IReadOnlyList<NwFaction> Factions
    {
      get
      {
        CExoArrayListCNWSFactionPtr factions = FactionManager.m_pFactionList;
        NwFaction[] retVal = new NwFaction[factions.Count];
        for (int i = 0; i < retVal.Length; i++)
        {
          retVal[i] = new NwFaction(factions[i]);
        }

        return retVal;
      }
    }

    private readonly CNWSFaction faction;

    internal NwFaction(CNWSFaction faction)
    {
      this.faction = faction;
    }

    /// <summary>
    /// Gets the average Good/Evil alignment value of members in this faction.<br/>
    /// @note This can be a costly operation when used on large NPC factions.
    /// </summary>
    public int AverageGoodEvilAlignment => faction.GetAverageGoodEvilAlignment();

    /// <summary>
    /// Gets the average Law/Chaos alignment value of members in this faction.<br/>
    /// @note This can be a costly operation when used on large NPC factions.
    /// </summary>
    public int AverageLawChaosAlignment => faction.GetAverageLawChaosAlignment();

    /// <summary>
    /// Gets the average level of members in this faction.<br/>
    /// @note This can be a costly operation when used on large NPC factions.
    /// </summary>
    public int AverageLevel => faction.GetAverageLevel();

    /// <summary>
    /// Gets the average amount of XP of members in this faction.<br/>
    /// @note This can be a costly operation when used on large NPC factions.
    /// </summary>
    public int AverageXP => faction.GetAverageXP();

    /// <summary>
    /// Gets the total amount of gold held by all members of this party.<br/>
    /// @note This can be a costly operation when used on large NPC factions.
    /// </summary>
    public int Gold => faction.GetGold();

    /// <summary>
    /// Gets the id of this faction.
    /// </summary>
    public int Id => faction.m_nFactionId;

    /// <summary>
    /// Gets the leader of this player faction (party).<br/>
    /// </summary>
    public NwPlayer? Leader => faction.GetLeader().ToNwPlayer();

    /// <summary>
    /// Gets the most common type of class among the members of this faction/party.<br/>
    /// @note This can be a costly operation when used on large NPC factions.
    /// </summary>
    public NwClass MostFrequentClass => NwClass.FromClassId(faction.GetMostFrequentClass())!;

    /// <summary>
    /// Gets the <see cref="StandardFaction"/> type of this faction.<br/>
    /// If this is a player or custom faction, returns an out-of-range value.
    /// </summary>
    public StandardFaction StandardFactionType => (StandardFaction)Id;

    /// <summary>
    /// Resolves a <see cref="NwFaction"/> from a faction id.
    /// </summary>
    /// <param name="factionId">The id of the faction to resolve.</param>
    /// <returns>The associated <see cref="NwFaction"/> instance. Null if the faction id is invalid.</returns>
    public static NwFaction? FromFactionId(int factionId)
    {
      CNWSFaction faction = FactionManager.GetFaction(factionId);
      return faction != null ? new NwFaction(faction) : null;
    }

    /// <summary>
    /// Resolves a <see cref="NwFaction"/> from a <see cref="StandardFaction"/>.
    /// </summary>
    /// <param name="factionType">The faction type to resolve.</param>
    /// <returns>The associated <see cref="NwFaction"/> instance. Null if the faction type is invalid.</returns>
    public static NwFaction? FromStandardFaction(StandardFaction factionType)
    {
      return FromFactionId((int)factionType);
    }

    public static bool operator ==(NwFaction? left, NwFaction? right)
    {
      return Equals(left, right);
    }

    public static implicit operator NwFaction?(StandardFaction faction)
    {
      return FromFactionId((int)faction);
    }

    public static bool operator !=(NwFaction left, NwFaction right)
    {
      return !Equals(left, right);
    }

    /// <summary>
    /// Adjusts how this faction feels about the specified creature.
    /// </summary>
    /// <param name="creature">The target creature for the reputation change.</param>
    /// <param name="adjustment">The adjustment in reputation to make.</param>
    public void AdjustReputation(NwCreature creature, int adjustment)
    {
      creature.Creature.AdjustReputation(faction.m_nFactionId, adjustment);
    }

    public bool Equals(NwFaction? other)
    {
      if (ReferenceEquals(null, other))
      {
        return false;
      }

      if (ReferenceEquals(this, other))
      {
        return true;
      }

      return faction.Equals(other.faction);
    }

    public override bool Equals(object? obj)
    {
      return ReferenceEquals(this, obj) || obj is NwFaction other && Equals(other);
    }

    /// <summary>
    /// Gets an integer between 0 and 100 (inclusive) that represents how this faction feels about the specified target.<br/>
    ///  -> 0-10 means this faction is hostile to the target<br/>
    ///  -> 11-89 means this faction is neutral to the target<br/>
    ///  -> 90-100 means this faction is friendly to the target.<br/>
    /// </summary>
    /// <param name="target">The target object to check.</param>
    public int GetAverageReputation(NwGameObject target)
    {
      return faction.GetAverageReputation(target);
    }

    /// <summary>
    /// Gets the member with the highest AC in this faction.
    /// </summary>
    /// <param name="referenceCreature">The reference creature. Bonuses and penalties against the reference creature will be considered when finding the best AC member.</param>
    /// <param name="visible">Highly recommended to set to "true" on large NPC factions. Includes only creatures visible to referenceCreature.</param>
    public NwCreature GetBestACMember(NwCreature? referenceCreature = null, bool visible = false)
    {
      return faction.GetBestAC(referenceCreature, visible.ToInt()).ToNwObject<NwCreature>()!;
    }

    public override int GetHashCode()
    {
      return faction.GetHashCode();
    }

    /// <summary>
    /// Gets the least damaged member in this faction that is visible from the specified object.
    /// </summary>
    /// <param name="referenceCreature">The reference creature, used to determine visibility.</param>
    /// <param name="visible">Highly recommended to set to "true" on large NPC factions. Includes only creatures visible to referenceCreature.</param>
    public NwCreature GetLeastDamagedMember(NwCreature? referenceCreature = null, bool visible = false)
    {
      return faction.GetLeastDamagedMember(referenceCreature, visible.ToInt()).ToNwObject<NwCreature>()!;
    }

    /// <summary>
    /// Gets all creatures that are members of this faction.<br/>
    /// @note This can be a very costly operation when used on large NPC factions.
    /// </summary>
    /// <returns>All creatures in this faction.</returns>
    public List<NwCreature> GetMembers()
    {
      List<NwCreature> members = [];

      for (int i = 0; i < faction.m_listFactionMembers.Count; i++)
      {
        NwCreature? member = faction.m_listFactionMembers[i].ToNwObjectSafe<NwCreature>();
        if (member != null)
        {
          members.Add(member);
        }
      }

      return members;
    }

    /// <summary>
    /// Gets the most damaged member in this faction that is visible from the specified object.
    /// </summary>
    /// <param name="referenceCreature">The reference creature, used to determine visibility.</param>
    /// <param name="visible">Highly recommended to set to "true" on large NPC factions. Includes only creatures visible to referenceCreature.</param>
    public NwCreature GetMostDamagedMember(NwCreature? referenceCreature = null, bool visible = false)
    {
      return faction.GetMostDamagedMember(referenceCreature, visible.ToInt()).ToNwObject<NwCreature>()!;
    }

    /// <summary>
    /// Gets an integer between 0 and 100 (inclusive) that represents how this faction feels about the specified target.<br/>
    ///  -> 0-10 means this faction is hostile to the target<br/>
    ///  -> 11-89 means this faction is neutral to the target<br/>
    ///  -> 90-100 means this faction is friendly to the target.
    /// </summary>
    /// <param name="target">The target object.</param>
    /// <returns>0-100 (inclusive) based on the standing of the target within this standard faction.</returns>
    public int GetReputation(NwGameObject target)
    {
      return NWScript.GetStandardFactionReputation(Id, target);
    }

    /// <summary>
    /// Gets the strongest member in this faction that is visible from the specified object.
    /// </summary>
    /// <param name="referenceCreature">The reference creature. Bonuses and penalties against the reference creature will be considered when finding the strongest member.</param>
    /// <param name="visible">Highly recommended to set to "true" on large NPC factions. Includes only creatures visible to referenceCreature.</param>
    public NwCreature GetStrongestMember(NwCreature? referenceCreature = null, bool visible = false)
    {
      return faction.GetStrongestMember(referenceCreature, visible.ToInt()).ToNwObject<NwCreature>()!;
    }

    /// <summary>
    /// Gets the weakest member in this faction that is visible from the specified object.
    /// </summary>
    /// <param name="referenceCreature">The reference creature. Bonuses and penalties against the reference creature will be considered when finding the weakest member.</param>
    /// <param name="visible">Highly recommended to set to "true" on large NPC factions. Includes only creatures visible to referenceCreature.</param>
    public NwCreature GetWeakestMember(NwCreature? referenceCreature = null, bool visible = false)
    {
      return faction.GetWeakestMember(referenceCreature, visible.ToInt()).ToNwObject<NwCreature>()!;
    }

    /// <summary>
    /// Gets the member with the lowest AC in this faction that is visible from the specified object.
    /// </summary>
    /// <param name="referenceCreature">The reference creature. Bonuses and penalties against the reference creature will be considered when finding the worst AC member.</param>
    /// <param name="visible">Highly recommended to set to "true" on large NPC factions. Includes only creatures visible to referenceCreature.</param>
    public NwCreature GetWorstACMember(NwCreature? referenceCreature = null, bool visible = false)
    {
      return faction.GetWorstAC(referenceCreature, visible.ToInt()).ToNwObject<NwCreature>()!;
    }

    /// <summary>
    /// Sets how this faction feels about the specified creature.<br/>
    ///  -> 0-10 means this faction is hostile to the target.<br/>
    ///  -> 11-89 means this faction is neutral to the target.<br/>
    ///  -> 90-100 means this faction is friendly to the target.
    /// </summary>
    /// <param name="target">The target object.</param>
    /// <param name="newReputation">A value between 0-100 (inclusive).</param>
    public void SetReputation(NwGameObject target, int newReputation)
    {
      NWScript.SetStandardFactionReputation(Id, newReputation, target);
    }

    internal void AddMember(NwCreature creature)
    {
      faction.AddMember(creature);
    }
  }
}
