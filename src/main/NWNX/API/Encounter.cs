using NWN.Core.NWNX;

namespace NWNX.API
{
  public static class Encounter
  {
    static Encounter()
    {
      PluginUtils.AssertPluginExists<EncounterPlugin>();
    }
  }
}