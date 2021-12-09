using System.Collections.Generic;
using NWN.Native.API;

namespace Anvil.API
{
  public sealed unsafe class CreatureLevelInfo
  {
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
    public NwClass Class
    {
      get => creature.Classes[levelStats.m_nClass - 1];
    }

    /// <summary>
    /// Gets the number of feats gained at this level.
    /// </summary>
    public int FeatCount
    {
      get => levelStats.m_lstFeats.Count;
    }

    /// <summary>
    /// Gets the feats gained at this level.
    /// </summary>
    public IReadOnlyList<NwFeat> Feats
    {
      get
      {
        NwFeat[] feats = new NwFeat[FeatCount];

        for (int i = 0; i < feats.Length; i++)
        {
          feats[i] = NwFeat.FromFeatId(levelStats.m_lstFeats[i]);
        }

        return feats;
      }
    }

    /// <summary>
    /// Gets or sets the hitpoints gained by this creature for this level.
    /// </summary>
    public byte HitDie
    {
      get => levelStats.m_nHitDie;
      set => levelStats.m_nHitDie = value;
    }

    /// <summary>
    /// Gets or sets the amount of skill points unspent at this level.
    /// </summary>
    public ushort SkillPointsRemaining
    {
      get => levelStats.m_nSkillPointsRemaining;
      set => levelStats.m_nSkillPointsRemaining = value;
    }

    /// <summary>
    /// Gets the skill ranks for the specified skill on this creature level.
    /// </summary>
    /// <param name="skill">The skill to query.</param>
    /// <returns>The number of skill ranks.</returns>
    public sbyte GetSkillRank(Skill skill)
    {
      return levelStats.m_lstSkillRanks[(int)skill].AsSByte();
    }

    /// <summary>
    /// Sets the skill ranks for the specified skill on this creature level.
    /// </summary>
    /// <param name="skill">The skill to modify.</param>
    /// <param name="rank">The new number of skill ranks.</param>
    public void SetSkillRank(Skill skill, sbyte rank)
    {
      levelStats.m_lstSkillRanks[(int)skill] = rank.AsByte();
    }
  }
}
