using NWM.API.Constants;
using NWN;
using NWNX;

namespace NWM.API
{
  public static class Utils
  {
    public static string GetUserDirectory() => UtilPlugin.GetUserDirectory();
    public static NwPlayer GetPCSpeaker() => NWScript.GetPCSpeaker().ToNwObject<NwPlayer>();
    public static Spell GetSpellId() => (Spell) NWScript.GetSpellId();
  }
}