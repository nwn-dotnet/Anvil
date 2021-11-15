using System.IO;
using Anvil.API;
using Anvil.Internal;

namespace Anvil.Services
{
  internal static class HomeStorage
  {
    private static readonly string AnvilHome = Path.GetFullPath(EnvironmentConfig.AnvilHome, NwServer.Instance.UserDirectory);

    internal static string PluginStorage
    {
      get => ResolvePath("Plugins");
    }

    internal static string Paket
    {
      get => ResolvePath("Paket");
    }

    private static string ResolvePath(string subPath)
    {
      string path = Path.Combine(AnvilHome, subPath);
      Directory.CreateDirectory(path);
      return path;
    }
  }
}
