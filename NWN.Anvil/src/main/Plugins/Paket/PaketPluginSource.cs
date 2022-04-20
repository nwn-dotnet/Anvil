using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Anvil.Internal;
using Anvil.Services;
using Microsoft.FSharp.Collections;
using Microsoft.FSharp.Core;
using NLog;
using Paket;

namespace Anvil.Plugins
{
  internal sealed class PaketPluginSource : IPluginSource
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    // https://docs.microsoft.com/en-us/nuget/create-packages/supporting-multiple-target-frameworks#architecture-specific-folders
    private static readonly string[] NativeDllPackagePaths = { "runtimes/linux-x64/native" };

    private readonly FSharpList<string> frameworks = ListModule.OfArray(Assemblies.TargetFrameworks);
    private readonly string linkFileFormat = Path.Combine(HomeStorage.Paket, ".paket/load/{0}/main.group.csx");
    private readonly string packagesFolderPath = Path.Combine(HomeStorage.Paket, "packages");

    private readonly string paketFilePath = Path.Combine(HomeStorage.Paket, "paket.dependencies");

    private readonly PluginManager pluginManager;
    private readonly FSharpList<string> scriptTypes = ListModule.OfArray(new[] { "csx" });

    public PaketPluginSource(PluginManager pluginManager)
    {
      this.pluginManager = pluginManager;
    }

    public IEnumerable<Plugin> Bootstrap()
    {
      Dependencies dependencies = GetDependencies();
      if (dependencies == null)
      {
        return Enumerable.Empty<Plugin>();
      }

      Logging.@event.Publish.AddHandler(OnLogEvent);

      InstallPackages(dependencies);
      return CreatePlugins(dependencies);
    }

    private IEnumerable<Plugin> CreatePlugins(Dependencies dependencies)
    {
      PaketAssemblyLoadFile loadFile = null;
      foreach (string framework in Assemblies.TargetFrameworks)
      {
        string linkFilePath = string.Format(linkFileFormat, framework);
        if (File.Exists(linkFilePath))
        {
          loadFile = new PaketAssemblyLoadFile(linkFilePath);
        }
      }

      if (loadFile == null)
      {
        throw new InvalidOperationException("Could not locate link file.");
      }

      Dictionary<string, string> nativeAssemblyPaths = GetNativeAssemblyPaths();

      List<Plugin> plugins = new List<Plugin>();
      FSharpList<Tuple<string, string, string>> entries = dependencies.GetDirectDependencies();

      Log.Info("Loading {PluginCount} DotNET plugin/s from: {PluginPath}", entries.Length, HomeStorage.Paket);
      foreach ((string _, string pluginName, string _) in entries)
      {
        if (Assemblies.IsReservedName(pluginName))
        {
          Log.Warn("Skipping plugin {Plugin} as it uses a reserved name", pluginName);
          continue;
        }

        if (!loadFile.AssemblyPaths.TryGetValue(pluginName, out string pluginPath))
        {
          Log.Warn("Cannot find path for plugin assembly {Plugin}", pluginName);
          continue;
        }

        if (!File.Exists(pluginPath))
        {
          Log.Warn("Cannot find plugin assembly {Plugin}", pluginPath);
          continue;
        }

        Plugin plugin = new Plugin(pluginManager, pluginPath)
        {
          AdditionalAssemblyPaths = loadFile.AssemblyPaths,
          UnmanagedAssemblyPaths = nativeAssemblyPaths,
        };

        plugins.Add(plugin);
      }

      return plugins;
    }

    private Dependencies GetDependencies()
    {
      if (!File.Exists(paketFilePath))
      {
        Log.Info("Skipping initialization of Paket as {PaketFile} does not exist", paketFilePath);
        return null;
      }

      return Dependencies.Locate(paketFilePath);
    }

    private Dictionary<string, string> GetNativeAssemblyPaths()
    {
      Dictionary<string, string> nativeAssemblyPaths = new Dictionary<string, string>();

      string[] packageFolders = Directory.GetDirectories(packagesFolderPath);
      foreach (string nativeSubDir in NativeDllPackagePaths)
      {
        foreach (string packageFolder in packageFolders)
        {
          string path = Path.Combine(packageFolder, nativeSubDir);
          if (Directory.Exists(path))
          {
            foreach (string assembly in Directory.GetFiles(path))
            {
              nativeAssemblyPaths[Path.GetFileNameWithoutExtension(assembly)] = assembly;
            }
          }
        }
      }

      return nativeAssemblyPaths;
    }

    private void InstallPackages(Dependencies dependencies)
    {
      dependencies.Install(false, true, false, false, false, SemVerUpdateMode.NoRestriction, false, true, frameworks, scriptTypes, FSharpOption<string>.None);
    }

    private void OnLogEvent(object sender, Logging.Trace args)
    {
      switch (args.Level)
      {
        case TraceLevel.Error:
          Log.Error(args.Text);
          break;
        case TraceLevel.Warning:
          Log.Warn(args.Text);
          break;
        default:
          Log.Info(args.Text);
          break;
      }
    }
  }
}
