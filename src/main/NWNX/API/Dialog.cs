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

/* 
int NWNX_Dialog_GetCurrentNodeType()
{
    string sFunc = "GetCurrentNodeType";

    NWNX_CallFunction(NWNX_Dialog, sFunc);
    return NWNX_GetReturnValueInt(NWNX_Dialog, sFunc);
}

int NWNX_Dialog_GetCurrentScriptType()
{
    string sFunc = "GetCurrentScriptType";

    NWNX_CallFunction(NWNX_Dialog, sFunc);
    return NWNX_GetReturnValueInt(NWNX_Dialog, sFunc);
}

int NWNX_Dialog_GetCurrentNodeID()
{
    string sFunc = "GetCurrentNodeID";

    NWNX_CallFunction(NWNX_Dialog, sFunc);
    return NWNX_GetReturnValueInt(NWNX_Dialog, sFunc);
}

int NWNX_Dialog_GetCurrentNodeIndex()
{
    string sFunc = "GetCurrentNodeIndex";

    NWNX_CallFunction(NWNX_Dialog, sFunc);
    return NWNX_GetReturnValueInt(NWNX_Dialog, sFunc);
}