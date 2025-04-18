using Anvil.API;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  [TestFixture]
  public sealed class NuiButtonImageTests
  {
    [Test]
    public void SerializeNuiButtonImageReturnsValidJson()
    {
      NuiButtonImage element = new NuiButtonImage("btn_resref")
      {
        Enabled = true,
      };

      Assert.That(JsonUtility.ToJson(element), Is.EqualTo("""{"label":"btn_resref","type":"button_image","enabled":true}"""));
      Assert.That(JsonUtility.ToJson<NuiElement>(element), Is.EqualTo("""{"label":"btn_resref","type":"button_image","enabled":true}"""));
    }
  }
}
