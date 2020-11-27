using System;
using System.Collections.Generic;

namespace NWN.Plugins
{
  public interface ITypeLoader : IDisposable
  {
    void Init();

    IReadOnlyCollection<Type> LoadedTypes { get; }
  }
}
