using LightInject;
using NWN.Plugins;

namespace NWN.Services
{
  public interface IContainerBuilder
  {
    ServiceContainer Setup(ITypeLoader typeLoader);
    void RegisterCoreService<T>(T instance);
    void BuildContainer();
  }
}
