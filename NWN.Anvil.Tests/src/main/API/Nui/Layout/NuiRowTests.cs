using Anvil.API;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  [TestFixture(Category = "API.Nui")]
  public sealed class NuiRowTests
  {
    [Test(Description = "Serializing a NuiRow creates a valid JSON structure.")]
    public void SerializeNuiRowReturnsValidJsonStructure()
    {
      NuiRow nuiRow = new NuiRow
      {
        Id = "test_row",
        Aspect = 1.5f,
        Enabled = new NuiBind<bool>("enabled_bind"),
        Height = 10f,
        Margin = 2f,
        Padding = 3f,
        ForegroundColor = new NuiBind<Color>("color_bind"),
        Tooltip = "test_tooltip",
        Width = 100f,
        Visible = false,
        Children =
        [
          new NuiLabel("test"),
          new NuiRow(),
        ],
      };

      Assert.That(JsonUtility.ToJson(nuiRow), Is.EqualTo("""{"type":"row","children":[{"text_halign":1,"value":"test","type":"label","text_valign":1},{"type":"row","children":[]}],"aspect":1.5,"enabled":{"bind":"enabled_bind"},"foreground_color":{"bind":"color_bind"},"height":10,"id":"test_row","margin":2,"padding":3,"tooltip":"test_tooltip","visible":false,"width":100}"""));
    }
  }
}
