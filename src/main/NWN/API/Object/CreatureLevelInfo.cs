using System.Collections.Generic;
using NWN.Native.API;
using ClassType = NWN.API.Constants.ClassType;
using Feat = NWN.API.Constants.Feat;

namespace NWN.API
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
    public List<Feat> Feats
    {
      get
      {
        List<Feat> feats = new List<Feat>(levelStats.m_lstFeats.num);
        for (int i = 0; i < levelStats.m_lstFeats.num; i++)
        {
          feats.Add((Feat)(*levelStats.m_lstFeats._OpIndex(i)));
        }

        return feats;
      }
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
  }
}
