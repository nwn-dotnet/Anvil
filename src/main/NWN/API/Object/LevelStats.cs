using System.Collections.Generic;
using NWN.Native.API;
using ClassType = NWN.API.Constants.ClassType;
using Feat = NWN.API.Constants.Feat;

namespace NWN.API
{
  public class LevelStats
  {
    private readonly NwCreature creature;
    private readonly CNWLevelStats levelStats;

    public LevelStats(NwCreature creature, CNWLevelStats levelStats)
    {
      this.creature = creature;
      this.levelStats = levelStats;
    }

    /// <summary>
    /// Gets the feats gained at this level.
    /// </summary>
    public IEnumerable<Feat> Feats
    {
      get
      {
        for (int i = 0; i < levelStats.m_lstFeats.num; i++)
        {
          yield return (Feat)levelStats.m_lstFeats._OpIndex(i).Read();
        }
      }
    }

    /// <summary>
    /// Gets the class chosen at this level.
    /// </summary>
    public ClassType Class
    {
      get => creature.Classes[levelStats.m_nClass - 1].Type;
    }

    /// <summary>
    /// Gets the amount of skill points unspent at this level.
    /// </summary>
    public ushort SkillPointsRemaining
    {
      get => levelStats.m_nSkillPointsRemaining;
      set => levelStats.m_nSkillPointsRemaining = value;
    }
  }
}
