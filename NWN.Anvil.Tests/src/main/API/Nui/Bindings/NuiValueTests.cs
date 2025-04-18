using System.Collections.Generic;
using Anvil.API;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  [TestFixture]
  public class NuiValueTests
  {
    [Test(Description = "Serializing a NuiValue<string> creates a valid JSON structure.")]
    [TestCase("test", """
                      "test"
                      """)]
    [TestCase(null, @"null")]
    [TestCase("", """
                  ""
                  """)]
    public void SerializeNuiValueStringReturnsValidJsonStructure(string rawValue, string expected)
    {
      NuiValue<string> value = new NuiValue<string>(rawValue);
      Assert.That(JsonUtility.ToJson(value), Is.EqualTo(expected));
      Assert.That(JsonUtility.ToJson<NuiProperty<string>>(value), Is.EqualTo(expected));
    }

    [Test(Description = "Serializing a NuiValueStrRef creates a valid JSON structure.")]
    [TestCase(0u, """{"strref":0}""")]
    [TestCase(null, @"null")]
    [TestCase(1000u, """{"strref":1000}""")]
    public void SerializeNuiValueStrRefReturnsValidJsonStructure(uint? rawValue, string expected)
    {
      NuiValueStrRef value = new NuiValueStrRef(rawValue != null ? new StrRef(rawValue.Value) : null);
      Assert.That(JsonUtility.ToJson(value), Is.EqualTo(expected));
      Assert.That(JsonUtility.ToJson<NuiProperty<string>>(value), Is.EqualTo(expected));
    }

    [Test(Description = "Serializing a NuiValue<int> creates a valid JSON structure.")]
    [TestCase(0, @"0")]
    [TestCase(-0, @"0")]
    [TestCase(10, @"10")]
    [TestCase(-10, @"-10")]
    [TestCase(int.MaxValue, @"2147483647")]
    [TestCase(int.MinValue, @"-2147483648")]
    public void SerializeNuiValueIntReturnsValidJsonStructure(int rawValue, string expected)
    {
      NuiValue<int> value = new NuiValue<int>(rawValue);
      Assert.That(JsonUtility.ToJson(value), Is.EqualTo(expected));
      Assert.That(JsonUtility.ToJson<NuiProperty<int>>(value), Is.EqualTo(expected));
    }

    [Test(Description = "Serializing a NuiValue<int?> creates a valid JSON structure.")]
    [TestCase(0, @"0")]
    [TestCase(-0, @"0")]
    [TestCase(10, @"10")]
    [TestCase(-10, @"-10")]
    [TestCase(null, @"null")]
    [TestCase(int.MaxValue, @"2147483647")]
    [TestCase(int.MinValue, @"-2147483648")]
    public void SerializeNuiValueNullableIntReturnsValidJsonStructure(int? rawValue, string expected)
    {
      NuiValue<int?> value = new NuiValue<int?>(rawValue);
      Assert.That(JsonUtility.ToJson(value), Is.EqualTo(expected));
      Assert.That(JsonUtility.ToJson<NuiProperty<int?>>(value), Is.EqualTo(expected));
    }

    [Test(Description = "Serializing a NuiValue<float> creates a valid JSON structure.")]
    [TestCase(0f, @"0.0")]
    [TestCase(0.1f, @"0.1")]
    [TestCase(0.125f, @"0.125")]
    [TestCase(2f, @"2.0")]
    [TestCase(2.5f, @"2.5")]
    [TestCase(2.5122f, @"2.5122")]
    [TestCase(float.NaN, """
                         "NaN"
                         """)]
    [TestCase(float.NegativeInfinity, """
                                      "-Infinity"
                                      """)]
    [TestCase(float.PositiveInfinity, """
                                      "Infinity"
                                      """)]
    public void SerializeNuiValueFloatReturnsValidJsonStructure(float rawValue, string expected)
    {
      NuiValue<float> value = new NuiValue<float>(rawValue);
      Assert.That(JsonUtility.ToJson(value), Is.EqualTo(expected));
      Assert.That(JsonUtility.ToJson<NuiProperty<float>>(value), Is.EqualTo(expected));
    }

    [Test(Description = "Serializing a NuiValue<float?> creates a valid JSON structure.")]
    [TestCase(0f, @"0.0")]
    [TestCase(0.1f, @"0.1")]
    [TestCase(0.125f, @"0.125")]
    [TestCase(2f, @"2.0")]
    [TestCase(2.5f, @"2.5")]
    [TestCase(2.5122f, @"2.5122")]
    [TestCase(null, @"null")]
    [TestCase(float.NaN, """
                         "NaN"
                         """)]
    [TestCase(float.NegativeInfinity, """
                                      "-Infinity"
                                      """)]
    [TestCase(float.PositiveInfinity, """
                                      "Infinity"
                                      """)]
    public void SerializeNuiValueFloatNullableReturnsValidJsonStructure(float? rawValue, string expected)
    {
      NuiValue<float?> value = new NuiValue<float?>(rawValue);
      Assert.That(JsonUtility.ToJson(value), Is.EqualTo(expected));
      Assert.That(JsonUtility.ToJson<NuiProperty<float?>>(value), Is.EqualTo(expected));
    }

    [Test(Description = "Serializing a NuiValue<NuiRect> creates a valid JSON structure.")]
    public void SerializeNuiValueNuiRectReturnsValidJsonStructure()
    {
      NuiValue<NuiRect> value = new NuiValue<NuiRect>(new NuiRect(100f, 50.251f, 30.11f, 20f));
      Assert.That(JsonUtility.ToJson<NuiProperty<NuiRect>>(value), Is.EqualTo("""{"h":20.0,"w":30.11,"x":100.0,"y":50.251}"""));
    }

    [Test(Description = "Serializing a NuiValue<List<int>> creates a valid JSON structure.")]
    public void SerializeNuiValueIntListReturnsValidJsonStructure()
    {
      NuiValue<List<int>> value = new NuiValue<List<int>>([1, 2, 3]);
      Assert.That(JsonUtility.ToJson<NuiProperty<List<int>>>(value), Is.EqualTo(@"[1,2,3]"));
    }

    [Test(Description = "Deerializing a NuiValue<string> creates a valid value/object.")]
    [TestCase("test", """
                      "test"
                      """)]
    [TestCase(null, @"null")]
    [TestCase("", """
                  ""
                  """)]
    public void DeserializeNuiValueStringReturnsValidJsonStructure(string expected, string serialized)
    {
      NuiValue<string>? value = JsonUtility.FromJson<NuiValue<string>>(serialized);
      Assert.That(value?.Value, Is.EqualTo(expected));
    }

    [Test(Description = "Deerializing a NuiValueStrRef creates a valid value/object.")]
    [TestCase(0u, """{"strref":0}""")]
    [TestCase(null, @"null")]
    [TestCase(1000u, """{"strref":1000}""")]
    public void DeserializeNuiValueStrRefReturnsValidJsonStructure(uint? expected, string serialized)
    {
      NuiValueStrRef? value = JsonUtility.FromJson<NuiValueStrRef>(serialized);
      Assert.That(value?.Value?.Id, Is.EqualTo(expected));
    }

    [Test(Description = "Deerializing a NuiValue<int> creates a valid value/object.")]
    [TestCase(0, @"0")]
    [TestCase(-0, @"0")]
    [TestCase(10, @"10")]
    [TestCase(-10, @"-10")]
    [TestCase(int.MaxValue, @"2147483647")]
    [TestCase(int.MinValue, @"-2147483648")]
    public void DeserializeNuiValueIntReturnsValidJsonStructure(int expected, string serialized)
    {
      NuiValue<int>? value = JsonUtility.FromJson<NuiValue<int>>(serialized);
      Assert.That(value?.Value, Is.EqualTo(expected));
    }

    [Test(Description = "Deerializing a NuiValue<int?> creates a valid value/object.")]
    [TestCase(0, @"0")]
    [TestCase(-0, @"0")]
    [TestCase(10, @"10")]
    [TestCase(-10, @"-10")]
    [TestCase(null, @"null")]
    [TestCase(int.MaxValue, @"2147483647")]
    [TestCase(int.MinValue, @"-2147483648")]
    public void DeserializeNuiValueNullableIntReturnsValidJsonStructure(int? expected, string serialized)
    {
      NuiValue<int?>? value = JsonUtility.FromJson<NuiValue<int?>>(serialized);
      Assert.That(value?.Value, Is.EqualTo(expected));
    }

    [Test(Description = "Deerializing a NuiValue<float> creates a valid value/object.")]
    [TestCase(0f, @"0.0")]
    [TestCase(0.1f, @"0.1")]
    [TestCase(0.125f, @"0.125")]
    [TestCase(2f, @"2.0")]
    [TestCase(2.5f, @"2.5")]
    [TestCase(2.5122f, @"2.5122")]
    [TestCase(float.NaN, """
                         "NaN"
                         """)]
    [TestCase(float.NegativeInfinity, """
                                      "-Infinity"
                                      """)]
    [TestCase(float.PositiveInfinity, """
                                      "Infinity"
                                      """)]
    public void DeserializeNuiValueFloatReturnsValidJsonStructure(float expected, string serialized)
    {
      NuiValue<float>? value = JsonUtility.FromJson<NuiValue<float>>(serialized);
      Assert.That(value?.Value, Is.EqualTo(expected));
    }

    [Test(Description = "Deerializing a NuiValue<float?> creates a valid value/object.")]
    [TestCase(0f, @"0.0")]
    [TestCase(0.1f, @"0.1")]
    [TestCase(0.125f, @"0.125")]
    [TestCase(2f, @"2.0")]
    [TestCase(2.5f, @"2.5")]
    [TestCase(2.5122f, @"2.5122")]
    [TestCase(null, @"null")]
    [TestCase(float.NaN, """
                         "NaN"
                         """)]
    [TestCase(float.NegativeInfinity, """
                                      "-Infinity"
                                      """)]
    [TestCase(float.PositiveInfinity, """
                                      "Infinity"
                                      """)]
    public void DeserializeNuiValueFloatNullableReturnsValidJsonStructure(float? expected, string serialized)
    {
      NuiValue<float?>? value = JsonUtility.FromJson<NuiValue<float?>>(serialized);
      Assert.That(value?.Value, Is.EqualTo(expected));
    }

    [Test(Description = "Deerializing a NuiValue<NuiRect> creates a valid value/object.")]
    public void DeserializeNuiValueNuiRectReturnsValidJsonStructure()
    {
      NuiValue<NuiRect>? value = JsonUtility.FromJson<NuiValue<NuiRect>>("""{"h":20.0,"w":30.11,"x":100.0,"y":50.251}""");
      NuiRect expected = new NuiRect(100.0f, 50.251f, 30.11f, 20.0f);

      Assert.That(value?.Value, Is.EqualTo(expected));
    }

    [Test(Description = "Deserializing a NuiValue<List<int>> creates a valid value/object.")]
    public void DeserializeNuiValueIntListReturnsValidJsonStructure()
    {
      NuiValue<List<int>>? value = JsonUtility.FromJson<NuiValue<List<int>>>(@"[1,2,3]");
      List<int> expected = [1, 2, 3];

      Assert.That(value?.Value, Is.EqualTo(expected));
    }
  }
}
