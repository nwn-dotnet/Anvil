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

      Assert.That(JsonUtility.ToJson(element), Is.EqualTo("""{"max":100.0,"min":0.0,"step":0.01,"type":"sliderf","value":{"bind":"value"}}"""));
      Assert.That(JsonUtility.ToJson<NuiElement>(element), Is.EqualTo("""{"max":100.0,"min":0.0,"step":0.01,"type":"sliderf","value":{"bind":"value"}}"""));
    }
  }
}
