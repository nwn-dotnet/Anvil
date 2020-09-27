using NWN.API;
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
    public class OnHealBefore : EventSkippable<OnHealBefore>
    {
      /// <summary>
      /// The creature performing the heal.
      /// </summary>
      public NwCreature Healer { get; private set; }

      /// <summary>
      /// The target being healed.
      /// </summary>
      public NwCreature Target { get; private set; }

      /// <summary>
      /// How much HP the heal will provide.
      /// </summary>
      public int AmountHealed { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Healer = (NwCreature) objSelf;
        Target = NWScript.StringToObject(EventsPlugin.GetEventData("TARGET_OBJECT_ID")).ToNwObject<NwCreature>();
        AmountHealed = EventsPlugin.GetEventData("HEAL_AMOUNT").ParseInt();
      }
    }

    [NWNXEvent("NWNX_ON_HEAL_AFTER")]
    public class OnHealAfter : EventSkippable<OnHealAfter>
    {
      /// <summary>
      /// The creature performing the heal.
      /// </summary>
      public NwCreature Healer { get; private set; }

      /// <summary>
      /// The target being healed.
      /// </summary>
      public NwCreature Target { get; private set; }

      /// <summary>
      /// How much HP the heal will provide.
      /// </summary>
      public int AmountHealed { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Healer = (NwCreature) objSelf;
        Target = NWScript.StringToObject(EventsPlugin.GetEventData("TARGET_OBJECT_ID")).ToNwObject<NwCreature>();
        AmountHealed = EventsPlugin.GetEventData("HEAL_AMOUNT").ParseInt();
      }
    }
  }
}