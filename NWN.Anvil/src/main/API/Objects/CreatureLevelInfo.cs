using System;
using System.Collections.Generic;
using System.Linq;
using Anvil.Native;
using NWN.Native.API;

namespace Anvil.API
{
  /// <summary>
  /// Provides level-specific data for a creature, including the chosen class,
  /// feats gained, hit die result, skill points, ability increase, and spellbook changes.
  /// </summary>
  /// <remarks>
  /// Use the per-level containers (feats, skill ranks, known spells added/removed) to inspect or adjust what was taken on this level.
  /// The known spell arrays are indexed by spell level (0 for cantrips, 1–9 for spell levels) and each entry is a mutable list.
  /// </remarks>
  public sealed unsafe class CreatureLevelInfo
  {
    private const int KnownSpellArraySize = 10; // Cantrips + 9 spell levels
    private static readonly int KnownSpellArrayListStructSize = sizeof(IntPtr) + sizeof(int) + sizeof(int);

    private readonly NwCreature creature;
    private readonly CNWLevelStats levelStats;

    internal CreatureLevelInfo(NwCreature creature, CNWLevelStats levelStats)
    {
      this.creature = creature;
      this.levelStats = levelStats;
    }

    /// <summary>
    /// Gets the class chosen at this level.
    /// </summary>
    public CreatureClassInfo ClassInfo
    {
      get
      {
        byte classId = levelStats.m_nClass;
        return creature.Classes.First(info => info.Class.Id == classId);
      }
    }

    /// <summary>
    /// Gets the number of feats gained at this level.
    /// </summary>
    public int FeatCount => levelStats.m_lstFeats.Count;

    /// <summary>
    /// Gets a mutable list of feats gained at this level.
    /// </summary>
    public IList<NwFeat> Feats
    {
      get
      {
        return new ListWrapper<ushort, NwFeat>(levelStats.m_lstFeats, featId => NwFeat.FromFeatId(featId)!, feat => feat.Id);
      }
    }

    /// <summary>
    /// Gets a mutable list of known spells added at this level.
    /// </summary>
    /// <remarks>
    /// The returned array has 10 entries indexed by spell level: 0 = cantrips, 1–9 = spell levels.
    /// Each entry contains a list of spells added on this level; lists may be empty when no changes were made for that level.
    /// </remarks>
    public IReadOnlyList<IList<NwSpell>> AddedKnownSpells
    {
      get
      {
        IList<NwSpell>[] spells = new IList<NwSpell>[KnownSpellArraySize];
        IntPtr ptr = levelStats.m_pAddedKnownSpellList.Pointer;

        for (int i = 0; i < spells.Length; i++)
        {
          CExoArrayListUInt32? spellList = CExoArrayListUInt32.FromPointer(ptr + i * KnownSpellArrayListStructSize);
          spells[i] = new ListWrapper<uint, NwSpell>(spellList, spellId => NwSpell.FromSpellId((int)spellId)!, spell => (uint)spell.Id);
        }

        return spells;
      }
    }

    /// <summary>
    /// Gets a mutable list of known spells removed at this level.
    /// </summary>
    /// <remarks>
    /// The returned array has 10 entries indexed by spell level: 0 = cantrips, 1–9 = spell levels.
    /// Each entry contains a list of spells removed on this level; lists may be empty when no changes were made for that level.
    /// </remarks>
    public IReadOnlyList<IList<NwSpell>> RemovedKnownSpells
    {
      get
      {
        IList<NwSpell>[] spells = new IList<NwSpell>[KnownSpellArraySize];
        IntPtr ptr = levelStats.m_pRemovedKnownSpellList.Pointer;

        for (int i = 0; i < spells.Length; i++)
        {
          CExoArrayListUInt32 spellList = CExoArrayListUInt32.FromPointer(ptr + i * KnownSpellArrayListStructSize);
          spells[i] = new ListWrapper<uint, NwSpell>(spellList, spellId => NwSpell.FromSpellId((int)spellId)!, spell => (uint)spell.Id);
        }

        return spells;
      }
    }

    /// <summary>
    /// Gets or sets the hitpoints gained by this creature for this level.
    /// </summary>
    /// <remarks>
    /// This is the raw hit die result stored for the level and does not auto-recalculate.
    /// </remarks>
    public byte HitDie
    {
      get => levelStats.m_nHitDie;
      set => levelStats.m_nHitDie = value;
    }

    /// <summary>
    /// Gets or sets the amount of skill points unspent at this level.
    /// </summary>
    /// <remarks>
    /// Adjusting this value affects only this level's allocation and does not change ranks directly.
    /// </remarks>
    public ushort SkillPointsRemaining
    {
      get => levelStats.m_nSkillPointsRemaining;
      set => levelStats.m_nSkillPointsRemaining = value;
    }

    /// <summary>
    /// Gets or sets the ability increased at this level.
    /// </summary>
    /// <remarks>
    /// Set to <see langword="null"/> when no ability increase was taken on this level.
    /// </remarks>
    public Ability? AbilityGained
    {
      get => levelStats.m_nAbilityGain <= 5 ? (Ability)levelStats.m_nAbilityGain : null;
      set => levelStats.m_nAbilityGain = value != null ? (byte)value.Value : byte.MaxValue;
    }

    /// <summary>
    /// Gets the skill ranks for the specified skill on this creature level.
    /// </summary>
    /// <param name="skill">The skill to query.</param>
    /// <returns>The number of skill ranks.</returns>
    public sbyte GetSkillRank(NwSkill skill)
    {
      return levelStats.m_lstSkillRanks[skill.Id].AsSByte();
    }

    /// <summary>
    /// Sets the skill ranks for the specified skill on this creature level.
    /// </summary>
    /// <param name="skill">The skill to modify.</param>
    /// <param name="rank">The new number of skill ranks.</param>
    public void SetSkillRank(NwSkill skill, sbyte rank)
    {
      levelStats.m_lstSkillRanks[skill.Id] = rank.AsByte();
    }
  }
}
