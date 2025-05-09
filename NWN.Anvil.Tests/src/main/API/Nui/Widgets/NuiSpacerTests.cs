using System.Text.Json;
using Anvil.API;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  public sealed class NuiSpacerTests
  {
    [Test]
    public void SerializeNuiSpacerReturnsValidJson()
    {
      NuiSpacer element = new NuiSpacer();

      Assert.That(JsonSerializer.Serialize(element), Is.EqualTo("""{"type":"spacer"}"""));
      Assert.That(JsonSerializer.Serialize((NuiElement)element), Is.EqualTo("""{"type":"spacer"}"""));
    }
  }
}
