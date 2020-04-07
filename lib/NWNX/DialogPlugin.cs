using NWM.API.Constants;
using NWM.API.NWNX.Constants;
using NWN;

namespace NWNX
{
  [NWNXPlugin(PLUGIN_NAME)]
  public class DialogPlugin
  {
    private const string PLUGIN_NAME = "NWNX_Dialog";

    public static void SetCurrentNodeText(string text, DialogLanguages language = DialogLanguages.English, Gender gender = Gender.Male)
    {
      Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "SetCurrentNodeText");
      Internal.NativeFunctions.nwnxPushInt((int) gender);
      Internal.NativeFunctions.nwnxPushInt((int) language);
      Internal.NativeFunctions.nwnxPushString(text);
      Internal.NativeFunctions.nwnxCallFunction();
    }
  }
}