using Anvil.API;
using NUnit.Framework;

// ReSharper disable UnusedAutoPropertyAccessor.Local
namespace Anvil.Tests.API
{
  [TestFixture(Category = "API.Utils")]
  public sealed class JsonUtilityTests
  {
    [Test(Description = "Serializing a value creates valid json.")]
    [TestCase(null, "null")]
    [TestCase(1, "1")]
    [TestCase(1f, "1.0")]
    [TestCase(1.532f, "1.532")]
    [TestCase(1.0d, "1.0")]
    [TestCase(1.689d, "1.689")]
    [TestCase(false, "false")]
    [TestCase(true, "true")]
    [TestCase("test", @"""test""")]
    [TestCase("", @"""""")]
    public void SerializeValueCreatesValidJson(object value, string expected)
    {
      Assert.That(JsonUtility.ToJson(value), Is.EqualTo(expected));
    }

    [Test(Description = "Serializing a struct creates valid json.")]
    public void SerializeStructCreatesValidJson()
    {
      TestStruct value = new TestStruct
      {
        TestB = true,
        TestF = 10f,
        TestI = 5,
        TestS = "test",
      };

      Assert.That(JsonUtility.ToJson(value), Is.EqualTo(@"{""TestI"":5,""TestS"":""test"",""TestF"":10.0,""TestB"":true}"));
    }

    [Test(Description = "Serializing a class creates valid json.")]
    public void SerializeClassCreatesValidJson()
    {
      TestClass value = new TestClass
      {
        TestB = true,
        TestF = 10f,
        TestI = 5,
        TestS = "test",
      };

      Assert.That(JsonUtility.ToJson(value), Is.EqualTo(@"{""TestI"":5,""TestS"":""test"",""TestF"":10.0,""TestB"":true}"));
    }

    [Test(Description = "Serializing a record creates valid json.")]
    public void SerializeRecordCreatesValidJson()
    {
      TestRecord value = new TestRecord
      {
        TestB = true,
        TestF = 10f,
        TestI = 5,
        TestS = "test",
      };

      Assert.That(JsonUtility.ToJson(value), Is.EqualTo(@"{""TestI"":5,""TestS"":""test"",""TestF"":10.0,""TestB"":true}"));
    }

    private struct TestStruct
    {
      public int TestI { get; set; }
      public string TestS { get; set; }
      public float TestF { get; set; }
      public bool TestB { get; set; }
    }

    private sealed class TestClass
    {
      public int TestI { get; set; }
      public string? TestS { get; set; }
      public float TestF { get; set; }
      public bool TestB { get; set; }
    }

    private sealed record TestRecord
    {
      public int TestI { get; set; }
      public string? TestS { get; set; }
      public float TestF { get; set; }
      public bool TestB { get; set; }
    }
  }
}
