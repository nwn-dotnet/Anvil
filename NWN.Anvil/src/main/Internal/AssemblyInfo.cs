using System.Reflection;

namespace Anvil.Internal
{
  internal static class AssemblyInfo
  {
    public static readonly AssemblyInformationalVersionAttribute VersionInfo = typeof(AssemblyInfo).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
  }
}
