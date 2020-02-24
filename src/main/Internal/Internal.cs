using System.Runtime.InteropServices;

namespace NWM.Internal
{
  public static class Internal
  {
    [DllImport("DotNETInterop")]
    internal static extern string GetUserDirectory();

    [DllImport("DotNETInterop")]
    internal static extern byte GetObjectType(uint objectId);
  }
}