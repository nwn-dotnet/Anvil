using NWN.Core.NWNX;

namespace NWMX.API
{
  public static class Dialog
  {
    static Dialog()
    {
      PluginUtils.AssertPluginExists<DialogPlugin>();
    }

    public static string CurrentNodeText
    {
      get => DialogPlugin.GetCurrentNodeText();
      set => DialogPlugin.SetCurrentNodeText(value);
    }
  }
}