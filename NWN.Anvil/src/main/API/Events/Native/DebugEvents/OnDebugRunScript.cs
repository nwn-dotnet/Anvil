using System;
using Anvil.API.Events;

namespace Anvil.API.Events
{
  /// <summary>
  /// Triggered when a script has been requested to execute from the nwscript debug window.
  /// </summary>
  public sealed class OnDebugRunScript : IEvent
  {
    /// <summary>
    /// Gets the player attempting to execute the script.
    /// </summary>
    public NwPlayer? Player { get; internal init; }

    /// <summary>
    /// Gets the name of the script that is attempting to be executed.
    /// </summary>
    public string ScriptName { get; internal init; } = null!;

    /// <summary>
    /// Gets the object currently set as "OBJECT_SELF"
    /// </summary>
    public NwObject? Target { get; internal init; }

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
    /// <inheritdoc cref="Events.OnDebugRunScript"/>
    public event Action<OnDebugRunScript> OnDebugRunScript
    {
      add => EventService.SubscribeAll<OnDebugRunScript, DebugEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDebugRunScript, DebugEventFactory>(value);
    }
  }
}
