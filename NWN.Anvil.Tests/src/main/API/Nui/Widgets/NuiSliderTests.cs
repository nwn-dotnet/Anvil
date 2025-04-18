using System.Text.Json;
using Anvil.API;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  public sealed class NuiSliderTests
  {
    [Test]
    public void SerializeNuiSliderReturnsValidJson()
    {
      NuiSlider element = new NuiSlider(new NuiBind<int>("value"), 0, 10)
      {
        Step = 2,
      };

      Assert.That(JsonSerializer.Serialize(element), Is.EqualTo("""{"max":10,"min":0,"step":2,"type":"slider","value":{"bind":"value"}}"""));
      Assert.That(JsonSerializer.Serialize((NuiElement)element), Is.EqualTo("""{"max":10,"min":0,"step":2,"type":"slider","value":{"bind":"value"}}"""));
    }
  }
}
