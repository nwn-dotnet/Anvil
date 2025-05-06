using Anvil.API;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  public sealed class NuiButtonSelectTests
  {
    [Test]
    public void SerializeNuiButtonSelectReturnsValidJson()
    {
      NuiButtonSelect element = new NuiButtonSelect("test_label", new NuiBind<bool>("selected"));

      Assert.That(JsonUtility.ToJson(element), Is.EqualTo("""{"label":"test_label","value":{"bind":"selected"},"type":"button_select"}"""));
      Assert.That(JsonUtility.ToJson<NuiElement>(element), Is.EqualTo("""{"label":"test_label","value":{"bind":"selected"},"type":"button_select"}"""));
    }
  }
}
