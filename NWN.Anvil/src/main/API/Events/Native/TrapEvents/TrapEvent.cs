namespace Anvil.API.Events
{
  public abstract class TrapEvent : IEvent
  {
    /// <summary>
    /// Gets if the creature is in range of the trap.
    /// </summary>
    public bool InRange { get; internal init; }

    /// <summary>
    /// Gets the creature performing the trap action.
    /// </summary>
    public NwCreature Creature { get; internal init; } = null!;

    /// <summary>
    /// Gets the target trap for this trap event.
    /// </summary>
    public NwGameObject Trap { get; internal init; } = null!;

    /// <summary>
    /// Gets or sets a value to override the trap action result, skipping the default game behaviour.<br/>
    /// Supported values: <see cref="ActionState.Complete"/>, <see cref="ActionState.Failed"/>.
    /// </summary>
    public ActionState? ResultOverride { get; set; }

    /// <summary>
    /// Gets the result of this trap event. This value is only valid in the after event.
    /// </summary>
    public ActionState Result { get; internal set; }

    NwObject IEvent.Context => Creature;
  }
}
