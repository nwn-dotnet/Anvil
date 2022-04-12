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
      Assert.That(row.BodyBag?.RowIndex, Is.EqualTo(bodyBag));
      Assert.That(row.Targetable, Is.EqualTo(targetable));
    }

    [Test(Description = "Body Bag Table entries match the expected values.")]
    [TestCase(0, "default", 6671u, null)]
    [TestCase(1, "Potion", 66483u, 194)]
    [TestCase(2, "Scroll", 66484u, 195)]
    [TestCase(3, "Treasure", 66490u, 196)]
    [TestCase(4, "Body", 66492u, 198)]
    [TestCase(5, "Bones", 66493u, 199)]
    [TestCase(6, "Pouch", 66494u, 200)]
    public void BodyBagTableReturnsValidData(int rowIndex, string label, uint? nameStrRef, int? appearance)
    {
      TwoDimArray<BodyBagTableEntry> table = NwGameTables.BodyBagTable;
      BodyBagTableEntry row = table.GetRow(rowIndex);

      Assert.That(row.RowIndex, Is.EqualTo(rowIndex));
      Assert.That(row.Label, Is.EqualTo(label));
      Assert.That(row.Name?.Id, Is.EqualTo(nameStrRef));
      Assert.That(row.Appearance?.RowIndex, Is.EqualTo(appearance));
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

    [Test(Description = "Light Color Table entries match the expected values.")]
    [TestCase(0, 0.00f, 0.00f, 0.00f, "Black", 0.00f, 0.00f, 0.00f)]
    [TestCase(1, 0.60f, 0.60f, 0.60f, "DimWhite", 0.33f, 0.33f, 0.33f)]
    [TestCase(2, 1.20f, 1.20f, 1.20f, "White", 0.66f, 0.66f, 0.66f)]
    [TestCase(3, 2.00f, 2.00f, 2.00f, "BrightWhite", 1.00f, 1.00f, 1.00f)]
    [TestCase(4, 0.94f, 0.94f, 0.50f, "PaleDarkYellow", 0.59f, 0.56f, 0.37f)]
    [TestCase(5, 0.96f, 0.87f, 0.03f, "DarkYellow", 0.42f, 0.39f, 0.00f)]
    [TestCase(6, 1.90f, 1.90f, 1.00f, "PaleYellow", 0.94f, 0.94f, 0.77f)]
    [TestCase(7, 1.90f, 1.70f, 0.06f, "Yellow", 0.96f, 0.87f, 0.03f)]
    [TestCase(8, 0.72f, 0.87f, 0.56f, "PaleDarkGreen", 0.38f, 0.57f, 0.02f)]
    [TestCase(9, 0.06f, 0.93f, 0.21f, "DarkGreen", 0.20f, 0.41f, 0.09f)]
    [TestCase(10, 1.40f, 1.80f, 1.40f, "PaleGreen", 0.72f, 0.87f, 0.56f)]
    [TestCase(11, 0.12f, 1.85f, 0.42f, "Green", 0.06f, 0.93f, 0.21f)]
    [TestCase(12, 0.65f, 0.96f, 0.91f, "PaleDarkAqua", 0.41f, 0.58f, 0.52f)]
    [TestCase(13, 0.18f, 0.84f, 0.94f, "DarkAqua", 0.08f, 0.37f, 0.41f)]
    [TestCase(14, 1.30f, 1.92f, 1.82f, "PaleAqua", 0.82f, 0.96f, 0.91f)]
    [TestCase(15, 0.36f, 1.68f, 1.90f, "Aqua", 0.18f, 0.84f, 0.94f)]
    [TestCase(16, 0.70f, 0.70f, 1.20f, "PaleDarkBlue", 0.30f, 0.25f, 0.59f)]
    [TestCase(17, 0.40f, 0.40f, 0.95f, "DarkBlue", 0.00f, 0.02f, 0.42f)]
    [TestCase(18, 1.40f, 1.40f, 2.40f, "PaleBlue", 0.69f, 0.65f, 0.92f)]
    [TestCase(19, 0.80f, 0.80f, 1.90f, "Blue", 0.00f, 0.06f, 0.95f)]
    [TestCase(20, 0.92f, 0.65f, 0.92f, "PaleDarkPurple", 0.59f, 0.26f, 0.58f)]
    [TestCase(21, 0.76f, 0.25f, 0.95f, "DarkPurple", 0.33f, 0.00f, 0.42f)]
    [TestCase(22, 1.84f, 1.30f, 1.84f, "PalePurple", 0.92f, 0.65f, 0.92f)]
    [TestCase(23, 1.50f, 0.50f, 1.90f, "Purple", 0.76f, 0.00f, 0.95f)]
    [TestCase(24, 1.00f, 0.50f, 0.50f, "PaleDarkRed", 0.60f, 0.36f, 0.26f)]
    [TestCase(25, 1.00f, 0.15f, 0.10f, "DarkRed", 0.42f, 0.04f, 0.00f)]
    [TestCase(26, 2.00f, 1.20f, 1.20f, "PaleRed", 0.92f, 0.72f, 0.65f)]
    [TestCase(27, 2.00f, 0.50f, 0.20f, "Red", 0.95f, 0.08f, 0.00f)]
    [TestCase(28, 1.00f, 0.75f, 0.30f, "PaleDarkOrange", 0.59f, 0.45f, 0.26f)]
    [TestCase(29, 1.00f, 0.50f, 0.00f, "DarkOrange", 0.42f, 0.15f, 0.00f)]
    [TestCase(30, 1.85f, 1.20f, 0.60f, "PaleOrange", 0.92f, 0.80f, 0.65f)]
    [TestCase(31, 1.90f, 0.70f, 0.00f, "Orange", 0.95f, 0.35f, 0.00f)]
    public void LightColorTableReturnsValidData(int rowIndex, float red, float green, float blue, string label, float toolsetRed, float toolsetGreen, float toolsetBlue)
    {
      TwoDimArray<LightColorTableEntry> table = NwGameTables.LightColorTable;
      LightColorTableEntry row = table.GetRow(rowIndex);

      Assert.That(row.RowIndex, Is.EqualTo(rowIndex));
      Assert.That(row.Red, Is.EqualTo(red));
      Assert.That(row.Green, Is.EqualTo(green));
      Assert.That(row.Blue, Is.EqualTo(blue));
      Assert.That(row.Label, Is.EqualTo(label));
      Assert.That(row.ToolsetRed, Is.EqualTo(toolsetRed));
      Assert.That(row.ToolsetGreen, Is.EqualTo(toolsetGreen));
      Assert.That(row.ToolsetBlue, Is.EqualTo(toolsetBlue));
    }

    [Test(Description = "Placeable sound table entries return valid data")]
    [TestCase(0, "medium_wood", "wood", "as_dr_woodmedop1", "as_dr_woodmedcl1", "cb_bu_woodlrg", null, "as_dr_locked1")]
    [TestCase(1, "large_wood", "wood", "as_dr_woodlgop1", "as_dr_woodlgcl1", "cb_bu_woodlrg", null, "as_dr_locked1")]
    [TestCase(2, "very_large_wood", "wood", "as_dr_woodvlgop1", "as_dr_woodvlgcl1", "cb_bu_woodlrg", null, "as_dr_locked1")]
    [TestCase(3, "very_large_wood_sliding", "wood", "as_dr_woodvlgop2", "as_dr_woodvlgcl2", "cb_bu_woodlrg", null, "as_dr_locked1")]
    [TestCase(4, "medium_stone", "stone", "as_dr_stonmedop1", "as_dr_stonmedcl1", "cb_bu_stonelrg", null, "as_dr_locked2")]
    [TestCase(5, "large_stone_gears", "stone", "as_dr_stonlgop1", "as_dr_stonlgcl1", "cb_bu_stonelrg", null, "as_dr_locked2")]
    [TestCase(6, "very_large_stone_gears", "stone", "as_dr_stonvlgop1", "as_dr_stonvlgcl1", "cb_bu_stonelrg", null, "as_dr_locked2")]
    [TestCase(7, "medium_metal", "plate", "as_dr_metlmedop1", "as_dr_metlmedcl1", "cb_bu_metallrg", null, "as_dr_locked3")]
    [TestCase(8, "large_metal", "plate", "as_dr_metllgop1", "as_dr_metllgcl1", "cb_bu_metallrg", null, "as_dr_locked3")]
    [TestCase(9, "very_large_metal", "plate", "as_dr_metlvlgop1", "as_dr_metlvlgcl1", "cb_bu_metallrg", null, "as_dr_locked3")]
    [TestCase(10, "medium_metal_sliding", "plate", "as_dr_metlmedop2", "as_dr_metlmedcl2", "cb_bu_metallrg", null, "as_dr_locked3")]
    [TestCase(11, "medium_metal_portcullis", "plate", "as_dr_metlprtop1", "as_dr_metlprtcl1", "cb_bu_metallrg", null, "as_dr_locked3")]
    [TestCase(12, "crate_small", "wood", "as_sw_crateop1", "as_sw_cratecl1", "cb_bu_woodsml", null, "as_sw_genericlk1")]
    [TestCase(13, "crate_large", "wood", "as_sw_crateop1", "as_sw_cratecl1", "cb_bu_woodlrg", null, "as_sw_genericlk1")]
    [TestCase(14, "chest_small", "wood", "as_sw_chestop1", "as_sw_chestcl1", "cb_bu_woodsml", null, "as_sw_genericlk1")]
    [TestCase(15, "chest_large", "wood", "as_sw_chestop1", "as_sw_chestcl1", "cb_bu_woodlrg", null, "as_sw_genericlk1")]
    [TestCase(16, "drawer", "wood", "as_sw_drawerop1", "as_sw_drawercl1", "cb_bu_woodlrg", null, "as_sw_genericlk1")]
    [TestCase(17, "generic_wood_small", "wood", "as_sw_genericop1", "as_sw_genericcl1", "cb_bu_woodsml", null, "as_sw_genericlk1")]
    [TestCase(18, "generic_wood_large", "wood", "as_sw_genericop1", "as_sw_genericcl1", "cb_bu_woodlrg", null, "as_sw_genericlk1")]
    [TestCase(19, "generic_stone_small", "stone", "as_sw_genericop1", "as_sw_genericcl1", "cb_bu_stonesml", null, "as_sw_genericlk1")]
    [TestCase(20, "generic_stone_large", "stone", "as_sw_genericop1", "as_sw_genericcl1", "cb_bu_stonelrg", null, "as_sw_genericlk1")]
    [TestCase(21, "generic_metal_small", "plate", "as_sw_genericop1", "as_sw_genericcl1", "cb_bu_metalsml", null, "as_sw_genericlk1")]
    [TestCase(22, "generic_metal_large", "plate", "as_sw_genericop1", "as_sw_genericcl1", "cb_bu_metallrg", null, "as_sw_genericlk1")]
    [TestCase(23, "cloth_object_small", "leather", "as_sw_clothop1", "as_sw_clothcl1", "cb_bu_matersml", null, "as_sw_clothlk1")]
    [TestCase(24, "cloth_object_large", "leather", "as_sw_clothop1", "as_sw_clothcl1", "cb_bu_materlrg", null, "as_sw_clothlk1")]
    [TestCase(25, "stone_object_small", "stone", "as_sw_stoneop1", "as_sw_stonecl1", "cb_bu_stonesml", null, "as_sw_stonelk1")]
    [TestCase(26, "stone_object_large", "stone", "as_sw_stoneop1", "as_sw_stonecl1", "cb_bu_stonelrg", null, "as_sw_stonelk1")]
    [TestCase(27, "metal_object_small", "plate", "as_sw_metalop1", "as_sw_metalcl1", "cb_bu_metalsml", null, "as_sw_genericlk1")]
    [TestCase(28, "metal_object_large", "plate", "as_sw_metalop1", "as_sw_metalcl1", "cb_bu_metallrg", null, "as_sw_genericlk1")]
    [TestCase(29, "lever", "wood", null, null, "cb_bu_metalsml", "as_sw_lever1", "as_sw_genericlk1")]
    [TestCase(30, "wood_pressure_plate", "wood", null, null, "cb_bu_woodsml", "as_sw_woodplate1", "as_sw_genericlk1")]
    [TestCase(31, "stone_pressure_plate", "stone", null, null, "cb_bu_stonesml", "as_sw_stonplate1", "as_sw_genericlk1")]
    [TestCase(32, "stone_water", "stone", "fs_water_hard1", "fs_water_hard1", "cb_bu_stonelrg", null, "as_sw_stonelk1")]
    [TestCase(33, "corpse", "leather", "as_sw_clothop1", "as_sw_clothcl1", "cb_ht_chunk", null, "as_sw_clothlk1")]
    [TestCase(34, "NOTHING", null, null, null, null, null, null)]
    public void PlaceableSoundTableReturnsValidData(int rowIndex, string label, string armorType, string opened, string closed, string destroyed, string used, string locked)
    {
      TwoDimArray<PlaceableSoundTableEntry> table = NwGameTables.PlaceableSoundTable;
      PlaceableSoundTableEntry row = table.GetRow(rowIndex);

      Assert.That(row.RowIndex, Is.EqualTo(rowIndex));
      Assert.That(row.Label, Is.EqualTo(label));
      Assert.That(row.ArmorType, Is.EqualTo(armorType));
      Assert.That(row.Opened, Is.EqualTo(opened));
      Assert.That(row.Closed, Is.EqualTo(closed));
      Assert.That(row.Destroyed, Is.EqualTo(destroyed));
      Assert.That(row.Used, Is.EqualTo(used));
      Assert.That(row.Locked, Is.EqualTo(locked));
    }

    [Test(Description = "Placeable table entries return valid data")]
    [TestCase(0, "Armoire", 5645u, "PLC_A01", null, null, null, null, 13, ShadowSize.Medium, false, null, null, true)]
    [TestCase(28, "Portal", 5670u, "PLC_D07", null, null, null, null, null, ShadowSize.Medium, false, null, null, true)]
    [TestCase(57, "Brazier", 5699u, "PLC_I05", 6, 0.003989f, 0.010825f, 0.874821f, 21, ShadowSize.Medium, false, null, null, true)]
    [TestCase(118, "ImpledCrpse2", 5761u, "PLC_O10", null, null, null, null, 33, ShadowSize.Medium, false, "PLC_P04", null, true)]
    [TestCase(157, "InvisObj", 5800u, "PLC_U02", null, null, null, null, 34, ShadowSize.Medium, false, null, null, true)]
    [TestCase(191, "TreasureLrg", 63234u, "PLC_C09", null, null, null, null, 21, ShadowSize.Medium, false, null, "PLC_G_ENV", true)]
    [TestCase(819, "new:Ballista,Arbalet", 111342u, "pwc_sieg_003", null, null, null, null, 18, ShadowSize.Medium, false, null, null, false)]
    [TestCase(15600, "silm: Fog (50m), Low", null, "silm_fog_50_lo", null, 0f, 0f, 1.5f, null, ShadowSize.Medium, false, null, null, null)]
    [TestCase(15398, "Lantern - Elven (Ambient Light)", null, "ptm_lantern01", 28, 0.00399f, -.57095f, 1.84292f, 17, ShadowSize.Medium, false, null, null, true)]
    public void PlaceableTableReturnsValidData(int rowIndex, string label, uint? strRef, string modelName, int? lightColor, float? lightOffsetX, float? lightOffsetY, float? lightOffsetZ, int? soundAppType, ShadowSize? shadowSize, bool? bodyBag, string lowGore, string reflection, bool? staticAllowed)
    {
      TwoDimArray<PlaceableTableEntry> table = NwGameTables.PlaceableTable;
      PlaceableTableEntry row = table.GetRow(rowIndex);

      Assert.That(row.RowIndex, Is.EqualTo(rowIndex));
      Assert.That(row.Label, Is.EqualTo(label));
      Assert.That(row.StrRef?.Id, Is.EqualTo(strRef));
      Assert.That(row.ModelName, Is.EqualTo(modelName));
      Assert.That(row.LightColor?.RowIndex, Is.EqualTo(lightColor));
      Assert.That(row.LightOffset?.X, Is.EqualTo(lightOffsetX));
      Assert.That(row.LightOffset?.Y, Is.EqualTo(lightOffsetY));
      Assert.That(row.LightOffset?.Z, Is.EqualTo(lightOffsetZ));
      Assert.That(row.SoundType?.RowIndex, Is.EqualTo(soundAppType));
      Assert.That(row.ShadowSize, Is.EqualTo(shadowSize));
    }

    [Test(Description = "Visual effect table entries return valid data")]
    [TestCase(0, "VFX_DUR_BLUR", "D", false, null, null, null, null, null, null, null, "sdr_blur", 501, null, null, null, null, null, null, null, null, null, null, null, null, null, null, false)]
    [TestCase(1, "VFX_DUR_DARKNESS", "D", false, null, null, null, "vdr_darkness", null, null, null, "sdr_darkness", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, false)]
    [TestCase(2, "VFX_DUR_ENTANGLE", "D", false, null, null, null, "vdr_entangle", "vdr_entang01", "vdr_entang02", null, "sdr_entangle", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, false)]
    [TestCase(3, "VFX_DUR_FREEDOM_OF_MOVEMENT", "D", false, null, null, null, "vdr_freemove", "vdr_freemov01", "vdr_freemov02", null, "sdr_freemove", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, false)]
    [TestCase(4, "VFX_DUR_GLOBE_INVULVERABILITY", "D", false, null, null, null, "vdr_globemax", "vdr_globe01", "vdr_globe02", null, "sdr_globemax", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, false)]
    [TestCase(5, "VFX_DUR_BLACKOUT", "D", false, null, null, null, null, null, null, null, "sdr_blackout", 333, null, null, null, null, null, null, null, null, null, null, null, null, null, null, false)]
    [TestCase(6, "VFX_DUR_INVISIBILITY", "D", false, null, null, null, null, null, null, null, null, 400, null, null, null, null, null, null, null, null, null, null, null, null, null, null, false)]
    [TestCase(7, "VFX_DUR_MIND_AFFECTING_NEGATIVE", "D", false, "vdr_mindhit", null, null, null, null, null, null, "sdr_mindhit", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, false)]
    [TestCase(8, "VFX_DUR_MIND_AFFECTING_POSITIVE", "D", false, "vdr_mindhelp", null, null, null, null, null, null, "sdr_mindhelp", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, false)]
    public void VisualEffectTableReturnsValidData(int rowIndex, string label, string typeFd, bool orientWithGround, string impHeadConNode,
      string impImpactNode, string impRootSNode, string impRootMNode, string impRootLNode, string impRootHNode, int? progFxImpact,
      string soundImpact, int? progFxDuration, string soundDuration, int? progFxCessation, string soundCessation, string cesHeadConNode,
      string cesImpactNode, string cesRootSNode, string cesRootMNode, string cesRootLNode, string cesRootHNode, int? shakeType,
      float? shakeDelay, float? shakeDuration, string lowViolence, string lowQuality, bool? orientWithObject)
    {
      TwoDimArray<VisualEffectTableEntry> table = NwGameTables.VisualEffectTable;
      VisualEffectTableEntry row = table.GetRow(rowIndex);

      Assert.That(row.RowIndex, Is.EqualTo(rowIndex));
      Assert.That(row.Label, Is.EqualTo(label));
      Assert.That(row.TypeFd, Is.EqualTo(typeFd));
      Assert.That(row.OrientWithGround, Is.EqualTo(orientWithGround));
      Assert.That(row.ImpHeadConNode, Is.EqualTo(impHeadConNode));
      Assert.That(row.ImpImpactNode, Is.EqualTo(impImpactNode));
      Assert.That(row.ImpRootSmallNode, Is.EqualTo(impRootSNode));
      Assert.That(row.ImpRootMediumNode, Is.EqualTo(impRootMNode));
      Assert.That(row.ImpRootLargeNode, Is.EqualTo(impRootLNode));
      Assert.That(row.ImpRootHugeNode, Is.EqualTo(impRootHNode));
      Assert.That(row.ProgFxImpact?.RowIndex, Is.EqualTo(progFxImpact));
      Assert.That(row.SoundImpact, Is.EqualTo(soundImpact));
      Assert.That(row.ProgFxDuration?.RowIndex, Is.EqualTo(progFxDuration));
      Assert.That(row.SoundDuration, Is.EqualTo(soundDuration));
      Assert.That(row.ProgFxCessastion, Is.EqualTo(progFxCessation));
      Assert.That(row.SoundCessastion, Is.EqualTo(soundCessation));
      Assert.That(row.CesHeadConNode, Is.EqualTo(cesHeadConNode));
      Assert.That(row.CesImpactNode, Is.EqualTo(cesImpactNode));
      Assert.That(row.CesRootSmallNode, Is.EqualTo(cesRootSNode));
      Assert.That(row.CesRootMediumNode, Is.EqualTo(cesRootMNode));
      Assert.That(row.CesRootLargeNode, Is.EqualTo(cesRootLNode));
      Assert.That(row.CesRootHugeNode, Is.EqualTo(cesRootHNode));
      Assert.That((int?)row.ShakeType, Is.EqualTo(shakeType));
      Assert.That(row.ShakeDelay, Is.EqualTo(shakeDelay));
      Assert.That(row.ShakeDuration, Is.EqualTo(shakeDuration));
      Assert.That(row.LowViolenceVariant, Is.EqualTo(lowViolence));
      Assert.That(row.LowQualityVariant, Is.EqualTo(lowQuality));
      Assert.That(row.OrientWithObject, Is.EqualTo(orientWithObject));
    }

    [Test(Description = "Programmed effect table entries return valid data")]
    public void ProgrammedEffectTableReturnsValidData()
    {
      TwoDimArray<ProgrammedEffectTableEntry> table = NwGameTables.ProgrammedEffectTable;

      ProgrammedEffectTableEntry envMapRow = table.GetRow(100);
      Assert.That(envMapRow.RowIndex, Is.EqualTo(100));
      Assert.That(envMapRow.Label, Is.EqualTo("EnvMap00"));
      Assert.That(envMapRow.Type, Is.EqualTo(ProgFxType.EnvironmentMap));
      Assert.That(envMapRow.GetParamString(1), Is.EqualTo("vdu_envmap_000"));
      Assert.That(envMapRow.GetParamString(2), Is.EqualTo(null));
      Assert.That(envMapRow.GetParamString(3), Is.EqualTo(null));
      Assert.That(envMapRow.GetParamString(4), Is.EqualTo(null));
      Assert.That(envMapRow.GetParamString(5), Is.EqualTo(null));
      Assert.That(envMapRow.GetParamString(6), Is.EqualTo(null));
      Assert.That(envMapRow.GetParamString(7), Is.EqualTo(null));
      Assert.That(envMapRow.GetParamString(8), Is.EqualTo(null));

      ProgrammedEffectTableEntry selfIllumRow = table.GetRow(200);
      Assert.That(selfIllumRow.RowIndex, Is.EqualTo(200));
      Assert.That(selfIllumRow.Label, Is.EqualTo("SelfIllumRed"));
      Assert.That(selfIllumRow.Type, Is.EqualTo(ProgFxType.StaticGlow));
      Assert.That(selfIllumRow.GetParamFloat(1), Is.EqualTo(1.0f));
      Assert.That(selfIllumRow.GetParamFloat(2), Is.EqualTo(0.0f));
      Assert.That(selfIllumRow.GetParamFloat(3), Is.EqualTo(0.0f));
      Assert.That(selfIllumRow.GetParamFloat(4), Is.EqualTo(null));
      Assert.That(selfIllumRow.GetParamFloat(5), Is.EqualTo(null));
      Assert.That(selfIllumRow.GetParamFloat(6), Is.EqualTo(null));
      Assert.That(selfIllumRow.GetParamFloat(7), Is.EqualTo(null));
      Assert.That(selfIllumRow.GetParamFloat(8), Is.EqualTo(null));

      ProgrammedEffectTableEntry lightRow = table.GetRow(300);
      Assert.That(lightRow.RowIndex, Is.EqualTo(300));
      Assert.That(lightRow.Label, Is.EqualTo("LightWhite5m"));
      Assert.That(lightRow.Type, Is.EqualTo(ProgFxType.Light));
      Assert.That(lightRow.GetParamString(1), Is.EqualTo("White_5m"));
      Assert.That(lightRow.GetParamFloat(2), Is.EqualTo(1.0f));
      Assert.That(lightRow.GetParamInt(3), Is.EqualTo(0));
      Assert.That(lightRow.GetParamInt(4), Is.EqualTo(20));
      Assert.That(lightRow.GetParamInt(5), Is.EqualTo(0));
      Assert.That(lightRow.GetParamInt(6), Is.EqualTo(0));
      Assert.That(lightRow.GetParamString(7), Is.EqualTo("fx_light_clr"));
      Assert.That(lightRow.GetParamString(8), Is.EqualTo(null));
    }

    [TestCase(0, "UNINJURED", 6409)]
    [TestCase(1, "BARELY_INJURED", 6410)]
    [TestCase(2, "INJURED", 6411)]
    [TestCase(3, "HEAVILY_WOUNDED", 6412)]
    [TestCase(4, "NEAR_DEATH", 6413)]
    public void DamageLevelTableReturnsValidData(int rowIndex, string label, int? strRef)
    {
      TwoDimArray<DamageLevelEntry> table = NwGameTables.DamageLevelTable;
      DamageLevelEntry row = table.GetRow(rowIndex);

      Assert.That(row.Label, Is.EqualTo(label));
      Assert.That(row.StrRef?.Id, Is.EqualTo(strRef));
    }
  }
}
