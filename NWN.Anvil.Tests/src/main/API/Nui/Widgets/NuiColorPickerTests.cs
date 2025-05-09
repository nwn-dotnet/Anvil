using System.Text.Json;
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

      Assert.That(JsonSerializer.Serialize(element), Is.EqualTo("""{"value":{"bind":"selected_color"},"type":"color_picker"}"""));
      Assert.That(JsonSerializer.Serialize((NuiElement)element), Is.EqualTo("""{"value":{"bind":"selected_color"},"type":"color_picker"}"""));
    }
  }
}
