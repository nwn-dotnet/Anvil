using System.Collections.Generic;
using Anvil.API;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  [TestFixture(Category = "API")]
  public sealed class ColorTests
  {
    [Test]
    [Description("Converts a color into a color token, then checks the resulting string for control characters.")]
    [TestCaseSource(nameof(ColorTestCases))]
    public void CreateColorTokenContainsNoNullTerminators(Color color)
    {
      string colorToken = color.ToColorToken();
      Assert.That(colorToken, Does.Not.Contain('\0'), "color token contains a null character");
    }

    [Test]
    [Description("Converting a color structure to a packed rgba value retains the correct value.")]
    [TestCaseSource(nameof(ColorTestCases))]
    public void ConvertColorToRgbaRetainsValue(Color color)
    {
      int rgba = color.ToRGBA();
      Color fromRgba = Color.FromRGBA(rgba);

      Assert.That(fromRgba, Is.EqualTo(color));
    }

    [Test]
    [Description("Converting a color structure to a packed unsigned rgba value retains the correct value.")]
    [TestCaseSource(nameof(ColorTestCases))]
    public void ConvertColorToUnsignedRgbaRetainsValue(Color color)
    {
      uint rgba = color.ToUnsignedRGBA();
      Color fromRgba = Color.FromRGBA(rgba);

      Assert.That(fromRgba, Is.EqualTo(color));
    }

    [Test]
    [Description("Converting a packed rgba value returns the correct value.")]
    [TestCase(0xFF0000FFu, 0xFF, 0, 0, 0xFF)]
    [TestCase(0x6E7882FFu, 0x6E, 0x78, 0x82, 0xFF)]
    [TestCase(0x6E141E10u, 0x6E, 0x14, 0x1E, 0x10)]
    public void ParseRGBAReturnsCorrectColor(uint packedColor, byte expectedRed, byte expectedGreen, byte expectedBlue, byte expectedAlpha)
    {
      Assert.That(Color.FromRGBA(packedColor), Is.EqualTo(new Color(expectedRed, expectedGreen, expectedBlue, expectedAlpha)));
    }

    [Test]
    [Description("Converting a rgba hex string returns the correct value.")]
    [TestCase("0xFF0000FF", 0xFF, 0, 0, 0xFF)]
    [TestCase("0x6E7882FF", 0x6E, 0x78, 0x82, 0xFF)]
    [TestCase("0x6E141E10", 0x6E, 0x14, 0x1E, 0x10)]
    [TestCase("0xff0000ff", 0xFF, 0, 0, 0xFF)]
    [TestCase("0x6e7882ff", 0x6E, 0x78, 0x82, 0xFF)]
    [TestCase("0x6e141e10", 0x6E, 0x14, 0x1E, 0x10)]
    [TestCase("FF0000FF", 0xFF, 0, 0, 0xFF)]
    [TestCase("6E7882FF", 0x6E, 0x78, 0x82, 0xFF)]
    [TestCase("6E141E10", 0x6E, 0x14, 0x1E, 0x10)]
    [TestCase("ff0000ff", 0xFF, 0, 0, 0xFF)]
    [TestCase("6e7882ff", 0x6E, 0x78, 0x82, 0xFF)]
    [TestCase("6e141e10", 0x6E, 0x14, 0x1E, 0x10)]
    [TestCase("#ff0000ff", 0xFF, 0, 0, 0xFF)]
    [TestCase("#6e7882ff", 0x6E, 0x78, 0x82, 0xFF)]
    [TestCase("#6e141e10", 0x6E, 0x14, 0x1E, 0x10)]
    public void ParseRGBAHexStringReturnsCorrectColor(string hexString, byte expectedRed, byte expectedGreen, byte expectedBlue, byte expectedAlpha)
    {
      Assert.That(Color.FromRGBA(hexString), Is.EqualTo(new Color(expectedRed, expectedGreen, expectedBlue, expectedAlpha)));
    }

    private static IEnumerable<Color> ColorTestCases()
    {
      yield return ColorConstants.Black;
      yield return ColorConstants.Blue;
      yield return ColorConstants.Brown;
      yield return ColorConstants.Cyan;
      yield return ColorConstants.Gray;
      yield return ColorConstants.Green;
      yield return ColorConstants.Lime;
      yield return ColorConstants.Magenta;
      yield return ColorConstants.Maroon;
      yield return ColorConstants.Navy;
      yield return ColorConstants.Olive;
      yield return ColorConstants.Orange;
      yield return ColorConstants.Pink;
      yield return ColorConstants.Purple;
      yield return ColorConstants.Red;
      yield return ColorConstants.Rose;
      yield return ColorConstants.Silver;
      yield return ColorConstants.Teal;
      yield return ColorConstants.White;
      yield return ColorConstants.Yellow;
    }
  }
}
