using Anvil.API;
using NUnit.Framework;

// ReSharper disable FunctionComplexityOverflow
namespace Anvil.Tests.API
{
  [TestFixture(Category = "API.TwoDimArray")]
  public sealed class NwGameTableTests
  {
    [TestCase(0, "Dwarf", 1985u, "Character_model", "D", "default", "R", "P", 1f, 1f, 1.15f, 0.95f, "NORM", 1.06f, 2.12f, 0.3f, 0.5f, 1.5f, 0.3f, 1.4f, "H", true, 6, true, true, null, 3, 9, 0, 0, true, 60, 30, "head_g", 6, true)]
    [TestCase(7, "Parrot", 110672u, "Parrot", "c_a_parrot", null, "R", "S", null, 1f, 1f, 1f, "FAST", 1f, 3.5f, 0.13f, 0.13f, 0.3f, 0.13f, 1.8f, "L", false, 2, true, true, "po_poly", 1, 9, -1, 5, false, 60, 30, "Parrot_head", 0, true)]
    [TestCase(201, "combat_dummy", 5681u, "cmbtdummy", "c_cmbtdummy", null, "N", "s", null, null, null, null, "VSLOW", null, null, 0.25f, 0.25f, 1f, 0.1f, 0.1f, "H", null, 10, null, null, null, 3, 9, 0, null, true, 60, 30, "impact", 0, true)]
    [TestCase(307, null, null, "User001", null, null, null, null, null, null, null, null, null, null, null, null, null, 1f, null, null, null, null, null, null, null, null, null, 9, -1, null, true, 60, 30, null, 0, true)]
    [TestCase(871, null, null, "USER_RESERVED", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null)]
    public void AppearanceTableReturnsValidData(int rowIndex, string label, uint? strRef, string name, string race, string envMap,
      string bloodColor, string modelType, float? weaponScale, float? wingTailScale, float? helmetScaleM, float? helmetScaleF, string moveRate,
      float? walkDist, float? runDist, float? perSpace, float? crePerSpace, float? height, float? hitDist, float? prefAttackDist, string targetHeight,
      bool? abortOnParry, int? racialType, bool? hasLegs, bool? hasArms, string portrait, int? sizeCategory, int? perceptionDist, int? footstepType,
      int? soundAppType, bool? headTrack, int? headArcH, int? headArcV, string headName, int? bodyBag, bool? targetable)
    {
      TwoDimArray<AppearanceTableEntry> table = NwGameTables.AppearanceTable;
      AppearanceTableEntry row = table.GetRow(rowIndex);

      Assert.That(row.RowIndex, Is.EqualTo(rowIndex));
      Assert.That(row.Label, Is.EqualTo(label));
      Assert.That(row.StrRef?.TokenNumber, Is.EqualTo(strRef));
      Assert.That(row.Name, Is.EqualTo(name));
      Assert.That(row.Race, Is.EqualTo(race));
      Assert.That(row.EnvironmentMap, Is.EqualTo(envMap));
      Assert.That(row.BloodColor, Is.EqualTo(bloodColor));
      Assert.That(row.ModelType, Is.EqualTo(modelType));
      Assert.That(row.WeaponScale, Is.EqualTo(weaponScale));
      Assert.That(row.WingTailScale, Is.EqualTo(wingTailScale));
      Assert.That(row.HelmetScaleM, Is.EqualTo(helmetScaleM));
      Assert.That(row.HelmetScaleF, Is.EqualTo(helmetScaleF));
      Assert.That(row.MovementRate, Is.EqualTo(moveRate));
      Assert.That(row.WalkDistance, Is.EqualTo(walkDist));
      Assert.That(row.RunDistance, Is.EqualTo(runDist));
      Assert.That(row.PersonalSpace, Is.EqualTo(perSpace));
      Assert.That(row.CreaturePersonalSpace, Is.EqualTo(crePerSpace));
      Assert.That(row.Height, Is.EqualTo(height));
      Assert.That(row.HitDistance, Is.EqualTo(hitDist));
      Assert.That(row.PreferredAttackDistance, Is.EqualTo(prefAttackDist));
      Assert.That(row.TargetHeight, Is.EqualTo(targetHeight));
      Assert.That(row.AbortOnParry, Is.EqualTo(abortOnParry));
      Assert.That(row.RacialType, Is.EqualTo(racialType));
      Assert.That(row.HasLegs, Is.EqualTo(hasLegs));
      Assert.That(row.HasArms, Is.EqualTo(hasArms));
      Assert.That(row.Portrait, Is.EqualTo(portrait));
      Assert.That(row.SizeCategory, Is.EqualTo(sizeCategory));
      Assert.That(row.PerceptionDistance, Is.EqualTo(perceptionDist));
      Assert.That(row.FootstepType, Is.EqualTo(footstepType));
      Assert.That(row.AppearanceSoundSet, Is.EqualTo(soundAppType));
      Assert.That(row.HeadTrack, Is.EqualTo(headTrack));
      Assert.That(row.HeadArcHorizontal, Is.EqualTo(headArcH));
      Assert.That(row.HeadArcVertical, Is.EqualTo(headArcV));
      Assert.That(row.HeadName, Is.EqualTo(headName));
      Assert.That(row.BodyBag, Is.EqualTo(bodyBag));
      Assert.That(row.Targetable, Is.EqualTo(targetable));
    }
  }
}
