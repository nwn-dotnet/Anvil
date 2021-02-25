using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NWN
{
  internal static class AssemblyConstants
  {
    internal static readonly Assembly ManagedAssembly = typeof(AssemblyConstants).Assembly;
    internal static readonly AssemblyName ManagedAssemblyName = ManagedAssembly.GetName();

    internal static readonly string AssemblyDir = Path.GetDirectoryName(ManagedAssembly.Location);

    public static readonly Assembly[] ManagedAssemblies =
    {
      ManagedAssembly,
      typeof(Core.NWNCore).Assembly,
      typeof(Native.API.NWNXLib).Assembly,
      typeof(NLog.Logger).Assembly,
      typeof(LightInject.ServiceContainer).Assembly
    };

    public static readonly List<string> ReservedAssemblyNames = ManagedAssemblies
      .Select(assembly => assembly.GetName().Name)
      .ToList();
  }
}
