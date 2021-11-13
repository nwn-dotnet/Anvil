using System.Diagnostics;
using System.IO;
using Anvil.Internal;
using Anvil.Services;
using Microsoft.FSharp.Collections;
using Microsoft.FSharp.Core;
using NLog;
using Paket;

namespace Anvil.Plugins
{
  [ServiceBinding(typeof(PaketPluginService))]
  public sealed class PaketPluginService
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly FSharpList<string> frameworks = ListModule.OfArray(new[] { Assemblies.TargetFramework });
    private readonly FSharpList<string> scriptTypes = ListModule.OfArray(new[] { "csx" });

    private readonly string paketFile;

    public PaketPluginService(HomeStorageService homeStorageService)
    {
      paketFile = Path.Combine(homeStorageService.Paket, "paket.dependencies");
      if (!File.Exists(paketFile))
      {
        Log.Info("Skipping initialization of Paket as {PaketFile} does not exist", paketFile);
        return;
      }

      Logging.@event.Publish.AddHandler(OnLogEvent);
      InstallDependencies();
    }

    private void InstallDependencies()
    {
      Dependencies dependencies = Dependencies.Locate(paketFile);
      dependencies.Install(false, false, false, false, false, SemVerUpdateMode.NoRestriction, false, true, frameworks, scriptTypes, FSharpOption<string>.None);
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
