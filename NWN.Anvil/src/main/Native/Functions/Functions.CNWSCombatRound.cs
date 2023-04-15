using Anvil.Services;

// ReSharper disable InconsistentNaming
namespace Anvil.Native
{
  internal static unsafe partial class Functions
  {
    public static class CNWSCombatRound
    {
      [NativeFunction("_ZN15CNWSCombatRound16StartCombatRoundEj", "")]
      public delegate void StartCombatRound(void* pCombatRound, uint oidTarget);
    }
  }
}
