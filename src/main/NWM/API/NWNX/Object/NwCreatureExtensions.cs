using NWM.API.Constants;
using NWNX;

namespace NWM.API.NWNX
{
  public static class NwCreatureExtensions
  {
    static NwCreatureExtensions()
    {
      PluginUtils.AssertPluginExists<CreaturePlugin>();
    }

    public static void AddFeat(this NwCreature creature, Feat feat, int level = 0)
    {
      if (level > 0)
      {
        CreaturePlugin.AddFeatByLevel(creature, feat, level);
      }
      else
      {
        CreaturePlugin.AddFeat(creature, feat);
      }
    }

    public static bool KnowsFeat(this NwCreature creature, Feat feat)
    {
      return CreaturePlugin.GetKnowsFeat(creature, feat).ToBool();
    }

    public static void RemoveFeat(this NwCreature creature, Feat feat)
    {
      CreaturePlugin.RemoveFeat(creature, feat);
    }

    public static int GetFeatLevel(this NwCreature creature, Feat feat)
    {
      return CreaturePlugin.GetFeatLevel(creature, feat);
    }
  }
}