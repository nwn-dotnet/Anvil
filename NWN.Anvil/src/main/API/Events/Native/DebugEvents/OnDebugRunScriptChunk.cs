using System;
using Anvil.API.Events;

namespace Anvil.API.Events
{
  /// <summary>
  /// Called when a client has requested a script chunk to be executed.
  /// </summary>
  public sealed class OnDebugRunScriptChunk : IEvent
  {
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
    /// Gets or sets if execution of the script chunk should be skipped.
    /// </summary>
    public bool Skip { get; set; }

    NwObject? IEvent.Context => Target;
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
