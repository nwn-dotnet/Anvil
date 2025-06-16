using System.Text.Json;
using Anvil.API;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  public sealed class NuiProgressTests
  {
    [Test]
    public void SerializeNuiProgressReturnsValidJson()
    {
      NuiProgress element = new NuiProgress(new NuiBind<float>("progress"));

      Assert.That(JsonSerializer.Serialize(element), Is.EqualTo("""{"type":"progress","value":{"bind":"progress"}}"""));
      Assert.That(JsonSerializer.Serialize((NuiElement)element), Is.EqualTo("""{"type":"progress","value":{"bind":"progress"}}"""));
    }
  }
}
