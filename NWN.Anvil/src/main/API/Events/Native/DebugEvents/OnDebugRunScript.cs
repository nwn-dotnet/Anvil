using System;
using Anvil.API.Events;

namespace Anvil.API.Events
{
  /// <summary>
  /// Called when a script has been requested to execute from the nwscript debug window.
  /// </summary>
  public sealed class OnDebugRunScript : IEvent
  {
    /// <summary>
    /// Gets the script that is attempting to be executed.
    /// </summary>
    public string ScriptName { get; internal init; } = null!;

    /// <summary>
    /// Gets the object currently set as "OBJECT_SELF"
    /// </summary>
    public NwObject? Target { get; internal init; }

    /// <summary>
    /// Gets or sets if execution of the script should be skipped.
    /// </summary>
    public bool Skip { get; set; }

    NwObject? IEvent.Context => Target;
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
