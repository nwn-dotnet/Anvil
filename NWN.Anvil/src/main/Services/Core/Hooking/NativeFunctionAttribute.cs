using System;

namespace Anvil.Services
{
  [AttributeUsage(AttributeTargets.Delegate)]
  public sealed class NativeFunctionAttribute : Attribute
  {
    public string GccExportName { get; }
    public string MsvcExportName { get; }
    public IntPtr Address { get; }

    public NativeFunctionAttribute(string gccExportName, string msvcExportName)
    {
      GccExportName = gccExportName;
      MsvcExportName = msvcExportName;

      // OperatingSystem os = Environment.OSVersion;
      // switch (os.Platform)
      // {
      //   case PlatformID.Win32S:
      //   case PlatformID.Win32Windows:
      //   case PlatformID.Win32NT:
      //   case PlatformID.WinCE:
      //     Address = NativeLibrary.GetExport(NativeLibrary.GetMainProgramHandle(), msvcExportName);
      //     break;
      //   default:
      //     Address = NativeLibrary.GetExport(NativeLibrary.GetMainProgramHandle(), gccExportName);
      //     break;
      // }
    }
  }
}
