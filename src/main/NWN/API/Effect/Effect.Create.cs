using System;
using NWN.API.Constants;
using NWN.Core;

namespace NWN.API
{
  public partial class Effect
  {
    /// <summary>
    /// Creates an effect that decreases a certain ability score.
    /// </summary>
    /// <param name="ability">The ability to decrease.</param>
    /// <param name="amount">The amount to increase the ability by.</param>
    public static Effect AbilityDecrease(Ability ability, int amount)
      => NWScript.EffectAbilityDecrease((int) ability, amount);

    /// <summary>
    /// Creates an effect that increases a certain ability score.
    /// </summary>
    /// <param name="ability">The ability to increase.</param>
    /// <param name="amount">The amount to increase the ability by.</param>
    public static Effect AbilityIncrease(Ability ability, int amount)
      => NWScript.EffectAbilityIncrease((int) ability, amount);

    /// <summary>
    /// Creates an effect that decreases the AC of an object.<br/>
    /// </summary>
    /// <param name="amount">The AC penalty.</param>
    /// <param name="acType">The type of AC to decrease (Dodge).</param>
    public static Effect ACDecrease(int amount, ACBonus acType = ACBonus.Dodge)
      => NWScript.EffectACDecrease(amount, (int) acType);

    /// <summary>
    /// Creates an effect that increases the AC of an object.
    /// </summary>
    /// <param name="amount">The AC bonus.</param>
    /// <param name="acType">The AC type of the AC bonus.</param>
    public static Effect ACIncrease(int amount, ACBonus acType = ACBonus.Dodge)
      => NWScript.EffectACIncrease(amount, (int) acType);

    /// <summary>
    /// Creates a special effect to make an object "fly in".
    /// </summary>
    public static Effect Appear() => NWScript.EffectAppear();

    // TODO implement Action params
    public static Effect AreaOfEffect(int areaEffectId, string onEnterScript = null, string heartbeatScript = null, string onExitScript = null)
      => NWScript.EffectAreaOfEffect(areaEffectId, onEnterScript, heartbeatScript, onExitScript);

    /// <summary>
    /// Creates an effect that applies a penalty to an attack roll.
    /// </summary>
    /// <param name="amount">The penalty to apply.</param>
    /// <param name="penaltyType">The weapon slot this penalty should be applied to.</param>
    public static Effect AttackDecrease(int amount, AttackBonus penaltyType = AttackBonus.Misc)
      => NWScript.EffectAttackDecrease(amount, (int) penaltyType);

    /// <summary>
    /// Creates an effect that applies a bonus to an attack roll.
    /// </summary>
    /// <param name="amount">The bonus to apply.</param>
    /// <param name="bonusType">The weapon slot this bonus should be applied to.</param>
    public static Effect AttackIncrease(int amount, AttackBonus bonusType = AttackBonus.Misc)
      => NWScript.EffectAttackIncrease(amount, (int) bonusType);

    /// <summary>
    /// Creates a visual beam effect.
    /// </summary>
    /// <param name="fxType">The beam effect to render.</param>
    /// <param name="emitter">The origin object of this beam .</param>
    /// <param name="origin">The origin body part of this beam.</param>
    /// <param name="missTarget">If true, the beam will hit a random position near or past the applied target to indicate a miss.</param>
    public static Effect Beam(VfxType fxType, NwGameObject emitter, BodyNode origin, bool missTarget = false)
      => NWScript.EffectBeam((int) fxType, emitter, (int) origin, missTarget.ToInt());

    /// <summary>
    /// Creates an effect that blinds a creature.
    /// </summary>
    public static Effect Blindness() => NWScript.EffectBlindness();

    /// <summary>
    /// Creates an effect that increases the personal reputation to the target by 50 points.
    /// </summary>
    public static Effect Charmed() => NWScript.EffectCharmed();

    /// <summary>
    /// Creates an effect that conceals an object.
    /// </summary>
    /// <param name="percentage">The percentage chance for other attackers to miss (1-100).</param>
    /// <param name="missChanceType">The type of attack to apply this concealment against.</param>
    public static Effect Concealment(int percentage, MissChanceType missChanceType = MissChanceType.Normal)
      => NWScript.EffectConcealment(percentage, (int) missChanceType);

    /// <summary>
    /// Creates a confuse effect.
    /// </summary>
    public static Effect Confused() => NWScript.EffectConfused();

    /// <summary>
    /// Creates a curse effect.
    /// </summary>
    /// <param name="strMod">The penalty to Strength.</param>
    /// <param name="dexMod">The penalty to Dexterity.</param>
    /// <param name="conMod">The penalty to Constitution.</param>
    /// <param name="intMod">The penalty to Intelligence.</param>
    /// <param name="wisMod">The penalty to Wisdom.</param>
    /// <param name="chaMod">The penalty to Charisma.</param>
    public static Effect Curse(int strMod = 1, int dexMod = 1, int conMod = 1, int intMod = 1, int wisMod = 1, int chaMod = 1)
      => NWScript.EffectCurse(strMod, dexMod, conMod, intMod, wisMod, chaMod);

    /// <summary>
    /// Creates an effect that is guaranteed to dominate a creature, bypassing immunities and cannot be resisted.
    /// </summary>
    public static Effect CutsceneDominated() => NWScript.EffectCutsceneDominated();

    /// <summary>
    /// Creates an effect that allows creatures to pathfind through other creatures without bumping.
    /// </summary>
    public static Effect CutsceneGhost() => NWScript.EffectCutsceneGhost();

    /// <summary>
    /// Creates an effect that will prevent the target from moving, but can otherwise act unpenalized.
    /// The cutscene version bypasses immunities and cannot be resisted.
    /// </summary>
    public static Effect CutsceneImmobilize() => NWScript.EffectCutsceneImmobilize();

    /// <summary>
    /// Creates an effect that is guaranteed to paralyze a creature, but can not be resisted.
    /// </summary>
    public static Effect CutsceneParalyze() => NWScript.EffectCutsceneParalyze();

    /// <summary>
    /// Creates an effect that immediately applies damage to a target.
    /// </summary>
    /// <param name="amount">The amount of damage to apply.</param>
    /// <param name="damageType">The damage type to apply.</param>
    public static Effect Damage(int amount, DamageType damageType = DamageType.Magical)
      => NWScript.EffectDamage(amount, (int) damageType);

    /// <summary>
    /// Creates an effect that applies a penalty to a specified damage type.
    /// </summary>
    /// <param name="penalty">The damage penalty to apply.</param>
    /// <param name="damageType">The damage type to apply the penalty to.</param>
    public static Effect DamageDecrease(int penalty, DamageType damageType = DamageType.Magical)
      => NWScript.EffectDamageDecrease(penalty, (int) damageType);

    /// <summary>
    /// Creates an effect that decreases immunity to a certain damage type.<br/>
    /// If a creature does not have an immunity to the specified type, the creature will instead become more vulnerable to the damage, to a max of 100% (double damage).
    /// </summary>
    /// <param name="damageType">The type of damage to decrease immunity against.</param>
    /// <param name="pctImmunity">The percentage decrease (1-100).</param>
    public static Effect DamageImmunityDecrease(DamageType damageType, int pctImmunity)
      => NWScript.EffectDamageImmunityDecrease((int) damageType, pctImmunity);

    /// <summary>
    /// Creates an effect that increases immunity to a certain damage type, to a max of 100% (complete immunity).
    /// </summary>
    /// <param name="damageType">The type of damage to increase immunity against.</param>
    /// <param name="pctImmunity">The percentage increase (1-100).</param>
    public static Effect DamageImmunityIncrease(DamageType damageType, int pctImmunity)
      => NWScript.EffectDamageImmunityIncrease((int) damageType, pctImmunity);

    /// <summary>
    /// Creates an effect that applies a bonus to a specified damage type.
    /// </summary>
    /// <param name="bonus">The damage bonus to apply.</param>
    /// <param name="damageType">The damage type to apply the bonus to.</param>
    public static Effect DamageIncrease(int bonus, DamageType damageType = DamageType.Magical)
      => NWScript.EffectDamageIncrease(bonus, (int) damageType);

    /// <summary>
    /// Creates an effect that resists a constant amount of damage from a physical attack with a certain magical power.
    /// </summary>
    /// <param name="amount">The damage to remove from each attack.</param>
    /// <param name="damagePower">The max enchantment/power bonus of the weapon this effect will resist.</param>
    /// <param name="totalAbsorb">The total amount of damage to absorb, before the effect is removed (0 = infinite).</param>
    public static Effect DamageReduction(int amount, DamagePower damagePower, int totalAbsorb = 0)
      => NWScript.EffectDamageReduction(amount, (int) damagePower, totalAbsorb);

    /// <summary>
    /// Creates an effect that resists a constant amount of damage from a specific damage type.
    /// </summary>
    /// <param name="damageType">The type of damage to resist.</param>
    /// <param name="amount">The damage to remove from each attack.</param>
    /// <param name="totalAbsorb">The total amount of damage to absorb, before the effect is removed (0 = infinite).</param>
    public static Effect DamageResistance(DamageType damageType, int amount, int totalAbsorb = 0)
      => NWScript.EffectDamageResistance((int) damageType, amount, totalAbsorb);

    /// <summary>
    /// Creates an effect that reflects damage to melee attackers from successful hits.
    /// </summary>
    /// <param name="damageAmount">The flat amount of damage to reflect back to the attacker.</param>
    /// <param name="randomAmount">A special damage amount to additionally reflect back to the attacker.</param>
    /// <param name="damageType">The type of the damage reflected to the attacker.</param>
    public static Effect DamageShield(int damageAmount, DamageBonus randomAmount, DamageType damageType)
      => NWScript.EffectDamageShield(damageAmount, (int) randomAmount, (int) damageType);

    /// <summary>
    /// Creates an effect that shrouds an area in darkness, applying a miss chance to all those standing within.<br/>
    /// Creatures with an Ultravision effect will bypass this miss chance.
    /// </summary>
    public static Effect Darkness() => NWScript.EffectDarkness();

    /// <summary>
    /// Creates an effect that dazes a creature, preventing all actions but walking movement.
    /// </summary>
    public static Effect Dazed() => NWScript.EffectDazed();

    /// <summary>
    /// Creates an effect that deafens a creature, applying a 20% spell failure chance for spells with (V)erbal components.
    /// </summary>
    public static Effect Deaf() => NWScript.EffectDeaf();

    /// <summary>
    /// Creates an effect that kills, or destroys an object.
    /// </summary>
    /// <param name="spectacularDeath">If true, the object in which this is applied will die in an extraordinary fashion (gibs).</param>
    public static Effect Death(bool spectacularDeath = false)
      => NWScript.EffectDeath(spectacularDeath.ToInt());

    /// <summary>
    /// Creates an effect that causes the object to "fly away", before destroying itself.<br/>
    /// This effect should not be applied to PCs.
    /// </summary>
    public static Effect Disappear() => NWScript.EffectDisappear();

    /// <summary>
    /// Creates an effect that causes the object to "fly away", before appearing in another location.
    /// </summary>
    /// <param name="location">The new location to re-appear at.</param>
    /// <param name="animationType">The appear/disappear animation nto use.</param>
    public static Effect DisappearAppear(Location location, int animationType = 1)
      => NWScript.EffectDisappearAppear(location, animationType);

    /// <summary>
    /// Creates an effect that applies a disease to a creature.
    /// </summary>
    /// <param name="diseaseType">The type of disease to apply.</param>
    public static Effect Disease(DiseaseType diseaseType) => NWScript.EffectDisease((int) diseaseType);

    /// <summary>
    /// Creates an effect that attempts to strip all (Sp)ell effects on a target, up to a specified caster level.<br/>
    /// (Su)pernatural and (Ex)traordinary effects can never be dispelled.
    /// </summary>
    /// <param name="casterLevel">The max (inclusive) caster level of spell to dispel.</param>
    public static Effect DispelMagicAll(int casterLevel) => NWScript.EffectDispelMagicAll(casterLevel);

    /// <summary>
    /// Creates an effect that will attempt to strip the highest level spell effect on a target, up to a specified caster level.
    /// </summary>
    /// <param name="casterLevel">The max (inclusive) caster level of spell to dispel.</param>
    public static Effect DispelMagicBest(int casterLevel) => NWScript.EffectDispelMagicBest(casterLevel);

    /// <summary>
    /// Creates a dominate effect.<br/>
    /// A dominated creature is added to the effect creators party. This means they will become instantly friendly and not attack each other, and they are treated as a normal friend as a henchman would be.
    /// </summary>
    public static Effect Dominated() => NWScript.EffectDominated();

    /// <summary>
    /// Creates an effect that prevents all movement, and applies a -2 to all attacks and a -4 to AC.
    /// </summary>
    public static Effect Entangle() => NWScript.EffectEntangle();

    /// <summary>
    /// Creates an effect that causes the creature to become invisible and un-perceivable, and taking on an ethereal appearance.<br/>
    /// The effect is cancelled when the creature performs a hostile action.
    /// </summary>
    public static Effect Ethereal() => NWScript.EffectEthereal();

    public static Effect Frightened() => NWScript.EffectFrightened();

    public static Effect Haste() => NWScript.EffectHaste();

    public static Effect Heal(int damageToHeal) => NWScript.EffectHeal(damageToHeal);

    public static Effect HitPointChangeWhenDying(float hpChangePerRound)
      => NWScript.EffectHitPointChangeWhenDying(hpChangePerRound);

    public static Effect Immunity(ImmunityType immunityType) => NWScript.EffectImmunity((int) immunityType);

    public static Effect Invisibility(InvisibilityType invisibilityType)
      => NWScript.EffectInvisibility((int) invisibilityType);

    public static Effect Knockdown() => NWScript.EffectKnockdown();

    public static Effect MissChance(int missPct, MissChanceType missChanceType = MissChanceType.Normal)
      => NWScript.EffectMissChance(missPct, (int) missChanceType);

    public static Effect ModifyAttacks(int numAttacks) => NWScript.EffectModifyAttacks(numAttacks);

    public static Effect MovementSpeedDecrease(int pctChange) => NWScript.EffectMovementSpeedDecrease(pctChange);

    public static Effect MovementSpeedIncrease(int pctChange) => NWScript.EffectMovementSpeedIncrease(pctChange);

    public static Effect NegativeLevel(int numLevels) => NWScript.EffectNegativeLevel(numLevels);

    public static Effect Paralyze() => NWScript.EffectParalyze();

    public static Effect Petrify() => NWScript.EffectPetrify();

    public static Effect Poison(PoisonType poisonType) => NWScript.EffectPoison((int) poisonType);

    public static Effect Polymorph(PolymorphType polymorphType, bool locked = false)
      => NWScript.EffectPolymorph((int) polymorphType, locked.ToInt());

    public static Effect Regenerate(int amountPerInterval, TimeSpan interval)
      => NWScript.EffectRegenerate(amountPerInterval, (float) interval.TotalSeconds);

    public static Effect Resurrection() => NWScript.EffectResurrection();

    public static Effect Sanctuary(int difficultyClass) => NWScript.EffectSanctuary(difficultyClass);

    public static Effect SavingThrowDecrease(SavingThrow savingThrow, int amount, SavingThrowType savingThrowType = SavingThrowType.All)
      => NWScript.EffectSavingThrowDecrease((int) savingThrow, amount, (int) savingThrowType);

    public static Effect SavingThrowIncrease(SavingThrow savingThrow, int amount, SavingThrowType savingThrowType = SavingThrowType.All)
      => NWScript.EffectSavingThrowIncrease((int) savingThrow, amount, (int) savingThrowType);

    public static Effect SeeInvisible() => NWScript.EffectSeeInvisible();

    public static Effect Silence() => NWScript.EffectSilence();

    public static Effect SkillDecrease(Skill skill, int amount)
      => NWScript.EffectSkillDecrease((int) skill, amount);

    public static Effect SkillIncrease(Skill skill, int amount)
      => NWScript.EffectSkillIncrease((int) skill, amount);

    public static Effect Sleep() => NWScript.EffectSleep();

    public static Effect Slow() => NWScript.EffectSlow();

    public static Effect SpellFailure(int failPct, SpellSchool spellSchool = SpellSchool.General)
      => NWScript.EffectSpellFailure(failPct, (int) spellSchool);

    public static Effect SpellImmunity(Spell spell = Spell.AllSpells)
      => NWScript.EffectSpellImmunity((int) spell);

    public static Effect SpellLevelAbsorption(int maxSpellLevel, int totalSpellsAbsorbed = 0, SpellSchool spellSchool = SpellSchool.General)
      => NWScript.EffectSpellLevelAbsorption(maxSpellLevel, totalSpellsAbsorbed, (int) spellSchool);

    public static Effect SpellResistanceDecrease(int amount) => NWScript.EffectSpellResistanceDecrease(amount);

    public static Effect SpellResistanceIncrease(int amount) => NWScript.EffectSpellResistanceIncrease(amount);

    public static Effect Stunned() => NWScript.EffectStunned();

    public static Effect SummonCreature(string creatureResRef, VfxType vfxType, TimeSpan delay = default, int appearType = 0)
      => NWScript.EffectSummonCreature(creatureResRef, (int) vfxType, (float) delay.TotalSeconds, appearType);

    public static Effect Swarm(bool loop, string creatureTemplate1, string creatureTemplate2 = null, string creatureTemplate3 = null, string creatureTemplate4 = null)
      => NWScript.EffectSwarm(loop.ToInt(), creatureTemplate1, creatureTemplate2, creatureTemplate3, creatureTemplate4);

    public static Effect TemporaryHitpoints(int hitPoints) => NWScript.EffectTemporaryHitpoints(hitPoints);

    public static Effect TimeStop() => NWScript.EffectTimeStop();

    public static Effect TrueSeeing() => NWScript.EffectTrueSeeing();

    public static Effect Turned() => NWScript.EffectTurned();

    public static Effect TurnResistanceDecrease(int hitDiceDecrease)
      => NWScript.EffectTurnResistanceDecrease(hitDiceDecrease);

    public static Effect TurnResistanceIncrease(int hitDiceIncrease)
      => NWScript.EffectTurnResistanceIncrease(hitDiceIncrease);

    public static Effect Ultravision() => NWScript.EffectUltravision();

    public static Effect VisualEffect(VfxType visualEffectId, bool missEffect = false)
      => NWScript.EffectVisualEffect((int) visualEffectId, missEffect.ToInt());
  }
}
