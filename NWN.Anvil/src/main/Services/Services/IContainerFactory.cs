using LightInject;

namespace Anvil.Services
{
  public interface IContainerFactory
  {
    ServiceContainer BuildCoreContainer(AnvilCore anvilCore);
    ServiceContainer BuildAnvilServiceContainer(ServiceContainer coreContainer);
  }
}
