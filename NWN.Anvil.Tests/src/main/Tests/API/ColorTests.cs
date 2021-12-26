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
      Assert.IsFalse(colorToken.Contains('\0'), "color token contains a null character");
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
