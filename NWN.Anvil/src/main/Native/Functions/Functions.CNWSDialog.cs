using Anvil.Services;

// ReSharper disable InconsistentNaming
namespace Anvil.Native
{
  internal static unsafe partial class Functions
  {
    public static class CNWSDialog
    {
      [NativeFunction("_ZN10CNWSDialog11CheckScriptEP10CNWSObjectRK7CResRefRK13CExoArrayListI11ScriptParamE", "")]
      public delegate int CheckScript(void* pDialog, void* pNWSObjectOwner, void* sActive, void* scriptParams);

      [NativeFunction("_ZN10CNWSDialog13GetStartEntryEP10CNWSObject", "")]
      public delegate uint GetStartEntry(void* pDialog, void* pNWSObjectOwner);

      [NativeFunction("_ZN10CNWSDialog21GetStartEntryOneLinerEP10CNWSObjectR13CExoLocStringR7CResRefS5_R13CExoArrayListI11ScriptParamE", "")]
      public delegate int GetStartEntryOneLiner(void* pDialog, void* pNWSObjectOwner, void* sOneLiner, void* sSound, void* sScript, void* scriptParams);

      [NativeFunction("_ZN10CNWSDialog11HandleReplyEjP10CNWSObjectjij", "")]
      public delegate int HandleReply(void* pDialog, uint nPlayerID, void* pNWSObjectOwner, uint nReplyIndex, int bEscapeDialog, uint currentEntryIndex);

      [NativeFunction("_ZN10CNWSDialog9RunScriptEP10CNWSObjectRK7CResRefRK13CExoArrayListI11ScriptParamE", "")]
      public delegate void RunScript(void* pDialog, void* pNWSObjectOwner, void* sScript, void* scriptParams);

      [NativeFunction("_ZN10CNWSDialog15SendDialogEntryEP10CNWSObjectjji", "")]
      public delegate int SendDialogEntry(void* pDialog, void* pNWSObjectOwner, uint nPlayerIdGUIOnly, uint iEntry, int bPlayHelloSound);

      [NativeFunction("_ZN10CNWSDialog17SendDialogRepliesEP10CNWSObjectj", "")]
      public delegate int SendDialogReplies(void* pDialog, void* pNWSObjectOwner, uint nPlayerIdGUIOnly);
    }
  }
}
