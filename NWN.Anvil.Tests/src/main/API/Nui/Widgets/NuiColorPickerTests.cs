using Anvil.API;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  public sealed class NuiColorPickerTests
  {
    [Test]
    public void SerializeNuiColorPickerReturnsValidJson()
    {
      NuiColorPicker element = new NuiColorPicker(new NuiBind<Color>("selected_color"));

      Assert.That(JsonUtility.ToJson(element), Is.EqualTo("""{"value":{"bind":"selected_color"},"type":"color_picker"}"""));
      Assert.That(JsonUtility.ToJson<NuiElement>(element), Is.EqualTo("""{"value":{"bind":"selected_color"},"type":"color_picker"}"""));
    }
  }
}
