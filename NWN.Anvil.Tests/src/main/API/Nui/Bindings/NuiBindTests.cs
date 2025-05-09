using System.Text.Json;
using Anvil.API;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  [TestFixture]
  public sealed class NuiBindTests
  {
    [Test(Description = "Serializing a NuiBind<string> creates a valid JSON structure.")]
    public void SerializeNuiBindStringReturnsValidJson()
    {
      NuiBind<string> bind = new NuiBind<string>("test");
      Assert.That(JsonSerializer.Serialize(bind), Is.EqualTo("""{"bind":"test"}"""));
      Assert.That(JsonSerializer.Serialize((NuiProperty<string>)bind), Is.EqualTo("""{"bind":"test"}"""));
    }

    [Test(Description = "Serializing a NuiBind<string> creates a valid JSON structure.")]
    public void SerializeNuiBindStrRefReturnsValidJson()
    {
      NuiBindStrRef bind = new NuiBindStrRef("test");
      Assert.That(JsonSerializer.Serialize(bind), Is.EqualTo("""{"bind":"test"}"""));
      Assert.That(JsonSerializer.Serialize((NuiProperty<string>)bind), Is.EqualTo("""{"bind":"test"}"""));
    }

    [Test(Description = "Serializing a NuiBind<NuiRect> creates a valid JSON structure.")]
    public void SerializeNuiBindNuiRectReturnsValidJson()
    {
      NuiBind<NuiRect> bind = new NuiBind<NuiRect>("test");
      Assert.That(JsonSerializer.Serialize(bind), Is.EqualTo("""{"bind":"test"}"""));
      Assert.That(JsonSerializer.Serialize((NuiProperty<NuiRect>)bind), Is.EqualTo("""{"bind":"test"}"""));
    }

    [Test(Description = "Deserializing a NuiBind<string> creates a valid value/object.")]
    public void DeserializeNuiBindStringReturnsValidJson()
    {
      NuiBind<string>? bind = JsonSerializer.Deserialize<NuiBind<string>>("""{"bind":"test"}""");
      Assert.That(bind?.Key, Is.EqualTo("test"));
    }

    [Test(Description = "Deserializing a NuiBind<string> creates a valid value/object.")]
    public void DeserializeNuiBindStrRefReturnsValidJson()
    {
      NuiBind<NuiBindStrRef>? bind = JsonSerializer.Deserialize<NuiBind<NuiBindStrRef>>("""{"bind":"test"}""");
      Assert.That(bind?.Key, Is.EqualTo("test"));
    }

    [Test(Description = "Deserializing a NuiBind<NuiRect> creates a valid value/object.")]
    public void DeserializeNuiBindNuiRectReturnsValidJson()
    {
      NuiBind<NuiRect>? bind = JsonSerializer.Deserialize<NuiBind<NuiRect>>("""{"bind":"test"}""");
      Assert.That(bind?.Key, Is.EqualTo("test"));
    }
  }
}
