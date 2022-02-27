using LightInject;

namespace Anvil.Services
{
  public interface IServiceManager
  {
    public ServiceContainer CoreServiceContainer { get; }

    public ServiceContainer AnvilServiceContainer { get; }

    /// <summary>
    /// Called during NWNX initialization. Core services should be initialized here.
    /// </summary>
    void Init();

    /// <summary>
    /// Called after module load. All other anvil/plugin services should be populated in the container here.
    /// </summary>
    void Load();

    /// <summary>
    /// Called after all services have been loaded and just before the module is ready to play. Run any last routines on all services here.
    /// </summary>
    void Start();

    /// <summary>
    /// Called before the server is shutting down/reloading. Anvil/plugin services should be unloaded here.
    /// </summary>
    void Unload();

    /// <summary>
    /// Called after the server is shut down/destroyed. Core services should be unloaded here.
    /// </summary>
    void Shutdown();
  }
}
