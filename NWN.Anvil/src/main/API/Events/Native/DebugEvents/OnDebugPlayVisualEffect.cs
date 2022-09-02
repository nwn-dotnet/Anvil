using System;
using System.Numerics;
using Anvil.API.Events;

namespace Anvil.API.Events
{
  /// <summary>
  /// Called when a client attempts to spawn a visual effect.
  /// </summary>
  public sealed class OnDebugPlayVisualEffect : IEvent
  {
    /// <summary>
    /// Gets the effect that is attempting to be played.
    /// </summary>
    public VisualEffectTableEntry Effect { get; internal init; } = null!;

    /// <summary>
    /// Gets the player attempting to spawn the visual effect.
    /// </summary>
    public NwPlayer? Player { get; internal init; }

    /// <summary>
    /// Gets the object/area target of the visual effect.
    /// </summary>
    public NwObject? TargetObject { get; internal init; }

    /// <summary>
    /// Gets the position target of the visual effect.
    /// </summary>
    public Vector3 TargetPosition { get; internal init; }

    /// <summary>
    /// Gets the duration of the visual effect.
    /// </summary>
    public TimeSpan Duration { get; internal init; }

    /// <summary>
    /// Gets or sets if execution of the script should be skipped.
    /// </summary>
    public bool Skip { get; set; }

    NwObject? IEvent.Context => Player?.ControlledCreature;
  }
}

namespace Anvil.API
{
  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnDebugPlayVisualEffect"/>
    public event Action<OnDebugPlayVisualEffect> OnDebugPlayVisualEffect
    {
      add => EventService.SubscribeAll<OnDebugPlayVisualEffect, DebugEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDebugPlayVisualEffect, DebugEventFactory>(value);
    }
  }
}
