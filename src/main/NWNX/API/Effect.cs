using NWN.Core.NWNX;

namespace NWNX.API
{
  public static class Effect
  {
    static Effect()
    {
      PluginUtils.AssertPluginExists<EffectPlugin>();
    }
  }
}