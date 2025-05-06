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

      Assert.That(JsonUtility.ToJson(element), Is.EqualTo("""{"label":"chk_label","value":{"bind":"is_checked"},"type":"check"}"""));
      Assert.That(JsonUtility.ToJson<NuiElement>(element), Is.EqualTo("""{"label":"chk_label","value":{"bind":"is_checked"},"type":"check"}"""));
    }
  }
}
