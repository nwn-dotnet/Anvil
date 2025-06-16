using System.Text.Json;
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

      Assert.That(JsonSerializer.Serialize(element), Is.EqualTo("""{"label":"Label","max":255,"multiline":true,"type":"textedit","value":{"bind":"input"},"wordwrap":true}"""));
      Assert.That(JsonSerializer.Serialize((NuiElement)element), Is.EqualTo("""{"label":"Label","max":255,"multiline":true,"type":"textedit","value":{"bind":"input"},"wordwrap":true}"""));
    }
  }
}
