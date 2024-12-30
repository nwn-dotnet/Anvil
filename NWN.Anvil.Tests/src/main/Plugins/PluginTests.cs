using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Anvil.API;
using Anvil.Plugins;
using Anvil.Services;
using Anvil.Tests.Generators;
using NUnit.Framework;

namespace Anvil.Tests.Plugins
{
  [TestFixture(Category = "Plugins")]
  public sealed class PluginTests
  {
    [Inject]
    private static PluginManager PluginManager { get; set; } = null!;

    [Inject]
    private static PluginStorageService PluginStorageService { get; set; } = null!;

    [Test]
    public async Task PluginLifecycleTest()
    {
      const string pluginName = "LifecycleTestPlugin";
      const string serviceName = "PluginLifecycleTestService";

      const string implementation =
        """
          public static bool InitCalled;
          public static bool UpdateCalled;
          public static bool DisposeCalled;

          public void Init()
          {
            InitCalled = true;
          }

          public void Update()
          {
            UpdateCalled = true;
          }

          public void Dispose()
          {
            DisposeCalled = true;
          }
        """;

      string source = PluginTestUtils.GenerateServiceClass(serviceName,
        [nameof(System), $"{nameof(Anvil)}.{nameof(Anvil.Services)}"],
        [nameof(IInitializable), nameof(IUpdateable), nameof(IDisposable)],
        implementation);

      string pluginPath = CreatePlugin(pluginName, source);
      WeakReference pluginRef = await RunPluginLifecycleTest(pluginPath, serviceName);

      GC.Collect();
      GC.WaitForPendingFinalizers();

      Assert.That(pluginRef.IsAlive, Is.False.After(10000, 1000), "Plugin was not unloaded");
    }

    [MethodImpl(MethodImplOptions.NoInlining)] // Required to allow GC/unload of plugin.
    private async Task<WeakReference> RunPluginLifecycleTest(string pluginPath, string serviceName)
    {
      Plugin plugin = PluginManager.LoadPlugin(pluginPath);

      Assert.That(plugin, Is.Not.Null);
      Assert.That(plugin.IsLoaded, Is.True);
      Assert.That(plugin.Assembly, Is.Not.Null);

      Type pluginServiceType = plugin.Assembly!.GetType(serviceName)!;
      Assert.That(pluginServiceType, Is.Not.Null);

      FieldInfo initCalledField = pluginServiceType.GetField("InitCalled", BindingFlags.Public | BindingFlags.Static)!;
      FieldInfo updateCalledField = pluginServiceType.GetField("UpdateCalled", BindingFlags.Public | BindingFlags.Static)!;
      FieldInfo disposeCalledField = pluginServiceType.GetField("DisposeCalled", BindingFlags.Public | BindingFlags.Static)!;

      Assert.That(initCalledField, Is.Not.Null);
      Assert.That(updateCalledField, Is.Not.Null);
      Assert.That(disposeCalledField, Is.Not.Null);

      Assert.That(initCalledField.GetValue(null), Is.True);
      Assert.That(updateCalledField.GetValue(null), Is.False);
      Assert.That(disposeCalledField.GetValue(null), Is.False);

      await NwTask.NextFrame();

      Assert.That(updateCalledField.GetValue(null), Is.True);
      Assert.That(disposeCalledField.GetValue(null), Is.False);

      WeakReference pluginRef = PluginManager.UnloadPlugin(plugin, false);
      await NwTask.NextFrame();

      Assert.That(disposeCalledField.GetValue(null), Is.True);

      return pluginRef;
    }

    private string CreatePlugin(string pluginName, string source)
    {
      string pluginRoot = Path.Combine(PluginStorageService.GetPluginStoragePath(typeof(PluginTests).Assembly), "TestPlugins", pluginName);
      if (Directory.Exists(pluginRoot))
      {
        Directory.Delete(pluginRoot, true);
      }

      Directory.CreateDirectory(pluginRoot);

      string pluginPath = Path.Combine(pluginRoot, $"{pluginName}.dll");

      using FileStream assemblyStream = File.Create(pluginPath);
      AssemblyGenerator.GenerateAssembly(assemblyStream, pluginName, source);

      return pluginRoot;
    }
  }
}
