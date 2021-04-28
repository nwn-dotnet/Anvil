using System;
using NWN.API;
using NWN.API.Constants;
using NWN.Core.NWNX;

namespace NWNX.API
{
  // TODO implement plugin functions
  public static class Creature
  {
    static Creature()
    {
      PluginUtils.AssertPluginExists<CreaturePlugin>();
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
