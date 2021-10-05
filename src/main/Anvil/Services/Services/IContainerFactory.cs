using Anvil.Plugins;
using LightInject;

namespace Anvil.Services
{
  public interface IContainerFactory
  {
    ServiceContainer Setup(PluginManager pluginManager);

    void RegisterCoreService<T>(T instance);

    void BuildContainer();
  }
}
