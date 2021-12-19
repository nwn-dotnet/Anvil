namespace Anvil.Services
{
  internal enum InternalBindingPriority
  {
    Highest = -99999,
    VeryHigh = -80000,
    High = -60000,
    AboveNormal = short.MinValue - 1,
    Normal = 0,
    BelowNormal = short.MaxValue + 1,
    Low = 60000,
    VeryLow = 80000,
    Lowest = 99999,

    /// <summary>
    /// The default priority for services without a configuration.
    /// </summary>
    Default = Normal,

    /// <summary>
    /// Use for Core services that must be initialized first (e.g PluginManager)
    /// </summary>
    Core = Highest,

    /// <summary>
    /// Use for general services that are needed for object APIs
    /// </summary>
    API = High,
  }
}
