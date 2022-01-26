using Anvil.API;
using NUnit.Framework;

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
  }
}
