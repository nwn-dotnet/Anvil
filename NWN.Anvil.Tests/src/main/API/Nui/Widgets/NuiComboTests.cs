using System.Collections.Generic;
using Anvil.API;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  public sealed class NuiComboTests
  {
    [Test]
    public void SerializeNuiComboReturnsValidJson()
    {
      NuiCombo element = new NuiCombo()
      {
        Entries = new List<NuiComboEntry>
        {
          new NuiComboEntry("Entry 1", 1),
          new NuiComboEntry("Entry 2", 2),
          new NuiComboEntry("Entry 3", 3),
        },
        Selected = 1,
      };

      Assert.That(JsonUtility.ToJson(element), Is.EqualTo("""{"elements":[["Entry 1",1],["Entry 2",2],["Entry 3",3]],"value":1,"type":"combo"}"""), JsonUtility.ToJson(element));
      Assert.That(JsonUtility.ToJson<NuiElement>(element), Is.EqualTo("""{"elements":[["Entry 1",1],["Entry 2",2],["Entry 3",3]],"value":1,"type":"combo"}"""));
    }
  }
}
