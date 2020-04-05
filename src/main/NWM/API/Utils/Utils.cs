using NWN;
using NWNX;

namespace NWM.API
{
  public static class Utils
  {
    public static string GetUserDirectory() => UtilPlugin.GetUserDirectory();
    public static NwPlayer GetPCSpeaker() => NWScript.GetPCSpeaker().ToNwObject<NwPlayer>();
  }
}