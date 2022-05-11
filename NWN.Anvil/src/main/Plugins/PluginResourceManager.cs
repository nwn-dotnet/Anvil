using Anvil.Services;

namespace Anvil.Plugins
{
  [ServiceBinding(typeof(PluginResourceManager))]
  [ServiceBindingOptions(InternalBindingPriority.API)]
  internal sealed class PluginResourceManager
  {
    public PluginResourceManager(PluginManager pluginManager, ResourceManager resourceManager)
    {
      if (pluginManager.ResourcePaths == null)
      {
        return;
      }

      foreach (string resourcePath in pluginManager.ResourcePaths)
      {
        resourceManager.CreateResourceDirectory(resourcePath);
      }
    }
  }
}
