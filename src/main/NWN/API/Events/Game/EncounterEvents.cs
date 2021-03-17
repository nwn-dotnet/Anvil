using NWN.API.Constants;
using NWN.Core;

// TODO Populate event data.
namespace NWN.API.Events
{
  /// <summary>
  /// Events for Encounter triggers.
  /// </summary>
  public static class EncounterEvents
  {
    [NativeEvent(EventScriptType.EncounterOnObjectEnter)]
    public sealed class OnEnter : IEvent
    {
      public NwEncounter Encounter { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Encounter;

      public OnEnter()
      {
        Encounter = NWScript.OBJECT_SELF.ToNwObject<NwEncounter>();
      }
    }

    [NativeEvent(EventScriptType.EncounterOnObjectExit)]
    public sealed class OnExit : IEvent
    {
      public NwEncounter Encounter { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Encounter;

      public OnExit()
      {
        Encounter = NWScript.OBJECT_SELF.ToNwObject<NwEncounter>();
      }
    }

    [NativeEvent(EventScriptType.EncounterOnHeartbeat)]
    public sealed class OnHeartbeat : IEvent
    {
      public NwEncounter Encounter { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Encounter;

      public OnHeartbeat()
      {
        Encounter = NWScript.OBJECT_SELF.ToNwObject<NwEncounter>();
      }
    }

    [NativeEvent(EventScriptType.EncounterOnEncounterExhausted)]
    public sealed class OnExhausted : IEvent
    {
      public NwEncounter Encounter { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Encounter;

      public OnExhausted()
      {
        Encounter = NWScript.OBJECT_SELF.ToNwObject<NwEncounter>();
      }
    }

    [NativeEvent(EventScriptType.EncounterOnUserDefinedEvent)]
    public sealed class OnUserDefined : IEvent
    {
      public NwEncounter Encounter { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Encounter;

      public OnUserDefined()
      {
        Encounter = NWScript.OBJECT_SELF.ToNwObject<NwEncounter>();
      }
    }
  }
}
