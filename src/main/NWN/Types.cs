using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NWN
{
  internal static class Types
  {
    private static readonly Assembly nwmAssembly;
    private static readonly string nwmAssemblyName;

    public static readonly IReadOnlyList<Type> AllLinkedTypes;

    static Types()
    {
      nwmAssembly = Assembly.GetExecutingAssembly();
      nwmAssemblyName = nwmAssembly.FullName;

      Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
      AllLinkedTypes = assemblies.Where(IsLinkedAssembly)
        .SelectMany(assembly => assembly.GetTypes()).ToList().AsReadOnly();
    }

    private static bool IsLinkedAssembly(Assembly assembly)
      => assembly == nwmAssembly || assembly.GetReferencedAssemblies().Any(name => name.ToString() == nwmAssemblyName);
  }
}