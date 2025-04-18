using System.Collections.Generic;
using Anvil.API;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  public sealed class NuiChartTests
  {
    [Test]
    public void SerializeNuiChartReturnsValidJson()
    {
      NuiChart element = new NuiChart
      {
        ChartSlots =
        [
          new NuiChartSlot(NuiChartType.Column, "slot1", ColorConstants.Maroon, new List<float>((List<float>)[1f, 2f, 3f])),
          new NuiChartSlot(NuiChartType.Column, "slot2", ColorConstants.Navy, new List<float>((List<float>)[0f, -1f, -2f])),
        ],
      };

      Assert.That(JsonUtility.ToJson(element), Is.EqualTo("""{"value":[{"type":1,"color":{"a":255,"b":0,"g":0,"r":128},"data":[1,2,3],"legend":"slot1"},{"type":1,"color":{"a":255,"b":128,"g":0,"r":0},"data":[0,-1,-2],"legend":"slot2"}],"type":"chart"}"""));
      Assert.That(JsonUtility.ToJson<NuiElement>(element), Is.EqualTo("""{"value":[{"type":1,"color":{"a":255,"b":0,"g":0,"r":128},"data":[1,2,3],"legend":"slot1"},{"type":1,"color":{"a":255,"b":128,"g":0,"r":0},"data":[0,-1,-2],"legend":"slot2"}],"type":"chart"}"""));
    }
  }
}
