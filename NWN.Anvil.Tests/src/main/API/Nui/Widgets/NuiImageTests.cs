using Anvil.API;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  public sealed class NuiImageTests
  {
    [Test]
    public void SerializeNuiImageReturnsValidJson()
    {
      NuiImage element = new NuiImage("gui_chr_arrowbtn")
      {
        Width = 16,
        Height = 16,
        ImageAspect = NuiAspect.ExactScaled,
        HorizontalAlign = NuiHAlign.Right,
        VerticalAlign = NuiVAlign.Middle,
        Margin = 0.0f,
      };

      Assert.That(JsonUtility.ToJson(element), Is.EqualTo("""{"image_halign":2,"image_aspect":4,"image_region":null,"value":"gui_chr_arrowbtn","type":"image","image_valign":0,"height":16,"margin":0,"width":16}"""));
      Assert.That(JsonUtility.ToJson<NuiElement>(element), Is.EqualTo("""{"image_halign":2,"image_aspect":4,"image_region":null,"value":"gui_chr_arrowbtn","type":"image","image_valign":0,"height":16,"margin":0,"width":16}"""));
    }
  }
}
