using NWM.Internal;

namespace NWM.API
{
  public static class Utils
  {
    public static string GetUserDirectory() => NWMInterop.GetUserDirectory();
  }
}