using System.Text.Json;
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

      Assert.That(JsonSerializer.Serialize(element), Is.EqualTo("""{"label":"test_label","value":{"bind":"selected"},"type":"button_select"}"""));
      Assert.That(JsonSerializer.Serialize((NuiElement)element), Is.EqualTo("""{"label":"test_label","value":{"bind":"selected"},"type":"button_select"}"""));
    }
  }
}
