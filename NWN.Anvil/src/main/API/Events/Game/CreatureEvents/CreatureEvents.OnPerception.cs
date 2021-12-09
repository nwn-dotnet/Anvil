using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Events for <see cref="NwCreature"/>.
  /// </summary>
  public static partial class CreatureEvents
  {
    /// <summary>
    /// Triggered by <see cref="NwCreature"/> when its perception is triggered by another <see cref="NwCreature"/>.
    /// </summary>
    [GameEvent(EventScriptType.CreatureOnNotice)]
    public sealed class OnPerception : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwCreature"/> associated with the perception event.
      /// </summary>
      public NwCreature Creature { get; } = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();

      /// <summary>
      /// Gets the <see cref="NwCreature"/> that was perceived by <see cref="NwCreature"/>.
      /// </summary>
      public NwCreature PerceivedCreature { get; } = NWScript.GetLastPerceived().ToNwObject<NwCreature>();

      /// <summary>
      /// Gets the <see cref="PerceptionEventType"/> event triggered.
      /// </summary>
      public PerceptionEventType PerceptionEventType { get; } = GetPerceptionEventType();

      NwObject IEvent.Context
      {
        get => Creature;
      }

      private static PerceptionEventType GetPerceptionEventType()
      {
        if (NWScript.GetLastPerceptionSeen().ToBool())
        {
          return PerceptionEventType.Seen;
        }

        if (NWScript.GetLastPerceptionVanished().ToBool())
        {
          return PerceptionEventType.Vanished;
        }

        if (NWScript.GetLastPerceptionHeard().ToBool())
        {
          return PerceptionEventType.Heard;
        }

        if (NWScript.GetLastPerceptionInaudible().ToBool())
        {
          return PerceptionEventType.Inaudible;
        }

        return PerceptionEventType.Unknown;
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="CreatureEvents.OnPerception"/>
    public event Action<CreatureEvents.OnPerception> OnPerception
    {
      add => EventService.Subscribe<CreatureEvents.OnPerception, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<CreatureEvents.OnPerception, GameEventFactory>(this, value);
    }
  }
}
