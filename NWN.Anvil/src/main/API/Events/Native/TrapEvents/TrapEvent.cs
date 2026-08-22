namespace Anvil.API.Events
{
  /// <summary>
  /// Base event type for trap actions.
  /// </summary>
  public abstract class TrapEvent : IEvent
  {
    /// <summary>
    /// Gets whether the creature is in range of the trap.
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
    /// Set to <see cref="ActionState.Complete"/> or <see cref="ActionState.Failed"/> to override the trap action result and skip default game behavior.
    /// </summary>
    public ActionState? ResultOverride { get; set; }

    /// <summary>
    /// Gets the result of this trap event. This value is only valid in the after event.
    /// </summary>
    public ActionState Result { get; internal set; }

    NwObject IEvent.Context => Creature;
  }
}
