using System;
using System.Collections.Generic;

namespace Anvil.Plugins
{
  public interface ITypeLoader : IDisposable
  {
    void Init();

    IReadOnlyCollection<Type> LoadedTypes { get; }

    IReadOnlyCollection<string> ResourcePaths { get; }
  }
}
