using System;
using System.Collections.Generic;
using SimpleInjector;

namespace NWN.Services
{
  public interface IBindingInstaller
  {
    void ConfigureBindings(Container container, IReadOnlyCollection<Type> allTypes);
  }
}