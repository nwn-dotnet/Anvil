using System;
using System.Linq;
using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;
using Microsoft.CodeAnalysis;

namespace Anvil.Tests
{
  [ServiceBinding(typeof(TestRunnerService))]
  internal sealed class TestRunnerService
  {
    [Inject]
    private PluginStorageService PluginStorageService { get; init; }

    public TestRunnerService()
    {
      NwModule.Instance.OnModuleLoad += OnModuleLoad;
    }

    private void OnModuleLoad(ModuleEvents.OnModuleLoad eventData)
    {
      string[] args = GetRunnerArguments();
      AnvilTestRunner testRunner = new AnvilTestRunner(typeof(TestRunnerService).Assembly);

      testRunner.Execute(args);
    }

    private string[] GetRunnerArguments()
    {
      string outputPath = PluginStorageService.GetPluginStoragePath(typeof(TestRunnerService).Assembly);
      string args = $"--work={outputPath}";
      return string.IsNullOrEmpty(args) ? Array.Empty<string>() : CommandLineParser.SplitCommandLineIntoArguments(args, false).ToArray();
    }
  }
}
