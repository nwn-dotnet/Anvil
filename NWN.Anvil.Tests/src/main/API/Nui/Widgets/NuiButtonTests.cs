using Anvil.API;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  public sealed class NuiButtonTests
  {
    [Test]
    public void SerializeNuiButtonReturnsValidJson()
    {
      NuiButton element = new NuiButton("btn_label");

      Assert.That(JsonUtility.ToJson(element), Is.EqualTo("""{"label":"btn_label","type":"button"}"""));
      Assert.That(JsonUtility.ToJson<NuiElement>(element), Is.EqualTo("""{"label":"btn_label","type":"button"}"""));
    }
  }
}
