using NWN.API;
using NWN.Core.NWNX;
using NWNX.API.Constants;

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

    public static NodeType GetCurrentNodeType() =>
        (NodeType) DialogPlugin.GetCurrentNodeType();

    public static ScriptType GetCurrentScriptType() =>
        (ScriptType)DialogPlugin.GetCurrentScriptType();

    public static int GetCurrentNodeID() =>
        DialogPlugin.GetCurrentNodeID();

    public static int GetCurrentNodeIndex() =>
        DialogPlugin.GetCurrentNodeIndex();

    public static void End(this NwCreature creature) =>
        DialogPlugin.End(creature);
  }
}
