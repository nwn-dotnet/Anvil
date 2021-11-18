using System.Collections.Generic;

namespace Anvil.Plugins
{
  internal interface IPluginSource
  {
    IEnumerable<Plugin> Bootstrap();
  }
}
