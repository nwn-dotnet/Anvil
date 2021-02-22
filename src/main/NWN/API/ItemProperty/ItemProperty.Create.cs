using NWN.API.Constants;
using NWN.Core;

namespace NWN.API
{
  public sealed partial class ItemProperty
  {
    public static ItemProperty AbilityBonus(IPAbility ability, int bonus)
      => NWScript.ItemPropertyAbilityBonus((int) ability, bonus);

    public static ItemProperty ACBonus(int bonus)
      => NWScript.ItemPropertyACBonus(bonus);

    public static ItemProperty ACBonusVsAlign(IPAlignmentGroup alignmentGroup, int bonus)
      => NWScript.ItemPropertyACBonusVsAlign((int) alignmentGroup, bonus);

    public static ItemProperty ACBonusVsDmgType(IPDamageType damageType, int bonus)
      => NWScript.ItemPropertyACBonusVsDmgType((int) damageType, bonus);

    public static ItemProperty ACBonusVsRace(IPRacialType racialType, int bonus)
      => NWScript.ItemPropertyACBonusVsRace((int) racialType, bonus);

    public static ItemProperty ACBonusVsSAlign(IPAlignment alignment, int bonus)
      => NWScript.ItemPropertyACBonusVsSAlign((int) alignment, bonus);

    public static ItemProperty Additional(IPAdditional additional)
      => NWScript.ItemPropertyAdditional((int) additional);

    public static ItemProperty ArcaneSpellFailure(IPArcaneSpellFailure spellFailure)
      => NWScript.ItemPropertyArcaneSpellFailure((int) spellFailure);

    public static ItemProperty AttackBonus(int bonus)
      => NWScript.ItemPropertyAttackBonus(bonus);

    public static ItemProperty AttackBonusVsAlign(IPAlignmentGroup alignmentGroup, int bonus)
      => NWScript.ItemPropertyAttackBonusVsAlign((int) alignmentGroup, bonus);

    public static ItemProperty AttackBonusVsRace(IPRacialType racialType, int bonus)
      => NWScript.ItemPropertyAttackBonusVsRace((int) racialType, bonus);

    public static ItemProperty AttackBonusVsSAlign(IPAlignment alignment, int bonus)
      => NWScript.ItemPropertyAttackBonusVsSAlign((int) alignment, bonus);

    public static ItemProperty AttackPenalty(int penalty)
      => NWScript.ItemPropertyAttackPenalty(penalty);

    public static ItemProperty BonusFeat(IPFeat feat)
      => NWScript.ItemPropertyBonusFeat((int) feat);

    public static ItemProperty BonusLevelSpell(IPClass classType, IPSpellLevel spellLevel)
      => NWScript.ItemPropertyBonusLevelSpell((int) classType, (int) spellLevel);

    public static ItemProperty BonusSavingThrow(IPSaveBaseType saveType, int bonus)
      => NWScript.ItemPropertyBonusSavingThrow((int) saveType, bonus);

    public static ItemProperty BonusSavingThrowVsX(IPSaveVs saveType, int bonus)
      => NWScript.ItemPropertyBonusSavingThrowVsX((int) saveType, bonus);

    public static ItemProperty BonusSpellResistance(IPSpellResistanceBonus resistBonus)
      => NWScript.ItemPropertyBonusSpellResistance((int) resistBonus);

    public static ItemProperty CastSpell(IPCastSpell spell, IPCastSpellNumUses uses)
      => NWScript.ItemPropertyCastSpell((int) spell, (int) uses);

    public static ItemProperty ContainerReducedWeight(IPContainerWeightReduction weightReduction)
      => NWScript.ItemPropertyContainerReducedWeight((int) weightReduction);

    public static ItemProperty DamageBonus(IPDamageType damageType, IPDamageBonus damageBonus)
      => NWScript.ItemPropertyDamageBonus((int) damageType, (int) damageBonus);

    public static ItemProperty DamageBonusVsAlign(IPAlignmentGroup alignmentGroup, IPDamageType damageType, IPDamageBonus damageBonus)
      => NWScript.ItemPropertyDamageBonusVsAlign((int) alignmentGroup, (int) damageType, (int) damageBonus);

    public static ItemProperty DamageBonusVsRace(IPRacialType racialType, IPDamageType damageType, IPDamageBonus damageBonus)
      => NWScript.ItemPropertyDamageBonusVsRace((int) racialType, (int) damageType, (int) damageBonus);

    public static ItemProperty DamageBonusVsSAlign(IPAlignment alignment, IPDamageType damageType, IPDamageBonus damageBonus)
      => NWScript.ItemPropertyDamageBonusVsSAlign((int) alignment, (int) damageType, (int) damageBonus);

    public static ItemProperty DamageImmunity(IPDamageType damageType, IPDamageImmunityType immunityType)
      => NWScript.ItemPropertyDamageImmunity((int) damageType, (int) immunityType);

    public static ItemProperty DamagePenalty(int penalty)
      => NWScript.ItemPropertyDamagePenalty(penalty);

    public static ItemProperty DamageReduction(IPDamageReduction damageReduction, IPDamageSoak damageSoak)
      => NWScript.ItemPropertyDamageReduction((int) damageReduction, (int) damageSoak);

    public static ItemProperty DamageResistance(IPDamageType damageType, IPDamageResist damageResist)
      => NWScript.ItemPropertyDamageResistance((int) damageType, (int) damageResist);

    public static ItemProperty DamageVulnerability(IPDamageType damageType, IPDamageVulnerabilityType damageVulnerability)
      => NWScript.ItemPropertyDamageVulnerability((int) damageType, (int) damageVulnerability);

    public static ItemProperty Darkvision()
      => NWScript.ItemPropertyDarkvision();

    public static ItemProperty DecreaseAbility(IPAbility ability, int penalty)
      => NWScript.ItemPropertyDecreaseAbility((int) ability, penalty);

    public static ItemProperty DecreaseAC(IPACModifierType modifierType, int penalty)
      => NWScript.ItemPropertyDecreaseAC((int) modifierType, penalty);

    public static ItemProperty DecreaseSkill(Skill skill, int penalty)
      => NWScript.ItemPropertyDecreaseSkill((int) skill, penalty);

    public static ItemProperty EnhancementBonus(int bonus)
      => NWScript.ItemPropertyEnhancementBonus(bonus);

    public static ItemProperty EnhancementBonusVsAlign(IPAlignmentGroup alignmentGroup, int bonus)
      => NWScript.ItemPropertyEnhancementBonusVsAlign((int) alignmentGroup, bonus);

    public static ItemProperty EnhancementBonusVsRace(IPRacialType racialType, int bonus)
      => NWScript.ItemPropertyEnhancementBonusVsRace((int) racialType, bonus);

    public static ItemProperty EnhancementBonusVsSAlign(IPAlignment alignment, int bonus)
      => NWScript.ItemPropertyEnhancementBonusVsSAlign((int) alignment, bonus);

    public static ItemProperty EnhancementPenalty(int penalty)
      => NWScript.ItemPropertyEnhancementPenalty(penalty);

    public static ItemProperty ExtraMeleeDamageType(IPDamageType damageType)
      => NWScript.ItemPropertyExtraMeleeDamageType((int) damageType);

    public static ItemProperty ExtraRangeDamageType(IPDamageType damageType)
      => NWScript.ItemPropertyExtraRangeDamageType((int) damageType);

    public static ItemProperty FreeAction()
      => NWScript.ItemPropertyFreeAction();

    public static ItemProperty Haste()
      => NWScript.ItemPropertyHaste();

    public static ItemProperty HealersKit(int level)
      => NWScript.ItemPropertyHealersKit(level);

    public static ItemProperty HolyAvenger()
      => NWScript.ItemPropertyHolyAvenger();

    public static ItemProperty ImmunityMisc(IPMiscImmunity immunityType)
      => NWScript.ItemPropertyImmunityMisc((int) immunityType);

    public static ItemProperty ImmunityToSpellLevel(IPSpellLevel spellLevel)
      => NWScript.ItemPropertyImmunityToSpellLevel((int) spellLevel);

    public static ItemProperty ImprovedEvasion()
      => NWScript.ItemPropertyImprovedEvasion();

    public static ItemProperty Keen()
      => NWScript.ItemPropertyKeen();

    public static ItemProperty Light(IPLightBrightness brightness, IPLightColor color)
      => NWScript.ItemPropertyLight((int) brightness, (int) color);

    public static ItemProperty LimitUseByAlign(IPAlignmentGroup alignmentGroup)
      => NWScript.ItemPropertyLimitUseByAlign((int) alignmentGroup);

    public static ItemProperty LimitUseByClass(IPClass classType)
      => NWScript.ItemPropertyLimitUseByClass((int) classType);

    public static ItemProperty LimitUseByRace(IPRacialType racialType)
      => NWScript.ItemPropertyLimitUseByRace((int) racialType);

    public static ItemProperty LimitUseBySAlign(IPAlignment alignment)
      => NWScript.ItemPropertyLimitUseBySAlign((int) alignment);

    public static ItemProperty MassiveCritical(IPDamageBonus damageBonus)
      => NWScript.ItemPropertyMassiveCritical((int) damageBonus);

    public static ItemProperty Material(int materialType)
      => NWScript.ItemPropertyMaterial(materialType);

    public static ItemProperty MaxRangeStrengthMod(int modifier)
      => NWScript.ItemPropertyMaxRangeStrengthMod(modifier);

    public static ItemProperty MonsterDamage(IPMonsterDamage monsterDamage)
      => NWScript.ItemPropertyMonsterDamage((int) monsterDamage);

    public static ItemProperty NoDamage()
      => NWScript.ItemPropertyNoDamage();

    public static ItemProperty OnHitCastSpell(IPCastSpell spell, IPSpellLevel spellLevel)
      => NWScript.ItemPropertyOnHitCastSpell((int) spell, (int) spellLevel);

    public static ItemProperty OnHitEffect(IPOnHitSaveDC saveDC, HitEffect effect)
      => NWScript.ItemPropertyOnHitProps(effect.Property, (int) saveDC, effect.Special);

    public static ItemProperty OnMonsterHitProperties(MonsterHitEffect effect)
      => NWScript.ItemPropertyOnMonsterHitProperties(effect.Property, effect.Special);

    public static ItemProperty Quality(IPQuality quality)
      => NWScript.ItemPropertyQuality((int) quality);

    public static ItemProperty ReducedSavingThrow(IPSaveBaseType saveType, int penalty)
      => NWScript.ItemPropertyReducedSavingThrow((int) saveType, penalty);

    public static ItemProperty ReducedSavingThrowVsX(IPSaveVs saveType, int penalty)
      => NWScript.ItemPropertyReducedSavingThrowVsX((int) saveType, penalty);

    public static ItemProperty Regeneration(int regenAmount)
      => NWScript.ItemPropertyRegeneration(regenAmount);

    public static ItemProperty SkillBonus(Skill skill, int bonus)
      => NWScript.ItemPropertySkillBonus((int) skill, bonus);

    public static ItemProperty SpecialWalk()
      => NWScript.ItemPropertySpecialWalk();

    public static ItemProperty SpellImmunitySchool(IPSpellSchool spellSchool)
      => NWScript.ItemPropertySpellImmunitySchool((int) spellSchool);

    public static ItemProperty SpellImmunitySpecific(IPSpellImmunity spell)
      => NWScript.ItemPropertySpellImmunitySpecific((int) spell);

    public static ItemProperty ThievesTools(int modifier)
      => NWScript.ItemPropertyThievesTools(modifier);

    public static ItemProperty Trap(IPTrapStrength trapStrength, IPTrapType trapType)
      => NWScript.ItemPropertyTrap((int) trapStrength, (int) trapType);

    public static ItemProperty TrueSeeing()
      => NWScript.ItemPropertyTrueSeeing();

    public static ItemProperty TurnResistance(int modifier)
      => NWScript.ItemPropertyTurnResistance(modifier);

    public static ItemProperty UnlimitedAmmo(IPUnlimitedAmmoType ammoType = IPUnlimitedAmmoType.Basic)
      => NWScript.ItemPropertyUnlimitedAmmo((int) ammoType);

    public static ItemProperty VampiricRegeneration(int regenAmount)
      => NWScript.ItemPropertyVampiricRegeneration(regenAmount);

    public static ItemProperty VisualEffect(ItemVisual itemVisual)
      => NWScript.ItemPropertyVisualEffect((int) itemVisual);

    public static ItemProperty WeightIncrease(IPWeightIncrease weightIncrease)
      => NWScript.ItemPropertyWeightIncrease((int) weightIncrease);

    public static ItemProperty WeightReduction(IPReducedWeight weightReduction)
      => NWScript.ItemPropertyWeightReduction((int) weightReduction);
  }
}
