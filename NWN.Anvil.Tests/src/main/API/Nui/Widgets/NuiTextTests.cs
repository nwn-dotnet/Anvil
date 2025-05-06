using Anvil.API;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  [TestFixture]
  public sealed class NuiTextTests
  {
    [Test(Description = "Serializing a NuiText creates a valid JSON structure.")]
    public void SerializeNuiTextReturnsValidJson()
    {
      NuiText element = new NuiText("Some Text")
      {
        Enabled = false,
        Border = false,
        Scrollbars = NuiScrollbars.Y,
      };

      Assert.That(JsonUtility.ToJson(element), Is.EqualTo("""{"value":"Some Text","border":false,"scrollbars":2,"type":"text","enabled":false}"""));
      Assert.That(JsonUtility.ToJson<NuiElement>(element), Is.EqualTo("""{"value":"Some Text","border":false,"scrollbars":2,"type":"text","enabled":false}"""));
    }
  }
}
