using Anvil.API;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  [TestFixture(Category = "API.Nui")]
  public sealed class NuiBindTests
  {
    [Test(Description = "Serializing a NuiBind<string> creates a valid JSON structure.")]
    public void SerializeNuiBindStringReturnsValidJsonStructure()
    {
      NuiBind<string> test = new NuiBind<string>("test");
      Assert.That(JsonUtility.ToJson(test), Is.EqualTo("""{"bind":"test"}"""));
      Assert.That(JsonUtility.ToJson<NuiProperty<string>>(test), Is.EqualTo("""{"bind":"test"}"""));
    }

    [Test(Description = "Serializing a NuiBind<string> creates a valid JSON structure.")]
    public void SerializeNuiBindStrRefReturnsValidJsonStructure()
    {
      NuiBindStrRef test = new NuiBindStrRef("test");
      Assert.That(JsonUtility.ToJson(test), Is.EqualTo("""{"bind":"test"}"""));
      Assert.That(JsonUtility.ToJson<NuiProperty<string>>(test), Is.EqualTo("""{"bind":"test"}"""));
    }

    [Test(Description = "Serializing a NuiBind<NuiRect> creates a valid JSON structure.")]
    public void SerializeNuiBindNuiRectReturnsValidJsonStructure()
    {
      NuiBind<NuiRect> test = new NuiBind<NuiRect>("test");
      Assert.That(JsonUtility.ToJson(test), Is.EqualTo("""{"bind":"test"}"""));
      Assert.That(JsonUtility.ToJson<NuiProperty<NuiRect>>(test), Is.EqualTo("""{"bind":"test"}"""));
    }

    [Test(Description = "Deerializing a NuiBind<string> creates a valid value/object.")]
    public void DeserializeNuiBindStringReturnsValidJsonStructure()
    {
      NuiBind<string>? test = JsonUtility.FromJson<NuiBind<string>>("""{"bind":"test"}""");
      Assert.That(test?.Key, Is.EqualTo("test"));
    }

    [Test(Description = "Deerializing a NuiBind<string> creates a valid value/object.")]
    public void DeserializeNuiBindStrRefReturnsValidJsonStructure()
    {
      NuiBind<NuiBindStrRef>? test = JsonUtility.FromJson<NuiBind<NuiBindStrRef>>("""{"bind":"test"}""");
      Assert.That(test?.Key, Is.EqualTo("test"));
    }

    [Test(Description = "Deerializing a NuiBind<NuiRect> creates a valid value/object.")]
    public void DeserializeNuiBindNuiRectReturnsValidJsonStructure()
    {
      NuiBind<NuiRect>? test = JsonUtility.FromJson<NuiBind<NuiRect>>("""{"bind":"test"}""");
      Assert.That(test?.Key, Is.EqualTo("test"));
    }
  }
}
