using Anvil.API;
using NUnit.Framework;
using NWN.Core;

namespace Anvil.Tests.API
{
  [TestFixture(Category = "API.Object")]
  public sealed class NwAreaTests
  {
    [Test(Description = "Applying an environment preset to an area correctly updates area settings.")]
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    [TestCase(4)]
    [TestCase(5)]
    [TestCase(6)]
    [TestCase(7)]
    [TestCase(8)]
    [TestCase(9)]
    [TestCase(10)]
    [TestCase(11)]
    [TestCase(12)]
    [TestCase(13)]
    [TestCase(14)]
    [TestCase(15)]
    [TestCase(16)]
    [TestCase(17)]
    [TestCase(18)]
    [TestCase(19)]
    [TestCase(20)]
    [TestCase(21)]
    [TestCase(22)]
    [TestCase(23)]
    [TestCase(24)]
    [TestCase(25)]
    public void ApplyEnvironmentPresetIsApplied(int presetRowIndex)
    {
      EnvironmentPreset preset = NwGameTables.EnvironmentPresetTable.GetRow(presetRowIndex);

      NwArea area = NwModule.Instance.StartingLocation.Area;
      area.ApplyEnvironmentPreset(preset);

      Assert.That(area.DayNightMode, Is.EqualTo(preset.DayNightMode));
      Assert.That(area.SunAmbientColor, Is.EqualTo(preset.SunAmbientColor));
      Assert.That(area.SunDiffuseColor, Is.EqualTo(preset.SunDiffuseColor));
      Assert.That(area.SunFogColor, Is.EqualTo(preset.SunFogColor));
      Assert.That(area.SunFogAmount, Is.EqualTo(preset.SunFogAmount));
      Assert.That(area.SunShadows, Is.EqualTo(preset.SunShadows));
      Assert.That(area.MoonAmbientColor, Is.EqualTo(preset.MoonAmbientColor));
      Assert.That(area.MoonDiffuseColor, Is.EqualTo(preset.MoonDiffuseColor));
      Assert.That(area.MoonFogColor, Is.EqualTo(preset.MoonFogColor));
      Assert.That(area.MoonFogAmount, Is.EqualTo(preset.MoonFogAmount));
      Assert.That(area.MoonShadows, Is.EqualTo(preset.MoonShadows));
      Assert.That(area.WindPower, Is.EqualTo(preset.WindPower));
      Assert.That(area.SnowChance, Is.EqualTo(preset.SnowChance));
      Assert.That(area.RainChance, Is.EqualTo(preset.RainChance));
      Assert.That(area.LightningChance, Is.EqualTo(preset.LightningChance));
    }

    [Test(Description = "Modifying an area flag correctly updates the area.")]
    [TestCase(AreaFlags.Interior)]
    [TestCase(AreaFlags.Natural)]
    [TestCase(AreaFlags.UnderGround)]
    public void ChangeAreaFlagsUpdatesAreaFlags(AreaFlags flag)
    {
      NwArea area = NwModule.Instance.StartingLocation.Area;
      area.AreaFlags |= flag;
      Assert.That(area.AreaFlags.HasFlag(flag), Is.EqualTo(true));
      area.AreaFlags &= ~flag;
      Assert.That(area.AreaFlags.HasFlag(flag), Is.EqualTo(false));
    }

    [Test(Description = "Changing the IsInterior value correctly updates the area flag.")]
    public void ChangeAreaInteriorUpdatesAreaFlags()
    {
      NwArea area = NwModule.Instance.StartingLocation.Area;
      area.IsInterior = true;
      area.IsAboveGround = true; // NWScript.GetIsAreaInterior() always returns true if this flag is set to false.

      Assert.That(area.IsInterior, Is.EqualTo(true));
      Assert.That(area.IsExterior, Is.EqualTo(false));
      Assert.That(NWScript.GetIsAreaInterior(area).ToBool(), Is.EqualTo(true));
      Assert.That(area.AreaFlags.HasFlag(AreaFlags.Interior), Is.EqualTo(true));

      area.IsInterior = false;
      Assert.That(area.IsInterior, Is.EqualTo(false));
      Assert.That(area.IsExterior, Is.EqualTo(true));
      Assert.That(NWScript.GetIsAreaInterior(area).ToBool(), Is.EqualTo(false));
      Assert.That(area.AreaFlags.HasFlag(AreaFlags.Interior), Is.EqualTo(false));
    }

    [Test(Description = "Changing the IsExterior value correctly updates the area flag.")]
    public void ChangeAreaExteriorUpdatesAreaFlags()
    {
      NwArea area = NwModule.Instance.StartingLocation.Area;
      area.IsExterior = true;
      area.IsAboveGround = true; // NWScript.GetIsAreaInterior() always returns true if this flag is set to false.

      Assert.That(area.IsExterior, Is.EqualTo(true));
      Assert.That(area.IsInterior, Is.EqualTo(false));
      Assert.That(NWScript.GetIsAreaInterior(area).ToBool(), Is.EqualTo(false));
      Assert.That(area.AreaFlags.HasFlag(AreaFlags.Interior), Is.EqualTo(false));

      area.IsExterior = false;
      Assert.That(area.IsExterior, Is.EqualTo(false));
      Assert.That(area.IsInterior, Is.EqualTo(true));
      Assert.That(NWScript.GetIsAreaInterior(area).ToBool(), Is.EqualTo(true));
      Assert.That(area.AreaFlags.HasFlag(AreaFlags.Interior), Is.EqualTo(true));
    }

    [Test(Description = "Changing the IsNatural value correctly updates the area flag.")]
    public void ChangeAreaNaturalUpdatesAreaFlags()
    {
      NwArea area = NwModule.Instance.StartingLocation.Area;

      area.IsNatural = true;
      Assert.That(area.IsNatural, Is.EqualTo(true));
      Assert.That(area.IsUrban, Is.EqualTo(false));
      Assert.That(NWScript.GetIsAreaNatural(area).ToBool(), Is.EqualTo(true));
      Assert.That(area.AreaFlags.HasFlag(AreaFlags.Natural), Is.EqualTo(true));

      area.IsNatural = false;
      Assert.That(area.IsNatural, Is.EqualTo(false));
      Assert.That(area.IsUrban, Is.EqualTo(true));
      Assert.That(NWScript.GetIsAreaNatural(area).ToBool(), Is.EqualTo(false));
      Assert.That(area.AreaFlags.HasFlag(AreaFlags.Natural), Is.EqualTo(false));
    }

    [Test(Description = "Changing the IsUrban value correctly updates the area flag.")]
    public void ChangeAreaUrbanUpdatesAreaFlags()
    {
      NwArea area = NwModule.Instance.StartingLocation.Area;

      area.IsUrban = true;
      Assert.That(area.IsUrban, Is.EqualTo(true));
      Assert.That(area.IsNatural, Is.EqualTo(false));
      Assert.That(NWScript.GetIsAreaNatural(area).ToBool(), Is.EqualTo(false));
      Assert.That(area.AreaFlags.HasFlag(AreaFlags.Natural), Is.EqualTo(false));

      area.IsUrban = false;
      Assert.That(area.IsUrban, Is.EqualTo(false));
      Assert.That(area.IsNatural, Is.EqualTo(true));
      Assert.That(NWScript.GetIsAreaNatural(area).ToBool(), Is.EqualTo(true));
      Assert.That(area.AreaFlags.HasFlag(AreaFlags.Natural), Is.EqualTo(true));
    }

    [Test(Description = "Changing the IsAboveGround value correctly updates the area flag.")]
    public void ChangeAreaAboveGroundUpdatesAreaFlags()
    {
      NwArea area = NwModule.Instance.StartingLocation.Area;

      area.IsAboveGround = true;
      Assert.That(area.IsAboveGround, Is.EqualTo(true));
      Assert.That(area.IsUnderGround, Is.EqualTo(false));
      Assert.That(NWScript.GetIsAreaAboveGround(area).ToBool(), Is.EqualTo(true));
      Assert.That(area.AreaFlags.HasFlag(AreaFlags.UnderGround), Is.EqualTo(false));

      area.IsAboveGround = false;
      Assert.That(area.IsAboveGround, Is.EqualTo(false));
      Assert.That(area.IsUnderGround, Is.EqualTo(true));
      Assert.That(NWScript.GetIsAreaAboveGround(area).ToBool(), Is.EqualTo(false));
      Assert.That(area.AreaFlags.HasFlag(AreaFlags.UnderGround), Is.EqualTo(true));
    }

    [Test(Description = "Changing the IsUnderGround value correctly updates the area flag.")]
    public void ChangeAreaUnderGroundUpdatesAreaFlags()
    {
      NwArea area = NwModule.Instance.StartingLocation.Area;

      area.IsUnderGround = true;
      Assert.That(area.IsUnderGround, Is.EqualTo(true));
      Assert.That(area.IsAboveGround, Is.EqualTo(false));
      Assert.That(NWScript.GetIsAreaAboveGround(area).ToBool(), Is.EqualTo(false));
      Assert.That(area.AreaFlags.HasFlag(AreaFlags.UnderGround), Is.EqualTo(true));

      area.IsUnderGround = false;
      Assert.That(area.IsUnderGround, Is.EqualTo(false));
      Assert.That(area.IsAboveGround, Is.EqualTo(true));
      Assert.That(NWScript.GetIsAreaAboveGround(area).ToBool(), Is.EqualTo(true));
      Assert.That(area.AreaFlags.HasFlag(AreaFlags.UnderGround), Is.EqualTo(false));
    }
  }
}
