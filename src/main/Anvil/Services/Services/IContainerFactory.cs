using Anvil.Plugins;
using LightInject;

namespace Anvil.Services
{
  public interface IContainerFactory
  {
    ServiceContainer Setup(ITypeLoader typeLoader);

    void RegisterCoreService<T>(T instance);

    void BuildContainer();
  }
}
