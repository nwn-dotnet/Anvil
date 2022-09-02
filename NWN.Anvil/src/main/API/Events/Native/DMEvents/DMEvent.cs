namespace Anvil.API.Events
{
  public abstract class DMEvent : IEvent
  {
    /// <summary>
    /// Gets or sets if this event should be skipped.
    /// </summary>
    public bool Skip { get; set; }

    /// <summary>
    /// Gets the dungeon master responsible for this event.
    /// </summary>
    public NwPlayer DungeonMaster { get; internal init; } = null!;

    NwObject? IEvent.Context => DungeonMaster.LoginCreature;
  }
}
