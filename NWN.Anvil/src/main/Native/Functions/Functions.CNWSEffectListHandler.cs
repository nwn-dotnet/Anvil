using Anvil.Services;

// ReSharper disable InconsistentNaming
namespace Anvil.Native
{
  internal static unsafe partial class Functions
  {
    public static class CNWSEffectListHandler
    {
      [NativeFunction("_ZN21CNWSEffectListHandler13OnApplyDamageEP10CNWSObjectP11CGameEffecti", "")]
      public delegate int OnApplyDamage(void* pEffectListHandler, void* pObject, void* pEffect, int bLoadingGame);

      [NativeFunction("_ZN21CNWSEffectListHandler13OnApplyDisarmEP10CNWSObjectP11CGameEffecti", "")]
      public delegate int OnApplyDisarm(void* pEffectListHandler, void* pObject, void* pEffect, int bLoadingGame);

      [NativeFunction("_ZN21CNWSEffectListHandler11OnApplyHealEP10CNWSObjectP11CGameEffecti", "")]
      public delegate int OnApplyHeal(void* pEffectListHandler, void* pObject, void* pGameEffect, int bLoadingGame);

      [NativeFunction("_ZN21CNWSEffectListHandler15OnEffectAppliedEP10CNWSObjectP11CGameEffecti", "")]
      public delegate int OnEffectApplied(void* pEffectListHandler, void* pObject, void* pEffect, int bLoadingGame);

      [NativeFunction("_ZN21CNWSEffectListHandler15OnEffectRemovedEP10CNWSObjectP11CGameEffect", "")]
      public delegate int OnEffectRemoved(void* pEffectListHandler, void* pObject, void* pEffect);

      [NativeFunction("_ZN21CNWSEffectListHandler26OnRemoveLimitMovementSpeedEP10CNWSObjectP11CGameEffect", "")]
      public delegate int OnRemoveLimitMovementSpeed(void* pEffectListHandler, void* pObject, void* pEffect);
    }
  }
}
