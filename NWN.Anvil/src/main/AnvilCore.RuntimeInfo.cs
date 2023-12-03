namespace Anvil
{
  public sealed partial class AnvilCore
  {
    private static RuntimeInfo runtimeInfo;

    private struct RuntimeInfo
    {
      public string? AssemblyName;
      public string? AssemblyVersion;
      public string? ServerVersion;
      public string? CoreVersion;
      public string? NativeVersion;
    }
  }
}
