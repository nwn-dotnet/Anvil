using NWN.Core.NWNX;

namespace NWNX.API
{
  public static class Feedback
  {
    static Feedback()
    {
      PluginUtils.AssertPluginExists<FeedbackPlugin>();
    }
  }
}