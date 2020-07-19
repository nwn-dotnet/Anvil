using NWN.Core.NWNX;

namespace NWNX.API
{
  public static class Profiler
  {
    static Profiler()
    {
      PluginUtils.AssertPluginExists<ProfilerPlugin>();
    }
  }
}