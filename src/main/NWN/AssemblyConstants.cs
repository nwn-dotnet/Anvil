using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace NWN
{
  internal static class AssemblyConstants
  {
    internal static readonly Assembly NWMAssembly = typeof(AssemblyConstants).Assembly;
    internal static readonly AssemblyName NWMName = NWMAssembly.GetName();
    internal static readonly string AssemblyDir = Path.GetDirectoryName(NWMAssembly.Location);

    internal static readonly AssemblyLoadContext NWMLoadContext = AssemblyLoadContext.GetLoadContext(NWMAssembly);
  }
}