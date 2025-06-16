using System.Collections.Generic;
using System.Text.Json;
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

      Assert.That(JsonSerializer.Serialize(element), Is.EqualTo("""{"elements":[["Entry 1",1],["Entry 2",2],["Entry 3",3]],"value":1,"type":"combo"}"""), JsonSerializer.Serialize(element));
      Assert.That(JsonSerializer.Serialize((NuiElement)element), Is.EqualTo("""{"elements":[["Entry 1",1],["Entry 2",2],["Entry 3",3]],"value":1,"type":"combo"}"""));
    }
  }
}
