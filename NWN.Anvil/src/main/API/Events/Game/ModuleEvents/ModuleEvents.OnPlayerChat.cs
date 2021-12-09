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
    /// Triggered when any <see cref="NwPlayer"/> sends a chat message. Private channel not hooked.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnPlayerChat)]
    public sealed class OnPlayerChat : IEvent
    {
      /// <summary>
      /// Gets or sets the message that is to be sent.
      /// </summary>
      public string Message
      {
        get => NWScript.GetPCChatMessage();
        set => NWScript.SetPCChatMessage(value);
      }

      /// <summary>
      /// Gets the <see cref="NwPlayer"/> that sent this message.
      /// </summary>
      public NwPlayer Sender { get; } = NWScript.GetPCChatSpeaker().ToNwPlayer();

      /// <summary>
      /// Gets or sets the volume of this message.
      /// </summary>
      public TalkVolume Volume
      {
        get => (TalkVolume)NWScript.GetPCChatVolume();
        set => NWScript.SetPCChatVolume((int)value);
      }

      NwObject IEvent.Context
      {
        get => Sender.ControlledCreature;
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwModule
  {
    /// <inheritdoc cref="ModuleEvents.OnPlayerChat"/>
    public event Action<ModuleEvents.OnPlayerChat> OnPlayerChat
    {
      add => EventService.SubscribeAll<ModuleEvents.OnPlayerChat, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnPlayerChat, GameEventFactory>(value);
    }
  }

  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="ModuleEvents.OnPlayerChat"/>
    public event Action<ModuleEvents.OnPlayerChat> OnPlayerChat
    {
      add => EventService.Subscribe<ModuleEvents.OnPlayerChat, GameEventFactory, GameEventFactory.RegistrationData>(ControlledCreature, new GameEventFactory.RegistrationData(NwModule.Instance), value);
      remove => EventService.Unsubscribe<ModuleEvents.OnPlayerChat, GameEventFactory>(ControlledCreature, value);
    }
  }
}
