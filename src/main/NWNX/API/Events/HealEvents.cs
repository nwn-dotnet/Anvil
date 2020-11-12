using NWN.API;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  /// <summary>
  /// Events called when a creature is being healed.
  /// </summary>
  public static class HealEvents
  {
    [NWNXEvent("NWNX_ON_HEAL_BEFORE")]
    public class OnHealBefore : NWNXEventSkippable<OnHealBefore>
    {
      /// <summary>
      /// Gets the creature performing the heal.
      /// </summary>
      public NwCreature Healer { get; private set; }

      /// <summary>
      /// Gets the target being healed.
      /// </summary>
      public NwCreature Target { get; private set; }

      /// <summary>
      /// Gets how much HP the heal will provide.
      /// </summary>
      public int AmountHealed { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Healer = (NwCreature) objSelf;
        Target = EventsPlugin.GetEventData("TARGET_OBJECT_ID").ParseObject<NwCreature>();
        AmountHealed = EventsPlugin.GetEventData("HEAL_AMOUNT").ParseInt();
      }
    }

    [NWNXEvent("NWNX_ON_HEAL_AFTER")]
    public class OnHealAfter : NWNXEventSkippable<OnHealAfter>
    {
      /// <summary>
      /// Gets the creature performing the heal.
      /// </summary>
      public NwCreature Healer { get; private set; }

      /// <summary>
      /// Gets the target being healed.
      /// </summary>
      public NwCreature Target { get; private set; }

      /// <summary>
      /// Gets how much HP the heal will provide.
      /// </summary>
      public int AmountHealed { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Healer = (NwCreature) objSelf;
        Target = EventsPlugin.GetEventData("TARGET_OBJECT_ID").ParseObject<NwCreature>();
        AmountHealed = EventsPlugin.GetEventData("HEAL_AMOUNT").ParseInt();
      }
    }
  }
}
