using System;
using System.Collections.Generic;

namespace Anvil.Plugins
{
  internal interface IPluginSource : IDisposable
  {
    IEnumerable<Plugin> Bootstrap();
  }
}
