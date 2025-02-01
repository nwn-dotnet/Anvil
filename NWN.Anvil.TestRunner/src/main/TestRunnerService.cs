using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;
using Microsoft.CodeAnalysis;
using NLog;
using NUnit.Framework.Internal.Commands;
using NUnitLite;

namespace Anvil.TestRunner
{
  [ServiceBinding(typeof(TestRunnerService))]
  [ServiceBindingOptions(BindingPriority = BindingPriority.Lowest)]
  internal sealed class TestRunnerService
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly MainThreadSynchronizationContext mainThreadSynchronizationContext;

    private readonly Queue<Assembly> testAssemblyQueue = new Queue<Assembly>();
    private readonly string outputDir;

    private Thread testWorkerThread;

    public TestRunnerService(MainThreadSynchronizationContext mainThreadSynchronizationContext, PluginStorageService pluginStorageService)
    {
      this.mainThreadSynchronizationContext = mainThreadSynchronizationContext;

      outputDir = pluginStorageService.GetPluginStoragePath(typeof(TestRunnerService).Assembly);
      NwModule.Instance.OnModuleLoad += OnModuleLoad;
      PopulateTestQueue();
    }

    private void PopulateTestQueue()
    {
      foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
      {
        if (assembly.GetCustomAttribute<ExecutePluginTestsAttribute>() != null)
        {
          testAssemblyQueue.Enqueue(assembly);
        }
      }
    }

    private void OnModuleLoad(ModuleEvents.OnModuleLoad eventData)
    {
      TestCommand.DefaultSynchronizationContext = mainThreadSynchronizationContext;
      testWorkerThread = new Thread(RunTests);
      testWorkerThread.Start();
    }

    private void RunTests()
    {
      while (testAssemblyQueue.Count > 0)
      {
        Assembly testAssembly = testAssemblyQueue.Dequeue();

        Log.Info($"Running tests for assembly {testAssembly.FullName}");
        TextRunner testRunner = new TextRunner(testAssembly);
        testRunner.Execute(GetRunnerArguments(testAssembly));
      }

      _ = Shutdown();
    }

    private async Task Shutdown()
    {
      testWorkerThread = null;
      await NwTask.SwitchToMainThread();
      NwServer.Instance.ShutdownServer();
    }

    private string[] GetRunnerArguments(Assembly assembly)
    {
      string outputPath = Path.Combine(outputDir, assembly.GetName().Name!);
      return ["--mainthread", $"--work={outputPath}"];
    }
  }
}
