using System;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public class OnCalendarTimeChange : IEvent
  {
    public TimeChangeType TimeChangeType { get; private init; }

    public uint OldValue { get; private init; }

    public uint NewValue { get; private init; }

    NwObject IEvent.Context => null;

    [NativeFunction(NWNXLib.Functions._ZN10CNWSModule10UpdateTimeEjjj)]
    internal delegate void UpdateTimeHook(IntPtr pModule, uint nCalendarDay, uint nTimeOfDay, uint nUpdateDifference);

    internal class Factory : NativeEventFactory<UpdateTimeHook>
    {
      public Factory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<UpdateTimeHook> RequestHook(HookService hookService)
        => hookService.RequestHook<UpdateTimeHook>(OnUpdateTime, HookOrder.Earliest);

      private void OnUpdateTime(IntPtr pModule, uint nCalendarDay, uint nTimeOfDay, uint nUpdateDifference)
      {
        CNWSModule module = new CNWSModule(pModule, false);
        uint hour = module.m_nCurrentHour;
        uint day = module.m_nCurrentDay;
        uint month = module.m_nCurrentMonth;
        uint year = module.m_nCurrentYear;
        uint dayState = module.m_nTimeOfDayState;

        Hook.CallOriginal(pModule, nCalendarDay, nTimeOfDay, nUpdateDifference);

        if (hour != module.m_nCurrentHour)
        {
          ProcessEvent(new OnCalendarTimeChange
          {
            TimeChangeType = TimeChangeType.Hour,
            OldValue = hour,
            NewValue = module.m_nCurrentHour,
          });
        }

        if (day != module.m_nCurrentDay)
        {
          ProcessEvent(new OnCalendarTimeChange
          {
            TimeChangeType = TimeChangeType.Day,
            OldValue = day,
            NewValue = module.m_nCurrentDay,
          });
        }

        if (month != module.m_nCurrentMonth)
        {
          ProcessEvent(new OnCalendarTimeChange
          {
            TimeChangeType = TimeChangeType.Month,
            OldValue = month,
            NewValue = module.m_nCurrentMonth,
          });
        }

        if (year != module.m_nCurrentYear)
        {
          ProcessEvent(new OnCalendarTimeChange
          {
            TimeChangeType = TimeChangeType.Year,
            OldValue = year,
            NewValue = module.m_nCurrentYear,
          });
        }

        if (dayState != module.m_nTimeOfDayState)
        {
          ProcessEvent(new OnCalendarTimeChange
          {
            TimeChangeType = TimeChangeType.TimeOfDay,
            OldValue = dayState,
            NewValue = module.m_nTimeOfDayState,
          });
        }
      }
    }
  }
}
