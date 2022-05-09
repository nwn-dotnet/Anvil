using System.IO;
using Anvil.API;
using Anvil.Internal;

namespace Anvil.Services
{
  internal static class HomeStorage
  {
    private static readonly string AnvilHome = Path.GetFullPath(EnvironmentConfig.AnvilHome, NwServer.Instance.UserDirectory);

    public static string NLogConfig => ResolveFilePath("nlog.config");

    public static string Paket => ResolveDirectoryPath("Paket");

    public static string PluginData => ResolveDirectoryPath("PluginData");

    public static string Plugins => ResolveDirectoryPath("Plugins");

    public static string ResourceTemp => ResolveDirectoryPath("ResourceTemp", false);

    private static void CreateDirectory(string folderPath)
    {
      // If a file exists at the folder path, check it is a symlink.
      // Running CreateDirectory on the symlink will cause an IOException.
      // If a normal file exists on the path, we should still throw the exception.
      if (!File.Exists(folderPath) || (File.GetAttributes(folderPath) & FileAttributes.ReparsePoint) == 0)
      {
        Directory.CreateDirectory(folderPath!);
      }
    }

    private static string ResolveDirectoryPath(string subPath, bool createIfMissing = true)
    {
      string path = Path.Combine(AnvilHome, subPath);
      if (createIfMissing)
      {
        CreateDirectory(path);
      }

      return path;
    }

    private static string ResolveFilePath(string subPath, bool createParentsIfMissing = true)
    {
      string path = Path.Combine(AnvilHome, subPath);
      if (createParentsIfMissing)
      {
        CreateDirectory(Path.GetDirectoryName(path)!);
      }

      return path;
    }
  }
}
