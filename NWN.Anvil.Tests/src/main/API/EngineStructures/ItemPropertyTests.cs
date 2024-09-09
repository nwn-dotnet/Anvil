using System.Linq;
using Anvil.API;
using Anvil.Native;
using NUnit.Framework;
using NWN.Native.API;
using ClassType = Anvil.API.ClassType;
using ItemProperty = Anvil.API.ItemProperty;
using RacialType = Anvil.API.RacialType;
using Skill = Anvil.API.Skill;

namespace Anvil.Tests.API
{
  [TestFixture(Category = "API.EngineStructure")]
  public sealed class ItemPropertyTests
  {
    [Test(Description = "Creating an item property and disposing the item property explicitly frees the associated memory.")]
    public void CreateAndDisposeItemPropertyFreesNativeStructure()
    {
      ItemProperty itemProperty = ItemProperty.Haste();
      Assert.That(itemProperty.IsValid, Is.True, "Item property was not valid after creation.");
      itemProperty.Dispose();
      Assert.That(itemProperty.IsValid, Is.False, "Item property was still valid after disposing.");
    }

    [Test(Description = "A soft item property reference created from a native object does not cause the original item property to be deleted.")]
    public void CreateSoftItemPropertyReferenceAndDisposeDoesNotFreeMemory()
    {
      ItemProperty itemProperty = ItemProperty.Haste();
      Assert.That(itemProperty.IsValid, Is.True, "Item property was not valid after creation.");

      CGameEffect gameEffect = itemProperty;
      Assert.That(gameEffect, Is.Not.Null, "Native Item property was not valid after implicit cast.");

      ItemProperty softReference = gameEffect.ToItemProperty(false)!;
      softReference.Dispose();
      Assert.That(softReference.IsValid, Is.True, "The soft reference disposed the memory of the original item property.");
    }

    [TestFixture(Category = "API.EngineStructure")]
    public abstract class ItemPropertyTest
    {
      protected abstract ItemProperty ItemProperty { get; }

      protected abstract ItemPropertyType PropertyType { get; }

      protected virtual int SubType => -1;
      protected virtual ItemPropertyCostTablesEntry? CostTable => null;
      protected virtual int CostTableValue => -1;
      protected virtual ItemPropertyParamTablesEntry? Param1Table => null;
      protected virtual int Param1TableValue => -1;
      protected virtual int UsesPerDay => -1;
      protected virtual int ChanceOfAppearing => 100;
      protected virtual int Usable => 1;
      protected virtual string CustomTag => "";

      [Test(Description = "The created item property supplies the correct parameters to the effect structure")]
      public void TestEffect()
      {
        ItemProperty itemProperty = ItemProperty;
        Assert.That(itemProperty.IsValid, Is.True);

        EffectParams<int> intParams = itemProperty.IntParams;
        EffectParams<string> stringParams = itemProperty.StringParams;

        Assert.That(intParams[0], Is.EqualTo((int)PropertyType), "Property type does not match");
        Assert.That(intParams[1], Is.EqualTo(SubType), "Property sub type does not match");
        Assert.That(intParams[2], Is.EqualTo(CostTable?.RowIndex ?? -1), "Property cost table does not match");
        Assert.That(intParams[3], Is.EqualTo(CostTableValue), "Property cost table value does not match");
        Assert.That(intParams[4], Is.EqualTo(Param1Table?.RowIndex ?? -1), "Property param1 table does not match");
        Assert.That(intParams[5], Is.EqualTo(Param1TableValue), "Property param1 table value does not match");
        Assert.That(intParams[6], Is.EqualTo(UsesPerDay), "Property uses per day does not match");
        Assert.That(intParams[7], Is.EqualTo(ChanceOfAppearing), "Property chance of appearing does not match");
        Assert.That(intParams[8], Is.EqualTo(Usable), "Property usable does not match");
        Assert.That(stringParams[0], Is.EqualTo(CustomTag), "Property custom tag does not match");
      }
    }

    public class AbilityBonusTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.AbilityBonus(IPAbility.Intelligence, 5);
      protected override ItemPropertyType PropertyType => ItemPropertyType.AbilityBonus;
      protected override int SubType => 3;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(1);
      protected override int CostTableValue => 5;
    }

    public class AcBonusTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.ACBonus(5);
      protected override ItemPropertyType PropertyType => ItemPropertyType.AcBonus;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(2);
      protected override int CostTableValue => 5;
    }

    public class AcBonusVsAlignmentGroupTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.ACBonusVsAlign(IPAlignmentGroup.Good, 3);
      protected override ItemPropertyType PropertyType => ItemPropertyType.AcBonusVsAlignmentGroup;
      protected override int SubType => (int)IPAlignmentGroup.Good;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(2);
      protected override int CostTableValue => 3;
    }

    public class AcBonusVsDamageTypeTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.ACBonusVsDmgType(IPDamageType.Bludgeoning, 3);
      protected override ItemPropertyType PropertyType => ItemPropertyType.AcBonusVsDamageType;
      protected override int SubType => (int)IPDamageType.Bludgeoning;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(2);
      protected override int CostTableValue => 3;
    }

    public class AcBonusVsRaceTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.ACBonusVsRace(NwRace.FromRacialType(RacialType.Outsider)!, 3);
      protected override ItemPropertyType PropertyType => ItemPropertyType.AcBonusVsRacialGroup;
      protected override int SubType => NwRace.FromRacialType(RacialType.Outsider)!.Id;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(2);
      protected override int CostTableValue => 3;
    }

    public class AcBonusVsSpecificAlignmentTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.ACBonusVsSAlign(IPAlignment.NeutralEvil, 3);
      protected override ItemPropertyType PropertyType => ItemPropertyType.AcBonusVsSpecificAlignment;
      protected override int SubType => (int)IPAlignment.NeutralEvil;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(2);
      protected override int CostTableValue => 3;
    }

    public class AdditionalTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.Additional(IPAdditional.Cursed);
      protected override ItemPropertyType PropertyType => ItemPropertyType.Additional;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(30);
      protected override int CostTableValue => (int)IPAdditional.Cursed;
    }

    public class ArcaneSpellFailureTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.ArcaneSpellFailure(IPArcaneSpellFailure.Minus20Pct);
      protected override ItemPropertyType PropertyType => ItemPropertyType.ArcaneSpellFailure;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(27);
      protected override int CostTableValue => (int)IPArcaneSpellFailure.Minus20Pct;
    }

    public class AttackBonusTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.AttackBonus(7);
      protected override ItemPropertyType PropertyType => ItemPropertyType.AttackBonus;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(2);
      protected override int CostTableValue => 7;
    }

    public class AttackBonusVsAlignmentGroupTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.AttackBonusVsAlign(IPAlignmentGroup.Neutral, 8);
      protected override ItemPropertyType PropertyType => ItemPropertyType.AttackBonusVsAlignmentGroup;
      protected override int SubType => (int)IPAlignmentGroup.Neutral;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(2);
      protected override int CostTableValue => 8;
    }

    public class AttackBonusVsRaceTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.AttackBonusVsRace(NwRace.FromRacialType(RacialType.Halfling)!, 9);
      protected override ItemPropertyType PropertyType => ItemPropertyType.AttackBonusVsRacialGroup;
      protected override int SubType => NwRace.FromRacialType(RacialType.Halfling)!.Id;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(2);
      protected override int CostTableValue => 9;
    }

    public class AttackBonusVsSpecificAlignmentTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.AttackBonusVsSAlign(IPAlignment.LawfulGood, 10);
      protected override ItemPropertyType PropertyType => ItemPropertyType.AttackBonusVsSpecificAlignment;
      protected override int SubType => (int)IPAlignment.LawfulGood;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(2);
      protected override int CostTableValue => 10;
    }

    public class AttackPenaltyTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.AttackPenalty(2);
      protected override ItemPropertyType PropertyType => ItemPropertyType.DecreasedAttackModifier;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(20);
      protected override int CostTableValue => 2;
    }

    public class BonusFeatTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.BonusFeat(IPFeat.Disarm);
      protected override ItemPropertyType PropertyType => ItemPropertyType.BonusFeat;
      protected override int SubType => (int)IPFeat.Disarm;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(0);
    }

    public class BonusLevelTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.BonusLevelSpell(IPClass.Cleric, IPSpellLevel.SL5);
      protected override ItemPropertyType PropertyType => ItemPropertyType.BonusSpellSlotOfLevelN;
      protected override int SubType => (int)IPClass.Cleric;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(13);
      protected override int CostTableValue => (int)IPSpellLevel.SL5;
    }

    public class BonusSavingThrowTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.BonusSavingThrow(IPSaveBaseType.Reflex, 2);
      protected override ItemPropertyType PropertyType => ItemPropertyType.SavingThrowBonusSpecific;
      protected override int SubType => (int)IPSaveBaseType.Reflex;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(2);
      protected override int CostTableValue => 2;
    }

    public class BonusSavingThrowVsXTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.BonusSavingThrowVsX(IPSaveVs.Divine, 5);
      protected override ItemPropertyType PropertyType => ItemPropertyType.SavingThrowBonus;
      protected override int SubType => (int)IPSaveVs.Divine;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(2);
      protected override int CostTableValue => 5;
    }

    public class SpellResistanceTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.BonusSpellResistance(IPSpellResistanceBonus.Plus22);
      protected override ItemPropertyType PropertyType => ItemPropertyType.SpellResistance;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(11);
      protected override int CostTableValue => (int)IPSpellResistanceBonus.Plus22;
    }

    public class CastSpellTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.CastSpell(IPCastSpell.Harm11, IPCastSpellNumUses.ChargesPerUse3);
      protected override ItemPropertyType PropertyType => ItemPropertyType.CastSpell;
      protected override int SubType => (int)IPCastSpell.Harm11;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(3);
      protected override int CostTableValue => (int)IPCastSpellNumUses.ChargesPerUse3;
    }

    public class ContainerReducedWeightTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.ContainerReducedWeight(IPContainerWeightReduction.Reduction60Pct);
      protected override ItemPropertyType PropertyType => ItemPropertyType.EnhancedContainerReducedWeight;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(15);
      protected override int CostTableValue => (int)IPContainerWeightReduction.Reduction60Pct;
    }

    public class DamageBonusTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.DamageBonus(IPDamageType.Electrical, IPDamageBonus.Plus1d12);
      protected override ItemPropertyType PropertyType => ItemPropertyType.DamageBonus;
      protected override int SubType => (int)IPDamageType.Electrical;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(4);
      protected override int CostTableValue => (int)IPDamageBonus.Plus1d12;
    }

    public class DamageBonusVsAlignmentGroupTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.DamageBonusVsAlign(IPAlignmentGroup.Chaotic, IPDamageType.Fire, IPDamageBonus.Plus2);
      protected override ItemPropertyType PropertyType => ItemPropertyType.DamageBonusVsAlignmentGroup;
      protected override int SubType => (int)IPAlignmentGroup.Chaotic;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(4);
      protected override int CostTableValue => (int)IPDamageBonus.Plus2;
      protected override int Param1TableValue => (int)IPDamageType.Fire;
    }

    public class DamageBonusVsRaceTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.DamageBonusVsRace(NwRace.FromRacialType(RacialType.Elemental)!, IPDamageType.Bludgeoning, IPDamageBonus.Plus2d6);
      protected override ItemPropertyType PropertyType => ItemPropertyType.DamageBonusVsRacialGroup;
      protected override int SubType => NwRace.FromRacialType(RacialType.Elemental)!.Id;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(4);
      protected override int CostTableValue => (int)IPDamageBonus.Plus2d6;
      protected override int Param1TableValue => (int)IPDamageType.Bludgeoning;
    }

    public class DamageBonusVsSpecificAlignmentTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.DamageBonusVsSAlign(IPAlignment.TrueNeutral, IPDamageType.Sonic, IPDamageBonus.Plus2d12);
      protected override ItemPropertyType PropertyType => ItemPropertyType.DamageBonusVsSpecificAlignment;
      protected override int SubType => (int)IPAlignment.TrueNeutral;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(4);
      protected override int CostTableValue => (int)IPDamageBonus.Plus2d12;
      protected override int Param1TableValue => (int)IPDamageType.Sonic;
    }

    public class DamageImmunityTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.DamageImmunity(IPDamageType.Acid, IPDamageImmunityType.Immunity25Pct);
      protected override ItemPropertyType PropertyType => ItemPropertyType.ImmunityDamageType;
      protected override int SubType => (int)IPDamageType.Acid;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(5);
      protected override int CostTableValue => (int)IPDamageImmunityType.Immunity25Pct;
    }

    public class DamagePenaltyTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.DamagePenalty(3);
      protected override ItemPropertyType PropertyType => ItemPropertyType.DecreasedDamage;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(20);
      protected override int CostTableValue => 3;
    }

    public class DamageReductionTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.DamageReduction(IPDamageReduction.DR5, IPDamageSoak.HP30);
      protected override ItemPropertyType PropertyType => ItemPropertyType.DamageReduction;
      protected override int SubType => (int)IPDamageReduction.DR5;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(6);
      protected override int CostTableValue => (int)IPDamageSoak.HP30;
    }

    public class DamageResistanceTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.DamageResistance(IPDamageType.Divine, IPDamageResist.Resist20);
      protected override ItemPropertyType PropertyType => ItemPropertyType.DamageResistance;
      protected override int SubType => (int)IPDamageType.Divine;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(7);
      protected override int CostTableValue => (int)IPDamageResist.Resist20;
    }

    public class DamageVulnerabilityTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.DamageVulnerability(IPDamageType.Negative, IPDamageVulnerabilityType.Vulnerable90Pct);
      protected override ItemPropertyType PropertyType => ItemPropertyType.DamageVulnerability;
      protected override int SubType => (int)IPDamageType.Negative;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(22);
      protected override int CostTableValue => (int)IPDamageVulnerabilityType.Vulnerable90Pct;
    }

    public class DarkvisionTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.Darkvision();
      protected override ItemPropertyType PropertyType => ItemPropertyType.Darkvision;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(0);
    }

    public class DecreaseAbilityTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.DecreaseAbility(IPAbility.Intelligence, 3);
      protected override ItemPropertyType PropertyType => ItemPropertyType.DecreasedAbilityScore;
      protected override int SubType => (int)IPAbility.Intelligence;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(21);
      protected override int CostTableValue => 3;
    }

    public class DecreaseACTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.DecreaseAC(IPACModifierType.Dodge, 7);
      protected override ItemPropertyType PropertyType => ItemPropertyType.DecreasedAc;
      protected override int SubType => (int)IPACModifierType.Dodge;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(20);
      protected override int CostTableValue => 7;
    }

    public class DecreaseSkillTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.DecreaseSkill(NwSkill.FromSkillType(Skill.Discipline)!, 5);
      protected override ItemPropertyType PropertyType => ItemPropertyType.DecreasedSkillModifier;
      protected override int SubType => NwSkill.FromSkillType(Skill.Discipline)!.Id;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(21);
      protected override int CostTableValue => 5;
    }

    public class EnhancementBonusTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.EnhancementBonus(4);
      protected override ItemPropertyType PropertyType => ItemPropertyType.EnhancementBonus;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(2);
      protected override int CostTableValue => 4;
    }

    public class EnhancementBonusVsAlignmentGroupTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.EnhancementBonusVsAlign(IPAlignmentGroup.Chaotic, 7);
      protected override ItemPropertyType PropertyType => ItemPropertyType.EnhancementBonusVsAlignmentGroup;
      protected override int SubType => (int)IPAlignmentGroup.Chaotic;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(2);
      protected override int CostTableValue => 7;
    }

    public class EnhancementBonusVsRaceTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.EnhancementBonusVsRace(NwRace.FromRacialType(RacialType.Dragon)!, 2);
      protected override ItemPropertyType PropertyType => ItemPropertyType.EnhancementBonusVsRacialGroup;
      protected override int SubType => NwRace.FromRacialType(RacialType.Dragon)!.Id;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(2);
      protected override int CostTableValue => 2;
    }

    public class EnhancementBonusVsSpecificAlignmentTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.EnhancementBonusVsSAlign(IPAlignment.ChaoticNeutral, 9);
      protected override ItemPropertyType PropertyType => ItemPropertyType.EnhancementBonusVsSpecificAlignment;
      protected override int SubType => (int)IPAlignment.ChaoticNeutral;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(2);
      protected override int CostTableValue => 9;
    }

    public class EnhancementPenaltyTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.EnhancementPenalty(6);
      protected override ItemPropertyType PropertyType => ItemPropertyType.DecreasedEnhancementModifier;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(20);
      protected override int CostTableValue => 6;
    }

    public class ExtraMeleeDamageTypeTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.ExtraMeleeDamageType(IPDamageType.Slashing);
      protected override ItemPropertyType PropertyType => ItemPropertyType.ExtraMeleeDamageType;
      protected override int SubType => (int)IPDamageType.Slashing;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(0);
    }

    public class ExtraRangeDamageTypeTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.ExtraRangeDamageType(IPDamageType.Bludgeoning);
      protected override ItemPropertyType PropertyType => ItemPropertyType.ExtraRangedDamageType;
      protected override int SubType => (int)IPDamageType.Bludgeoning;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(0);
    }

    public class FreeActionTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.FreeAction();
      protected override ItemPropertyType PropertyType => ItemPropertyType.FreedomOfMovement;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(0);
    }

    public class HasteTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.Haste();
      protected override ItemPropertyType PropertyType => ItemPropertyType.Haste;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(0);
    }

    public class HealersKitTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.HealersKit(4);
      protected override ItemPropertyType PropertyType => ItemPropertyType.HealersKit;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(25);
      protected override int CostTableValue => 4;
    }

    public class HolyAvengerTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.HolyAvenger();
      protected override ItemPropertyType PropertyType => ItemPropertyType.HolyAvenger;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(0);
    }

    public class ImmunityMiscTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.ImmunityMisc(IPMiscImmunity.DeathMagic);
      protected override ItemPropertyType PropertyType => ItemPropertyType.ImmunityMiscellaneous;
      protected override int SubType => (int)IPMiscImmunity.DeathMagic;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(0);
    }

    public class ImmunityToSpellLevelTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.ImmunityToSpellLevel(IPSpellLevel.SL5);
      protected override ItemPropertyType PropertyType => ItemPropertyType.ImmunitySpellsByLevel;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(23);
      protected override int CostTableValue => (int)IPSpellLevel.SL5;
    }

    public class ImprovedEvasionTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.ImprovedEvasion();
      protected override ItemPropertyType PropertyType => ItemPropertyType.ImprovedEvasion;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(0);
    }

    public class KeenTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.Keen();
      protected override ItemPropertyType PropertyType => ItemPropertyType.Keen;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(0);
    }

    public class LightTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.Light(IPLightBrightness.Normal, IPLightColor.Purple);
      protected override ItemPropertyType PropertyType => ItemPropertyType.Light;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(18);
      protected override int CostTableValue => (int)IPLightBrightness.Normal;
      protected override int Param1TableValue => (int)IPLightColor.Purple;
    }

    public class LimitUseByAlignTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.LimitUseByAlign(IPAlignmentGroup.Good);
      protected override ItemPropertyType PropertyType => ItemPropertyType.UseLimitationAlignmentGroup;
      protected override int SubType => (int)IPAlignmentGroup.Good;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(0);
    }

    public class LimitUseByClassTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.LimitUseByClass(NwClass.FromClassType(ClassType.Bard)!);
      protected override ItemPropertyType PropertyType => ItemPropertyType.UseLimitationClass;
      protected override int SubType => NwClass.FromClassType(ClassType.Bard)!.Id;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(0);
    }

    public class LimitUseByRaceTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.LimitUseByRace(NwRace.FromRacialType(RacialType.Undead)!);
      protected override ItemPropertyType PropertyType => ItemPropertyType.UseLimitationRacialType;
      protected override int SubType => NwRace.FromRacialType(RacialType.Undead)!.Id;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(0);
    }

    public class LimitUseBySAlignTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.LimitUseBySAlign(IPAlignment.NeutralGood);
      protected override ItemPropertyType PropertyType => ItemPropertyType.UseLimitationSpecificAlignment;
      protected override int SubType => (int)IPAlignment.NeutralGood;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(0);
    }

    public class MassiveCriticalTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.MassiveCritical(IPDamageBonus.Plus2d8);
      protected override ItemPropertyType PropertyType => ItemPropertyType.MassiveCriticals;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(4);
      protected override int CostTableValue => (int)IPDamageBonus.Plus2d8;
    }

    public class MaterialTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.Material(3);
      protected override ItemPropertyType PropertyType => ItemPropertyType.Material;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(28);
      protected override int CostTableValue => 3;
    }

    public class MaxRangeStrengthModTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.MaxRangeStrengthMod(2);
      protected override ItemPropertyType PropertyType => ItemPropertyType.Mighty;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(2);
      protected override int CostTableValue => 2;
    }

    public class MonsterDamageTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.MonsterDamage(IPMonsterDamage.Damage1d4);
      protected override ItemPropertyType PropertyType => ItemPropertyType.MonsterDamage;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(19);
      protected override int CostTableValue => (int)IPMonsterDamage.Damage1d4;
    }

    public class NoDamageTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.NoDamage();
      protected override ItemPropertyType PropertyType => ItemPropertyType.NoDamage;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(0);
    }

    public class OnHitCastSpellTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.OnHitCastSpell(IPCastSpell.InflictSeriousWounds9, 20);
      protected override ItemPropertyType PropertyType => ItemPropertyType.OnHitCastSpell;
      protected override int SubType => (int)IPCastSpell.InflictSeriousWounds9;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(26);
      protected override int CostTableValue => 20 - 1;
    }

    public class OnHitEffectTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.OnHitEffect(IPOnHitSaveDC.DC26, HitEffect.AbilityDrain(IPAbility.Wisdom));
      protected override ItemPropertyType PropertyType => ItemPropertyType.OnHitProperties;
      protected override int SubType => HitEffect.AbilityDrain(IPAbility.Wisdom).Property;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(24);
      protected override int CostTableValue => (int)IPOnHitSaveDC.DC26;
      protected override int Param1TableValue => HitEffect.AbilityDrain(IPAbility.Wisdom).Special;
    }

    public class OnMonsterHitEffectTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.OnMonsterHitProperties(MonsterHitEffect.Stun(IPOnHitDuration.Duration50Pct2Rounds));
      protected override ItemPropertyType PropertyType => ItemPropertyType.OnMonsterHit;
      protected override int SubType => MonsterHitEffect.Stun(IPOnHitDuration.Duration50Pct2Rounds).Property;
      protected override int Param1TableValue => HitEffect.AbilityDrain(IPAbility.Wisdom).Special;
    }

    public class QualityTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.Quality(IPQuality.Masterwork);
      protected override ItemPropertyType PropertyType => ItemPropertyType.Quality;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(29);
      protected override int CostTableValue => (int)IPQuality.Masterwork;
    }

    public class ReducedSavingThrowTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.ReducedSavingThrow(IPSaveBaseType.Reflex, 5);
      protected override ItemPropertyType PropertyType => ItemPropertyType.DecreasedSavingThrowsSpecific;
      protected override int SubType => (int)IPSaveBaseType.Reflex;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(20);
      protected override int CostTableValue => 5;
    }

    public class ReducedSavingThrowVsXTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.ReducedSavingThrowVsX(IPSaveVs.MindAffecting, 2);
      protected override ItemPropertyType PropertyType => ItemPropertyType.DecreasedSavingThrows;
      protected override int SubType => (int)IPSaveVs.MindAffecting;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(20);
      protected override int CostTableValue => 2;
    }

    public class RegenerationTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.Regeneration(12);
      protected override ItemPropertyType PropertyType => ItemPropertyType.Regeneration;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(2);
      protected override int CostTableValue => 12;
    }

    public class SkillBonusTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.SkillBonus(NwSkill.FromSkillType(Skill.Persuade)!, 9);
      protected override ItemPropertyType PropertyType => ItemPropertyType.SkillBonus;
      protected override int SubType => NwSkill.FromSkillType(Skill.Persuade)!.Id;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(25);
      protected override int CostTableValue => 9;
    }

    public class SpecialWalkTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.SpecialWalk();
      protected override ItemPropertyType PropertyType => ItemPropertyType.SpecialWalk;
      protected override int SubType => 1;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(0);
    }

    public class SpellImmunitySchoolTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.SpellImmunitySchool(IPSpellSchool.Necromancy);
      protected override ItemPropertyType PropertyType => ItemPropertyType.ImmunitySpellSchool;
      protected override int SubType => (int)IPSpellSchool.Necromancy;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(0);
    }

    public class SpellImmunitySpecificTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.SpellImmunitySpecific(IPSpellImmunity.Dismissal);
      protected override ItemPropertyType PropertyType => ItemPropertyType.ImmunitySpecificSpell;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(16);
      protected override int CostTableValue => (int)IPSpellImmunity.Dismissal;
    }

    public class ThievesToolsTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.ThievesTools(10);
      protected override ItemPropertyType PropertyType => ItemPropertyType.ThievesTools;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(25);
      protected override int CostTableValue => 10;
    }

    public class TrapTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.Trap(IPTrapStrength.Deadly, IPTrapType.Fire);
      protected override ItemPropertyType PropertyType => ItemPropertyType.Trap;
      protected override int SubType => (int)IPTrapStrength.Deadly;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(17);
      protected override int CostTableValue => (int)IPTrapType.Fire;
    }

    public class TrueSeeingTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.TrueSeeing();
      protected override ItemPropertyType PropertyType => ItemPropertyType.TrueSeeing;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(0);
    }

    public class TurnResistanceTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.TurnResistance(20);
      protected override ItemPropertyType PropertyType => ItemPropertyType.TurnResistance;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(25);
      protected override int CostTableValue => 20;
    }

    public class UnlimitedAmmoTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.UnlimitedAmmo(IPUnlimitedAmmoType.Fire1d6);
      protected override ItemPropertyType PropertyType => ItemPropertyType.UnlimitedAmmunition;
      protected override int SubType => 0; // ???
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(14);
      protected override int CostTableValue => (int)IPUnlimitedAmmoType.Fire1d6;
    }

    public class VampiricRegenerationTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.VampiricRegeneration(13);
      protected override ItemPropertyType PropertyType => ItemPropertyType.RegenerationVampiric;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(2);
      protected override int CostTableValue => 13;
    }

    public class VisualEffectTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.VisualEffect(ItemVisual.Holy);
      protected override ItemPropertyType PropertyType => ItemPropertyType.VisualEffect;
      protected override int SubType => (int)ItemVisual.Holy;
    }

    public class WeightIncreaseTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.WeightIncrease(IPWeightIncrease.Plus50Lbs);
      protected override ItemPropertyType PropertyType => ItemPropertyType.WeightIncrease;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(0);
      protected override int Param1TableValue => (int)IPWeightIncrease.Plus50Lbs;
    }

    public class WeightReductionTest : ItemPropertyTest
    {
      protected override ItemProperty ItemProperty => ItemProperty.WeightReduction(IPReducedWeight.Minus60Pct);
      protected override ItemPropertyType PropertyType => ItemPropertyType.BaseItemWeightReduction;
      protected override ItemPropertyCostTablesEntry? CostTable => NwGameTables.ItemPropertyCostTables.ElementAtOrDefault(10);
      protected override int CostTableValue => (int)IPReducedWeight.Minus60Pct;
    }
  }
}
