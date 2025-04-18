using Anvil.API;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  [TestFixture]
  public sealed class NuiDrawListTests
  {
    [Test(Description = "Serializing a NuiDrawListArc creates a valid JSON structure.")]
    public void SerializeNuiDrawListArcReturnsValidJson()
    {
      NuiDrawListArc item = new NuiDrawListArc(ColorConstants.Pink, true, 1.0f, new NuiVector(1.0f, 2.0f), 2.0f, 90f, 170f)
      {
        Enabled = false,
      };

      Assert.That(JsonUtility.ToJson(item), Is.EqualTo("""{"amax":170.0,"amin":90.0,"c":{"x":1.0,"y":2.0},"radius":2.0,"type":3,"color":{"a":255,"b":170,"g":170,"r":255},"enabled":false,"fill":true,"line_thickness":1.0,"order":1,"render":0}"""));
      Assert.That(JsonUtility.ToJson<NuiDrawListItem>(item), Is.EqualTo("""{"amax":170.0,"amin":90.0,"c":{"x":1.0,"y":2.0},"radius":2.0,"type":3,"color":{"a":255,"b":170,"g":170,"r":255},"enabled":false,"fill":true,"line_thickness":1.0,"order":1,"render":0}"""));
    }

    [Test(Description = "Serializing a NuiDrawListCircle creates a valid JSON structure.")]
    public void SerializeNuiDrawListCircleReturnsValidJson()
    {
      NuiDrawListCircle item = new NuiDrawListCircle(ColorConstants.Pink, true, 1.0f, new NuiRect(1.0f, 2.0f, 3.0f, 4.0f))
      {
        Enabled = false,
      };

      Assert.That(JsonUtility.ToJson(item), Is.EqualTo("""{"rect":{"h":4.0,"w":3.0,"x":1.0,"y":2.0},"type":2,"color":{"a":255,"b":170,"g":170,"r":255},"enabled":false,"fill":true,"line_thickness":1.0,"order":1,"render":0}"""));
      Assert.That(JsonUtility.ToJson<NuiDrawListItem>(item), Is.EqualTo("""{"rect":{"h":4.0,"w":3.0,"x":1.0,"y":2.0},"type":2,"color":{"a":255,"b":170,"g":170,"r":255},"enabled":false,"fill":true,"line_thickness":1.0,"order":1,"render":0}"""));
    }

    [Test(Description = "Serializing a NuiDrawListCurve creates a valid JSON structure.")]
    public void SerializeNuiDrawListCurveReturnsValidJson()
    {
      NuiDrawListCurve item = new NuiDrawListCurve(ColorConstants.Pink, 1.0f, new NuiVector(10.0f, 5.0f), new NuiVector(6.0f, 2.0f), new NuiVector(9.5f, 3.0f), new NuiVector(22.0f, 11.3f))
      {
        Enabled = false,
      };

      Assert.That(JsonUtility.ToJson(item), Is.EqualTo("""{"ctrl0":{"x":9.5,"y":3.0},"ctrl1":{"x":22.0,"y":11.3},"a":{"x":10.0,"y":5.0},"b":{"x":6.0,"y":2.0},"type":1,"color":{"a":255,"b":170,"g":170,"r":255},"enabled":false,"fill":false,"line_thickness":1.0,"order":1,"render":0}"""));
      Assert.That(JsonUtility.ToJson<NuiDrawListItem>(item), Is.EqualTo("""{"ctrl0":{"x":9.5,"y":3.0},"ctrl1":{"x":22.0,"y":11.3},"a":{"x":10.0,"y":5.0},"b":{"x":6.0,"y":2.0},"type":1,"color":{"a":255,"b":170,"g":170,"r":255},"enabled":false,"fill":false,"line_thickness":1.0,"order":1,"render":0}"""));
    }

    [Test(Description = "Serializing a NuiDrawListImage creates a valid JSON structure.")]
    public void SerializeNuiDrawListImageReturnsValidJson()
    {
      NuiDrawListImage item = new NuiDrawListImage("test_img", new NuiRect(1.0f, 2.0f, 3.0f, 4.0f))
      {
        Enabled = false,
      };

      Assert.That(JsonUtility.ToJson(item), Is.EqualTo("""{"image_aspect":3,"image_halign":1,"rect":{"h":4.0,"w":3.0,"x":1.0,"y":2.0},"image":"test_img","type":5,"image_valign":1,"color":null,"enabled":false,"fill":null,"line_thickness":null,"order":1,"render":0}"""));
      Assert.That(JsonUtility.ToJson<NuiDrawListItem>(item), Is.EqualTo("""{"image_aspect":3,"image_halign":1,"rect":{"h":4.0,"w":3.0,"x":1.0,"y":2.0},"image":"test_img","type":5,"image_valign":1,"color":null,"enabled":false,"fill":null,"line_thickness":null,"order":1,"render":0}"""));
    }

    [Test(Description = "Serializing a NuiDrawListPolyLine creates a valid JSON structure.")]
    public void SerializeNuiDrawListPolyLineReturnsValidJson()
    {
      NuiDrawListPolyLine item = new NuiDrawListPolyLine(ColorConstants.Pink, true, 2.0f, [2.0f, 4.0f, 6.0f, 11.0f])
      {
        Enabled = false,
      };

      Assert.That(JsonUtility.ToJson(item), Is.EqualTo("""{"points":[2.0,4.0,6.0,11.0],"type":0,"color":{"a":255,"b":170,"g":170,"r":255},"enabled":false,"fill":true,"line_thickness":2.0,"order":1,"render":0}"""));
      Assert.That(JsonUtility.ToJson<NuiDrawListItem>(item), Is.EqualTo("""{"points":[2.0,4.0,6.0,11.0],"type":0,"color":{"a":255,"b":170,"g":170,"r":255},"enabled":false,"fill":true,"line_thickness":2.0,"order":1,"render":0}"""));
    }

    [Test(Description = "Serializing a NuiDrawListText creates a valid JSON structure.")]
    public void SerializeNuiDrawListTextReturnsValidJson()
    {
      NuiDrawListText item = new NuiDrawListText(ColorConstants.Pink, new NuiRect(5.0f, 6.0f, 7.0f, 8.0f), "Test string")
      {
        Enabled = false,
      };

      Assert.That(JsonUtility.ToJson(item), Is.EqualTo("""{"rect":{"h":8.0,"w":7.0,"x":5.0,"y":6.0},"text":"Test string","type":4,"color":{"a":255,"b":170,"g":170,"r":255},"enabled":false,"fill":null,"line_thickness":null,"order":1,"render":0}"""));
      Assert.That(JsonUtility.ToJson<NuiDrawListItem>(item), Is.EqualTo("""{"rect":{"h":8.0,"w":7.0,"x":5.0,"y":6.0},"text":"Test string","type":4,"color":{"a":255,"b":170,"g":170,"r":255},"enabled":false,"fill":null,"line_thickness":null,"order":1,"render":0}"""));
    }
  }
}
