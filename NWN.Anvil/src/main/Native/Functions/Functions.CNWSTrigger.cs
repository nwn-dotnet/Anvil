using Anvil.Services;

// ReSharper disable InconsistentNaming
namespace Anvil.Native
{
  internal static unsafe partial class Functions
  {
    public static class CNWSTrigger
    {
      [NativeFunction("_ZN11CNWSTrigger12EventHandlerEjjPvjj", "?EventHandler@CNWSTrigger@@UEAAXIIPEAXII@Z")]
      public delegate void EventHandler(void* pTrigger, uint nEventId, uint nCallerObjectId, void* pScript, uint nCalendarDay, uint nTimeOfDay);
    }
  }
}
