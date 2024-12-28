using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Anvil.Internal;
using Anvil.Plugins;
using LightInject;
using NLog;

namespace Anvil.Services
{
  internal sealed class AnvilServiceManager : IServiceManager
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly AnvilContainerFactory anvilContainerFactory;
    private readonly AnvilCoreServiceManager anvilCoreServiceManager;
    private PluginManager pluginManager = null!;

    private bool shuttingDown;
    private bool anvilContainerAvailable;

    public ServiceContainer AnvilServiceContainer { get; private set; }

    public ServiceContainer CoreServiceContainer { get; }

    public event Action<IServiceContainer>? OnContainerCreate;
    public event Action<IServiceContainer>? OnContainerDispose;

    public AnvilServiceManager()
    {
      anvilContainerFactory = new AnvilContainerFactory(this);

      CoreServiceContainer = anvilContainerFactory.CreateContainer();
      AnvilServiceContainer = anvilContainerFactory.CreateContainer(CoreServiceContainer);

      anvilCoreServiceManager = new AnvilCoreServiceManager(CoreServiceContainer);
    }

    public void InjectProperties(object? instance)
    {
      if (anvilContainerAvailable)
      {
        AnvilServiceContainer.InjectProperties(instance);
      }
      else
      {
        CoreServiceContainer.InjectProperties(instance);
      }
    }

    IServiceContainer IServiceManager.CreatePluginContainer(IEnumerable<Type> pluginTypes)
    {
      ServiceContainer pluginContainer = anvilContainerFactory.CreateContainer(AnvilServiceContainer);
      foreach (Type type in pluginTypes)
      {
        TryRegisterAnvilService(pluginContainer, type);
      }

      pluginContainer.ConstructAllServices();
      OnContainerCreate?.Invoke(pluginContainer);
      return pluginContainer;
    }

    void IServiceManager.DisposePluginContainer(IServiceContainer container)
    {
      OnContainerDispose?.Invoke(container);
      container.Dispose();
    }

    void IServiceManager.Init()
    {
      pluginManager = CoreServiceContainer.GetInstance<PluginManager>();
      anvilCoreServiceManager.Init();
    }

    void IServiceManager.Load()
    {
      anvilCoreServiceManager.Load();

      Log.Info("Loading anvil services...");
      InstallAnvilServiceContainer();
      anvilContainerAvailable = true;
    }

    void IServiceManager.Shutdown()
    {
      // Prevent multiple invocations
      if (shuttingDown)
      {
        return;
      }

      shuttingDown = true;
      anvilCoreServiceManager.Shutdown();
    }

    void IServiceManager.Start()
    {
      anvilCoreServiceManager.Start();
      AnvilServiceContainer.ConstructAllServices();
      OnContainerCreate?.Invoke(AnvilServiceContainer);
    }

    void IServiceManager.Unload()
    {
      UnloadAnvilServices();
      anvilCoreServiceManager.Unload();
    }

    private static bool IsServiceRequirementsMet(PluginManager pluginManager, ServiceBindingOptionsAttribute? options)
    {
      return options?.PluginDependencies == null || options.PluginDependencies.All(plugin => pluginManager.GetPlugin(plugin)?.IsLoaded == true);
    }

    private void InstallAnvilServiceContainer()
    {
      foreach (Type type in Assemblies.AllTypes)
      {
        TryRegisterAnvilService(AnvilServiceContainer, type);
      }

      foreach (Plugin plugin in pluginManager.Plugins)
      {
        if (plugin.PluginTypes != null && !plugin.PluginInfo.Isolated)
        {
          foreach (Type type in plugin.PluginTypes)
          {
            TryRegisterAnvilService(AnvilServiceContainer, type);
          }
        }
      }
    }

    private void TryRegisterAnvilService(IServiceContainer container, Type bindToType)
    {
      if (bindToType.IsAnvilService(out ServiceBindingAttribute[]? bindings, out ServiceBindingOptionsAttribute? options) && IsServiceRequirementsMet(pluginManager, options))
      {
        container.RegisterAnvilService(bindToType, bindings.Select(attribute => attribute.BindFrom), options);
      }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void UnloadAnvilServices()
    {
      Log.Info("Unloading anvil services...");

      // This must always happen in a separate scope/method, otherwise in Debug and some Release configurations, AnvilServiceContainer will hold a strong reference and prevent plugin unload.
      OnContainerDispose?.Invoke(AnvilServiceContainer);
      AnvilServiceContainer.Dispose();
      AnvilServiceContainer = anvilContainerFactory.CreateContainer(CoreServiceContainer);
    }
  }
}
