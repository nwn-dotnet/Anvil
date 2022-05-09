using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Global module events.
  /// </summary>
  public static partial class ModuleEvents
  {
    /// <summary>
    /// Triggered when a <see cref="NwPlayer"/> tries to cancel a cutscene (ESC).
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnPlayerCancelCutscene)]
    public sealed class OnCutsceneAbort : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlayer"/> that triggered the event.
      /// </summary>
      public NwPlayer Player { get; } = NWScript.GetLastPCToCancelCutscene().ToNwPlayer()!;

      NwObject? IEvent.Context => Player.ControlledCreature;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwModule
  {
    /// <inheritdoc cref="ModuleEvents.OnCutsceneAbort"/>
    public event Action<ModuleEvents.OnCutsceneAbort> OnCutsceneAbort
    {
      add => EventService.SubscribeAll<ModuleEvents.OnCutsceneAbort, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnCutsceneAbort, GameEventFactory>(value);
    }
  }

  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="ModuleEvents.OnCutsceneAbort"/>
    public event Action<ModuleEvents.OnCutsceneAbort> OnCutsceneAbort
    {
      add => EventService.Subscribe<ModuleEvents.OnCutsceneAbort, GameEventFactory, GameEventFactory.RegistrationData>(ControlledCreature, new GameEventFactory.RegistrationData(NwModule.Instance), value);
      remove => EventService.Unsubscribe<ModuleEvents.OnCutsceneAbort, GameEventFactory>(ControlledCreature, value);
    }
  }
}
