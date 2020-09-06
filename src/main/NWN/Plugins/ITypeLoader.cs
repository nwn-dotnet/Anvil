using System;
using System.Collections.Generic;

namespace NWN.Plugins
{
  public interface ITypeLoader
  {
    IReadOnlyCollection<Type> LoadedTypes { get; }
  }
}