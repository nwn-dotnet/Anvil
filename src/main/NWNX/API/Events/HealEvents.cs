using NWN.API;
using NWN.API.Events;
using NWN.Core;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  /// <summary>
  /// Events called when a creature is being healed.
  /// </summary>
  public static class HealEvents
  {
    [NWNXEvent("NWNX_ON_HEAL_BEFORE")]
    public sealed class OnHealBefore : IEventSkippable
    {
      /// <summary>
      /// Gets the creature performing the heal.
      /// </summary>
      public NwCreature Healer { get; } = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();

      /// <summary>
      /// Gets the target being healed.
      /// </summary>
      public NwCreature Target { get; } = EventsPlugin.GetEventData("TARGET_OBJECT_ID").ParseObject<NwCreature>();

      /// <summary>
      /// Gets how much HP the heal will provide.
      /// </summary>
      public int AmountHealed { get; } = EventsPlugin.GetEventData("HEAL_AMOUNT").ParseInt();

      public bool Skip { get; set; }

      NwObject IEvent.Context => Healer;
    }

    [NWNXEvent("NWNX_ON_HEAL_AFTER")]
    public sealed class OnHealAfter : IEventSkippable
    {
      /// <summary>
      /// Gets the creature performing the heal.
      /// </summary>
      public NwCreature Healer { get; } = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();

      /// <summary>
      /// Gets the target being healed.
      /// </summary>
      public NwCreature Target { get; } = EventsPlugin.GetEventData("TARGET_OBJECT_ID").ParseObject<NwCreature>();

      /// <summary>
      /// Gets how much HP the heal will provide.
      /// </summary>
      public int AmountHealed { get; } = EventsPlugin.GetEventData("HEAL_AMOUNT").ParseInt();

      public bool Skip { get; set; }

      NwObject IEvent.Context => Healer;
    }
  }
}
