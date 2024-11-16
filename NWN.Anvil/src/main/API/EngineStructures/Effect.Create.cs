using System;
using System.Collections.Generic;
using Anvil.Services;
using NWN.Core;

namespace Anvil.API
{
  public sealed partial class Effect
  {
    /// <summary>
    /// Creates an effect that decreases a certain ability score.
    /// </summary>
    /// <param name="ability">The ability to decrease.</param>
    /// <param name="amount">The amount to increase the ability by.</param>
    public static Effect AbilityDecrease(Ability ability, int amount)
    {
      return NWScript.EffectAbilityDecrease((int)ability, amount)!;
    }

    /// <summary>
    /// Creates an effect that increases a certain ability score.
    /// </summary>
    /// <param name="ability">The ability to increase.</param>
    /// <param name="amount">The amount to increase the ability by.</param>
    public static Effect AbilityIncrease(Ability ability, int amount)
    {
      return NWScript.EffectAbilityIncrease((int)ability, amount)!;
    }

    /// <summary>
    /// Creates an effect that decreases the AC of an object.<br/>
    /// </summary>
    /// <param name="amount">The AC penalty.</param>
    /// <param name="acType">The type of AC to decrease (Dodge).</param>
    public static Effect ACDecrease(int amount, ACBonus acType = ACBonus.Dodge)
    {
      return NWScript.EffectACDecrease(amount, (int)acType)!;
    }

    /// <summary>
    /// Creates an effect that increases the AC of an object.
    /// </summary>
    /// <param name="amount">The AC bonus.</param>
    /// <param name="acType">The AC type of the AC bonus.</param>
    public static Effect ACIncrease(int amount, ACBonus acType = ACBonus.Dodge)
    {
      return NWScript.EffectACIncrease(amount, (int)acType)!;
    }

    /// <summary>
    /// Creates a special effect to make an object "fly in".
    /// </summary>
    public static Effect Appear()
    {
      return NWScript.EffectAppear()!;
    }

    /// <summary>
    /// Creates an area of effect (AOE) effect.
    /// </summary>
    /// <param name="vfxType">The persistent area visual effect to use for this effect.</param>
    /// <param name="onEnterHandle">The callback to invoke when something enters this area of effect.</param>
    /// <param name="heartbeatHandle">The callback to invoke when something is inside the area of effect during a heartbeat (~6 seconds)</param>
    /// <param name="onExitHandle">The callback to invoke when something leaves this area of effect.</param>
    public static Effect AreaOfEffect(PersistentVfxTableEntry vfxType, ScriptCallbackHandle? onEnterHandle = null, ScriptCallbackHandle? heartbeatHandle = null, ScriptCallbackHandle? onExitHandle = null)
    {
      onEnterHandle?.AssertValid();
      heartbeatHandle?.AssertValid();
      onExitHandle?.AssertValid();

      return NWScript.EffectAreaOfEffect(vfxType.RowIndex, onEnterHandle?.ScriptName ?? string.Empty, heartbeatHandle?.ScriptName ?? string.Empty, onExitHandle?.ScriptName ?? string.Empty)!;
    }

    /// <summary>
    /// Creates an effect that applies a penalty to an attack roll.
    /// </summary>
    /// <param name="amount">The penalty to apply.</param>
    /// <param name="penaltyType">The weapon slot this penalty should be applied to.</param>
    public static Effect AttackDecrease(int amount, AttackBonus penaltyType = AttackBonus.Misc)
    {
      return NWScript.EffectAttackDecrease(amount, (int)penaltyType)!;
    }

    /// <summary>
    /// Creates an effect that applies a bonus to an attack roll.
    /// </summary>
    /// <param name="amount">The bonus to apply.</param>
    /// <param name="bonusType">The weapon slot this bonus should be applied to.</param>
    public static Effect AttackIncrease(int amount, AttackBonus bonusType = AttackBonus.Misc)
    {
      return NWScript.EffectAttackIncrease(amount, (int)bonusType)!;
    }

    /// <summary>
    /// Creates a visual beam effect.
    /// </summary>
    /// <param name="fxType">The beam effect to render.</param>
    /// <param name="emitter">The origin object of this beam .</param>
    /// <param name="origin">The origin body part of this beam.</param>
    /// <param name="missTarget">If true, the beam will hit a random position near or past the applied target to indicate a miss.</param>
    public static Effect Beam(VfxType fxType, NwGameObject emitter, BodyNode origin, bool missTarget = false)
    {
      return NWScript.EffectBeam((int)fxType, emitter, (int)origin, missTarget.ToInt())!;
    }

    /// <summary>
    /// Creates an effect that blinds a creature.
    /// </summary>
    public static Effect Blindness()
    {
      return NWScript.EffectBlindness()!;
    }

    /// <summary>
    /// Creates an effect that grants a bonus feat, similar to <see cref="ItemProperty.BonusFeat"/>.
    /// </summary>
    /// <param name="feat">The feat to grant.</param>
    public static Effect BonusFeat(NwFeat feat)
    {
      return NWScript.EffectBonusFeat(feat.Id)!;
    }

    /// <summary>
    /// Creates an effect that increases the personal reputation to the target by 50 points.
    /// </summary>
    public static Effect Charmed()
    {
      return NWScript.EffectCharmed()!;
    }

    /// <summary>
    /// Creates an effect that conceals an object.
    /// </summary>
    /// <param name="percentage">A positive number representing the percentage chance for other attackers to miss (1-100).</param>
    /// <param name="missChanceType">The type of attack to apply this concealment against.</param>
    public static Effect Concealment(int percentage, MissChanceType missChanceType = MissChanceType.Normal)
    {
      return NWScript.EffectConcealment(percentage, (int)missChanceType)!;
    }

    /// <summary>
    /// Creates a confuse effect.
    /// </summary>
    public static Effect Confused()
    {
      return NWScript.EffectConfused()!;
    }

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
    {
      return NWScript.EffectCurse(strMod, dexMod, conMod, intMod, wisMod, chaMod)!;
    }

    /// <summary>
    /// Creates an effect that is guaranteed to dominate a creature, bypassing immunities and cannot be resisted.
    /// </summary>
    public static Effect CutsceneDominated()
    {
      return NWScript.EffectCutsceneDominated()!;
    }

    /// <summary>
    /// Creates an effect that allows creatures to pathfind through other creatures without bumping.
    /// </summary>
    public static Effect CutsceneGhost()
    {
      return NWScript.EffectCutsceneGhost()!;
    }

    /// <summary>
    /// Creates an effect that will prevent the target from moving, but can otherwise act unpenalized.
    /// The cutscene version bypasses immunities and cannot be resisted.
    /// </summary>
    public static Effect CutsceneImmobilize()
    {
      return NWScript.EffectCutsceneImmobilize()!;
    }

    /// <summary>
    /// Creates an effect that is guaranteed to paralyze a creature, but can not be resisted.
    /// </summary>
    public static Effect CutsceneParalyze()
    {
      return NWScript.EffectCutsceneParalyze()!;
    }

    /// <summary>
    /// Creates an effect that immediately applies damage to a target.
    /// </summary>
    /// <param name="amount">The amount of damage to apply.</param>
    /// <param name="damageType">The damage type to apply.</param>
    public static Effect Damage(int amount, DamageType damageType = DamageType.Magical)
    {
      return NWScript.EffectDamage(amount, (int)damageType)!;
    }

    /// <summary>
    /// Creates an effect that applies a penalty to a specified damage type.
    /// </summary>
    /// <param name="penalty">The damage penalty to apply.</param>
    /// <param name="damageType">The damage type to apply the penalty to.</param>
    public static Effect DamageDecrease(int penalty, DamageType damageType = DamageType.Magical)
    {
      return NWScript.EffectDamageDecrease(penalty, (int)damageType)!;
    }

    /// <summary>
    /// Creates an effect that decreases immunity to a certain damage type.<br/>
    /// If a creature does not have an immunity to the specified type, the creature will instead become more vulnerable to the damage, to a max of 100% (double damage).
    /// </summary>
    /// <param name="damageType">The type of damage to decrease immunity against.</param>
    /// <param name="pctImmunity">A positive number representing the percentage decrease (1-100).</param>
    public static Effect DamageImmunityDecrease(DamageType damageType, int pctImmunity)
    {
      return NWScript.EffectDamageImmunityDecrease((int)damageType, pctImmunity)!;
    }

    /// <summary>
    /// Creates an effect that increases immunity to a certain damage type, to a max of 100% (complete immunity).
    /// </summary>
    /// <param name="damageType">The type of damage to increase immunity against.</param>
    /// <param name="pctImmunity">A positive number representing the percentage increase (1-100).</param>
    public static Effect DamageImmunityIncrease(DamageType damageType, int pctImmunity)
    {
      return NWScript.EffectDamageImmunityIncrease((int)damageType, pctImmunity)!;
    }

    /// <summary>
    /// Creates an effect that applies a bonus to a specified damage type.
    /// </summary>
    /// <param name="bonus">The damage bonus to apply.</param>
    /// <param name="damageType">The damage type to apply the bonus to.</param>
    public static Effect DamageIncrease(int bonus, DamageType damageType = DamageType.Magical)
    {
      return NWScript.EffectDamageIncrease(bonus, (int)damageType)!;
    }

    /// <summary>
    /// Creates an effect that applies a bonus to a specified damage type.
    /// </summary>
    /// <param name="bonus">The damage bonus to apply.</param>
    /// <param name="damageType">The damage type to apply the bonus to.</param>
    public static Effect DamageIncrease(DamageBonus bonus, DamageType damageType = DamageType.Magical)
    {
      return NWScript.EffectDamageIncrease((int)bonus, (int)damageType)!;
    }

    /// <summary>
    /// Creates an effect that resists a constant amount of damage from a physical attack with a certain magical power.
    /// </summary>
    /// <param name="amount">The damage to remove from each attack.</param>
    /// <param name="damagePower">The max enchantment/power bonus of the weapon this effect will resist.</param>
    /// <param name="totalAbsorb">The total amount of damage to absorb, before the effect is removed (0 = infinite).</param>
    /// <param name="rangedOnly">Set to true to have this damage reduction effect only apply to ranged attacks.</param>
    public static Effect DamageReduction(int amount, DamagePower damagePower, int totalAbsorb = 0, bool rangedOnly = false)
    {
      return NWScript.EffectDamageReduction(amount, (int)damagePower, totalAbsorb, rangedOnly.ToInt())!;
    }

    /// <summary>
    /// Creates an effect that resists a constant amount of damage from a specific damage type.
    /// </summary>
    /// <param name="damageType">The type of damage to resist.</param>
    /// <param name="amount">The damage to remove from each attack.</param>
    /// <param name="totalAbsorb">The total amount of damage to absorb, before the effect is removed (0 = infinite).</param>
    /// <param name="rangedOnly">Set to true to have this damage resistance effect only apply to ranged attacks.</param>
    public static Effect DamageResistance(DamageType damageType, int amount, int totalAbsorb = 0, bool rangedOnly = false)
    {
      return NWScript.EffectDamageResistance((int)damageType, amount, totalAbsorb, rangedOnly.ToInt())!;
    }

    /// <summary>
    /// Creates an effect that reflects damage to melee attackers from successful hits.
    /// </summary>
    /// <param name="damageAmount">The flat amount of damage to reflect back to the attacker.</param>
    /// <param name="randomAmount">A special damage amount to additionally reflect back to the attacker.</param>
    /// <param name="damageType">The type of the damage reflected to the attacker.</param>
    public static Effect DamageShield(int damageAmount, DamageBonus randomAmount, DamageType damageType)
    {
      return NWScript.EffectDamageShield(damageAmount, (int)randomAmount, (int)damageType)!;
    }

    /// <summary>
    /// Creates an effect that shrouds an area in darkness, applying a miss chance to all those standing within.<br/>
    /// Creatures with an Ultravision effect will bypass this miss chance.
    /// </summary>
    public static Effect Darkness()
    {
      return NWScript.EffectDarkness()!;
    }

    /// <summary>
    /// Creates an effect that dazes a creature, preventing all actions but walking movement.
    /// </summary>
    public static Effect Dazed()
    {
      return NWScript.EffectDazed()!;
    }

    /// <summary>
    /// Creates an effect that deafens a creature, applying a 20% spell failure chance for spells with (V)erbal components.
    /// </summary>
    public static Effect Deaf()
    {
      return NWScript.EffectDeaf()!;
    }

    /// <summary>
    /// Creates an effect that kills, or destroys an object.
    /// </summary>
    /// <param name="spectacularDeath">If true, the object in which this is applied will die in an extraordinary fashion (gibs).</param>
    /// <param name="feedback">If false, excludes the "XXX: Dead" feedback message.</param>
    public static Effect Death(bool spectacularDeath = false, bool feedback = true)
    {
      return NWScript.EffectDeath(spectacularDeath.ToInt(), feedback.ToInt())!;
    }

    /// <summary>
    /// Creates an effect that causes the object to "fly away", before destroying itself.<br/>
    /// This effect should not be applied to PCs.
    /// </summary>
    public static Effect Disappear()
    {
      return NWScript.EffectDisappear()!;
    }

    /// <summary>
    /// Creates an effect that causes the object to "fly away", before appearing in another location.
    /// </summary>
    /// <param name="location">The new location to re-appear at.</param>
    /// <param name="animationType">The appear/disappear animation nto use.</param>
    public static Effect DisappearAppear(Location location, int animationType = 1)
    {
      return NWScript.EffectDisappearAppear(location, animationType)!;
    }

    /// <summary>
    /// Creates an effect that applies a disease to a creature.
    /// </summary>
    /// <param name="diseaseType">The type of disease to apply.</param>
    public static Effect Disease(DiseaseType diseaseType)
    {
      return NWScript.EffectDisease((int)diseaseType)!;
    }

    /// <summary>
    /// Creates an effect that attempts to strip all (Sp)ell effects on a target, up to a specified caster level.<br/>
    /// (Su)pernatural and (Ex)traordinary effects can never be dispelled.
    /// </summary>
    /// <param name="casterLevel">The max (inclusive) caster level of spell to dispel.</param>
    public static Effect DispelMagicAll(int casterLevel)
    {
      return NWScript.EffectDispelMagicAll(casterLevel)!;
    }

    /// <summary>
    /// Creates an effect that will attempt to strip the highest level spell effect on a target, up to a specified caster level.
    /// </summary>
    /// <param name="casterLevel">The max (inclusive) caster level of spell to dispel.</param>
    public static Effect DispelMagicBest(int casterLevel)
    {
      return NWScript.EffectDispelMagicBest(casterLevel)!;
    }

    /// <summary>
    /// Creates a dominate effect.<br/>
    /// A dominated creature is added to the effect creators party. This means they will become instantly friendly and not attack each other, and they are treated as a normal friend as a henchman would be.
    /// </summary>
    public static Effect Dominated()
    {
      return NWScript.EffectDominated()!;
    }

    /// <summary>
    /// Creates an effect that prevents all movement, and applies a -2 to all attacks and a -4 to AC.
    /// </summary>
    public static Effect Entangle()
    {
      return NWScript.EffectEntangle()!;
    }

    /// <summary>
    /// Creates an effect that causes the creature to become invisible and un-perceivable, and taking on an ethereal appearance.<br/>
    /// The effect is cancelled when the creature performs a hostile action.
    /// </summary>
    public static Effect Ethereal()
    {
      return NWScript.EffectEthereal()!;
    }

    /// <summary>
    /// Creates a frightened effect for use in making creatures shaken or flee.
    /// </summary>
    public static Effect Frightened()
    {
      return NWScript.EffectFrightened()!;
    }

    /// <summary>
    /// Creates an effect that forces creatures to always walk.
    /// </summary>
    public static Effect ForceWalk()
    {
      return NWScript.EffectForceWalk()!;
    }

    /// <summary>
    /// Creates a haste effect.<br/>
    /// Haste effects add +4 dodge AC, +1 action/round and 50% movement speed increase.
    /// </summary>
    public static Effect Haste()
    {
      return NWScript.EffectHaste()!;
    }

    /// <summary>
    /// Creates a heal effect that will heal the object it is applied to by the specified damage amount.
    /// </summary>
    /// <param name="damageToHeal">The amount of damage to heal.</param>
    public static Effect Heal(int damageToHeal)
    {
      return NWScript.EffectHeal(damageToHeal)!;
    }

    /// <summary>
    /// Creates a special effect for healing/damaging dying players.
    /// </summary>
    /// <param name="hpChangePerRound">The HP change each round.</param>
    public static Effect HitPointChangeWhenDying(float hpChangePerRound)
    {
      return NWScript.EffectHitPointChangeWhenDying(hpChangePerRound)!;
    }

    /// <summary>
    /// Creates an icon effect. Icons appear in the top right and in the character sheet and examine panels.
    /// </summary>
    /// <param name="icon">The icon to display.</param>
    public static Effect Icon(EffectIconTableEntry icon)
    {
      return NWScript.EffectIcon(icon.RowIndex)!;
    }

    /// <summary>
    /// Creates an immunity effect. This provides immunity to a specific effect type.
    /// </summary>
    /// <param name="immunityType">The type of immunity.</param>
    public static Effect Immunity(ImmunityType immunityType)
    {
      return NWScript.EffectImmunity((int)immunityType)!;
    }

    /// <summary>
    /// Creates an invisiblity effect. Behaviour is defined from the specified invisibility type.
    /// </summary>
    /// <param name="invisibilityType">The type of invisibility to apply.</param>
    public static Effect Invisibility(InvisibilityType invisibilityType)
    {
      return NWScript.EffectInvisibility((int)invisibilityType)!;
    }

    /// <summary>
    /// Creates a knockdown effect. This effect knocks creatures off their feet, they will sit until the effect is removed.
    /// </summary>
    public static Effect Knockdown()
    {
      return NWScript.EffectKnockdown()!;
    }

    /// <summary>
    /// Creates a new linked effect from the specified effects.<br/>
    /// If you remove/dispel one of the effects from a target which is linked to others, the other linked effects will be removed/dispelled too.
    /// </summary>
    /// <param name="baseEffect">The base effect.</param>
    /// <param name="effects">The effects to link.</param>
    /// <returns>The new composite effect linking both effects.</returns>
    public static Effect LinkEffects(Effect baseEffect, params Effect[] effects)
    {
      return LinkEffects(baseEffect, (IEnumerable<Effect>)effects);
    }

    /// <inheritdoc cref="LinkEffects(Anvil.API.Effect,Anvil.API.Effect[])"/>
    public static Effect LinkEffects(Effect baseEffect, IEnumerable<Effect> effects)
    {
      Effect current = baseEffect;
      foreach (Effect effect in effects)
      {
        current = NWScript.EffectLinkEffects(effect, current)!;
      }

      return current;
    }

    /// <summary>
    /// Creates a new effect used to hinder the attacks of the creature it is applied to.
    /// </summary>
    /// <param name="missPct">A positive number representing the percentage chance to miss.</param>
    /// <param name="missChanceType">The type of attack that will have the hindered attack.</param>
    public static Effect MissChance(int missPct, MissChanceType missChanceType = MissChanceType.Normal)
    {
      return NWScript.EffectMissChance(missPct, (int)missChanceType)!;
    }

    /// <summary>
    /// Creates an effect that modifies the amount of attacks a creature can perform.
    /// </summary>
    /// <param name="numAttacks">The number of attacks to add to the target (max 5).</param>
    public static Effect ModifyAttacks(int numAttacks)
    {
      return NWScript.EffectModifyAttacks(numAttacks)!;
    }

    /// <summary>
    /// Creates an effect that reduces the movement speed of a creature.
    /// </summary>
    /// <param name="pctChange">A positive number representing the percentage speed to decrease (0-99)</param>
    public static Effect MovementSpeedDecrease(int pctChange)
    {
      return NWScript.EffectMovementSpeedDecrease(pctChange)!;
    }

    /// <summary>
    /// Creates an effect that increases the movement speed of a creature.
    /// </summary>
    /// <param name="pctChange">A positive number representing the percentage speed to increase (0-99)</param>
    public static Effect MovementSpeedIncrease(int pctChange)
    {
      return NWScript.EffectMovementSpeedIncrease(pctChange)!;
    }

    /// <summary>
    /// Creates an effect that will "decrease" the level of the target.
    /// </summary>
    /// <param name="numLevels">The number of negative levels to apply.</param>
    public static Effect NegativeLevel(int numLevels)
    {
      return NWScript.EffectNegativeLevel(numLevels)!;
    }

    /// <summary>
    /// Creates an effect that pacifies a creature, making them unable to do attacks.
    /// </summary>
    public static Effect Pacified()
    {
      return NWScript.EffectPacified()!;
    }

    /// <summary>
    /// Creates a paralyze effect.
    /// </summary>
    /// <remarks>
    /// Paralysis is not a mind effect - it stops all movement (and also in-game pauses the creature in their current action), and so they are prone, have no dexterity or strength (act as if had the lowest, 3) and all sneak attacks work against them.<br/>
    /// By default creatures level 4 and below in 10 meters can be "coup de grace" ie instantly killed on a successful hit.
    /// </remarks>
    public static Effect Paralyze()
    {
      return NWScript.EffectParalyze()!;
    }

    /// <summary>
    /// Creates an effect that will petrify a creature. It is similar to <see cref="Paralyze"/>, but also applies the <see cref="VfxType.DurPetrify"/> visual effect.
    /// </summary>
    public static Effect Petrify()
    {
      return NWScript.EffectPetrify()!;
    }

    /// <summary>
    /// Creates a poison effect that sets the poisoned status effect, with varying penalties based on the type chosen.
    /// </summary>
    /// <param name="poisonType">The type of poison to apply.</param>
    public static Effect Poison(PoisonType poisonType)
    {
      return NWScript.EffectPoison((int)poisonType)!;
    }

    /// <summary>
    /// Creates a polymorph effect that changes the target into a different kind of creature.
    /// </summary>
    /// <param name="polymorphType">The polymorph to apply.</param>
    /// <param name="locked">If true, players cannot dismiss the polymorph effect.</param>
    public static Effect Polymorph(PolymorphTableEntry polymorphType, bool locked = false)
    {
      return NWScript.EffectPolymorph(polymorphType.RowIndex, locked.ToInt())!;
    }

    /// <summary>
    /// Creates a regeneration effect that will cause the target to heal the specified amount at each interval.
    /// </summary>
    /// <param name="amountPerInterval">The amount to heal.</param>
    /// <param name="interval">The interval at which the healing is applied.</param>
    public static Effect Regenerate(int amountPerInterval, TimeSpan interval)
    {
      return NWScript.EffectRegenerate(amountPerInterval, (float)interval.TotalSeconds)!;
    }

    /// <summary>
    /// Creates an effect that raises a creature who is dead to 1 hit point.
    /// </summary>
    public static Effect Resurrection()
    {
      return NWScript.EffectResurrection()!;
    }

    /// <summary>
    /// Creates a custom scripted effect.
    /// </summary>
    /// <param name="onAppliedHandle">The callback to invoke when this effect is applied.</param>
    /// <param name="onRemovedHandle">The callback to invoke when this effect is removed (via script, death, resting, or dispelling).</param>
    /// <param name="onIntervalHandle">The callback to invoke per interval.</param>
    /// <param name="interval">The interval in which to call onIntervalHandle.</param>
    /// <param name="data">Optional string of data saved with the effect, retrievable with Effect.StringParams[0].</param>
    public static Effect RunAction(ScriptCallbackHandle? onAppliedHandle = null, ScriptCallbackHandle? onRemovedHandle = null, ScriptCallbackHandle? onIntervalHandle = null, TimeSpan interval = default, string data = "")
    {
      onAppliedHandle?.AssertValid();
      onRemovedHandle?.AssertValid();
      onIntervalHandle?.AssertValid();

      return NWScript.EffectRunScript(onAppliedHandle?.ScriptName ?? string.Empty, onRemovedHandle?.ScriptName ?? string.Empty, onIntervalHandle?.ScriptName ?? string.Empty, (float)interval.TotalSeconds, data)!;
    }

    /// <summary>
    /// Creates a sanctuary effect. Sanctuary effects work similar to invisibility effects, but also cause creatures to be unable to hear the target creature, if they fail the DC.
    /// </summary>
    /// <param name="difficultyClass">The difficulty class for other creatures to beat the effect, allowing them to see and hear the creature.</param>
    public static Effect Sanctuary(int difficultyClass)
    {
      return NWScript.EffectSanctuary(difficultyClass)!;
    }

    /// <summary>
    /// Creates an effect to decrease one saving throw type.
    /// </summary>
    /// <param name="savingThrow">The saving throw to decrease.</param>
    /// <param name="amount">The amount to decrease the saving throw.</param>
    /// <param name="savingThrowType">The saving throw sub-type to decrease.</param>
    public static Effect SavingThrowDecrease(SavingThrow savingThrow, int amount, SavingThrowType savingThrowType = SavingThrowType.All)
    {
      return NWScript.EffectSavingThrowDecrease((int)savingThrow, amount, (int)savingThrowType)!;
    }

    /// <summary>
    /// Creates an effect to increase one saving throw type.
    /// </summary>
    /// <param name="savingThrow">The saving throw to increase.</param>
    /// <param name="amount">The amount to increase the saving throw.</param>
    /// <param name="savingThrowType">The saving throw sub-type to increase.</param>
    public static Effect SavingThrowIncrease(SavingThrow savingThrow, int amount, SavingThrowType savingThrowType = SavingThrowType.All)
    {
      return NWScript.EffectSavingThrowIncrease((int)savingThrow, amount, (int)savingThrowType)!;
    }

    /// <summary>
    /// Creates an effect that allows a creature to see through magical invisibility.
    /// </summary>
    public static Effect SeeInvisible()
    {
      return NWScript.EffectSeeInvisible()!;
    }

    /// <summary>
    /// Creates an effect that silences a creature. Silent creatures may not cast spell with verbal components.
    /// </summary>
    public static Effect Silence()
    {
      return NWScript.EffectSilence()!;
    }

    /// <summary>
    /// Creates an effect that decreases a specific skill score.
    /// </summary>
    /// <param name="skill">The skill to decrease.</param>
    /// <param name="amount">The amount to decrease the skill by.</param>
    public static Effect SkillDecrease(NwSkill skill, int amount)
    {
      return NWScript.EffectSkillDecrease(skill.Id, amount)!;
    }

    /// <summary>
    /// Creates an effect that decreases all skill scores.
    /// </summary>
    /// <param name="amount">The amount to decrease all skills by.</param>
    public static Effect SkillDecreaseAll(int amount)
    {
      return NWScript.EffectSkillDecrease((int)Skill.AllSkills, amount)!;
    }

    /// <summary>
    /// Creates an effect that increases a specific skill score.
    /// </summary>
    /// <param name="skill">The skill to increase.</param>
    /// <param name="amount">The amount to increase the skill by.</param>
    public static Effect SkillIncrease(NwSkill skill, int amount)
    {
      return NWScript.EffectSkillIncrease(skill.Id, amount)!;
    }

    /// <summary>
    /// Creates an effect that increases all skill scores.
    /// </summary>
    /// <param name="amount">The amount to increase all skills by.</param>
    public static Effect SkillIncreaseAll(int amount)
    {
      return NWScript.EffectSkillIncrease((int)Skill.AllSkills, amount)!;
    }

    /// <summary>
    /// Creates a sleep effect. Sleeping creatures may not act, and are subject to coup de grace (4 or less HD)
    /// </summary>
    public static Effect Sleep()
    {
      return NWScript.EffectSleep()!;
    }

    /// <summary>
    /// Creates a slow effect. Slowed creatures have no benefits from any haste applied to them, or if they have no haste, have 1 attack per round, -2 AC, -2 attack, -2 Reflex Saving Throw, and -50% movement speed.
    /// </summary>
    public static Effect Slow()
    {
      return NWScript.EffectSlow()!;
    }

    /// <summary>
    /// Creates an effect that inhibits spells.
    /// </summary>
    /// <param name="failPct">A positive number representing the percent chance of spell failing (1-100)</param>
    /// <param name="spellSchool">The spell school that is affected.</param>
    public static Effect SpellFailure(int failPct, SpellSchool spellSchool = SpellSchool.General)
    {
      return NWScript.EffectSpellFailure(failPct, (int)spellSchool)!;
    }

    /// <summary>
    /// Creates an effect that provides immunity to a specific spell
    /// </summary>
    /// <param name="spell">The spell to provide immunity for.</param>
    public static Effect SpellImmunity(Spell spell = API.Spell.AllSpells)
    {
      return NWScript.EffectSpellImmunity((int)spell)!;
    }

    /// <summary>
    /// Creates an effect that absorbs a certain amount of spells.
    /// </summary>
    /// <param name="maxSpellLevel">The max level spell that can be absorbed.</param>
    /// <param name="totalSpellsAbsorbed">The total number of spells that can be absorbed.</param>
    /// <param name="spellSchool">The spell school to absorb specifically.</param>
    public static Effect SpellLevelAbsorption(int maxSpellLevel, int totalSpellsAbsorbed = 0, SpellSchool spellSchool = SpellSchool.General)
    {
      return NWScript.EffectSpellLevelAbsorption(maxSpellLevel, totalSpellsAbsorbed, (int)spellSchool)!;
    }

    /// <summary>
    /// Creates an effect that decreases the spell resistance of a creature.
    /// </summary>
    /// <param name="amount">The amount to decrease.</param>
    public static Effect SpellResistanceDecrease(int amount)
    {
      return NWScript.EffectSpellResistanceDecrease(amount)!;
    }

    /// <summary>
    /// Creates an effect that increases the spell resistance of a creature.
    /// </summary>
    /// <param name="amount">The amount to increase.</param>
    public static Effect SpellResistanceIncrease(int amount)
    {
      return NWScript.EffectSpellResistanceIncrease(amount)!;
    }

    /// <summary>
    /// Creates a stun effect. This is a mind-effecting effect, and so immunity to mind effects prevents this from working.<br/>
    /// Stunned creatures are flat-footed (no AC dex bonus), and cannot move.
    /// </summary>
    public static Effect Stunned()
    {
      return NWScript.EffectStunned()!;
    }

    /// <summary>
    /// Creates an effect to summon a creature.<br/>
    /// THIS IS OBJECT CONTEXT SENSITIVE! Use <see cref="NwObject.WaitForObjectContext"/> to correctly assign the right owner of the master.
    /// </summary>
    /// <param name="creatureResRef">The template/ResRef of the creature to summon.</param>
    /// <param name="vfxType">A visual effect to display upon summoning.</param>
    /// <param name="delay">A delay between the visual effect, and the creature actually being added to the area.</param>
    /// <param name="appearType">The appear animation to use.</param>
    public static Effect SummonCreature(string creatureResRef, VfxType vfxType, TimeSpan delay = default, int appearType = 0)
    {
      return NWScript.EffectSummonCreature(creatureResRef, (int)vfxType, (float)delay.TotalSeconds, appearType)!;
    }

    /// <summary>
    /// Creates a swarm effect. This is exactly the same as <see cref="SummonCreature"/>, except, after one dies, another takes its place.
    /// </summary>
    /// <param name="loop">If true, while the effect is active and the last creature dies, it will loop back to the first template.</param>
    /// <param name="creatureTemplate1">The blueprint of the first creature to spawn.</param>
    /// <param name="creatureTemplate2">The blueprint of the second creature to spawn.</param>
    /// <param name="creatureTemplate3">The blueprint of the third creature to spawn.</param>
    /// <param name="creatureTemplate4">The blueprint of the fourth creature to spawn.</param>
    public static Effect Swarm(bool loop, string creatureTemplate1, string creatureTemplate2 = "", string creatureTemplate3 = "", string creatureTemplate4 = "")
    {
      return NWScript.EffectSwarm(loop.ToInt(), creatureTemplate1, creatureTemplate2, creatureTemplate3, creatureTemplate4)!;
    }

    /// <summary>
    /// Creates an effect that will give temporary hitpoints to a target.
    /// </summary>
    /// <param name="hitPoints">The amount of temporary hit points to add.</param>
    public static Effect TemporaryHitpoints(int hitPoints)
    {
      return NWScript.EffectTemporaryHitpoints(hitPoints)!;
    }

    /// <summary>
    /// Creates a time stop effect.<br/>
    /// Time stop applies a special module-wide pause which only the object it is applied to (and DM's) can move and cast spells without penalties or hindrance.<br/>
    /// This is not recommended for multiplayer games as it will cause everyone in the module to pause.<br/>
    /// You can query the current timestop state with <see cref="NwServer.IsTimestopPaused"/>.
    /// </summary>
    public static Effect TimeStop()
    {
      return NWScript.EffectTimeStop()!;
    }

    /// <summary>
    /// Creates a time stop immunity effect.<br/>
    /// Immunity allows objects to continue taking actions during an active time stop effect.
    /// </summary>
    public static Effect TimeStopImmunity()
    {
      return NWScript.EffectTimeStopImmunity()!;
    }

    /// <summary>
    /// Creates a true seeing effect. Creatures with true sight can see through stealth, invisibility, sanctuary and darkness.
    /// </summary>
    public static Effect TrueSeeing()
    {
      return NWScript.EffectTrueSeeing()!;
    }

    /// <summary>
    /// Creates a turned effect. Turning bypass all fear immunity effects, but should only be used in the turn undead script.
    /// </summary>
    public static Effect Turned()
    {
      return NWScript.EffectTurned()!;
    }

    /// <summary>
    /// Creates an effect that decreases a creature's turn resistance, making them more susceptible to turning.
    /// </summary>
    /// <param name="hitDiceDecrease">A positive number representing the number of hit dice to decrease.</param>
    public static Effect TurnResistanceDecrease(int hitDiceDecrease)
    {
      return NWScript.EffectTurnResistanceDecrease(hitDiceDecrease)!;
    }

    /// <summary>
    /// Creates an effect that increases a creature's turn resistance, making them more resistant to turning.
    /// </summary>
    /// <param name="hitDiceIncrease">A positive number representing the number of hit dice to increase.</param>
    public static Effect TurnResistanceIncrease(int hitDiceIncrease)
    {
      return NWScript.EffectTurnResistanceIncrease(hitDiceIncrease)!;
    }

    /// <summary>
    /// Creates an Ultravision effect. Ultravision lets the target see through <see cref="Darkness"/> and <see cref="Invisibility"/> effects with <see cref="InvisibilityType.Darkness"/>.
    /// </summary>
    public static Effect Ultravision()
    {
      return NWScript.EffectUltravision()!;
    }

    /// <summary>
    /// Creates an effect that plays a visual effect when applied.
    /// </summary>
    /// <param name="visualEffectId">The visual effect to apply.</param>
    /// <param name="missEffect">If true, a random vector near or past the target will be generated, as the location for the effect.</param>
    /// <param name="fScale">A scaling factor to apply to the visual effect.</param>
    /// <param name="vTranslate">A translation vector transform to apply to the visual effect.</param>
    /// <param name="vRotate">A rotation vector transform to apply to the visual effect.</param>
    public static Effect VisualEffect(VfxType visualEffectId, bool missEffect = false, float fScale = 1.0f, System.Numerics.Vector3 vTranslate = default, System.Numerics.Vector3 vRotate = default)
    {
      return NWScript.EffectVisualEffect((int)visualEffectId, missEffect.ToInt(), fScale, vTranslate, vRotate)!;
    }

    /// <summary>
    /// Creates an effect that plays a visual effect when applied.
    /// </summary>
    /// <param name="visualEffect">The visual effect to apply.</param>
    /// <param name="missEffect">If true, a random vector near or past the target will be generated, as the location for the effect.</param>
    /// <param name="fScale">A scaling factor to apply to the visual effect.</param>
    /// <param name="vTranslate">A translation vector transform to apply to the visual effect.</param>
    /// <param name="vRotate">A rotation vector transform to apply to the visual effect.</param>
    public static Effect VisualEffect(VisualEffectTableEntry visualEffect, bool missEffect = false, float fScale = 1.0f, System.Numerics.Vector3 vTranslate = default, System.Numerics.Vector3 vRotate = default)
    {
      return NWScript.EffectVisualEffect(visualEffect.RowIndex, missEffect.ToInt(), fScale, vTranslate, vRotate)!;
    }

    /// <summary>
    /// Creates an effect that gives a creature with melee/ranged/touched attacks a bonus to hit.
    /// </summary>
    /// <param name="bonus">The additional attack bonus.</param>
    public static Effect EnemyAttackBonus(int bonus)
    {
      return NWScript.EffectEnemyAttackBonus(bonus)!;
    }
  }
}
