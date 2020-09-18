using System;
using System.Collections.Generic;

namespace NWN.Plugins
{
  public interface ITypeLoader
  {
    void Init();
    IReadOnlyCollection<Type> LoadedTypes { get; }
  }
}