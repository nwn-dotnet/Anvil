using System.Numerics;
using Anvil.Services;

// ReSharper disable InconsistentNaming
namespace Anvil.Native
{
  internal static unsafe partial class Functions
  {
    public static class CNWSObject
    {
      [NativeFunction("_ZN10CNWSObject18AddUseObjectActionEj", "")]
      public delegate int AddUseObjectAction(void* pObject, uint oidObjectToUse);

      [NativeFunction("_ZN10CNWSObjectD1Ev", "")]
      public delegate void Destructor(void* pObject);

      [NativeFunction("_ZN10CNWSObject14GetDamageLevelEv", "")]
      public delegate byte GetDamageLevel(void* pObject);

      [NativeFunction("_ZN10CNWSObject18SpellCastAndImpactEj6Vectorjhjiihi", "")]
      public delegate void SpellCastAndImpact(void* pObject, int nSpellId, Vector3 targetPosition, uint oidTarget,
        byte nMultiClass, uint itemObj, int bSpellCountered, int bCounteringSpell, byte projectilePathType, int bInstantSpell);
    }
  }
}
