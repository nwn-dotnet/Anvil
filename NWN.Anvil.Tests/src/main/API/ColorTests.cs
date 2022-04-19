using System.Collections.Generic;
using Anvil.API;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  [TestFixture(Category = "API")]
  public sealed class ColorTests
  {
    [Test]
    [Description("Converting a color structure to a packed rgba value retains the correct color values.")]
    [TestCaseSource(nameof(ColorTestCases))]
    public void ConvertColorToRgbaRetainsColorValues(Color color)
    {
      int rgba = color.ToRGBA();
      Color fromRgba = Color.FromRGBA(rgba);

      Assert.That(fromRgba, Is.EqualTo(color));
    }

    [Test]
    [Description("Converting a color structure to a packed unsigned rgba value retains the correct color values.")]
    [TestCaseSource(nameof(ColorTestCases))]
    public void ConvertColorToUnsignedRgbaRetainsValue(Color color)
    {
      uint rgba = color.ToUnsignedRGBA();
      Color fromRgba = Color.FromRGBA(rgba);

      Assert.That(fromRgba, Is.EqualTo(color));
    }

    [Test]
    [Description("Converts a color into a color token, then checks the resulting string for control characters.")]
    [TestCaseSource(nameof(ColorTestCases))]
    public void CreateColorTokenContainsNoNullTerminators(Color color)
    {
      string colorToken = color.ToColorToken();
      Assert.That(colorToken, Does.Not.Contain('\0'), "color token contains a null character");
    }

    [Test]
    [Description("Deserializing a NUI color json structure creates the correct color.")]
    [TestCase(@"{""r"":255,""g"":0,""b"":0,""a"":0}", 255, 0, 0, 0)]
    [TestCase(@"{""r"":255,""g"":100,""b"":0,""a"":0}", 255, 100, 0, 0)]
    [TestCase(@"{""r"":255,""g"":100,""b"":10,""a"":30}", 255, 100, 10, 30)]
    public void DeserializeColorCreatesCorrectColor(string json, byte expectedRed, byte expectedGreen, byte expectedBlue, byte expectedAlpha)
    {
      Color color = JsonConvert.DeserializeObject<Color>(json);
      Assert.That(color, Is.EqualTo(new Color(expectedRed, expectedGreen, expectedBlue, expectedAlpha)));
    }

    [Test]
    [Description("Identical colors are considered equal.")]
    [TestCaseSource(nameof(ColorTestCases))]
    public void IdenticalColorsAreEqual(Color color)
    {
      Color color2 = new Color(color.Red, color.Green, color.Blue, color.Alpha);
      Assert.AreEqual(color, color2);
    }

    [Test]
    [Description("Converting a rgba hex string returns the correct color.")]
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

    [Test]
    [Description("Converting a packed rgba value returns the correct color.")]
    [TestCase(0xFF0000FFu, 0xFF, 0, 0, 0xFF)]
    [TestCase(0x6E7882FFu, 0x6E, 0x78, 0x82, 0xFF)]
    [TestCase(0x6E141E10u, 0x6E, 0x14, 0x1E, 0x10)]
    public void ParseRGBAReturnsCorrectColor(uint packedColor, byte expectedRed, byte expectedGreen, byte expectedBlue, byte expectedAlpha)
    {
      Assert.That(Color.FromRGBA(packedColor), Is.EqualTo(new Color(expectedRed, expectedGreen, expectedBlue, expectedAlpha)));
    }

    [Test]
    [Description("Serializing a color structure to json retains the correct color values.")]
    [TestCaseSource(nameof(ColorTestCases))]
    public void SerializeColorRetainsColorValues(Color color)
    {
      string serializedColor = JsonConvert.SerializeObject(color);
      Color deserializedColor = JsonConvert.DeserializeObject<Color>(serializedColor);

      Assert.That(deserializedColor, Is.EqualTo(color));
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
