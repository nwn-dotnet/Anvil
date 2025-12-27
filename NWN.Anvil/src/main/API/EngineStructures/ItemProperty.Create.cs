using System;
using NWN.Core;

namespace Anvil.API
{
  public sealed partial class ItemProperty
  {
    /// <summary>
    /// Creates an item property that grants an ability score bonus.
    /// Bonus must be between 1 and 12.
    /// </summary>
    /// <param name="ability">The ability to modify.</param>
    /// <param name="bonus">The bonus amount (1–12).</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty AbilityBonus(IPAbility ability, int bonus)
    {
      return NWScript.ItemPropertyAbilityBonus((int)ability, bonus)!;
    }

    /// <summary>
    /// Creates an Armor Class bonus item property.
    /// Bonus must be between 1 and 20; modifier type depends on item.
    /// </summary>
    /// <param name="bonus">The AC bonus (1–20).</param>
    /// <returns>The created item property.</returns>
    /// <remarks>
    /// Items that gain a Deflection bonus: Creature Skins, staves, rings, melee weapons, helmets, cloaks, belts, gloves, and ranged weapons that use ammunition.<br/>
    /// Items that gain an Armor bonus: Armors and bracers.<br/>
    /// Items that gain a Natural Armor bonus: Amulets.<br/>
    /// Items that gain a Shield bonus: Shields.<br/>
    /// Items that gain a Dodge bonus: Boots.
    /// </remarks>
    public static ItemProperty ACBonus(int bonus)
    {
      return NWScript.ItemPropertyACBonus(bonus)!;
    }

    /// <summary>
    /// Creates an Armor Class bonus versus an alignment group.
    /// Bonus must be between 1 and 20; modifier type depends on item.
    /// </summary>
    /// <param name="alignmentGroup">The alignment group.</param>
    /// <param name="bonus">The AC bonus (1–20).</param>
    /// <returns>The created item property.</returns>
    /// <remarks>
    /// Items that gain a Deflection bonus: Creature Skins, staves, rings, melee weapons, helmets, cloaks, belts, gloves, and ranged weapons that use ammunition.<br/>
    /// Items that gain an Armor bonus: Armors and bracers.<br/>
    /// Items that gain a Natural Armor bonus: Amulets.<br/>
    /// Items that gain a Shield bonus: Shields.<br/>
    /// Items that gain a Dodge bonus: Boots.
    /// </remarks>
    public static ItemProperty ACBonusVsAlign(IPAlignmentGroup alignmentGroup, int bonus)
    {
      return NWScript.ItemPropertyACBonusVsAlign((int)alignmentGroup, bonus)!;
    }

    /// <summary>
    /// Creates an Armor Class bonus versus a damage type.
    /// Only physical types (Bludgeoning, Piercing, Slashing) are valid. Bonus must be 1–20.
    /// </summary>
    /// <param name="damageType">The damage type (physical only).</param>
    /// <param name="bonus">The AC bonus (1–20).</param>
    /// <returns>The created item property.</returns>
    /// <remarks>
    /// Items that gain a Deflection bonus: Creature Skins, staves, rings, melee weapons, helmets, cloaks, belts, gloves, and ranged weapons that use ammunition.<br/>
    /// Items that gain an Armor bonus: Armors and bracers.<br/>
    /// Items that gain a Natural Armor bonus: Amulets.<br/>
    /// Items that gain a Shield bonus: Shields.<br/>
    /// Items that gain a Dodge bonus: Boots.
    /// </remarks>
    public static ItemProperty ACBonusVsDmgType(IPDamageType damageType, int bonus)
    {
      return NWScript.ItemPropertyACBonusVsDmgType((int)damageType, bonus)!;
    }

    [Obsolete("Use the NwRace/RacialType overload instead.")]
    public static ItemProperty ACBonusVsRace(IPRacialType racialType, int bonus)
    {
      return NWScript.ItemPropertyACBonusVsRace((int)racialType, bonus)!;
    }

    /// <summary>
    /// Creates an Armor Class bonus versus a racial type.
    /// Bonus must be between 1 and 20; modifier type depends on item.
    /// </summary>
    /// <param name="race">The race.</param>
    /// <param name="bonus">The AC bonus (1–20).</param>
    /// <returns>The created item property.</returns>
    /// <remarks>
    /// Items that gain a Deflection bonus: Creature Skins, staves, rings, melee weapons, helmets, cloaks, belts, gloves, and ranged weapons that use ammunition.<br/>
    /// Items that gain an Armor bonus: Armors and bracers.<br/>
    /// Items that gain a Natural Armor bonus: Amulets.<br/>
    /// Items that gain a Shield bonus: Shields.<br/>
    /// Items that gain a Dodge bonus: Boots.
    /// </remarks>
    public static ItemProperty ACBonusVsRace(NwRace race, int bonus)
    {
      return NWScript.ItemPropertyACBonusVsRace(race.Id, bonus)!;
    }

    /// <summary>
    /// Creates an Armor Class bonus versus a specific alignment.
    /// Bonus must be between 1 and 20; modifier type depends on item.
    /// </summary>
    /// <param name="alignment">The alignment.</param>
    /// <param name="bonus">The AC bonus (1–20).</param>
    /// <returns>The created item property.</returns>
    /// <remarks>
    /// Items that gain a Deflection bonus: Creature Skins, staves, rings, melee weapons, helmets, cloaks, belts, gloves, and ranged weapons that use ammunition.<br/>
    /// Items that gain an Armor bonus: Armors and bracers.<br/>
    /// Items that gain a Natural Armor bonus: Amulets.<br/>
    /// Items that gain a Shield bonus: Shields.<br/>
    /// Items that gain a Dodge bonus: Boots.
    /// </remarks>
    public static ItemProperty ACBonusVsSAlign(IPAlignment alignment, int bonus)
    {
      return NWScript.ItemPropertyACBonusVsSAlign((int)alignment, bonus)!;
    }

    /// <summary>
    /// Creates an item property from the Additional category.
    /// </summary>
    /// <param name="additional">The additional property.</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty Additional(IPAdditional additional)
    {
      return NWScript.ItemPropertyAdditional((int)additional)!;
    }

    /// <summary>
    /// Creates an Arcane Spell Failure item property.
    /// </summary>
    /// <param name="spellFailure">The arcane spell failure type.</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty ArcaneSpellFailure(IPArcaneSpellFailure spellFailure)
    {
      return NWScript.ItemPropertyArcaneSpellFailure((int)spellFailure)!;
    }

    /// <summary>
    /// Creates an Attack bonus item property.
    /// Bonus must be between 1 and 20.
    /// </summary>
    /// <param name="bonus">The attack bonus (1–20).</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty AttackBonus(int bonus)
    {
      return NWScript.ItemPropertyAttackBonus(bonus)!;
    }

    /// <summary>
    /// Creates an Attack bonus versus an alignment group.
    /// Bonus must be between 1 and 20.
    /// </summary>
    /// <param name="alignmentGroup">The alignment group.</param>
    /// <param name="bonus">The attack bonus (1–20).</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty AttackBonusVsAlign(IPAlignmentGroup alignmentGroup, int bonus)
    {
      return NWScript.ItemPropertyAttackBonusVsAlign((int)alignmentGroup, bonus)!;
    }

    [Obsolete("Use the NwRace/RacialType overload instead.")]
    public static ItemProperty AttackBonusVsRace(IPRacialType racialType, int bonus)
    {
      return NWScript.ItemPropertyAttackBonusVsRace((int)racialType, bonus)!;
    }

    /// <summary>
    /// Creates an Attack bonus versus a racial type.
    /// Bonus must be between 1 and 20.
    /// </summary>
    /// <param name="race">The race.</param>
    /// <param name="bonus">The attack bonus (1–20).</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty AttackBonusVsRace(NwRace race, int bonus)
    {
      return NWScript.ItemPropertyAttackBonusVsRace(race.Id, bonus)!;
    }

    /// <summary>
    /// Creates an Attack bonus versus a specific alignment.
    /// Bonus must be between 1 and 20.
    /// </summary>
    /// <param name="alignment">The alignment.</param>
    /// <param name="bonus">The attack bonus (1–20).</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty AttackBonusVsSAlign(IPAlignment alignment, int bonus)
    {
      return NWScript.ItemPropertyAttackBonusVsSAlign((int)alignment, bonus)!;
    }

    /// <summary>
    /// Creates an Attack penalty item property.
    /// Penalty must be a positive integer between 1 and 5 (1 = -1).
    /// </summary>
    /// <param name="penalty">The attack penalty (1–5, interpreted as negative).</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty AttackPenalty(int penalty)
    {
      return NWScript.ItemPropertyAttackPenalty(penalty)!;
    }

    /// <summary>
    /// Grants a bonus feat when applied to an item.
    /// </summary>
    /// <param name="feat">The feat to grant.</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty BonusFeat(IPFeat feat)
    {
      return NWScript.ItemPropertyBonusFeat((int)feat)!;
    }

    /// <summary>
    /// Grants a bonus spell slot for the specified class and level.
    /// Spell level must be 0–9.
    /// </summary>
    /// <param name="classType">The spellcasting class.</param>
    /// <param name="spellLevel">The spell level (0–9).</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty BonusLevelSpell(IPClass classType, IPSpellLevel spellLevel)
    {
      return NWScript.ItemPropertyBonusLevelSpell((int)classType, (int)spellLevel)!;
    }

    /// <summary>
    /// Grants a saving throw bonus to a base save type.
    /// Bonus must be between 1 and 20.
    /// </summary>
    /// <param name="saveType">The base save type.</param>
    /// <param name="bonus">The bonus amount (1–20).</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty BonusSavingThrow(IPSaveBaseType saveType, int bonus)
    {
      return NWScript.ItemPropertyBonusSavingThrow((int)saveType, bonus)!;
    }

    /// <summary>
    /// Grants a saving throw bonus versus a specific effect or damage type.
    /// Bonus must be between 1 and 20.
    /// </summary>
    /// <param name="saveType">The specific save-vs type.</param>
    /// <param name="bonus">The bonus amount (1–20).</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty BonusSavingThrowVsX(IPSaveVs saveType, int bonus)
    {
      return NWScript.ItemPropertyBonusSavingThrowVsX((int)saveType, bonus)!;
    }

    /// <summary>
    /// Grants bonus spell resistance.
    /// </summary>
    /// <param name="resistBonus">The spell resistance bonus type.</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty BonusSpellResistance(IPSpellResistanceBonus resistBonus)
    {
      return NWScript.ItemPropertyBonusSpellResistance((int)resistBonus)!;
    }

    /// <summary>
    /// Adds a cast spell property with a limited number of uses.
    /// Allowed spells depend on item type; higher spell level increases item cost.
    /// </summary>
    /// <param name="spell">The spell to cast.</param>
    /// <param name="uses">The number of uses.</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty CastSpell(IPCastSpell spell, IPCastSpellNumUses uses)
    {
      return NWScript.ItemPropertyCastSpell((int)spell, (int)uses)!;
    }

    /// <summary>
    /// Creates a container reduced weight property for special containers.
    /// </summary>
    /// <param name="weightReduction">The container weight reduction type.</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty ContainerReducedWeight(IPContainerWeightReduction weightReduction)
    {
      return NWScript.ItemPropertyContainerReducedWeight((int)weightReduction)!;
    }

    /// <summary>
    /// Creates a custom item property by specifying raw table indices.
    /// </summary>
    /// <param name="type">ItemPropertyTable row index.</param>
    /// <param name="subType">ItemPropertySubTypeTable row index, or -1.</param>
    /// <param name="costTableValue">ItemPropertyCostTable row index, or -1.</param>
    /// <param name="param1Value">ItemPropertyParamTable row index, or -1.</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty Custom(int type, int subType = -1, int costTableValue = -1, int param1Value = -1)
    {
      return NWScript.ItemPropertyCustom(type, subType, costTableValue, param1Value)!;
    }

    /// <summary>
    /// Creates a custom item property using strongly typed table entries.
    /// </summary>
    /// <param name="property">The base property entry.</param>
    /// <param name="subType">Optional subtype entry.</param>
    /// <param name="costTableValue">Optional cost table entry.</param>
    /// <param name="paramTableValue">Optional parameter table entry.</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty Custom(ItemPropertyTableEntry property, ItemPropertySubTypeTableEntry? subType = null, ItemPropertyCostTableEntry? costTableValue = null, ItemPropertyParamTableEntry? paramTableValue = null)
    {
      return NWScript.ItemPropertyCustom(property.RowIndex, subType?.RowIndex ?? -1, costTableValue?.RowIndex ?? -1, paramTableValue?.RowIndex ?? -1)!;
    }

    /// <summary>
    /// Grants a damage bonus of a given type.
    /// </summary>
    /// <param name="damageType">The damage type.</param>
    /// <param name="damageBonus">The bonus magnitude.</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty DamageBonus(IPDamageType damageType, IPDamageBonus damageBonus)
    {
      return NWScript.ItemPropertyDamageBonus((int)damageType, (int)damageBonus)!;
    }

    /// <summary>
    /// Grants a damage bonus versus an alignment group.
    /// </summary>
    /// <param name="alignmentGroup">The alignment group.</param>
    /// <param name="damageType">The damage type.</param>
    /// <param name="damageBonus">The bonus magnitude.</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty DamageBonusVsAlign(IPAlignmentGroup alignmentGroup, IPDamageType damageType, IPDamageBonus damageBonus)
    {
      return NWScript.ItemPropertyDamageBonusVsAlign((int)alignmentGroup, (int)damageType, (int)damageBonus)!;
    }

    [Obsolete("Use the NwRace/RacialType overload instead.")]
    public static ItemProperty DamageBonusVsRace(IPRacialType racialType, IPDamageType damageType, IPDamageBonus damageBonus)
    {
      return NWScript.ItemPropertyDamageBonusVsRace((int)racialType, (int)damageType, (int)damageBonus)!;
    }

    /// <summary>
    /// Grants a damage bonus versus a racial type.
    /// Only certain damage types are valid (Acid, Bludgeoning, Cold, Electrical, Fire, Piercing, Slashing, Sonic).
    /// </summary>
    /// <param name="race">The race.</param>
    /// <param name="damageType">The damage type.</param>
    /// <param name="damageBonus">The bonus magnitude.</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty DamageBonusVsRace(NwRace race, IPDamageType damageType, IPDamageBonus damageBonus)
    {
      return NWScript.ItemPropertyDamageBonusVsRace(race.Id, (int)damageType, (int)damageBonus)!;
    }

    /// <summary>
    /// Grants a damage bonus versus a specific alignment.
    /// Only certain damage types are valid (Acid, Bludgeoning, Cold, Electrical, Fire, Piercing, Slashing, Sonic).
    /// </summary>
    /// <param name="alignment">The alignment.</param>
    /// <param name="damageType">The damage type.</param>
    /// <param name="damageBonus">The bonus magnitude.</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty DamageBonusVsSAlign(IPAlignment alignment, IPDamageType damageType, IPDamageBonus damageBonus)
    {
      return NWScript.ItemPropertyDamageBonusVsSAlign((int)alignment, (int)damageType, (int)damageBonus)!;
    }

    /// <summary>
    /// Grants damage immunity for the specified damage type and percentage.
    /// Only certain damage types are valid (Acid, Bludgeoning, Cold, Electrical, Fire, Piercing, Slashing, Sonic).
    /// </summary>
    /// <param name="damageType">The damage type.</param>
    /// <param name="immunityType">The immunity percentage category.</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty DamageImmunity(IPDamageType damageType, IPDamageImmunityType immunityType)
    {
      return NWScript.ItemPropertyDamageImmunity((int)damageType, (int)immunityType)!;
    }

    /// <summary>
    /// Creates a damage penalty item property.
    /// Penalty must be a positive integer between 1 and 5 (1 = -1).
    /// </summary>
    /// <param name="penalty">The damage penalty (1–5, interpreted as negative).</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty DamagePenalty(int penalty)
    {
      return NWScript.ItemPropertyDamagePenalty(penalty)!;
    }

    /// <summary>
    /// Applies damage reduction that requires a minimum enhancement to bypass, soaking a fixed amount of damage.
    /// </summary>
    /// <param name="damageReduction">The required enhancement level to bypass.</param>
    /// <param name="damageSoak">The amount of damage soaked.</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty DamageReduction(IPDamageReduction damageReduction, IPDamageSoak damageSoak)
    {
      return NWScript.ItemPropertyDamageReduction((int)damageReduction, (int)damageSoak)!;
    }

    /// <summary>
    /// Applies damage resistance against a type, reducing damage by a fixed amount each round.
    /// </summary>
    /// <param name="damageType">The damage type.</param>
    /// <param name="damageResist">The resist amount.</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty DamageResistance(IPDamageType damageType, IPDamageResist damageResist)
    {
      return NWScript.ItemPropertyDamageResistance((int)damageType, (int)damageResist)!;
    }

    /// <summary>
    /// Makes the wielder more vulnerable to a damage type by a percentage.
    /// </summary>
    /// <param name="damageType">The damage type.</param>
    /// <param name="damageVulnerability">The vulnerability percentage category.</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty DamageVulnerability(IPDamageType damageType, IPDamageVulnerabilityType damageVulnerability)
    {
      return NWScript.ItemPropertyDamageVulnerability((int)damageType, (int)damageVulnerability)!;
    }

    /// <summary>
    /// Grants Darkvision.
    /// </summary>
    /// <returns>The created item property.</returns>
    public static ItemProperty Darkvision()
    {
      return NWScript.ItemPropertyDarkvision()!;
    }

    /// <summary>
    /// Decreases an ability score.
    /// Modifier must be a positive integer between 1 and 10 (1 = -1).
    /// </summary>
    /// <param name="ability">The ability.</param>
    /// <param name="penalty">The penalty amount (1–10, interpreted as negative).</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty DecreaseAbility(IPAbility ability, int penalty)
    {
      return NWScript.ItemPropertyDecreaseAbility((int)ability, penalty)!;
    }

    /// <summary>
    /// Decreases Armor Class using the specified modifier type.
    /// Penalty must be a positive integer between 1 and 5 (1 = -1).
    /// </summary>
    /// <param name="modifierType">The AC modifier type.</param>
    /// <param name="penalty">The AC penalty (1–5, interpreted as negative).</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty DecreaseAC(IPACModifierType modifierType, int penalty)
    {
      return NWScript.ItemPropertyDecreaseAC((int)modifierType, penalty)!;
    }

    /// <summary>
    /// Decreases a skill.
    /// Penalty must be a positive integer between 1 and 10 (1 = -1).
    /// </summary>
    /// <param name="skill">The skill to decrease.</param>
    /// <param name="penalty">The penalty amount (1–10, interpreted as negative).</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty DecreaseSkill(NwSkill skill, int penalty)
    {
      return NWScript.ItemPropertyDecreaseSkill(skill.Id, penalty)!;
    }

    /// <summary>
    /// Grants an enhancement bonus.
    /// Bonus must be between 1 and 20.
    /// </summary>
    /// <param name="bonus">The enhancement bonus (1–20).</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty EnhancementBonus(int bonus)
    {
      return NWScript.ItemPropertyEnhancementBonus(bonus)!;
    }

    /// <summary>
    /// Grants an enhancement bonus versus an alignment group.
    /// Bonus must be between 1 and 20.
    /// </summary>
    /// <param name="alignmentGroup">The alignment group.</param>
    /// <param name="bonus">The enhancement bonus (1–20).</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty EnhancementBonusVsAlign(IPAlignmentGroup alignmentGroup, int bonus)
    {
      return NWScript.ItemPropertyEnhancementBonusVsAlign((int)alignmentGroup, bonus)!;
    }

    [Obsolete("Use the NwRace/RacialType overload instead.")]
    public static ItemProperty EnhancementBonusVsRace(IPRacialType racialType, int bonus)
    {
      return NWScript.ItemPropertyEnhancementBonusVsRace((int)racialType, bonus)!;
    }

    /// <summary>
    /// Grants an enhancement bonus versus a racial type.
    /// Bonus must be between 1 and 20.
    /// </summary>
    /// <param name="race">The race.</param>
    /// <param name="bonus">The enhancement bonus (1–20).</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty EnhancementBonusVsRace(NwRace race, int bonus)
    {
      return NWScript.ItemPropertyEnhancementBonusVsRace(race.Id, bonus)!;
    }

    /// <summary>
    /// Grants an enhancement bonus versus a specific alignment.
    /// Bonus must be between 1 and 20.
    /// </summary>
    /// <param name="alignment">The alignment.</param>
    /// <param name="bonus">The enhancement bonus (1–20).</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty EnhancementBonusVsSAlign(IPAlignment alignment, int bonus)
    {
      return NWScript.ItemPropertyEnhancementBonusVsSAlign((int)alignment, bonus)!;
    }

    /// <summary>
    /// Applies an enhancement penalty.
    /// Penalty must be a positive integer between 1 and 5 (1 = -1).
    /// </summary>
    /// <param name="penalty">The enhancement penalty (1–5, interpreted as negative).</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty EnhancementPenalty(int penalty)
    {
      return NWScript.ItemPropertyEnhancementPenalty(penalty)!;
    }

    /// <summary>
    /// Adds extra melee base damage type to a weapon.
    /// Only physical types (Piercing, Slashing, Bludgeoning). Applies to melee weapons only.
    /// </summary>
    /// <param name="damageType">The base damage type.</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty ExtraMeleeDamageType(IPDamageType damageType)
    {
      return NWScript.ItemPropertyExtraMeleeDamageType((int)damageType)!;
    }

    /// <summary>
    /// Adds extra ranged base damage type to a weapon.
    /// Only physical types (Piercing, Slashing, Bludgeoning). Applies to ranged weapons only.
    /// </summary>
    /// <param name="damageType">The base damage type.</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty ExtraRangeDamageType(IPDamageType damageType)
    {
      return NWScript.ItemPropertyExtraRangeDamageType((int)damageType)!;
    }

    /// <summary>
    /// Grants Free Action.
    /// </summary>
    /// <returns>The created item property.</returns>
    public static ItemProperty FreeAction()
    {
      return NWScript.ItemPropertyFreeAction()!;
    }

    /// <summary>
    /// Grants Haste to the wielder.
    /// </summary>
    /// <returns>The created item property.</returns>
    public static ItemProperty Haste()
    {
      return NWScript.ItemPropertyHaste()!;
    }

    /// <summary>
    /// Creates a Healer's Kit property.
    /// Level must be between 1 and 12.
    /// </summary>
    /// <param name="level">The kit level (1–12).</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty HealersKit(int level)
    {
      return NWScript.ItemPropertyHealersKit(level)!;
    }

    /// <summary>
    /// Grants Holy Avenger properties to a weapon.
    /// </summary>
    /// <returns>The created item property.</returns>
    public static ItemProperty HolyAvenger()
    {
      return NWScript.ItemPropertyHolyAvenger()!;
    }

    /// <summary>
    /// Grants immunity to a miscellaneous effect.
    /// </summary>
    /// <param name="immunityType">The immunity type.</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty ImmunityMisc(IPMiscImmunity immunityType)
    {
      return NWScript.ItemPropertyImmunityMisc((int)immunityType)!;
    }

    /// <summary>
    /// Grants immunity to spells up to and including a specified spell level.
    /// Level must be between 1 and 9.
    /// </summary>
    /// <param name="spellLevel">The highest spell level to be immune to (1–9).</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty ImmunityToSpellLevel(IPSpellLevel spellLevel)
    {
      return NWScript.ItemPropertyImmunityToSpellLevel((int)spellLevel)!;
    }

    /// <summary>
    /// Grants Improved Evasion.
    /// </summary>
    /// <returns>The created item property.</returns>
    public static ItemProperty ImprovedEvasion()
    {
      return NWScript.ItemPropertyImprovedEvasion()!;
    }

    /// <summary>
    /// Grants Keen (increases critical threat range).
    /// </summary>
    /// <returns>The created item property.</returns>
    public static ItemProperty Keen()
    {
      return NWScript.ItemPropertyKeen()!;
    }

    /// <summary>
    /// Adds a light aura to the item.
    /// </summary>
    /// <param name="brightness">Light intensity.</param>
    /// <param name="color">Light color.</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty Light(IPLightBrightness brightness, IPLightColor color)
    {
      return NWScript.ItemPropertyLight((int)brightness, (int)color)!;
    }

    /// <summary>
    /// Limits use by an alignment group.
    /// </summary>
    /// <param name="alignmentGroup">Allowed alignment group.</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty LimitUseByAlign(IPAlignmentGroup alignmentGroup)
    {
      return NWScript.ItemPropertyLimitUseByAlign((int)alignmentGroup)!;
    }

    [Obsolete("Use the NwClass/IPClass overload instead.")]
    public static ItemProperty LimitUseByClass(IPClass classType)
    {
      return NWScript.ItemPropertyLimitUseByClass((int)classType)!;
    }

    /// <summary>
    /// Limits use by class.
    /// </summary>
    /// <param name="classType">Allowed class.</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty LimitUseByClass(NwClass classType)
    {
      return NWScript.ItemPropertyLimitUseByClass(classType.Id)!;
    }

    [Obsolete("Use the NwRace/RacialType overload instead.")]
    public static ItemProperty LimitUseByRace(IPRacialType racialType)
    {
      return NWScript.ItemPropertyLimitUseByRace((int)racialType)!;
    }

    /// <summary>
    /// Limits use by race.
    /// </summary>
    /// <param name="race">Allowed race.</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty LimitUseByRace(NwRace race)
    {
      return NWScript.ItemPropertyLimitUseByRace(race.Id)!;
    }

    /// <summary>
    /// Limits use by a specific alignment.
    /// </summary>
    /// <param name="alignment">Allowed alignment.</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty LimitUseBySAlign(IPAlignment alignment)
    {
      return NWScript.ItemPropertyLimitUseBySAlign((int)alignment)!;
    }

    /// <summary>
    /// Adds Massive Criticals extra damage on critical hits.
    /// </summary>
    /// <param name="damageBonus">The extra damage bonus category.</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty MassiveCritical(IPDamageBonus damageBonus)
    {
      return NWScript.ItemPropertyMassiveCritical((int)damageBonus)!;
    }

    /// <summary>
    /// Sets the material type for crafting/quality systems.
    /// </summary>
    /// <param name="materialType">The material type id.</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty Material(int materialType)
    {
      return NWScript.ItemPropertyMaterial(materialType)!;
    }

    /// <summary>
    /// Sets the maximum range strength modifier (Mighty).
    /// Modifier must be between 1 and 20.
    /// </summary>
    /// <param name="modifier">Maximum strength modifier (1–20).</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty MaxRangeStrengthMod(int modifier)
    {
      return NWScript.ItemPropertyMaxRangeStrengthMod(modifier)!;
    }

    /// <summary>
    /// Sets monster natural weapon damage.
    /// Only applies to monster natural weapons.
    /// </summary>
    /// <param name="monsterDamage">Monster damage category.</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty MonsterDamage(IPMonsterDamage monsterDamage)
    {
      return NWScript.ItemPropertyMonsterDamage((int)monsterDamage)!;
    }

    /// <summary>
    /// Causes the weapon to deal no damage in combat.
    /// </summary>
    /// <returns>The created item property.</returns>
    public static ItemProperty NoDamage()
    {
      return NWScript.ItemPropertyNoDamage()!;
    }

    /// <summary>
    /// Applies an on-hit cast spell effect using caster level.
    /// </summary>
    /// <param name="spell">The spell to cast.</param>
    /// <param name="casterLevel">The caster level.</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty OnHitCastSpell(IPCastSpell spell, int casterLevel)
    {
      return NWScript.ItemPropertyOnHitCastSpell((int)spell, casterLevel)!;
    }

    [Obsolete("Use the OnHitCastSpell(IPCastSpell, int) overload instead.")]
    public static ItemProperty OnHitCastSpell(IPCastSpell spell, IPSpellLevel spellLevel)
    {
      return NWScript.ItemPropertyOnHitCastSpell((int)spell, (int)spellLevel)!;
    }

    /// <summary>
    /// Applies an On Hit effect with a save DC and optional special parameter.
    /// </summary>
    /// <param name="saveDC">The save DC category.</param>
    /// <param name="effect">The effect descriptor.</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty OnHitEffect(IPOnHitSaveDC saveDC, HitEffect effect)
    {
      return NWScript.ItemPropertyOnHitProps(effect.Property, (int)saveDC, effect.Special)!;
    }

    /// <summary>
    /// Applies a monster On Hit property; only works on monster natural weapons.
    /// </summary>
    /// <param name="effect">The monster hit effect.</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty OnMonsterHitProperties(MonsterHitEffect effect)
    {
      ItemProperty property = NWScript.ItemPropertyCustom((int)ItemPropertyType.OnMonsterHit, effect.Property, -1, effect.Special)!;
      property.IntParams[2] = -1;

      return property;
    }

    /// <summary>
    /// Sets item quality.
    /// </summary>
    /// <param name="quality">The quality category.</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty Quality(IPQuality quality)
    {
      return NWScript.ItemPropertyQuality((int)quality)!;
    }

    /// <summary>
    /// Applies a saving throw penalty to a base save type.
    /// Penalty must be a positive integer between 1 and 20 (1 = -1).
    /// </summary>
    /// <param name="saveType">The base save type.</param>
    /// <param name="penalty">The penalty amount (1–20, interpreted as negative).</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty ReducedSavingThrow(IPSaveBaseType saveType, int penalty)
    {
      return NWScript.ItemPropertyReducedSavingThrow((int)saveType, penalty)!;
    }

    /// <summary>
    /// Applies a saving throw penalty versus a specific effect or damage type.
    /// Penalty must be a positive integer between 1 and 20 (1 = -1).
    /// </summary>
    /// <param name="saveType">The specific save-vs type.</param>
    /// <param name="penalty">The penalty amount (1–20, interpreted as negative).</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty ReducedSavingThrowVsX(IPSaveVs saveType, int penalty)
    {
      return NWScript.ItemPropertyReducedSavingThrowVsX((int)saveType, penalty)!;
    }

    /// <summary>
    /// Grants regeneration each round.
    /// Amount must be between 1 and 20.
    /// </summary>
    /// <param name="regenAmount">The regeneration amount (1–20).</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty Regeneration(int regenAmount)
    {
      return NWScript.ItemPropertyRegeneration(regenAmount)!;
    }

    /// <summary>
    /// Grants a skill bonus.
    /// Bonus must be between 1 and 50.
    /// </summary>
    /// <param name="skill">The skill.</param>
    /// <param name="bonus">The bonus amount (1–50).</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty SkillBonus(NwSkill skill, int bonus)
    {
      return NWScript.ItemPropertySkillBonus(skill.Id, bonus)!;
    }

    /// <summary>
    /// Applies a special walk animation to the user.
    /// </summary>
    /// <returns>The created item property.</returns>
    public static ItemProperty SpecialWalk()
    {
      return NWScript.ItemPropertySpecialWalk()!;
    }

    /// <summary>
    /// Grants spell immunity versus a specific spell.
    /// </summary>
    /// <param name="spellSchool">The spell school.</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty SpellImmunitySchool(IPSpellSchool spellSchool)
    {
      return NWScript.ItemPropertySpellImmunitySchool((int)spellSchool)!;
    }

    /// <summary>
    /// Grants spell immunity versus a specific spell.
    /// </summary>
    /// <param name="spell">The specific spell.</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty SpellImmunitySpecific(IPSpellImmunity spell)
    {
      return NWScript.ItemPropertySpellImmunitySpecific((int)spell)!;
    }

    /// <summary>
    /// Grants a Thieves' Tools modifier.
    /// Modifier must be between 1 and 12.
    /// </summary>
    /// <param name="modifier">The tool modifier (1–12).</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty ThievesTools(int modifier)
    {
      return NWScript.ItemPropertyThievesTools(modifier)!;
    }

    /// <summary>
    /// Creates a Trap property with level and type.
    /// </summary>
    /// <param name="trapStrength">Trap strength.</param>
    /// <param name="trapType">Trap type.</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty Trap(IPTrapStrength trapStrength, IPTrapType trapType)
    {
      return NWScript.ItemPropertyTrap((int)trapStrength, (int)trapType)!;
    }

    /// <summary>
    /// Grants True Seeing.
    /// </summary>
    /// <returns>The created item property.</returns>
    public static ItemProperty TrueSeeing()
    {
      return NWScript.ItemPropertyTrueSeeing()!;
    }

    /// <summary>
    /// Grants Turn Resistance.
    /// Bonus must be between 1 and 50.
    /// </summary>
    /// <param name="modifier">The resistance bonus (1–50).</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty TurnResistance(int modifier)
    {
      return NWScript.ItemPropertyTurnResistance(modifier)!;
    }

    /// <summary>
    /// Grants unlimited ammunition to a ranged weapon.
    /// Optional special ammo effects can be specified.
    /// </summary>
    /// <param name="ammoType">Unlimited ammo type; defaults to basic.</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty UnlimitedAmmo(IPUnlimitedAmmoType ammoType = IPUnlimitedAmmoType.Basic)
    {
      return NWScript.ItemPropertyUnlimitedAmmo((int)ammoType)!;
    }

    /// <summary>
    /// Grants Vampiric Regeneration.
    /// Amount must be between 1 and 20.
    /// </summary>
    /// <param name="regenAmount">The regeneration amount (1–20).</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty VampiricRegeneration(int regenAmount)
    {
      return NWScript.ItemPropertyVampiricRegeneration(regenAmount)!;
    }

    /// <summary>
    /// Applies a persistent visual effect to the item.
    /// </summary>
    /// <param name="itemVisual">The item visual effect.</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty VisualEffect(ItemVisual itemVisual)
    {
      return NWScript.ItemPropertyVisualEffect((int)itemVisual)!;
    }

    /// <summary>
    /// Increases item weight.
    /// </summary>
    /// <param name="weightIncrease">The weight increase category.</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty WeightIncrease(IPWeightIncrease weightIncrease)
    {
      return NWScript.ItemPropertyWeightIncrease((int)weightIncrease)!;
    }

    /// <summary>
    /// Applies weight reduction for containers.
    /// </summary>
    /// <param name="weightReduction">The weight reduction category.</param>
    /// <returns>The created item property.</returns>
    public static ItemProperty WeightReduction(IPReducedWeight weightReduction)
    {
      return NWScript.ItemPropertyWeightReduction((int)weightReduction)!;
    }
  }
}
