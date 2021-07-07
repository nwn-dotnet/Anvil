using LightInject;
using NWN.Plugins;

namespace NWN.Services
{
  public interface IContainerFactory
  {
    ServiceContainer Setup(ITypeLoader typeLoader);

    void RegisterCoreService<T>(T instance);

    void BuildContainer();
  }
}
