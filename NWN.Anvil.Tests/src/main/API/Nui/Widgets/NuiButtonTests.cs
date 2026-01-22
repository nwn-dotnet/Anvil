using System.Text.Json;
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

      Assert.That(JsonSerializer.Serialize(element), Is.EqualTo("""{"label":"btn_label","type":"button"}"""));
      Assert.That(JsonSerializer.Serialize((NuiElement)element), Is.EqualTo("""{"label":"btn_label","type":"button"}"""));
    }
  }
}
