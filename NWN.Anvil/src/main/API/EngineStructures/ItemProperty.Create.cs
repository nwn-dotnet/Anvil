using System;
using NWN.Core;

namespace Anvil.API
{
  public sealed partial class ItemProperty
  {
    public static ItemProperty AbilityBonus(IPAbility ability, int bonus)
    {
      return NWScript.ItemPropertyAbilityBonus((int)ability, bonus)!;
    }

    public static ItemProperty ACBonus(int bonus)
    {
      return NWScript.ItemPropertyACBonus(bonus)!;
    }

    public static ItemProperty ACBonusVsAlign(IPAlignmentGroup alignmentGroup, int bonus)
    {
      return NWScript.ItemPropertyACBonusVsAlign((int)alignmentGroup, bonus)!;
    }

    public static ItemProperty ACBonusVsDmgType(IPDamageType damageType, int bonus)
    {
      return NWScript.ItemPropertyACBonusVsDmgType((int)damageType, bonus)!;
    }

    [Obsolete("Use the NwRace/RacialType overload instead.")]
    public static ItemProperty ACBonusVsRace(IPRacialType racialType, int bonus)
    {
      return NWScript.ItemPropertyACBonusVsRace((int)racialType, bonus)!;
    }

    public static ItemProperty ACBonusVsRace(NwRace race, int bonus)
    {
      return NWScript.ItemPropertyACBonusVsRace(race.Id, bonus)!;
    }

    public static ItemProperty ACBonusVsSAlign(IPAlignment alignment, int bonus)
    {
      return NWScript.ItemPropertyACBonusVsSAlign((int)alignment, bonus)!;
    }

    public static ItemProperty Additional(IPAdditional additional)
    {
      return NWScript.ItemPropertyAdditional((int)additional)!;
    }

    public static ItemProperty ArcaneSpellFailure(IPArcaneSpellFailure spellFailure)
    {
      return NWScript.ItemPropertyArcaneSpellFailure((int)spellFailure)!;
    }

    public static ItemProperty AttackBonus(int bonus)
    {
      return NWScript.ItemPropertyAttackBonus(bonus)!;
    }

    public static ItemProperty AttackBonusVsAlign(IPAlignmentGroup alignmentGroup, int bonus)
    {
      return NWScript.ItemPropertyAttackBonusVsAlign((int)alignmentGroup, bonus)!;
    }

    [Obsolete("Use the NwRace/RacialType overload instead.")]
    public static ItemProperty AttackBonusVsRace(IPRacialType racialType, int bonus)
    {
      return NWScript.ItemPropertyAttackBonusVsRace((int)racialType, bonus)!;
    }

    public static ItemProperty AttackBonusVsRace(NwRace race, int bonus)
    {
      return NWScript.ItemPropertyAttackBonusVsRace(race.Id, bonus)!;
    }

    public static ItemProperty AttackBonusVsSAlign(IPAlignment alignment, int bonus)
    {
      return NWScript.ItemPropertyAttackBonusVsSAlign((int)alignment, bonus)!;
    }

    public static ItemProperty AttackPenalty(int penalty)
    {
      return NWScript.ItemPropertyAttackPenalty(penalty)!;
    }

    public static ItemProperty BonusFeat(IPFeat feat)
    {
      return NWScript.ItemPropertyBonusFeat((int)feat)!;
    }

    public static ItemProperty BonusLevelSpell(IPClass classType, IPSpellLevel spellLevel)
    {
      return NWScript.ItemPropertyBonusLevelSpell((int)classType, (int)spellLevel)!;
    }

    public static ItemProperty BonusSavingThrow(IPSaveBaseType saveType, int bonus)
    {
      return NWScript.ItemPropertyBonusSavingThrow((int)saveType, bonus)!;
    }

    public static ItemProperty BonusSavingThrowVsX(IPSaveVs saveType, int bonus)
    {
      return NWScript.ItemPropertyBonusSavingThrowVsX((int)saveType, bonus)!;
    }

    public static ItemProperty BonusSpellResistance(IPSpellResistanceBonus resistBonus)
    {
      return NWScript.ItemPropertyBonusSpellResistance((int)resistBonus)!;
    }

    public static ItemProperty CastSpell(IPCastSpell spell, IPCastSpellNumUses uses)
    {
      return NWScript.ItemPropertyCastSpell((int)spell, (int)uses)!;
    }

    public static ItemProperty ContainerReducedWeight(IPContainerWeightReduction weightReduction)
    {
      return NWScript.ItemPropertyContainerReducedWeight((int)weightReduction)!;
    }

    public static ItemProperty Custom(int type, int subType = -1, int costTableValue = -1, int param1Value = -1)
    {
      return NWScript.ItemPropertyCustom(type, subType, costTableValue, param1Value)!;
    }

    public static ItemProperty Custom(ItemPropertyTableEntry property, ItemPropertySubTypeTableEntry? subType = null, ItemPropertyCostTableEntry? costTableValue = null, ItemPropertyParamTableEntry? paramTableValue = null)
    {
      return NWScript.ItemPropertyCustom(property.RowIndex, subType?.RowIndex ?? -1, costTableValue?.RowIndex ?? -1, paramTableValue?.RowIndex ?? -1)!;
    }

    public static ItemProperty DamageBonus(IPDamageType damageType, IPDamageBonus damageBonus)
    {
      return NWScript.ItemPropertyDamageBonus((int)damageType, (int)damageBonus)!;
    }

    public static ItemProperty DamageBonusVsAlign(IPAlignmentGroup alignmentGroup, IPDamageType damageType, IPDamageBonus damageBonus)
    {
      return NWScript.ItemPropertyDamageBonusVsAlign((int)alignmentGroup, (int)damageType, (int)damageBonus)!;
    }

    [Obsolete("Use the NwRace/RacialType overload instead.")]
    public static ItemProperty DamageBonusVsRace(IPRacialType racialType, IPDamageType damageType, IPDamageBonus damageBonus)
    {
      return NWScript.ItemPropertyDamageBonusVsRace((int)racialType, (int)damageType, (int)damageBonus)!;
    }

    public static ItemProperty DamageBonusVsRace(NwRace race, IPDamageType damageType, IPDamageBonus damageBonus)
    {
      return NWScript.ItemPropertyDamageBonusVsRace(race.Id, (int)damageType, (int)damageBonus)!;
    }

    public static ItemProperty DamageBonusVsSAlign(IPAlignment alignment, IPDamageType damageType, IPDamageBonus damageBonus)
    {
      return NWScript.ItemPropertyDamageBonusVsSAlign((int)alignment, (int)damageType, (int)damageBonus)!;
    }

    public static ItemProperty DamageImmunity(IPDamageType damageType, IPDamageImmunityType immunityType)
    {
      return NWScript.ItemPropertyDamageImmunity((int)damageType, (int)immunityType)!;
    }

    public static ItemProperty DamagePenalty(int penalty)
    {
      return NWScript.ItemPropertyDamagePenalty(penalty)!;
    }

    public static ItemProperty DamageReduction(IPDamageReduction damageReduction, IPDamageSoak damageSoak)
    {
      return NWScript.ItemPropertyDamageReduction((int)damageReduction, (int)damageSoak)!;
    }

    public static ItemProperty DamageResistance(IPDamageType damageType, IPDamageResist damageResist)
    {
      return NWScript.ItemPropertyDamageResistance((int)damageType, (int)damageResist)!;
    }

    public static ItemProperty DamageVulnerability(IPDamageType damageType, IPDamageVulnerabilityType damageVulnerability)
    {
      return NWScript.ItemPropertyDamageVulnerability((int)damageType, (int)damageVulnerability)!;
    }

    public static ItemProperty Darkvision()
    {
      return NWScript.ItemPropertyDarkvision()!;
    }

    public static ItemProperty DecreaseAbility(IPAbility ability, int penalty)
    {
      return NWScript.ItemPropertyDecreaseAbility((int)ability, penalty)!;
    }

    public static ItemProperty DecreaseAC(IPACModifierType modifierType, int penalty)
    {
      return NWScript.ItemPropertyDecreaseAC((int)modifierType, penalty)!;
    }

    public static ItemProperty DecreaseSkill(NwSkill skill, int penalty)
    {
      return NWScript.ItemPropertyDecreaseSkill(skill.Id, penalty)!;
    }

    public static ItemProperty EnhancementBonus(int bonus)
    {
      return NWScript.ItemPropertyEnhancementBonus(bonus)!;
    }

    public static ItemProperty EnhancementBonusVsAlign(IPAlignmentGroup alignmentGroup, int bonus)
    {
      return NWScript.ItemPropertyEnhancementBonusVsAlign((int)alignmentGroup, bonus)!;
    }

    [Obsolete("Use the NwRace/RacialType overload instead.")]
    public static ItemProperty EnhancementBonusVsRace(IPRacialType racialType, int bonus)
    {
      return NWScript.ItemPropertyEnhancementBonusVsRace((int)racialType, bonus)!;
    }

    public static ItemProperty EnhancementBonusVsRace(NwRace race, int bonus)
    {
      return NWScript.ItemPropertyEnhancementBonusVsRace(race.Id, bonus)!;
    }

    public static ItemProperty EnhancementBonusVsSAlign(IPAlignment alignment, int bonus)
    {
      return NWScript.ItemPropertyEnhancementBonusVsSAlign((int)alignment, bonus)!;
    }

    public static ItemProperty EnhancementPenalty(int penalty)
    {
      return NWScript.ItemPropertyEnhancementPenalty(penalty)!;
    }

    public static ItemProperty ExtraMeleeDamageType(IPDamageType damageType)
    {
      return NWScript.ItemPropertyExtraMeleeDamageType((int)damageType)!;
    }

    public static ItemProperty ExtraRangeDamageType(IPDamageType damageType)
    {
      return NWScript.ItemPropertyExtraRangeDamageType((int)damageType)!;
    }

    public static ItemProperty FreeAction()
    {
      return NWScript.ItemPropertyFreeAction()!;
    }

    public static ItemProperty Haste()
    {
      return NWScript.ItemPropertyHaste()!;
    }

    public static ItemProperty HealersKit(int level)
    {
      return NWScript.ItemPropertyHealersKit(level)!;
    }

    public static ItemProperty HolyAvenger()
    {
      return NWScript.ItemPropertyHolyAvenger()!;
    }

    public static ItemProperty ImmunityMisc(IPMiscImmunity immunityType)
    {
      return NWScript.ItemPropertyImmunityMisc((int)immunityType)!;
    }

    public static ItemProperty ImmunityToSpellLevel(IPSpellLevel spellLevel)
    {
      return NWScript.ItemPropertyImmunityToSpellLevel((int)spellLevel)!;
    }

    public static ItemProperty ImprovedEvasion()
    {
      return NWScript.ItemPropertyImprovedEvasion()!;
    }

    public static ItemProperty Keen()
    {
      return NWScript.ItemPropertyKeen()!;
    }

    public static ItemProperty Light(IPLightBrightness brightness, IPLightColor color)
    {
      return NWScript.ItemPropertyLight((int)brightness, (int)color)!;
    }

    public static ItemProperty LimitUseByAlign(IPAlignmentGroup alignmentGroup)
    {
      return NWScript.ItemPropertyLimitUseByAlign((int)alignmentGroup)!;
    }

    [Obsolete("Use the NwClass/IPClass overload instead.")]
    public static ItemProperty LimitUseByClass(IPClass classType)
    {
      return NWScript.ItemPropertyLimitUseByClass((int)classType)!;
    }

    public static ItemProperty LimitUseByClass(NwClass classType)
    {
      return NWScript.ItemPropertyLimitUseByClass(classType.Id)!;
    }

    [Obsolete("Use the NwRace/RacialType overload instead.")]
    public static ItemProperty LimitUseByRace(IPRacialType racialType)
    {
      return NWScript.ItemPropertyLimitUseByRace((int)racialType)!;
    }

    public static ItemProperty LimitUseByRace(NwRace race)
    {
      return NWScript.ItemPropertyLimitUseByRace(race.Id)!;
    }

    public static ItemProperty LimitUseBySAlign(IPAlignment alignment)
    {
      return NWScript.ItemPropertyLimitUseBySAlign((int)alignment)!;
    }

    public static ItemProperty MassiveCritical(IPDamageBonus damageBonus)
    {
      return NWScript.ItemPropertyMassiveCritical((int)damageBonus)!;
    }

    public static ItemProperty Material(int materialType)
    {
      return NWScript.ItemPropertyMaterial(materialType)!;
    }

    public static ItemProperty MaxRangeStrengthMod(int modifier)
    {
      return NWScript.ItemPropertyMaxRangeStrengthMod(modifier)!;
    }

    public static ItemProperty MonsterDamage(IPMonsterDamage monsterDamage)
    {
      return NWScript.ItemPropertyMonsterDamage((int)monsterDamage)!;
    }

    public static ItemProperty NoDamage()
    {
      return NWScript.ItemPropertyNoDamage()!;
    }

    public static ItemProperty OnHitCastSpell(IPCastSpell spell, int casterLevel)
    {
      return NWScript.ItemPropertyOnHitCastSpell((int)spell, casterLevel)!;
    }

    [Obsolete("Use the OnHitCastSpell(IPCastSpell, int) overload instead.")]
    public static ItemProperty OnHitCastSpell(IPCastSpell spell, IPSpellLevel spellLevel)
    {
      return NWScript.ItemPropertyOnHitCastSpell((int)spell, (int)spellLevel)!;
    }

    public static ItemProperty OnHitEffect(IPOnHitSaveDC saveDC, HitEffect effect)
    {
      return NWScript.ItemPropertyOnHitProps(effect.Property, (int)saveDC, effect.Special)!;
    }

    public static ItemProperty OnMonsterHitProperties(MonsterHitEffect effect)
    {
      return NWScript.ItemPropertyOnMonsterHitProperties(effect.Property, effect.Special)!;
    }

    public static ItemProperty Quality(IPQuality quality)
    {
      return NWScript.ItemPropertyQuality((int)quality)!;
    }

    public static ItemProperty ReducedSavingThrow(IPSaveBaseType saveType, int penalty)
    {
      return NWScript.ItemPropertyReducedSavingThrow((int)saveType, penalty)!;
    }

    public static ItemProperty ReducedSavingThrowVsX(IPSaveVs saveType, int penalty)
    {
      return NWScript.ItemPropertyReducedSavingThrowVsX((int)saveType, penalty)!;
    }

    public static ItemProperty Regeneration(int regenAmount)
    {
      return NWScript.ItemPropertyRegeneration(regenAmount)!;
    }

    public static ItemProperty SkillBonus(NwSkill skill, int bonus)
    {
      return NWScript.ItemPropertySkillBonus(skill.Id, bonus)!;
    }

    public static ItemProperty SpecialWalk()
    {
      return NWScript.ItemPropertySpecialWalk()!;
    }

    public static ItemProperty SpellImmunitySchool(IPSpellSchool spellSchool)
    {
      return NWScript.ItemPropertySpellImmunitySchool((int)spellSchool)!;
    }

    public static ItemProperty SpellImmunitySpecific(IPSpellImmunity spell)
    {
      return NWScript.ItemPropertySpellImmunitySpecific((int)spell)!;
    }

    public static ItemProperty ThievesTools(int modifier)
    {
      return NWScript.ItemPropertyThievesTools(modifier)!;
    }

    public static ItemProperty Trap(IPTrapStrength trapStrength, IPTrapType trapType)
    {
      return NWScript.ItemPropertyTrap((int)trapStrength, (int)trapType)!;
    }

    public static ItemProperty TrueSeeing()
    {
      return NWScript.ItemPropertyTrueSeeing()!;
    }

    public static ItemProperty TurnResistance(int modifier)
    {
      return NWScript.ItemPropertyTurnResistance(modifier)!;
    }

    public static ItemProperty UnlimitedAmmo(IPUnlimitedAmmoType ammoType = IPUnlimitedAmmoType.Basic)
    {
      return NWScript.ItemPropertyUnlimitedAmmo((int)ammoType)!;
    }

    public static ItemProperty VampiricRegeneration(int regenAmount)
    {
      return NWScript.ItemPropertyVampiricRegeneration(regenAmount)!;
    }

    public static ItemProperty VisualEffect(ItemVisual itemVisual)
    {
      return NWScript.ItemPropertyVisualEffect((int)itemVisual)!;
    }

    public static ItemProperty WeightIncrease(IPWeightIncrease weightIncrease)
    {
      return NWScript.ItemPropertyWeightIncrease((int)weightIncrease)!;
    }

    public static ItemProperty WeightReduction(IPReducedWeight weightReduction)
    {
      return NWScript.ItemPropertyWeightReduction((int)weightReduction)!;
    }
  }
}
