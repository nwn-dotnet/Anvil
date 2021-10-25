namespace Anvil.Services
{
  public enum BindingPriority : short
  {
    Highest = short.MinValue,
    VeryHigh = -30000,
    High = -20000,
    AboveNormal = -10000,
    Normal = 0,
    BelowNormal = 10000,
    Low = 20000,
    VeryLow = 30000,
    Lowest = short.MaxValue,
  }
}
