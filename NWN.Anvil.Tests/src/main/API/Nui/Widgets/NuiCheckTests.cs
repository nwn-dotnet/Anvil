using System.Text.Json;
using Anvil.API;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  public sealed class NuiCheckTests
  {
    [Test]
    public void SerializeNuiCheckReturnsValidJson()
    {
      NuiCheck element = new NuiCheck("chk_label", new NuiBind<bool>("is_checked"));

      Assert.That(JsonSerializer.Serialize(element), Is.EqualTo("""{"label":"chk_label","value":{"bind":"is_checked"},"type":"check"}"""));
      Assert.That(JsonSerializer.Serialize((NuiElement)element), Is.EqualTo("""{"label":"chk_label","value":{"bind":"is_checked"},"type":"check"}"""));
    }
  }
}
