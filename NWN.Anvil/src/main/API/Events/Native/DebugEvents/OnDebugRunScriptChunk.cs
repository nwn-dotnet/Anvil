using System;
using Anvil.API.Events;

namespace Anvil.API.Events
{
  /// <summary>
  /// Triggered when a DM/player in debug mode has requested a script chunk to be executed.
  /// </summary>
  public sealed class OnDebugRunScriptChunk : IEvent
  {
    /// <summary>
    /// Gets the player attempting to execute the script chunk.
    /// </summary>
    public NwPlayer? Player { get; internal init; }

    /// <summary>
    /// Gets the raw script chunk that is attempted to be executed.
    /// </summary>
    public string ScriptChunk { get; internal init; } = null!;

    /// <summary>
    /// Gets the object currently set as "OBJECT_SELF"
    /// </summary>
    public NwObject? Target { get; internal init; }

    /// <summary>
    /// Gets if the script chunk was requested to be wrapped in a main function.
    /// </summary>
    public bool WrapIntoMain { get; internal init; }

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
    /// <inheritdoc cref="Events.OnDebugRunScriptChunk"/>
    public event Action<OnDebugRunScriptChunk> OnDebugRunScriptChunk
    {
      add => EventService.SubscribeAll<OnDebugRunScriptChunk, DebugEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDebugRunScriptChunk, DebugEventFactory>(value);
    }
  }
}
