using System.IO;
using System.Reflection;

namespace NWN
{
  internal static class AssemblyConstants
  {
    internal static readonly Assembly NWMAssembly = Assembly.GetExecutingAssembly();
    internal static readonly string AssemblyDir = Path.GetDirectoryName(NWMAssembly.Location);
  }
}