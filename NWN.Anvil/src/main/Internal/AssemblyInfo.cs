using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("NWN.Anvil.TestRunner")]
[assembly: InternalsVisibleTo("NWN.Anvil.Tests")]

namespace Anvil.Internal
{
  internal static class AssemblyInfo
  {
    public static readonly AssemblyInformationalVersionAttribute VersionInfo = typeof(AssemblyInfo).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!;
  }
}
