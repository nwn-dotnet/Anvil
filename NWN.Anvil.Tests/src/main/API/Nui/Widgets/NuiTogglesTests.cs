using Anvil.API;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  public sealed class NuiTogglesTests
  {
    [Test]
    public void SerializeNuiTogglesReturnsValidJson()
    {
      NuiToggles element = new NuiToggles(NuiDirection.Vertical, ["Tab 1", "Tab 2", "Tab 3"]);

      Assert.That(JsonUtility.ToJson(element), Is.EqualTo("""{"type":"tabbar","direction":1,"elements":["Tab 1","Tab 2","Tab 3"]}"""));
      Assert.That(JsonUtility.ToJson<NuiElement>(element), Is.EqualTo("""{"type":"tabbar","direction":1,"elements":["Tab 1","Tab 2","Tab 3"]}"""));
    }
  }
}
