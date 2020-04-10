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
        CreaturePlugin.AddFeatByLevel(creature, (int) feat, level);
      }
      else
      {
        CreaturePlugin.AddFeat(creature, (int) feat);
      }
    }

    public static bool KnowsFeat(this NwCreature creature, Feat feat)
    {
      return CreaturePlugin.GetKnowsFeat(creature, (int) feat).ToBool();
    }

    public static void RemoveFeat(this NwCreature creature, Feat feat)
    {
      CreaturePlugin.RemoveFeat(creature, (int) feat);
    }

    public static int GetFeatLevel(this NwCreature creature, Feat feat)
    {
      return CreaturePlugin.GetFeatLevel(creature, (int) feat);
    }

    public static int GetMemorisedSpellCountByLevel(this NwCreature creature, ClassType classType, int spellLevel)
    {
      return CreaturePlugin.GetMemorisedSpellCountByLevel(creature, (int) classType, spellLevel);
    }

    public static void ClearMemorisedSpell(this NwCreature creature, ClassType classType, int spellLevel, int index)
    {
      CreaturePlugin.ClearMemorisedSpell(creature, (int) classType, spellLevel, index);
    }

    public static ClassType GetClassByLevel(this NwCreature creature, int level)
    {
      return (ClassType) CreaturePlugin.GetClassByLevel(creature, level);
    }
  }
}