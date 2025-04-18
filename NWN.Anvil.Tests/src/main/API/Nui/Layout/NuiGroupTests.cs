using Anvil.API;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  [TestFixture]
  public sealed class NuiGroupTests
  {
    [Test(Description = "Serializing a NuiGroup creates a valid JSON structure.")]
    public void SerializeNuiGroupReturnsValidJson()
    {
      NuiGroup layout = new NuiGroup
      {
        Id = "test_group",
        Aspect = 1.5f,
        Border = true,
        Enabled = new NuiBind<bool>("enabled_bind"),
        Height = 10f,
        Margin = 2f,
        Padding = 3f,
        ForegroundColor = new NuiBind<Color>("color_bind"),
        Scrollbars = NuiScrollbars.Both,
        Tooltip = "test_tooltip",
        Width = 100f,
        Visible = false,
        Layout = new NuiColumn
        {
          Children =
          [
            new NuiLabel("Test"),
          ],
        },
      };

      Assert.That(JsonUtility.ToJson(layout), Is.EqualTo("""{"border":true,"scrollbars":3,"type":"group","children":[{"type":"col","children":[{"text_halign":1,"value":"Test","type":"label","text_valign":1}]}],"aspect":1.5,"enabled":{"bind":"enabled_bind"},"foreground_color":{"bind":"color_bind"},"height":10,"id":"test_group","margin":2,"padding":3,"tooltip":"test_tooltip","visible":false,"width":100}"""));
      Assert.That(JsonUtility.ToJson<NuiLayout>(layout), Is.EqualTo("""{"border":true,"scrollbars":3,"type":"group","children":[{"type":"col","children":[{"text_halign":1,"value":"Test","type":"label","text_valign":1}]}],"aspect":1.5,"enabled":{"bind":"enabled_bind"},"foreground_color":{"bind":"color_bind"},"height":10,"id":"test_group","margin":2,"padding":3,"tooltip":"test_tooltip","visible":false,"width":100}"""));
    }
  }
}
