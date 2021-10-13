using System.Collections.Generic;
using Anvil.Plugins;
using LightInject;

namespace Anvil.Services
{
  public interface IContainerFactory
  {
    ServiceContainer CreateContainer(PluginManager pluginManager, IEnumerable<object> coreServices);
  }
}
