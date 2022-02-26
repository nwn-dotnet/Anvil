using LightInject;

namespace Anvil.Services
{
  public interface IContainerFactory
  {
    ServiceContainer BuildAnvilServiceContainer(ServiceContainer coreContainer);
    ServiceContainer BuildCoreContainer(AnvilCore anvilCore);
  }
}
