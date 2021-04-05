using System;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnDetectModeUpdate : IEvent
  {
    /// <summary>
    /// Gets or sets a value indicating whether this creature should be prevented from entering/exiting detect mode.
    /// </summary>
    public bool Prevent { get; set; }

    public ToggleModeEventType EventType { get; private init; }

    public NwCreature Creature { get; private init; }

    NwObject IEvent.Context => Creature;

    [NativeFunction(NWNXLib.Functions._ZN12CNWSCreature13SetDetectModeEh)]
    internal delegate void SetDetectModeHook(IntPtr pCreature, byte nDetectMode);

    internal class Factory : NativeEventFactory<SetDetectModeHook>
    {
      public Factory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<SetDetectModeHook> RequestHook(HookService hookService)
        => hookService.RequestHook<SetDetectModeHook>(OnSetDetectMode, HookOrder.Early);

      private void OnSetDetectMode(IntPtr pCreature, byte nDetectMode)
      {
        CNWSCreature creature = new CNWSCreature(pCreature, false);

        bool willBeDetecting = nDetectMode != 0;
        bool currentlyDetecting = creature.m_nDetectMode != 0;

        if (!currentlyDetecting && willBeDetecting)
        {
          HandleEnter(creature, nDetectMode);
        }
        else if (currentlyDetecting && !willBeDetecting)
        {
          HandleExit(creature, nDetectMode);
        }
        else
        {
          Hook.CallOriginal(pCreature, nDetectMode);
        }
      }

      private void HandleEnter(CNWSCreature creature, byte nDetectMode)
      {
        OnDetectModeUpdate eventData = ProcessEvent(new OnDetectModeUpdate
        {
          Creature = creature.m_idSelf.ToNwObject<NwCreature>(),
          EventType = ToggleModeEventType.Enter
        });

        if (!eventData.Prevent)
        {
          Hook.CallOriginal(creature, nDetectMode);
        }
        else
        {
          creature.ClearActivities(0);
        }
      }

      private void HandleExit(CNWSCreature creature, byte nDetectMode)
      {
        OnDetectModeUpdate eventData = ProcessEvent(new OnDetectModeUpdate
        {
          Creature = creature.m_idSelf.ToNwObject<NwCreature>(),
          EventType = ToggleModeEventType.Exit
        });

        if (!eventData.Prevent)
        {
          Hook.CallOriginal(creature, nDetectMode);
        }
        else
        {
          creature.SetActivity(0, true.ToInt());
        }
      }
    }
  }
}
