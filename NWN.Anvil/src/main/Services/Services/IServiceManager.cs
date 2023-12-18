using LightInject;

namespace Anvil.Services
{
  /// <summary>
  /// The interface that manages all core, anvil and plugin services.<br/>
  /// Advanced: implement in your own class and specify in <see cref="AnvilCore.Init(System.IntPtr,int,IServiceManager)"/> to implement your own service manager.
  /// </summary>
  public interface IServiceManager
  {
    /// <summary>
    /// The container holding services for anvil and anvil plugins.
    /// </summary>
    public ServiceContainer AnvilServiceContainer { get; }

    /// <summary>
    /// The container holding internal core services. (logging, function hooking, etc).
    /// </summary>
    public ServiceContainer CoreServiceContainer { get; }

    /// <summary>
    /// Invoked by the injection service. Implementation for services injected into an object at runtime.
    /// </summary>
    /// <param name="instance">The instance to inject.</param>
    public void InjectProperties(object? instance);

    /// <summary>
    /// Called during NWNX initialization. Core services should be initialized here.
    /// </summary>
    void Init();

    /// <summary>
    /// Called after module load. All other anvil/plugin services should be populated in the container here.
    /// </summary>
    void Load();

    /// <summary>
    /// Called after the server is shut down/destroyed. Core services should be unloaded here.
    /// </summary>
    void Shutdown();

    /// <summary>
    /// Called after all services have been loaded and just before the module is ready to play. Run any last routines on all services here.
    /// </summary>
    void Start();

    /// <summary>
    /// Called before the server is shutting down/reloading. Anvil/plugin services should be unloaded here.
    /// </summary>
    void Unload();
  }
}
