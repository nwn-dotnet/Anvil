using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Anvil.Internal
{
  internal static class Assemblies
  {
    public static readonly Assembly Anvil = typeof(Assemblies).Assembly;
    public static readonly Assembly Core = typeof(NWN.Core.NWNCore).Assembly;
    public static readonly Assembly Native = typeof(NWN.Native.API.NWNXLib).Assembly;

    public static readonly Assembly[] AllAssemblies;
    public static readonly Type[] AllTypes;
    public static readonly string[] ReservedNames;
    public static readonly string[] RuntimeAssemblies;

    public static readonly string AssemblyDir = Path.GetDirectoryName(Anvil.Location)!;
    public static readonly string[] TargetFrameworks = { "net8.0" };

    static Assemblies()
    {
      AllAssemblies = new[]
      {
        Anvil,
        Core,
        Native,
        typeof(NLog.Logger).Assembly,
        typeof(LightInject.ServiceContainer).Assembly,
        typeof(Newtonsoft.Json.JsonConvert).Assembly,
        typeof(Paket.Dependencies).Assembly,
      };

      AllTypes = AllAssemblies.SelectMany(assembly => assembly.GetTypes()).ToArray();
      ReservedNames = AllAssemblies.Select(assembly => assembly.GetName().Name).ToArray()!;
      RuntimeAssemblies = Directory.GetFiles(RuntimeEnvironment.GetRuntimeDirectory(), "*.dll");
    }

    public static bool IsReservedName(string name)
    {
      return ReservedNames.Contains(name);
    }
  }
}
