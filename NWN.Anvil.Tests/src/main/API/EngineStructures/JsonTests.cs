using Anvil.API;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  [TestFixture(Category = "API.EngineStructure")]
  public sealed class JsonTests
  {
    [Test(Description = "Creating a json structure and disposing it explicitly frees the associated memory.")]
    public void CreateAndDisposeJsonFreesNativeStructure()
    {
      int[] test = { 1, 2 };
      Json json = JsonUtility.ToJsonStructure(test);

      Assert.That(json.IsValid, Is.True, "Json struct was not valid after creation.");
      json.Dispose();
      Assert.That(json.IsValid, Is.False, "Json struct was still valid after disposing.");
    }
  }
}
