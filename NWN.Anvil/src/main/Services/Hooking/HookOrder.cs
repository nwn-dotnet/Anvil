namespace Anvil.Services
{
  public static class HookOrder
  {
    public const int Default = 0;
    public const int Earliest = -3000000;
    public const int Early = -1000000;
    public const int Final = int.MaxValue;
    public const int Late = 1000000;
    public const int Latest = 3000000;
    public const int SharedHook = int.MinValue;
    public const int VeryEarly = -2000000;
    public const int VeryLate = 2000000;
  }
}
