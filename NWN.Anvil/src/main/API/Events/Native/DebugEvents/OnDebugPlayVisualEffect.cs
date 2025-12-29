using System;
using System.Numerics;
using Anvil.API.Events;

namespace Anvil.API.Events
{
  /// <summary>
  /// Triggered when a DM/player in debug mode attempts to spawn a visual effect.
  /// </summary>
  public sealed class OnDebugPlayVisualEffect : IEvent
  {
    /// <summary>
    /// Gets the effect that is attempting to be played.
    /// </summary>
    public VisualEffectTableEntry? Effect { get; internal init; }

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
    /// Set to true to skip execution.
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
