using System;
using System.Collections.Generic;

namespace Anvil.Plugins
{
  public interface ITypeLoader
  {
    void Init();

    IReadOnlyCollection<Type> LoadedTypes { get; }

    IReadOnlyCollection<string> ResourcePaths { get; }

    void Dispose();
  }
}
