using System.Text.Json;
using Anvil.API;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  public sealed class NuiOptionsTests
  {
    [Test]
    public void SerializeNuiOptionsReturnsValidJson()
    {
      NuiOptions element = new NuiOptions
      {
        Direction = NuiDirection.Vertical,
        Options = ["option1", "option2"],
        Selection = new NuiBind<int>("selection"),
      };

      Assert.That(JsonSerializer.Serialize(element), Is.EqualTo("""{"direction":1,"elements":["option1","option2"],"value":{"bind":"selection"},"type":"options"}"""));
      Assert.That(JsonSerializer.Serialize((NuiElement)element), Is.EqualTo("""{"direction":1,"elements":["option1","option2"],"value":{"bind":"selection"},"type":"options"}"""));
    }
  }
}
