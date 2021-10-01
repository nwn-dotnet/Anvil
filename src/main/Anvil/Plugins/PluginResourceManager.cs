using Anvil.Services;

namespace Anvil.Plugins
{
  [ServiceBinding(typeof(PluginResourceManager))]
  [ServiceBindingOptions(BindingOrder.API)]
  internal sealed class PluginResourceManager
  {
    public PluginResourceManager(PluginManager pluginManager, ResourceManager resourceManager)
    {
      foreach (string resourcePath in pluginManager.ResourcePaths)
      {
        resourceManager.CreateResourceDirectory(resourcePath);
      }
    }
  }
}
