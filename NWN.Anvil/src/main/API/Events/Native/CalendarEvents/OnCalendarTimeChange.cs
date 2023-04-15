using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnCalendarTimeChange : IEvent
  {
    public uint NewValue { get; private init; }

    public uint OldValue { get; private init; }
    public TimeChangeType TimeChangeType { get; private init; }

    NwObject? IEvent.Context => null;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSModule.UpdateTime> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, uint, uint, uint, void> pHook = &OnUpdateTime;
        Hook = HookService.RequestHook<Functions.CNWSModule.UpdateTime>(pHook, HookOrder.Earliest);
        return new IDisposable[] { Hook };
      }

      [UnmanagedCallersOnly]
      private static void OnUpdateTime(void* pModule, uint nCalendarDay, uint nTimeOfDay, uint nUpdateDifference)
      {
        CNWSModule module = CNWSModule.FromPointer(pModule);
        uint hour = module.m_nCurrentHour;
        uint day = module.m_nCurrentDay;
        uint month = module.m_nCurrentMonth;
        uint year = module.m_nCurrentYear;
        uint dayState = module.m_nTimeOfDayState;

        Hook.CallOriginal(pModule, nCalendarDay, nTimeOfDay, nUpdateDifference);

        if (hour != module.m_nCurrentHour)
        {
          OnCalendarTimeChange eventData = ProcessEvent(EventCallbackType.Before, new OnCalendarTimeChange
          {
            TimeChangeType = TimeChangeType.Hour,
            OldValue = hour,
            NewValue = module.m_nCurrentHour,
          });

          ProcessEvent(EventCallbackType.After, eventData);
        }

        if (day != module.m_nCurrentDay)
        {
          OnCalendarTimeChange eventData = ProcessEvent(EventCallbackType.Before, new OnCalendarTimeChange
          {
            TimeChangeType = TimeChangeType.Day,
            OldValue = day,
            NewValue = module.m_nCurrentDay,
          });

          ProcessEvent(EventCallbackType.After, eventData);
        }

        if (month != module.m_nCurrentMonth)
        {
          OnCalendarTimeChange eventData = ProcessEvent(EventCallbackType.Before, new OnCalendarTimeChange
          {
            TimeChangeType = TimeChangeType.Month,
            OldValue = month,
            NewValue = module.m_nCurrentMonth,
          });

          ProcessEvent(EventCallbackType.After, eventData);
        }

        if (year != module.m_nCurrentYear)
        {
          OnCalendarTimeChange eventData = ProcessEvent(EventCallbackType.Before, new OnCalendarTimeChange
          {
            TimeChangeType = TimeChangeType.Year,
            OldValue = year,
            NewValue = module.m_nCurrentYear,
          });

          ProcessEvent(EventCallbackType.After, eventData);
        }

        if (dayState != module.m_nTimeOfDayState)
        {
          OnCalendarTimeChange eventData = ProcessEvent(EventCallbackType.Before, new OnCalendarTimeChange
          {
            TimeChangeType = TimeChangeType.TimeOfDay,
            OldValue = dayState,
            NewValue = module.m_nTimeOfDayState,
          });

          ProcessEvent(EventCallbackType.After, eventData);
        }
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnCalendarTimeChange"/>
    public event Action<OnCalendarTimeChange> OnCalendarTimeChange
    {
      add => EventService.SubscribeAll<OnCalendarTimeChange, OnCalendarTimeChange.Factory>(value);
      remove => EventService.UnsubscribeAll<OnCalendarTimeChange, OnCalendarTimeChange.Factory>(value);
    }
  }
}
