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

    private static readonly string PaketFile = "paket.dependencies";
    private static readonly string PackagesFolder = "packages";
    private static readonly string LinkFile = $".paket/load/{Assemblies.TargetFramework}/main.group.csx";
    private static readonly string[] NativeDllPackagePaths = { "runtimes/linux-x64/native" };

    private readonly PluginManager pluginManager;

    private readonly FSharpList<string> frameworks = ListModule.OfArray(new[] { Assemblies.TargetFramework });
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

    private Dependencies GetDependencies()
    {
      string paketFile = Path.Combine(HomeStorage.Paket, PaketFile);
      if (!File.Exists(paketFile))
      {
        Log.Info("Skipping initialization of Paket as {PaketFile} does not exist", paketFile);
        return null;
      }

      return Dependencies.Locate(paketFile);
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

    private void InstallPackages(Dependencies dependencies)
    {
      dependencies.Install(false, true, false, false, false, SemVerUpdateMode.NoRestriction, false, true, frameworks, scriptTypes, FSharpOption<string>.None);
    }

    private IEnumerable<Plugin> CreatePlugins(Dependencies dependencies)
    {
      PaketAssemblyLoadFile loadFile = new PaketAssemblyLoadFile(Path.Combine(HomeStorage.Paket, LinkFile));
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

    private Dictionary<string, string> GetNativeAssemblyPaths()
    {
      Dictionary<string, string> nativeAssemblyPaths = new Dictionary<string, string>();

      string[] packageFolders = Directory.GetDirectories(Path.Combine(HomeStorage.Paket, PackagesFolder));
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
  }
}
