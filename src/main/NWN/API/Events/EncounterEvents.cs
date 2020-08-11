using NWN.API.Constants;

namespace NWN.API.Events
{
  public static class EncounterEvents
  {
    [ScriptEvent(EventScriptType.EncounterOnObjectEnter)]
    public sealed class OnEnter : Event<NwEncounter, OnEnter>
    {
      public NwEncounter Encounter { get; private set; }

      protected override void PrepareEvent(NwEncounter objSelf)
      {
        Encounter = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.EncounterOnObjectExit)]
    public sealed class OnExit : Event<NwEncounter, OnExit>
    {
      public NwEncounter Encounter { get; private set; }

      protected override void PrepareEvent(NwEncounter objSelf)
      {
        Encounter = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.EncounterOnHeartbeat)]
    public sealed class OnHeartbeat : Event<NwEncounter, OnHeartbeat>
    {
      public NwEncounter Encounter { get; private set; }

      protected override void PrepareEvent(NwEncounter objSelf)
      {
        Encounter = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.EncounterOnEncounterExhausted)]
    public sealed class OnExhausted : Event<NwEncounter, OnExhausted>
    {
      public NwEncounter Encounter { get; private set; }

      protected override void PrepareEvent(NwEncounter objSelf)
      {
        Encounter = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.EncounterOnUserDefinedEvent)]
    public sealed class OnUserDefined : Event<NwEncounter, OnUserDefined>
    {
      public NwEncounter Encounter { get; private set; }

      protected override void PrepareEvent(NwEncounter objSelf)
      {
        Encounter = objSelf;
      }
    }
  }
}