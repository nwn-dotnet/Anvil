using System.IO;
using Anvil.API;
using Anvil.Internal;

namespace Anvil.Services
{
  /// <summary>
  /// Full directory paths for the anvil home directory.
  /// </summary>
  public static class HomeStorage
  {
    /// <summary>
    /// The base anvil home directory.
    /// </summary>
    private static readonly string AnvilHome = Path.GetFullPath(EnvironmentConfig.AnvilHome, NwServer.Instance.UserDirectory);

    /// <summary>
    /// The NLog config directory.
    /// </summary>
    public static string NLogConfig => ResolveFilePath("nlog.config");

    /// <summary>
    /// The root folder for Paket configuration and package cache.
    /// </summary>
    public static string Paket => ResolveDirectoryPath("Paket");

    /// <summary>
    /// The root plugin data directory. Each plugin is assigned a unique directory in this folder.
    /// </summary>
    public static string PluginData => ResolveDirectoryPath("PluginData");

    /// <summary>
    /// The root plugin directory. Each plugin is assigned a unique directory in this folder.
    /// </summary>
    public static string Plugins => ResolveDirectoryPath("Plugins");

    /// <summary>
    /// The temporary resources directory. Used by some Anvil APIs for ephemeral resources to be visible by ResMan.
    /// </summary>
    public static string ResourceTemp => ResolveDirectoryPath("ResourceTemp", false);

    private static void CreateDirectory(string folderPath)
    {
      // If a file exists at the folder path, check it is a symlink.
      // Running CreateDirectory on the symlink will cause an IOException.
      // If a normal file exists on the path, we should still throw the exception.
      if (!File.Exists(folderPath) || (File.GetAttributes(folderPath) & FileAttributes.ReparsePoint) == 0)
      {
        Directory.CreateDirectory(folderPath);
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
