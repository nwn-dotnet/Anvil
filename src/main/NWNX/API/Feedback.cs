using NWN.Core.NWNX;

namespace NWNX.API
{
  // TODO implement plugin functions
  public static class Feedback
  {
    static Feedback()
    {
      PluginUtils.AssertPluginExists<FeedbackPlugin>();
    }
  }
}
