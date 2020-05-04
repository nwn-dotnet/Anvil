// ReSharper disable once CheckNamespace

using NWM.API;
using NWM.API.Constants;

namespace NWN
{
  public partial class Effect
  {
    public static Effect AbilityDecrease(Ability ability, int amount) => NWScript.EffectAbilityDecrease((int) ability, amount);
    public static Effect AbilityIncrease(Ability ability, int amount) => NWScript.EffectAbilityIncrease((int) ability, amount);
    public static Effect ACDecrease(int amount, ACBonus acType = ACBonus.Dodge) => NWScript.EffectACDecrease(amount, (int) acType);
    public static Effect ACIncrease(int amount, ACBonus acType = ACBonus.Dodge) => NWScript.EffectACIncrease(amount, (int) acType);
    public static Effect Appear() => NWScript.EffectAppear();
    // public static Effect AreaOfEffect()
    public static Effect AttackDecrease(int amount, AttackBonus penaltyType = AttackBonus.Misc) => NWScript.EffectAttackDecrease(amount, (int) penaltyType);
    public static Effect AttackIncrease(int amount, AttackBonus penaltyType = AttackBonus.Misc) => NWScript.EffectAttackIncrease(amount, (int) penaltyType);
    public static Effect Beam(VfxType fxType, NwGameObject emitter, BodyNode origin, bool missTarget = false) => NWScript.EffectBeam((int) fxType, emitter, (int) origin, missTarget.ToInt());
    public static Effect Blindness() => NWScript.EffectBlindness();
    public static Effect Charmed() => NWScript.EffectCharmed();
    public static Effect Concealment(int percentage, MissChanceType missChanceType = MissChanceType.Normal) => NWScript.EffectConcealment(percentage, (int) missChanceType);
    public static Effect Confused() => NWScript.EffectConfused();
    public static Effect Curse(int strMod = 1, int dexMod = 1, int conMod = 1, int intMod = 1, int wisMod = 1, int chaMod = 1) => NWScript.EffectCurse(strMod, dexMod, conMod, intMod, wisMod, chaMod);
    public static Effect CutsceneDominated() => NWScript.EffectCutsceneDominated();
    public static Effect CutsceneGhost() => NWScript.EffectCutsceneGhost();
    public static Effect CutsceneImmobilize() => NWScript.EffectCutsceneImmobilize();
    public static Effect CutsceneParalyze() => NWScript.EffectCutsceneParalyze();
  }
}