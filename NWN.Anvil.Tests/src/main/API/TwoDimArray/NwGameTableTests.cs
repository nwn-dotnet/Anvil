using Anvil.API;
using NUnit.Framework;

// ReSharper disable FunctionComplexityOverflow
namespace Anvil.Tests.API
{
  [TestFixture(Category = "API.TwoDimArray")]
  public sealed class NwGameTableTests
  {
    [Test(Description = "Appearance table entries match the expected values.")]
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
      Assert.That(row.StrRef?.Id, Is.EqualTo(strRef));
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

    [Test(Description = "Environment table entries match the expected values.")]
    [TestCase(0, "ExteriorClear", 57937u, DayNightMode.EnableDayNightCycle, 50, 50, 100, 255, 255, 255, true, 0, 0, 0, 100, 100, 200, true, 104, 126, 145, 50, 50, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.5f)]
    [TestCase(1, "ExteriorOvercast", 57938u, DayNightMode.EnableDayNightCycle, 69, 73, 80, 168, 166, 147, true, 0, 0, 0, 77, 77, 77, false, 122, 122, 122, 0, 0, 0, 0, 30, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.3f)]
    [TestCase(2, "ExteriorDark", 57939u, DayNightMode.EnableDayNightCycle, 60, 58, 50, 124, 137, 139, true, 0, 0, 0, 63, 55, 39, false, 53, 53, 32, 0, 0, 0, 7, 15, 4, 0, 0, 0, 27, 0, 0, 0, 2, 2, 2, 0, 0, 0, 0, 0, 0.3f)]
    [TestCase(3, "ExteriorSunset", 57940u, DayNightMode.EnableDayNightCycle, 61, 67, 78, 180, 120, 75, true, 0, 0, 0, 100, 100, 200, true, 50, 50, 100, 50, 50, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.6f)]
    [TestCase(4, "ExteriorPoisoned", 57941u, DayNightMode.EnableDayNightCycle, 78, 49, 39, 180, 197, 148, true, 48, 40, 33, 66, 82, 56, false, 107, 132, 74, 24, 49, 23, 10, 10, 9, 0, 0, 0, 11, 0, 0, 0, 2, 2, 2, 0, 0, 0, 0, 0, 0.3f)]
    [TestCase(5, "ExteriorEvil", 57942u, DayNightMode.EnableDayNightCycle, 0, 22, 125, 230, 174, 174, true, 0, 0, 0, 121, 53, 53, false, 78, 33, 33, 75, 37, 35, 10, 10, 27, 0, 0, 0, 27, 0, 0, 0, 12, 12, 8, 8, 0, 0, 0, 0, 0.3f)]
    [TestCase(6, "ExteriorDry", 57943u, DayNightMode.EnableDayNightCycle, 91, 79, 60, 225, 200, 111, true, 0, 0, 0, 81, 72, 26, false, 185, 160, 74, 60, 61, 33, 10, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.5f)]
    [TestCase(7, "ExteriorFoggy", 57944u, DayNightMode.EnableDayNightCycle, 128, 128, 128, 182, 182, 182, false, 0, 0, 0, 128, 128, 128, false, 128, 128, 128, 41, 41, 41, 15, 15, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0, 0.2f)]
    [TestCase(8, "ExteriorRaining", 57945u, DayNightMode.EnableDayNightCycle, 70, 70, 70, 182, 182, 182, false, 0, 0, 0, 128, 128, 128, false, 128, 128, 128, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 100, 0, 0.2f)]
    [TestCase(9, "ExteriorSnowing", 57946u, DayNightMode.EnableDayNightCycle, 80, 79, 96, 214, 214, 214, false, 0, 0, 0, 80, 80, 80, false, 187, 187, 187, 30, 30, 30, 7, 7, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 1, 100, 0, 0, 0.2f)]
    [TestCase(10, "ExteriorStormy", 57947u, DayNightMode.EnableDayNightCycle, 60, 60, 60, 120, 120, 120, false, 0, 0, 0, 40, 40, 40, false, 60, 60, 60, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 100, 50, 0.3f)]
    [TestCase(11, "ExteriorWindy", 57948u, DayNightMode.EnableDayNightCycle, 69, 73, 80, 168, 166, 147, true, 0, 0, 0, 77, 77, 77, false, 122, 122, 122, 0, 0, 0, 0, 5, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0.5f)]
    [TestCase(12, "InteriorBright", 57949u, DayNightMode.AlwaysNight, 0, 0, 0, 0, 0, 0, false, 60, 60, 60, 170, 170, 170, false, 0, 0, 0, 0, 0, 0, 0, 5, 30, 4, 0, 0, 14, 13, 0, 0, 3, 3, 2, 0, 0, 0, 0, 0, 0.6f)]
    [TestCase(13, "InteriorNormal", 57950u, DayNightMode.AlwaysNight, 0, 0, 0, 0, 0, 0, false, 45, 45, 45, 135, 138, 98, false, 0, 0, 0, 0, 0, 0, 0, 5, 30, 4, 0, 0, 14, 13, 0, 0, 3, 3, 2, 0, 0, 0, 0, 0, 0.6f)]
    [TestCase(14, "InteriorDark", 57951u, DayNightMode.AlwaysNight, 0, 0, 0, 0, 0, 0, false, 5, 5, 5, 20, 20, 20, false, 0, 0, 0, 0, 0, 0, 0, 9, 5, 0, 0, 0, 13, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0.6f)]
    [TestCase(15, "InteriorPoisoned", 57952u, DayNightMode.AlwaysNight, 0, 0, 0, 0, 0, 0, false, 70, 47, 47, 63, 130, 53, false, 0, 0, 0, 80, 106, 62, 0, 15, 10, 0, 0, 0, 11, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0.6f)]
    [TestCase(16, "InteriorEvil", 57953u, DayNightMode.AlwaysNight, 0, 0, 0, 0, 0, 0, false, 17, 23, 68, 151, 53, 53, false, 0, 0, 0, 94, 0, 0, 0, 7, 27, 16, 0, 0, 27, 13, 0, 0, 12, 12, 0, 0, 0, 0, 0, 0, 0.6f)]
    [TestCase(17, "InteriorCold", 57954u, DayNightMode.AlwaysNight, 0, 0, 0, 0, 0, 0, false, 32, 51, 51, 114, 160, 186, false, 0, 0, 0, 132, 175, 185, 0, 6, 14, 13, 0, 0, 14, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.6f)]
    [TestCase(18, "InteriorSewers", 57955u, DayNightMode.AlwaysNight, 0, 0, 0, 0, 0, 0, false, 39, 67, 35, 96, 66, 32, false, 0, 0, 0, 70, 57, 28, 0, 10, 1, 0, 0, 0, 3, 2, 2, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0.6f)]
    [TestCase(19, "InteriorMagical1", 57956u, DayNightMode.AlwaysNight, 0, 0, 0, 0, 0, 0, false, 52, 64, 84, 112, 68, 130, false, 0, 0, 0, 46, 40, 57, 0, 14, 22, 13, 0, 0, 22, 0, 0, 0, 11, 10, 0, 0, 0, 0, 0, 0, 0.6f)]
    [TestCase(20, "InteriorMagical2", 57957u, DayNightMode.AlwaysNight, 0, 0, 0, 0, 0, 0, false, 36, 40, 219, 255, 128, 0, false, 0, 0, 0, 0, 54, 108, 0, 3, 31, 0, 0, 0, 31, 0, 0, 0, 9, 8, 8, 0, 0, 0, 0, 0, 0.6f)]
    [TestCase(21, "InteriorTorchLit", 57959u, DayNightMode.AlwaysNight, 0, 0, 0, 0, 0, 0, false, 0, 0, 0, 0, 0, 0, false, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 1, 0, 0, 0, 3, 3, 2, 2, 0, 0, 0, 0, 0.6f)]
    [TestCase(22, "ResetToBlack", 57996u, DayNightMode.EnableDayNightCycle, 0, 0, 0, 0, 0, 0, false, 0, 0, 0, 0, 0, 0, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.5f)]
    [TestCase(23, "ExteriorRedFog", 57997u, DayNightMode.AlwaysNight, 0, 0, 0, 0, 0, 0, false, 0, 0, 0, 0, 0, 0, false, 0, 0, 0, 180, 20, 20, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.0f)]
    [TestCase(24, "ExteriorMuggy", 111465u, DayNightMode.EnableDayNightCycle, 146, 148, 129, 168, 166, 147, true, 60, 58, 50, 124, 137, 139, false, 125, 125, 119, 0, 0, 0, 0, 15, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 20, 0, 0.5f)]
    [TestCase(25, "InteriorBarrows", 111466u, DayNightMode.AlwaysNight, 0, 0, 0, 0, 0, 0, false, 120, 103, 71, 135, 138, 98, false, 0, 0, 0, 0, 0, 0, 0, 8, 30, 4, 0, 0, 14, 13, 0, 0, 3, 3, 2, 0, 0, 0, 0, 0, 0.6f)]
    public void EnvironmentTableReturnsValidData(int rowIndex, string label, uint strRef, DayNightMode dayNight, byte lightAmbRed,
      byte lightAmbGreen, byte lightAmbBlue, byte lightDiffRed, byte lightDiffGreen, byte lightDiffBlue, bool lightShadows,
      byte darkAmbRed, byte darkAmbGreen, byte darkAmbBlue, byte darkDiffRed, byte darkDiffGreen, byte darkDiffBlue, bool darkShadows,
      byte lightFogRed, byte lightFogGreen, byte lightFogBlue, byte darkFogRed, byte darkFogGreen, byte darkFogBlue, byte lightFog, byte darkFog,
      int main1Color1, int main1Color2, int main1Color3, int main1Color4, int main2Color1, int main2Color2, int main2Color3, int main2Color4,
      int secondaryColor1, int secondaryColor2, int secondaryColor3, int secondaryColor4, byte wind, byte snow, byte rain, byte lightning, float shadowAlpha)
    {
      TwoDimArray<EnvironmentPreset> table = NwGameTables.EnvironmentPresetTable;
      EnvironmentPreset row = table.GetRow(rowIndex);

      Assert.That(row.RowIndex, Is.EqualTo(rowIndex));
      Assert.That(row.Label, Is.EqualTo(label));
      Assert.That(row.StrRef, Is.EqualTo(new StrRef(strRef)));
      Assert.That(row.DayNightMode, Is.EqualTo(dayNight));
      Assert.That(row.SunAmbientColor, Is.EqualTo(new Color(lightAmbRed, lightAmbGreen, lightAmbBlue)));
      Assert.That(row.SunDiffuseColor, Is.EqualTo(new Color(lightDiffRed, lightDiffGreen, lightDiffBlue)));
      Assert.That(row.SunShadows, Is.EqualTo(lightShadows));
      Assert.That(row.MoonAmbientColor, Is.EqualTo(new Color(darkAmbRed, darkAmbGreen, darkAmbBlue)));
      Assert.That(row.MoonDiffuseColor, Is.EqualTo(new Color(darkDiffRed, darkDiffGreen, darkDiffBlue)));
      Assert.That(row.MoonShadows, Is.EqualTo(darkShadows));
      Assert.That(row.SunFogColor, Is.EqualTo(new Color(lightFogRed, lightFogGreen, lightFogBlue)));
      Assert.That(row.MoonFogColor, Is.EqualTo(new Color(darkFogRed, darkFogGreen, darkFogBlue)));
      Assert.That(row.SunFogAmount, Is.EqualTo(lightFog));
      Assert.That(row.MoonFogAmount, Is.EqualTo(darkFog));
      Assert.That(row.Main1Color1, Is.EqualTo(main1Color1));
      Assert.That(row.Main1Color2, Is.EqualTo(main1Color2));
      Assert.That(row.Main1Color3, Is.EqualTo(main1Color3));
      Assert.That(row.Main1Color4, Is.EqualTo(main1Color4));
      Assert.That(row.Main2Color1, Is.EqualTo(main2Color1));
      Assert.That(row.Main2Color2, Is.EqualTo(main2Color2));
      Assert.That(row.Main2Color3, Is.EqualTo(main2Color3));
      Assert.That(row.Main2Color4, Is.EqualTo(main2Color4));
      Assert.That(row.SecondaryColor1, Is.EqualTo(secondaryColor1));
      Assert.That(row.SecondaryColor2, Is.EqualTo(secondaryColor2));
      Assert.That(row.SecondaryColor3, Is.EqualTo(secondaryColor3));
      Assert.That(row.SecondaryColor4, Is.EqualTo(secondaryColor4));
      Assert.That(row.WindPower, Is.EqualTo(wind));
      Assert.That(row.SnowChance, Is.EqualTo(snow));
      Assert.That(row.RainChance, Is.EqualTo(rain));
      Assert.That(row.LightningChance, Is.EqualTo(lightning));
      Assert.That(row.ShadowAlpha, Is.EqualTo(shadowAlpha));
    }
  }
}
