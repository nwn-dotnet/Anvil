using Anvil.API;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  public sealed class NuiTextEditTests
  {
    [Test]
    public void SerializeNuiTextEditReturnsValidJson()
    {
      NuiTextEdit element = new NuiTextEdit("Label", new NuiBind<string>("input"), 255, true);

      Assert.That(JsonUtility.ToJson(element), Is.EqualTo("""{"label":"Label","max":255,"multiline":true,"type":"textedit","value":{"bind":"input"},"wordwrap":true}"""));
      Assert.That(JsonUtility.ToJson<NuiElement>(element), Is.EqualTo("""{"label":"Label","max":255,"multiline":true,"type":"textedit","value":{"bind":"input"},"wordwrap":true}"""));
    }
  }
}
