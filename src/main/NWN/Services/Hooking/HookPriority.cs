namespace NWN.Services
{
  public static class HookPriority
  {
    public const int SharedHook = int.MinValue;
    public const int Earliest = -1000000;
    public const int VeryEarly = -10000;
    public const int Early = -100;
    public const int Default = 0;
    public const int Late = 100;
    public const int VeryLate = 10000;
    public const int Latest = 1000000;
    public const int Final = int.MaxValue;
  }
}
