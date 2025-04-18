using Anvil.API;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  [TestFixture]
  public sealed class NuiBindTests
  {
    [Test(Description = "Serializing a NuiBind<string> creates a valid JSON structure.")]
    public void SerializeNuiBindStringReturnsValidJsonStructure()
    {
      NuiBind<string> bind = new NuiBind<string>("test");
      Assert.That(JsonUtility.ToJson(bind), Is.EqualTo("""{"bind":"test"}"""));
      Assert.That(JsonUtility.ToJson<NuiProperty<string>>(bind), Is.EqualTo("""{"bind":"test"}"""));
    }

    [Test(Description = "Serializing a NuiBind<string> creates a valid JSON structure.")]
    public void SerializeNuiBindStrRefReturnsValidJsonStructure()
    {
      NuiBindStrRef bind = new NuiBindStrRef("test");
      Assert.That(JsonUtility.ToJson(bind), Is.EqualTo("""{"bind":"test"}"""));
      Assert.That(JsonUtility.ToJson<NuiProperty<string>>(bind), Is.EqualTo("""{"bind":"test"}"""));
    }

    [Test(Description = "Serializing a NuiBind<NuiRect> creates a valid JSON structure.")]
    public void SerializeNuiBindNuiRectReturnsValidJsonStructure()
    {
      NuiBind<NuiRect> bind = new NuiBind<NuiRect>("test");
      Assert.That(JsonUtility.ToJson(bind), Is.EqualTo("""{"bind":"test"}"""));
      Assert.That(JsonUtility.ToJson<NuiProperty<NuiRect>>(bind), Is.EqualTo("""{"bind":"test"}"""));
    }

    [Test(Description = "Deerializing a NuiBind<string> creates a valid value/object.")]
    public void DeserializeNuiBindStringReturnsValidJsonStructure()
    {
      NuiBind<string>? bind = JsonUtility.FromJson<NuiBind<string>>("""{"bind":"test"}""");
      Assert.That(bind?.Key, Is.EqualTo("test"));
    }

    [Test(Description = "Deerializing a NuiBind<string> creates a valid value/object.")]
    public void DeserializeNuiBindStrRefReturnsValidJsonStructure()
    {
      NuiBind<NuiBindStrRef>? bind = JsonUtility.FromJson<NuiBind<NuiBindStrRef>>("""{"bind":"test"}""");
      Assert.That(bind?.Key, Is.EqualTo("test"));
    }

    [Test(Description = "Deerializing a NuiBind<NuiRect> creates a valid value/object.")]
    public void DeserializeNuiBindNuiRectReturnsValidJsonStructure()
    {
      NuiBind<NuiRect>? bind = JsonUtility.FromJson<NuiBind<NuiRect>>("""{"bind":"test"}""");
      Assert.That(bind?.Key, Is.EqualTo("test"));
    }
  }
}
