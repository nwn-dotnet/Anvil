using System.IO;
using Anvil.API;
using Anvil.Internal;

namespace Anvil.Services
{
  internal static class HomeStorage
  {
    private static readonly string AnvilHome = Path.GetFullPath(EnvironmentConfig.AnvilHome, NwServer.Instance.UserDirectory);

    public static string Plugins
    {
      get => ResolveDirectoryPath("Plugins");
    }

    public static string PluginData
    {
      get => ResolveDirectoryPath("PluginData");
    }

    public static string Paket
    {
      get => ResolveDirectoryPath("Paket");
    }

    public static string ResourceTemp
    {
      get => ResolveDirectoryPath("ResourceTemp", false);
    }

    public static string NLogConfig
    {
      get => ResolveFilePath("nlog.config");
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

    private static void CreateDirectory(string folderPath)
    {
      // If a file exists at the folder path, return if it is a symlink...
      if (File.Exists(folderPath) && (File.GetAttributes(folderPath) & FileAttributes.ReparsePoint) != 0)
      {
        return;
      }

      // ...Otherwise attempt to create the directory normally. If a file is at the path, Directory.CreateDirectory will throw an exception.
      Directory.CreateDirectory(folderPath);
    }
  }
}
