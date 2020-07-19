using NWN.Core.NWNX;

namespace NWNX.API
{
  public static class SkillRanks
  {
    static SkillRanks()
    {
      PluginUtils.AssertPluginExists<SkillranksPlugin>();
    }
  }
}