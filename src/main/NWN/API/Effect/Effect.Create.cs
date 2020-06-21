using System;
using NWN.API.Constants;
using NWN.Core;

namespace NWN.API
{
  public partial class Effect
  {
    /// <summary>
    /// Creates an Ability decrease effect.
    /// </summary>
    /// <param name="ability">The ability to decrease.</param>
    /// <param name="amount">The amount to increase the ability by.</param>
    public static Effect AbilityDecrease(Ability ability, int amount) => NWScript.EffectAbilityDecrease((int) ability, amount);

    /// <summary>
    /// Creates an Ability increase effect.
    /// </summary>
    /// <param name="ability">The ability to increase.</param>
    /// <param name="amount">The amount to increase the ability by.</param>
    public static Effect AbilityIncrease(Ability ability, int amount) => NWScript.EffectAbilityIncrease((int) ability, amount);

    /// <summary>
    /// Creates an AC decrease effect.<br/>
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="acType">The type of AC to decrease (Dodge)</param>
    public static Effect ACDecrease(int amount, ACBonus acType = ACBonus.Dodge) => NWScript.EffectACDecrease(amount, (int) acType);

    /// <summary>
    /// Creates an AC increase effect.
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="acType"></param>
    public static Effect ACIncrease(int amount, ACBonus acType = ACBonus.Dodge) => NWScript.EffectACIncrease(amount, (int) acType);

    /// <summary>
    /// Creates a special effect to make an object "fly in".
    /// </summary>
    public static Effect Appear() => NWScript.EffectAppear();

    public static Effect AreaOfEffect(int areaEffectId, string onEnterScript = null, string heartbeatScript = null, string onExitScript = null) => NWScript.EffectAreaOfEffect(areaEffectId, onEnterScript, heartbeatScript, onExitScript);

    /// <summary>
    /// Creates an Attack decrease effect.
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="penaltyType"></param>
    public static Effect AttackDecrease(int amount, AttackBonus penaltyType = AttackBonus.Misc) => NWScript.EffectAttackDecrease(amount, (int) penaltyType);

    /// <summary>
    /// Creates an Attack increase effect.
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="penaltyType"></param>
    public static Effect AttackIncrease(int amount, AttackBonus penaltyType = AttackBonus.Misc) => NWScript.EffectAttackIncrease(amount, (int) penaltyType);

    /// <summary>
    /// Creates a beam effect.
    /// </summary>
    /// <param name="fxType"></param>
    /// <param name="emitter"></param>
    /// <param name="origin"></param>
    /// <param name="missTarget"></param>
    public static Effect Beam(VfxType fxType, NwGameObject emitter, BodyNode origin, bool missTarget = false) => NWScript.EffectBeam((int) fxType, emitter, (int) origin, missTarget.ToInt());

    /// <summary>
    /// Creates a blindness effect.
    /// </summary>
    public static Effect Blindness() => NWScript.EffectBlindness();

    /// <summary>
    /// Creates a charm effect.
    /// </summary>
    public static Effect Charmed() => NWScript.EffectCharmed();

    /// <summary>
    /// Creates an effect to conceal an object.
    /// </summary>
    /// <param name="percentage"></param>
    /// <param name="missChanceType"></param>
    public static Effect Concealment(int percentage, MissChanceType missChanceType = MissChanceType.Normal) => NWScript.EffectConcealment(percentage, (int) missChanceType);

    /// <summary>
    /// Creates a confuse effect.
    /// </summary>
    public static Effect Confused() => NWScript.EffectConfused();

    /// <summary>
    /// Creates a curse effect.
    /// </summary>
    /// <param name="strMod"></param>
    /// <param name="dexMod"></param>
    /// <param name="conMod"></param>
    /// <param name="intMod"></param>
    /// <param name="wisMod"></param>
    /// <param name="chaMod"></param>
    public static Effect Curse(int strMod = 1, int dexMod = 1, int conMod = 1, int intMod = 1, int wisMod = 1, int chaMod = 1) => NWScript.EffectCurse(strMod, dexMod, conMod, intMod, wisMod, chaMod);

    /// <summary>
    /// Creates a dominate effect that cannot be resisted.
    /// </summary>
    public static Effect CutsceneDominated() => NWScript.EffectCutsceneDominated();

    /// <summary>
    /// Creates an effect that allows creatures to pathfind through other creatures without bumping.
    /// </summary>
    public static Effect CutsceneGhost() => NWScript.EffectCutsceneGhost();

    public static Effect CutsceneImmobilize() => NWScript.EffectCutsceneImmobilize();
    public static Effect CutsceneParalyze() => NWScript.EffectCutsceneParalyze();
    public static Effect Damage(int amount, DamageType damageType = DamageType.Magical, DamagePower damagePower = DamagePower.Normal) => NWScript.EffectDamage(amount, (int) damageType, (int) damagePower);
    public static Effect DamageDecrease(int penalty, DamageType damageType = DamageType.Magical) => NWScript.EffectDamageDecrease(penalty, (int) damageType);
    public static Effect DamageImmunityDecrease(DamageType damageType, int pctImmunity) => NWScript.EffectDamageImmunityDecrease((int) damageType, pctImmunity);
    public static Effect DamageImmunityIncrease(DamageType damageType, int pctImmunity) => NWScript.EffectDamageImmunityIncrease((int) damageType, pctImmunity);
    public static Effect DamageIncrease(int bonus, DamageType damageType = DamageType.Magical) => NWScript.EffectDamageIncrease(bonus, (int) damageType);
    public static Effect DamageReduction(int amount, DamagePower damagePower, int totalAbsorb = 0) => NWScript.EffectDamageReduction(amount, (int) damagePower, totalAbsorb);
    public static Effect DamageResistance(DamageType damageType, int amount, int totalAbsorb = 0) => NWScript.EffectDamageResistance((int) damageType, amount, totalAbsorb);
    public static Effect DamageShield(int damageAmount, DamageBonus randomAmount, DamageType damageType) => NWScript.EffectDamageShield(damageAmount, (int) randomAmount, (int) damageType);
    public static Effect Darkness() => NWScript.EffectDarkness();
    public static Effect Dazed() => NWScript.EffectDazed();
    public static Effect Deaf() => NWScript.EffectDeaf();
    public static Effect Death(bool spectacularDeath = false, bool displayFeedback = true) => NWScript.EffectDeath(spectacularDeath.ToInt(), displayFeedback.ToInt());
    public static Effect Disappear() => NWScript.EffectDisappear();
    public static Effect DisappearAppear(Location location, int animationType = 1) => NWScript.EffectDisappearAppear(location, animationType);
    public static Effect Disease(DiseaseType diseaseType) => NWScript.EffectDisease((int) diseaseType);
    public static Effect DispelMagicAll(int casterLevel) => NWScript.EffectDispelMagicAll(casterLevel);
    public static Effect DispelMagicBest(int casterLevel) => NWScript.EffectDispelMagicBest(casterLevel);
    public static Effect Dominated() => NWScript.EffectDominated();
    public static Effect Entangle() => NWScript.EffectEntangle();
    public static Effect Ethereal() => NWScript.EffectEthereal();
    public static Effect Frightened() => NWScript.EffectFrightened();
    public static Effect Haste() => NWScript.EffectHaste();
    public static Effect Heal(int damageToHeal) => NWScript.EffectHeal(damageToHeal);
    public static Effect HitPointChangeWhenDying(float hpChangePerRound) => NWScript.EffectHitPointChangeWhenDying(hpChangePerRound);
    public static Effect Immunity(ImmunityType immunityType) => NWScript.EffectImmunity((int) immunityType);
    public static Effect Invisibility(InvisibilityType invisibilityType) => NWScript.EffectInvisibility((int) invisibilityType);
    public static Effect Knockdown() => NWScript.EffectKnockdown();
    public static Effect MissChance(int missPct, MissChanceType missChanceType = MissChanceType.Normal) => NWScript.EffectMissChance(missPct, (int) missChanceType);
    public static Effect ModifyAttacks(int numAttacks) => NWScript.EffectModifyAttacks(numAttacks);
    public static Effect MovementSpeedDecrease(int pctChange) => NWScript.EffectMovementSpeedDecrease(pctChange);
    public static Effect MovementSpeedIncrease(int pctChange) => NWScript.EffectMovementSpeedIncrease(pctChange);
    public static Effect NegativeLevel(int numLevels) => NWScript.EffectNegativeLevel(numLevels);
    public static Effect Paralyze() => NWScript.EffectParalyze();
    public static Effect Petrify() => NWScript.EffectPetrify();
    public static Effect Poison(PoisonType poisonType) => NWScript.EffectPoison((int) poisonType);
    public static Effect Polymorph(PolymorphType polymorphType, bool locked = false) => NWScript.EffectPolymorph((int) polymorphType, locked.ToInt());
    public static Effect Regenerate(int amountPerInterval, TimeSpan interval) => NWScript.EffectRegenerate(amountPerInterval, (float) interval.TotalSeconds);
    public static Effect Resurrection() => NWScript.EffectResurrection();
    public static Effect Sanctuary(int difficultyClass) => NWScript.EffectSanctuary(difficultyClass);
    public static Effect SavingThrowDecrease(SavingThrow savingThrow, int amount, SavingThrowType savingThrowType = SavingThrowType.All) => NWScript.EffectSavingThrowDecrease((int) savingThrow, amount, (int) savingThrowType);
    public static Effect SavingThrowIncrease(SavingThrow savingThrow, int amount, SavingThrowType savingThrowType = SavingThrowType.All) => NWScript.EffectSavingThrowIncrease((int) savingThrow, amount, (int) savingThrowType);
    public static Effect SeeInvisible() => NWScript.EffectSeeInvisible();
    public static Effect Silence() => NWScript.EffectSilence();
    public static Effect SkillDecrease(Skill skill, int amount) => NWScript.EffectSkillDecrease((int) skill, amount);
    public static Effect SkillIncrease(Skill skill, int amount) => NWScript.EffectSkillIncrease((int) skill, amount);
    public static Effect Sleep() => NWScript.EffectSleep();
    public static Effect Slow() => NWScript.EffectSlow();
    public static Effect SpellFailure(int failPct, SpellSchool spellSchool = SpellSchool.General) => NWScript.EffectSpellFailure(failPct, (int) spellSchool);
    public static Effect SpellImmunity(Spell spell = Spell.AllSpells) => NWScript.EffectSpellImmunity((int) spell);
    public static Effect SpellLevelAbsorption(int maxSpellLevel, int totalSpellsAbsorbed = 0, SpellSchool spellSchool = SpellSchool.General) => NWScript.EffectSpellLevelAbsorption(maxSpellLevel, totalSpellsAbsorbed, (int) spellSchool);
    public static Effect SpellResistanceDecrease(int amount) => NWScript.EffectSpellResistanceDecrease(amount);
    public static Effect SpellResistanceIncrease(int amount) => NWScript.EffectSpellResistanceIncrease(amount);
    public static Effect Stunned() => NWScript.EffectStunned();
    public static Effect SummonCreature(string creatureResRef, VfxType vfxType, TimeSpan delay = default, int appearType = 0) => NWScript.EffectSummonCreature(creatureResRef, (int) vfxType, (float) delay.TotalSeconds, appearType);
    public static Effect Swarm(bool loop, string creatureTemplate1, string creatureTemplate2 = null, string creatureTemplate3 = null, string creatureTemplate4 = null) => NWScript.EffectSwarm(loop.ToInt(), creatureTemplate1, creatureTemplate2, creatureTemplate3, creatureTemplate4);
    public static Effect TemporaryHitpoints(int hitPoints) => NWScript.EffectTemporaryHitpoints(hitPoints);
    public static Effect TimeStop() => NWScript.EffectTimeStop();
    public static Effect TrueSeeing() => NWScript.EffectTrueSeeing();
    public static Effect Turned() => NWScript.EffectTurned();
    public static Effect TurnResistanceDecrease(int hitDiceDecrease) => NWScript.EffectTurnResistanceDecrease(hitDiceDecrease);
    public static Effect TurnResistanceIncrease(int hitDiceIncrease) => NWScript.EffectTurnResistanceIncrease(hitDiceIncrease);
    public static Effect Ultravision() => NWScript.EffectUltravision();
    public static Effect VisualEffect(VfxType visualEffectId, bool missEffect = false) => NWScript.EffectVisualEffect((int) visualEffectId, missEffect.ToInt());
  }
}