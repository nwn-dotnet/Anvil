namespace Anvil.API.Events
{
  /// <summary>
  /// Specifies the target scope for dumping local variables.
  /// </summary>
  public enum DumpLocalsType
  {
    /// <summary>
    /// Dump locals for a specific object.
    /// </summary>
    DumpLocals = 0,

    /// <summary>
    /// Dump locals for the current area.
    /// </summary>
    DumpAreaLocals = 1,

    /// <summary>
    /// Dump locals for the module.
    /// </summary>
    DumpModuleLocals = 2,
  }
}
