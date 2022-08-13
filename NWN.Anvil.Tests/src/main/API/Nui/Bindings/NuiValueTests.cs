using System.Collections.Generic;
using Anvil.API;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  [TestFixture(Category = "API.Nui")]
  public class NuiValueTests
  {
    [Test(Description = "Serializing a NuiValue<string> creates a valid JSON structure.")]
    [TestCase("test", @"""test""")]
    [TestCase(null, @"null")]
    [TestCase("", @"""""")]
    public void SerializeNuiValueStringReturnsValidJsonStructure(string value, string expected)
    {
      NuiValue<string> test = new NuiValue<string>(value);
      Assert.That(JsonUtility.ToJson(test), Is.EqualTo(expected));
    }

    [Test(Description = "Serializing a NuiValueStrRef creates a valid JSON structure.")]
    [TestCase(0, @"{""strref"":0}")]
    [TestCase(null, @"null")]
    [TestCase(1000, @"{""strref"":1000}")]
    public void SerializeNuiValueStrRefReturnsValidJsonStructure(string value, string expected)
    {
      NuiValue<string> test = new NuiValue<string>(value);
      Assert.That(JsonUtility.ToJson(test), Is.EqualTo(expected));
    }

    [Test(Description = "Serializing a NuiValue<int> creates a valid JSON structure.")]
    [TestCase(0, @"0")]
    [TestCase(-0, @"0")]
    [TestCase(10, @"10")]
    [TestCase(-10, @"-10")]
    [TestCase(int.MaxValue, @"2147483647")]
    [TestCase(int.MinValue, @"-2147483648")]
    public void SerializeNuiValueIntReturnsValidJsonStructure(int value, string expected)
    {
      NuiValue<int> test = new NuiValue<int>(value);
      Assert.That(JsonUtility.ToJson(test), Is.EqualTo(expected));
    }

    [Test(Description = "Serializing a NuiValue<int?> creates a valid JSON structure.")]
    [TestCase(0, @"0")]
    [TestCase(-0, @"0")]
    [TestCase(10, @"10")]
    [TestCase(-10, @"-10")]
    [TestCase(null, @"null")]
    [TestCase(int.MaxValue, @"2147483647")]
    [TestCase(int.MinValue, @"-2147483648")]
    public void SerializeNuiValueNullableIntReturnsValidJsonStructure(int? value, string expected)
    {
      NuiValue<int?> test = new NuiValue<int?>(value);
      Assert.That(JsonUtility.ToJson(test), Is.EqualTo(expected));
    }

    [Test(Description = "Serializing a NuiValue<float> creates a valid JSON structure.")]
    [TestCase(0f, @"0.0")]
    [TestCase(0.1f, @"0.1")]
    [TestCase(0.125f, @"0.125")]
    [TestCase(2f, @"2.0")]
    [TestCase(2.5f, @"2.5")]
    [TestCase(2.5122f, @"2.5122")]
    [TestCase(float.NaN, @"""NaN""")]
    [TestCase(float.NegativeInfinity, @"""-Infinity""")]
    [TestCase(float.PositiveInfinity, @"""Infinity""")]
    public void SerializeNuiValueFloatReturnsValidJsonStructure(float value, string expected)
    {
      NuiValue<float> test = new NuiValue<float>(value);
      Assert.That(JsonUtility.ToJson(test), Is.EqualTo(expected));
    }

    [Test(Description = "Serializing a NuiValue<float?> creates a valid JSON structure.")]
    [TestCase(0f, @"0.0")]
    [TestCase(0.1f, @"0.1")]
    [TestCase(0.125f, @"0.125")]
    [TestCase(2f, @"2.0")]
    [TestCase(2.5f, @"2.5")]
    [TestCase(2.5122f, @"2.5122")]
    [TestCase(null, @"null")]
    [TestCase(float.NaN, @"""NaN""")]
    [TestCase(float.NegativeInfinity, @"""-Infinity""")]
    [TestCase(float.PositiveInfinity, @"""Infinity""")]
    public void SerializeNuiValueFloatNullableReturnsValidJsonStructure(float? value, string expected)
    {
      NuiValue<float?> test = new NuiValue<float?>(value);
      Assert.That(JsonUtility.ToJson(test), Is.EqualTo(expected));
    }

    [Test(Description = "Serializing a NuiValue<NuiRect> creates a valid JSON structure.")]
    public void SerializeNuiValueNuiRectReturnsValidJsonStructure()
    {
      NuiValue<NuiRect> test = new NuiValue<NuiRect>(new NuiRect(100f, 50.251f, 30.11f, 20f));
      Assert.That(JsonUtility.ToJson(test), Is.EqualTo(@"{""h"":20.0,""w"":30.11,""x"":100.0,""y"":50.251}"));
    }

    [Test(Description = "Serializing a NuiValue<List<int>> creates a valid JSON structure.")]
    public void SerializeNuiValueIntListReturnsValidJsonStructure()
    {
      NuiValue<List<int>> test = new NuiValue<List<int>>(new List<int> { 1, 2, 3 });
      Assert.That(JsonUtility.ToJson(test), Is.EqualTo(@"[1,2,3]"));
    }

    [Test(Description = "Deerializing a NuiValue<string> creates a valid value/object.")]
    [TestCase("test", @"""test""")]
    [TestCase(null, @"null")]
    [TestCase("", @"""""")]
    public void DeserializeNuiValueStringReturnsValidJsonStructure(string expected, string serialized)
    {
      NuiValue<string>? test = JsonUtility.FromJson<NuiValue<string>>(serialized);
      Assert.That(test?.Value, Is.EqualTo(expected));
    }

    [Test(Description = "Deerializing a NuiValueStrRef creates a valid value/object.")]
    [TestCase(0u, @"{""strref"":0}")]
    [TestCase(null, @"null")]
    [TestCase(1000u, @"{""strref"":1000}")]
    public void DeserializeNuiValueStrRefReturnsValidJsonStructure(uint? expected, string serialized)
    {
      NuiValueStrRef? test = JsonUtility.FromJson<NuiValueStrRef>(serialized);
      Assert.That(test?.Value?.Id, Is.EqualTo(expected));
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
      NuiValue<int>? test = JsonUtility.FromJson<NuiValue<int>>(serialized);
      Assert.That(test?.Value, Is.EqualTo(expected));
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
      NuiValue<int?>? test = JsonUtility.FromJson<NuiValue<int?>>(serialized);
      Assert.That(test?.Value, Is.EqualTo(expected));
    }

    [Test(Description = "Deerializing a NuiValue<float> creates a valid value/object.")]
    [TestCase(0f, @"0.0")]
    [TestCase(0.1f, @"0.1")]
    [TestCase(0.125f, @"0.125")]
    [TestCase(2f, @"2.0")]
    [TestCase(2.5f, @"2.5")]
    [TestCase(2.5122f, @"2.5122")]
    [TestCase(float.NaN, @"""NaN""")]
    [TestCase(float.NegativeInfinity, @"""-Infinity""")]
    [TestCase(float.PositiveInfinity, @"""Infinity""")]
    public void DeserializeNuiValueFloatReturnsValidJsonStructure(float expected, string serialized)
    {
      NuiValue<float>? test = JsonUtility.FromJson<NuiValue<float>>(serialized);
      Assert.That(test?.Value, Is.EqualTo(expected));
    }

    [Test(Description = "Deerializing a NuiValue<float?> creates a valid value/object.")]
    [TestCase(0f, @"0.0")]
    [TestCase(0.1f, @"0.1")]
    [TestCase(0.125f, @"0.125")]
    [TestCase(2f, @"2.0")]
    [TestCase(2.5f, @"2.5")]
    [TestCase(2.5122f, @"2.5122")]
    [TestCase(null, @"null")]
    [TestCase(float.NaN, @"""NaN""")]
    [TestCase(float.NegativeInfinity, @"""-Infinity""")]
    [TestCase(float.PositiveInfinity, @"""Infinity""")]
    public void DeserializeNuiValueFloatNullableReturnsValidJsonStructure(float? expected, string serialized)
    {
      NuiValue<float?>? test = JsonUtility.FromJson<NuiValue<float?>>(serialized);
      Assert.That(test?.Value, Is.EqualTo(expected));
    }

    [Test(Description = "Deerializing a NuiValue<NuiRect> creates a valid value/object.")]
    public void DeserializeNuiValueNuiRectReturnsValidJsonStructure()
    {
      NuiValue<NuiRect>? test = JsonUtility.FromJson<NuiValue<NuiRect>>(@"{""h"":20.0,""w"":30.11,""x"":100.0,""y"":50.251}");
      NuiRect expected = new NuiRect(100.0f, 50.251f, 30.11f, 20.0f);

      Assert.That(test?.Value, Is.EqualTo(expected));
    }

    [Test(Description = "Deserializing a NuiValue<List<int>> creates a valid value/object.")]
    public void DeserializeNuiValueIntListReturnsValidJsonStructure()
    {
      NuiValue<List<int>>? test = JsonUtility.FromJson<NuiValue<List<int>>>(@"[1,2,3]");
      List<int> expected = new List<int> { 1, 2, 3 };

      Assert.That(test?.Value, Is.EqualTo(expected));
    }
  }
}
