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
    /// Gets the feats gained at this level.
    /// </summary>
    public IReadOnlyList<Feat> Feats
    {
      get
      {
        Feat[] feats = new Feat[FeatCount];

        for (int i = 0; i < feats.Length; i++)
        {
          feats[i] = (Feat)levelStats.m_lstFeats.element[i];
        }

        return feats;
      }
    }

    /// <summary>
    /// Gets the number of feats gained at this level.
    /// </summary>
    public int FeatCount
    {
      get => levelStats.m_lstFeats.num;
    }

    /// <summary>
    /// Gets the class chosen at this level.
    /// </summary>
    public ClassType Class
    {
      get => creature.Classes[levelStats.m_nClass - 1];
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
    /// Gets or sets the hitpoints gained by this creature for this level.
    /// </summary>
    public byte HitDie
    {
      get => levelStats.m_nHitDie;
      set => levelStats.m_nHitDie = value;
    }
  }
}
