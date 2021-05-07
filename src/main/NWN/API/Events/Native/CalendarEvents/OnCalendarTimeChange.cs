using System.Runtime.InteropServices;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed unsafe class OnCalendarTimeChange : IEvent
  {
    public TimeChangeType TimeChangeType { get; private init; }

    public uint OldValue { get; private init; }

    public uint NewValue { get; private init; }

    NwObject IEvent.Context => null;

    internal sealed unsafe class Factory : NativeEventFactory<Factory.UpdateTimeHook>
    {
      internal delegate void UpdateTimeHook(void* pModule, uint nCalendarDay, uint nTimeOfDay, uint nUpdateDifference);

      protected override FunctionHook<UpdateTimeHook> RequestHook()
      {
        delegate* unmanaged<void*, uint, uint, uint, void> pHook = &OnUpdateTime;
        return HookService.RequestHook<UpdateTimeHook>(NWNXLib.Functions._ZN10CNWSModule10UpdateTimeEjjj, pHook, HookOrder.Earliest);
      }

      [UnmanagedCallersOnly]
      private static void OnUpdateTime(void* pModule, uint nCalendarDay, uint nTimeOfDay, uint nUpdateDifference)
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
