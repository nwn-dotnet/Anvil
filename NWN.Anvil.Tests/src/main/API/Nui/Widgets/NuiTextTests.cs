using Anvil.API;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  [TestFixture(Category = "API.Nui")]
  public sealed class NuiTextTests
  {
    [Test(Description = "Serializing a NuiText creates a valid JSON structure.")]
    public void SerializeNuiDrawListArcReturnsValidJsonStructure()
    {
      NuiText nuiText = new NuiText("Some Text")
      {
        Enabled = false,
        Border = false,
        Scrollbars = NuiScrollbars.Y,
      };

      Assert.That(JsonUtility.ToJson(nuiText), Is.EqualTo(@"{""value"":""Some Text"",""border"":false,""scrollbars"":2,""type"":""text"",""enabled"":false}"));
    }
  }
}
