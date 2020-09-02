using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using NWN.Services;

namespace NWN
{
  public static class Internal
  {
    [UsedImplicitly] // Called by NWNX
    public static int Bootstrap(IntPtr arg, int argLength)
    {
      LoadServiceAssemblies();
      return NManager.Init(arg, argLength, new ServiceBindingInstaller());
    }

    private static void LoadServiceAssemblies()
    {
      IEnumerable<string> assemblyPaths = Directory.GetFiles(AssemblyConstants.AssemblyDir)
        .Where(file => Path.GetExtension(file).ToLower() == ".dll");

      foreach (string assemblyPath in assemblyPaths)
      {
        if (assemblyPath != AssemblyConstants.NWMAssembly.Location)
        {
          try
          {
            Assembly.Load(AssemblyName.GetAssemblyName(assemblyPath));
          }
          catch (BadImageFormatException) {}
          catch (IOException) {}
        }
      }
    }
  }
}