using NWN.API.Constants;

// TODO Populate event data.
namespace NWN.API.Events
{
  /// <summary>
  /// Events for Encounter triggers.
  /// </summary>
  public static class EncounterEvents
  {
    [NativeEvent(EventScriptType.EncounterOnObjectEnter)]
    public sealed class OnEnter : NativeEvent<NwEncounter, OnEnter>
    {
      public NwEncounter Encounter { get; private set; }

      protected override void PrepareEvent(NwEncounter objSelf)
      {
        Encounter = objSelf;
      }
    }

    [NativeEvent(EventScriptType.EncounterOnObjectExit)]
    public sealed class OnExit : NativeEvent<NwEncounter, OnExit>
    {
      public NwEncounter Encounter { get; private set; }

      protected override void PrepareEvent(NwEncounter objSelf)
      {
        Encounter = objSelf;
      }
    }

    [NativeEvent(EventScriptType.EncounterOnHeartbeat)]
    public sealed class OnHeartbeat : NativeEvent<NwEncounter, OnHeartbeat>
    {
      public NwEncounter Encounter { get; private set; }

      protected override void PrepareEvent(NwEncounter objSelf)
      {
        Encounter = objSelf;
      }
    }

    [NativeEvent(EventScriptType.EncounterOnEncounterExhausted)]
    public sealed class OnExhausted : NativeEvent<NwEncounter, OnExhausted>
    {
      public NwEncounter Encounter { get; private set; }

      protected override void PrepareEvent(NwEncounter objSelf)
      {
        Encounter = objSelf;
      }
    }

    [NativeEvent(EventScriptType.EncounterOnUserDefinedEvent)]
    public sealed class OnUserDefined : NativeEvent<NwEncounter, OnUserDefined>
    {
      public NwEncounter Encounter { get; private set; }

      protected override void PrepareEvent(NwEncounter objSelf)
      {
        Encounter = objSelf;
      }
    }
  }
}
