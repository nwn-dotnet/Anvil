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

      Assert.That(JsonUtility.ToJson(element), Is.EqualTo("""{"type":"progress","value":{"bind":"progress"}}"""));
      Assert.That(JsonUtility.ToJson<NuiElement>(element), Is.EqualTo("""{"type":"progress","value":{"bind":"progress"}}"""));
    }
  }
}
