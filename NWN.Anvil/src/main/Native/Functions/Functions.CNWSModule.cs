using Anvil.Services;

// ReSharper disable InconsistentNaming
namespace Anvil.Native
{
  internal static unsafe partial class Functions
  {
    public static class CNWSModule
    {
      [NativeFunction("_ZN10CNWSModule20LoadModuleInProgressEii", "?LoadModuleInProgress@CNWSModule@@QEAAIHH@Z")]
      public delegate uint LoadModuleInProgress(void* pModule, int nAreasLoaded, int nAreasToLoad);

      [NativeFunction("_ZN10CNWSModule10UpdateTimeEjjj", "?UpdateTime@CNWSModule@@QEAAXIII@Z")]
      public delegate void UpdateTime(void* pModule, uint nCalendarDay, uint nTimeOfDay, uint nUpdateDifference);
    }
  }
}
