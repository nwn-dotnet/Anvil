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
  [TestFixture]
  public sealed class PluginTests
  {
    private const string CoreScriptName = "anvtest_core";

    [ServiceBinding(typeof(CoreScriptHandlerTestService))]
    private sealed class CoreScriptHandlerTestService
    {
      public static int CallCount;

      [ScriptHandler(CoreScriptName)]
      private void OnScript()
      {
        CallCount++;
      }
    }

    [Inject]
    private static PluginManager PluginManager { get; set; } = null!;

    [Inject]
    private static PluginStorageService PluginStorageService { get; set; } = null!;

    [Inject]
    private static VirtualMachine VirtualMachine { get; set; } = null!;

    [Inject]
    private static ScriptHandleFactory ScriptHandleFactory { get; set; } = null!;

    // Test Dependencies
    [Inject]
    private static SchedulerService SchedulerService { get; set; } = null!;

    [Inject]
    private static PluginTestDependency PluginTestDependency { get; set; } = null!;

    [Inject]
    private static HookService HookService { get; set; } = null!;

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
        [nameof(IUpdateable)],
        [nameof(IInitializable), nameof(IUpdateable), nameof(IDisposable)],
        implementation);

      string pluginPath = CreatePlugin(pluginName, source);
      WeakReference pluginRef = await RunPluginLifecycleTest(pluginPath, serviceName);

      WaitAndCheckForPluginUnload(pluginRef);
    }

    [MethodImpl(MethodImplOptions.NoInlining)] // Required to allow GC/unload of plugin.
    private async Task<WeakReference> RunPluginLifecycleTest(string pluginPath, string serviceName)
    {
      (Plugin plugin, Type pluginServiceType) = LoadPlugin(pluginPath, serviceName);

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

      await NwTask.NextFrame();

      return pluginRef;
    }

    [Test]
    public void PluginAnvilApiTest()
    {
      const string pluginName = "AnvilApiTestPlugin";
      const string serviceName = "AnvilApiTestService";

      const string implementation =
        """
          public static string ModuleName;
          public static NwServer ServerInstance;

          public void Init()
          {
            ModuleName = NwModule.Instance.Name;
            ServerInstance = NwServer.Instance;
          }
        """;

      string source = PluginTestUtils.GenerateServiceClass(serviceName,
        [nameof(System), $"{nameof(Anvil)}.{nameof(Anvil.Services)}", $"{nameof(Anvil)}.{nameof(Anvil.API)}"],
        [],
        [nameof(IInitializable)],
        implementation);

      string pluginPath = CreatePlugin(pluginName, source);
      WeakReference pluginRef = RunPluginAnvilApiTest(pluginPath, serviceName);

      WaitAndCheckForPluginUnload(pluginRef);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private WeakReference RunPluginAnvilApiTest(string pluginPath, string serviceName)
    {
      (Plugin plugin, Type pluginServiceType) = LoadPlugin(pluginPath, serviceName);

      FieldInfo moduleNameField = pluginServiceType.GetField("ModuleName", BindingFlags.Public | BindingFlags.Static)!;
      FieldInfo serverInstanceField = pluginServiceType.GetField("ServerInstance", BindingFlags.Public | BindingFlags.Static)!;
      Assert.That(moduleNameField.GetValue(null), Is.EqualTo(NwModule.Instance.Name));
      Assert.That(serverInstanceField.GetValue(null), Is.EqualTo(NwServer.Instance));

      return PluginManager.UnloadPlugin(plugin, false);
    }

    [Test]
    public void PluginEventSubscriptionsRemovedOnUnloadTest()
    {
      const string pluginName = "EventSubscriptionTestPlugin";
      const string serviceName = "EventSubscriptionTestService";

      // Subscriptions are intentionally never removed: Anvil must remove them when the plugin unloads.
      const string implementation =
        """
          public void Init()
          {
            NwModule.Instance.OnHeartbeat += OnModuleHeartbeat;
            NwModule.Instance.Areas.First().OnHeartbeat += _ => {};
          }

          private void OnModuleHeartbeat(ModuleEvents.OnHeartbeat eventData) {}
        """;

      string source = PluginTestUtils.GenerateServiceClass(serviceName,
        [nameof(System), "System.Linq", $"{nameof(Anvil)}.{nameof(Anvil.Services)}", $"{nameof(Anvil)}.{nameof(Anvil.API)}", $"{nameof(Anvil)}.{nameof(Anvil.API)}.{nameof(Anvil.API.Events)}"],
        [],
        [nameof(IInitializable)],
        implementation);

      string pluginPath = CreatePlugin(pluginName, source);
      WeakReference pluginRef = RunPluginLoadAndUnload(pluginPath, serviceName);

      WaitAndCheckForPluginUnload(pluginRef);
    }

    [Test]
    public void PluginScriptHandlersRemovedOnUnloadTest()
    {
      const string pluginName = "ScriptHandleTestPlugin";
      const string serviceName = "ScriptHandleTestService";
      const string scriptName = "anvtest_handle";

      // The handle is intentionally never disposed: Anvil must remove it when the plugin unloads.
      const string implementation =
        $$"""
          public ScriptHandleTestService(ScriptHandleFactory scriptHandleFactory)
          {
            scriptHandleFactory.RegisterScriptHandler("{{scriptName}}", _ => ScriptHandleResult.Handled);
          }
        """;

      string source = PluginTestUtils.GenerateServiceClass(serviceName,
        [nameof(System), $"{nameof(Anvil)}.{nameof(Anvil.Services)}", $"{nameof(Anvil)}.{nameof(Anvil.API)}"],
        [],
        [],
        implementation);

      string pluginPath = CreatePlugin(pluginName, source);
      WeakReference pluginRef = RunPluginLoadAndUnload(pluginPath, serviceName);

      Assert.That(ScriptHandleFactory.IsScriptRegistered(scriptName), Is.False);
      WaitAndCheckForPluginUnload(pluginRef);
    }

    [Test]
    public void PluginScriptHandlerAttributeTest()
    {
      const string pluginName = "ScriptHandlerAttributeTestPlugin";
      const string serviceName = "ScriptHandlerAttributeTestService";
      const string scriptName = "anvtest_attr";

      const string implementation =
        $$"""
          public static int CallCount;

          [ScriptHandler("{{scriptName}}")]
          private void OnScript()
          {
            CallCount++;
          }
        """;

      string source = PluginTestUtils.GenerateServiceClass(serviceName,
        [nameof(System), $"{nameof(Anvil)}.{nameof(Anvil.Services)}"],
        [],
        [],
        implementation);

      string pluginPath = CreatePlugin(pluginName, source);
      WeakReference pluginRef = RunPluginScriptHandlerAttributeTest(pluginPath, serviceName, scriptName);

      WaitAndCheckForPluginUnload(pluginRef);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private WeakReference RunPluginScriptHandlerAttributeTest(string pluginPath, string serviceName, string scriptName)
    {
      (Plugin plugin, Type pluginServiceType) = LoadPlugin(pluginPath, serviceName);
      FieldInfo callCountField = pluginServiceType.GetField("CallCount", BindingFlags.Public | BindingFlags.Static)!;

      VirtualMachine.Execute(scriptName);
      Assert.That(callCountField.GetValue(null), Is.EqualTo(1));

      int coreCallsBeforeUnload = CoreScriptHandlerTestService.CallCount;
      WeakReference pluginRef = PluginManager.UnloadPlugin(plugin, false);

      // Unloading a plugin must not strip the script handlers of core services.
      VirtualMachine.Execute(CoreScriptName);
      Assert.That(CoreScriptHandlerTestService.CallCount, Is.EqualTo(coreCallsBeforeUnload + 1));

      return pluginRef;
    }

    [Test]
    public void PluginStaticInjectionTest()
    {
      const string pluginName = "StaticInjectionTestPlugin";
      const string serviceName = "StaticInjectionTestService";

      const string implementation =
        """
          [Inject]
          public static ScriptHandleFactory ScriptHandleFactory { get; set; }
        """;

      string source = PluginTestUtils.GenerateServiceClass(serviceName,
        [nameof(System), $"{nameof(Anvil)}.{nameof(Anvil.Services)}"],
        [],
        [],
        implementation);

      string pluginPath = CreatePlugin(pluginName, source);
      WeakReference pluginRef = RunPluginStaticInjectionTest(pluginPath, serviceName);

      WaitAndCheckForPluginUnload(pluginRef);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private WeakReference RunPluginStaticInjectionTest(string pluginPath, string serviceName)
    {
      (Plugin plugin, Type pluginServiceType) = LoadPlugin(pluginPath, serviceName);
      PropertyInfo property = pluginServiceType.GetProperty("ScriptHandleFactory", BindingFlags.Public | BindingFlags.Static)!;
      Assert.That(property.GetValue(null), Is.Not.Null);

      return PluginManager.UnloadPlugin(plugin, false);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private WeakReference RunPluginLoadAndUnload(string pluginPath, string serviceName)
    {
      (Plugin plugin, Type _) = LoadPlugin(pluginPath, serviceName);
      return PluginManager.UnloadPlugin(plugin, false);
    }

    [Test]
    public void PluginDependencyTest()
    {
      const string pluginName = "DependencyTestPlugin";
      const string serviceName = "DependencyTestService";

      const string implementation =
        """
          public static SchedulerService SchedulerService;
          public static PluginManager PluginManager;
          public static HookService HookService;
          public static PluginTestDependency PluginTestDependency;

          [Inject]
          private static PluginManager InjectedPluginManager { get; set; } = null!;

          [Inject]
          private HookService InjectedHookService { get; init; } = null!;

          public DependencyTestService(SchedulerService schedulerService, PluginTestDependency pluginTestDependency)
          {
            SchedulerService = schedulerService;
            PluginTestDependency = pluginTestDependency;
          }

          public void Init()
          {
            PluginManager = InjectedPluginManager;
            HookService = InjectedHookService;
          }
        """;

      string source = PluginTestUtils.GenerateServiceClass(serviceName,
        [nameof(System), $"{nameof(Anvil)}.{nameof(Anvil.Services)}", $"{nameof(Anvil)}.{nameof(Anvil.API)}", $"{nameof(Anvil)}.{nameof(Anvil.Plugins)}", $"{nameof(Anvil)}.{nameof(Tests)}.{nameof(Plugins)}"],
        [],
        [nameof(IInitializable)],
        implementation);

      string pluginPath = CreatePlugin(pluginName, source);
      WeakReference pluginRef = RunPluginDependencyTest(pluginPath, serviceName);

      WaitAndCheckForPluginUnload(pluginRef);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private WeakReference RunPluginDependencyTest(string pluginPath, string serviceName)
    {
      (Plugin plugin, Type pluginServiceType) = LoadPlugin(pluginPath, serviceName);

      FieldInfo schedulerServiceField = pluginServiceType.GetField("SchedulerService", BindingFlags.Public | BindingFlags.Static)!;
      FieldInfo pluginManagerField = pluginServiceType.GetField("PluginManager", BindingFlags.Public | BindingFlags.Static)!;
      FieldInfo hookServiceField = pluginServiceType.GetField("HookService", BindingFlags.Public | BindingFlags.Static)!;
      FieldInfo testDependencyField = pluginServiceType.GetField("PluginTestDependency", BindingFlags.Public | BindingFlags.Static)!;

      Assert.That(schedulerServiceField.GetValue(null), Is.EqualTo(SchedulerService));
      Assert.That(pluginManagerField.GetValue(null), Is.EqualTo(PluginManager));
      Assert.That(hookServiceField.GetValue(null), Is.EqualTo(HookService));
      Assert.That(testDependencyField.GetValue(null), Is.EqualTo(PluginTestDependency));

      WeakReference pluginRef = PluginManager.UnloadPlugin(plugin, false);

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

    [MethodImpl(MethodImplOptions.NoInlining)]
    private (Plugin, Type) LoadPlugin(string pluginPath, string serviceName)
    {
      Plugin plugin = PluginManager.LoadPlugin(pluginPath);

      Assert.That(plugin, Is.Not.Null);
      Assert.That(plugin.IsLoaded, Is.True);
      Assert.That(plugin.PluginInfo, Is.Not.Null);
      Assert.That(plugin.Assembly, Is.Not.Null);

      Type pluginServiceType = plugin.Assembly!.GetType(serviceName)!;
      Assert.That(pluginServiceType, Is.Not.Null);

      return (plugin, pluginServiceType);
    }

    private void WaitAndCheckForPluginUnload(WeakReference pluginRef)
    {
      for (int i = 0; i < 10 && pluginRef.IsAlive; i++)
      {
        GC.Collect();
        GC.WaitForPendingFinalizers();
      }

      Assert.That(pluginRef.IsAlive, Is.False.After(10000, 1000), "Plugin was not unloaded");
    }
  }

  [ServiceBinding(typeof(PluginTestDependency))]
  public class PluginTestDependency;
}
