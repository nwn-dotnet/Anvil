using System.Collections.Generic;
using System.Text.Json;
using Anvil.API;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  [TestFixture]
  public sealed class NuiValueTests
  {
    [Test(Description = "Serializing a NuiValue<string> creates a valid JSON structure.")]
    [TestCase("test", """
                      "test"
                      """)]
    [TestCase(null, @"null")]
    [TestCase("", """
                  ""
                  """)]
    public void SerializeNuiValueStringReturnsValidJson(string rawValue, string expected)
    {
      NuiValue<string> value = new NuiValue<string>(rawValue);
      Assert.That(JsonSerializer.Serialize(value), Is.EqualTo(expected));
      Assert.That(JsonSerializer.Serialize((NuiProperty<string>)value), Is.EqualTo(expected));
    }

    [Test(Description = "Serializing a NuiValueStrRef creates a valid JSON structure.")]
    [TestCase(0u, """{"strref":0}""")]
    [TestCase(null, @"null")]
    [TestCase(1000u, """{"strref":1000}""")]
    public void SerializeNuiValueStrRefReturnsValidJson(uint? rawValue, string expected)
    {
      NuiValueStrRef value = new NuiValueStrRef(rawValue != null ? new StrRef(rawValue.Value) : null);
      Assert.That(JsonSerializer.Serialize(value), Is.EqualTo(expected));
      Assert.That(JsonSerializer.Serialize((NuiProperty<string>)value), Is.EqualTo(expected));
    }

    [Test(Description = "Serializing a NuiValue<int> creates a valid JSON structure.")]
    [TestCase(0, @"0")]
    [TestCase(-0, @"0")]
    [TestCase(10, @"10")]
    [TestCase(-10, @"-10")]
    [TestCase(int.MaxValue, @"2147483647")]
    [TestCase(int.MinValue, @"-2147483648")]
    public void SerializeNuiValueIntReturnsValidJson(int rawValue, string expected)
    {
      NuiValue<int> value = new NuiValue<int>(rawValue);
      Assert.That(JsonSerializer.Serialize(value), Is.EqualTo(expected));
      Assert.That(JsonSerializer.Serialize((NuiProperty<int>)value), Is.EqualTo(expected));
    }

    [Test(Description = "Serializing a NuiValue<int?> creates a valid JSON structure.")]
    [TestCase(0, @"0")]
    [TestCase(-0, @"0")]
    [TestCase(10, @"10")]
    [TestCase(-10, @"-10")]
    [TestCase(null, @"null")]
    [TestCase(int.MaxValue, @"2147483647")]
    [TestCase(int.MinValue, @"-2147483648")]
    public void SerializeNuiValueNullableIntReturnsValidJson(int? rawValue, string expected)
    {
      NuiValue<int?> value = new NuiValue<int?>(rawValue);
      Assert.That(JsonSerializer.Serialize(value), Is.EqualTo(expected));
      Assert.That(JsonSerializer.Serialize((NuiProperty<int?>)value), Is.EqualTo(expected));
    }

    [Test(Description = "Serializing a NuiValue<float> creates a valid JSON structure.")]
    [TestCase(0f, @"0")]
    [TestCase(0.1f, @"0.1")]
    [TestCase(0.125f, @"0.125")]
    [TestCase(2f, @"2")]
    [TestCase(2.5f, @"2.5")]
    [TestCase(2.5122f, @"2.5122")]
    public void SerializeNuiValueFloatReturnsValidJson(float rawValue, string expected)
    {
      NuiValue<float> value = new NuiValue<float>(rawValue);
      Assert.That(JsonSerializer.Serialize(value), Is.EqualTo(expected));
      Assert.That(JsonSerializer.Serialize((NuiProperty<float>)value), Is.EqualTo(expected));
    }

    [Test(Description = "Serializing a NuiValue<float?> creates a valid JSON structure.")]
    [TestCase(0f, @"0")]
    [TestCase(0.1f, @"0.1")]
    [TestCase(0.125f, @"0.125")]
    [TestCase(2f, @"2")]
    [TestCase(2.5f, @"2.5")]
    [TestCase(2.5122f, @"2.5122")]
    [TestCase(null, @"null")]
    public void SerializeNuiValueFloatNullableReturnsValidJson(float? rawValue, string expected)
    {
      NuiValue<float?> value = new NuiValue<float?>(rawValue);
      Assert.That(JsonSerializer.Serialize(value), Is.EqualTo(expected));
      Assert.That(JsonSerializer.Serialize((NuiProperty<float?>)value), Is.EqualTo(expected));
    }

    [Test(Description = "Serializing a NuiValue<NuiRect> creates a valid JSON structure.")]
    public void SerializeNuiValueNuiRectReturnsValidJson()
    {
      NuiValue<NuiRect> value = new NuiValue<NuiRect>(new NuiRect(100f, 50.251f, 30.11f, 20f));
      Assert.That(JsonSerializer.Serialize((NuiProperty<NuiRect>)value), Is.EqualTo("""{"h":20,"w":30.11,"x":100,"y":50.251}"""));
    }

    [Test(Description = "Serializing a NuiValue<List<int>> creates a valid JSON structure.")]
    public void SerializeNuiValueIntListReturnsValidJson()
    {
      NuiValue<List<int>> value = new NuiValue<List<int>>([1, 2, 3]);
      Assert.That(JsonSerializer.Serialize((NuiProperty<List<int>>)value), Is.EqualTo(@"[1,2,3]"));
    }

    [Test(Description = "Deserializing a NuiValue<string> creates a valid value/object.")]
    [TestCase("test", """
                      "test"
                      """)]
    [TestCase(null, @"null")]
    [TestCase("", """
                  ""
                  """)]
    public void DeserializeNuiValueStringReturnsValidJson(string expected, string serialized)
    {
      NuiValue<string>? value = JsonSerializer.Deserialize<NuiValue<string>>(serialized);
      Assert.That(value?.Value, Is.EqualTo(expected));
    }

    [Test(Description = "Deserializing a NuiValueStrRef creates a valid value/object.")]
    [TestCase(0u, """{"strref":0}""")]
    [TestCase(null, @"null")]
    [TestCase(1000u, """{"strref":1000}""")]
    public void DeserializeNuiValueStrRefReturnsValidJson(uint? expected, string serialized)
    {
      NuiValueStrRef? value = JsonSerializer.Deserialize<NuiValueStrRef>(serialized);
      Assert.That(value?.Value?.Id, Is.EqualTo(expected));
    }

    [Test(Description = "Deserializing a NuiValue<int> creates a valid value/object.")]
    [TestCase(0, @"0")]
    [TestCase(-0, @"0")]
    [TestCase(10, @"10")]
    [TestCase(-10, @"-10")]
    [TestCase(int.MaxValue, @"2147483647")]
    [TestCase(int.MinValue, @"-2147483648")]
    public void DeserializeNuiValueIntReturnsValidJson(int expected, string serialized)
    {
      NuiValue<int>? value = JsonSerializer.Deserialize<NuiValue<int>>(serialized);
      Assert.That(value?.Value, Is.EqualTo(expected));
    }

    [Test(Description = "Deserializing a NuiValue<int?> creates a valid value/object.")]
    [TestCase(0, @"0")]
    [TestCase(-0, @"0")]
    [TestCase(10, @"10")]
    [TestCase(-10, @"-10")]
    [TestCase(null, @"null")]
    [TestCase(int.MaxValue, @"2147483647")]
    [TestCase(int.MinValue, @"-2147483648")]
    public void DeserializeNuiValueNullableIntReturnsValidJson(int? expected, string serialized)
    {
      NuiValue<int?>? value = JsonSerializer.Deserialize<NuiValue<int?>>(serialized);
      Assert.That(value?.Value, Is.EqualTo(expected));
    }

    [Test(Description = "Deserializing a NuiValue<float> creates a valid value/object.")]
    [TestCase(0f, @"0")]
    [TestCase(0.1f, @"0.1")]
    [TestCase(0.125f, @"0.125")]
    [TestCase(2f, @"2")]
    [TestCase(2.5f, @"2.5")]
    [TestCase(2.5122f, @"2.5122")]
    public void DeserializeNuiValueFloatReturnsValidJson(float expected, string serialized)
    {
      NuiValue<float>? value = JsonSerializer.Deserialize<NuiValue<float>>(serialized);
      Assert.That(value?.Value, Is.EqualTo(expected));
    }

    [Test(Description = "Deserializing a NuiValue<float?> creates a valid value/object.")]
    [TestCase(0f, @"0")]
    [TestCase(0.1f, @"0.1")]
    [TestCase(0.125f, @"0.125")]
    [TestCase(2f, @"2")]
    [TestCase(2.5f, @"2.5")]
    [TestCase(2.5122f, @"2.5122")]
    [TestCase(null, @"null")]
    public void DeserializeNuiValueFloatNullableReturnsValidJson(float? expected, string serialized)
    {
      NuiValue<float?>? value = JsonSerializer.Deserialize<NuiValue<float?>>(serialized);
      Assert.That(value?.Value, Is.EqualTo(expected));
    }

    [Test(Description = "Deserializing a NuiValue<NuiRect> creates a valid value/object.")]
    public void DeserializeNuiValueNuiRectReturnsValidJson()
    {
      NuiValue<NuiRect>? value = JsonSerializer.Deserialize<NuiValue<NuiRect>>("""{"h":20,"w":30.11,"x":100,"y":50.251}""");
      NuiRect expected = new NuiRect(100.0f, 50.251f, 30.11f, 20.0f);

      Assert.That(value?.Value, Is.EqualTo(expected));
    }

    [Test(Description = "Deserializing a NuiValue<List<int>> creates a valid value/object.")]
    public void DeserializeNuiValueIntListReturnsValidJson()
    {
      NuiValue<List<int>>? value = JsonSerializer.Deserialize<NuiValue<List<int>>>(@"[1,2,3]");
      List<int> expected = [1, 2, 3];

      Assert.That(value?.Value, Is.EqualTo(expected));
    }
  }
}
