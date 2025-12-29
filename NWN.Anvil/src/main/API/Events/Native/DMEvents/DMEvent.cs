namespace Anvil.API.Events
{
  /// <summary>
  /// Base class for DM-related native events.
  /// </summary>
  public abstract class DMEvent : IEvent
  {
    /// <summary>
    /// Set to true to skip execution.
    /// </summary>
    public bool Skip { get; set; }

    /// <summary>
    /// Gets the dungeon master responsible for this event.
    /// </summary>
    public NwPlayer DungeonMaster { get; internal init; } = null!;

    NwObject? IEvent.Context => DungeonMaster.LoginCreature;
  }
}
