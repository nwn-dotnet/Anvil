using NWN.Core.NWNX;

namespace NWNX.API
{
  // TODO implement plugin functions
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
