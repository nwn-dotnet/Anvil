using System.Text.Json;
using Anvil.API;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  public sealed class NuiSliderFloatTests
  {
    [Test]
    public void SerializeNuiSliderFloatReturnsValidJson()
    {
      NuiSliderFloat element = new NuiSliderFloat(new NuiBind<float>("value"), 0f, 100f);

      Assert.That(JsonSerializer.Serialize(element), Is.EqualTo("""{"max":100,"min":0,"step":0.01,"type":"sliderf","value":{"bind":"value"}}"""));
      Assert.That(JsonSerializer.Serialize((NuiElement)element), Is.EqualTo("""{"max":100,"min":0,"step":0.01,"type":"sliderf","value":{"bind":"value"}}"""));
    }
  }
}
