using Anvil.API;
using NUnit.Framework;
using NWN.Native.API;

namespace Anvil.Tests.API
{
  [TestFixture]
  public sealed class StrRefTests
  {
    [Test(Description = "A valid StrRef token resolves to the correct string")]
    [TestCase(473u, "Strength Information")]
    [TestCase(8296u, "This object is locked.")]
    [TestCase(8307u, "locked")]
    [TestCase(7035u, "Inventory")]
    [TestCase(8301u, "You may only have <CUSTOM0> <CUSTOM1> at a time.")]
    public void ResolveValidStrRefReturnsValidString(uint strId, string expectedString)
    {
      StrRef strRef = new StrRef(strId);
      Assert.That(strRef.ToString(), Is.EqualTo(expectedString));
    }

    [Test(Description = "An invalid StrRef token resolves to a default placeholder string")]
    public void ResolveInvalidStrRefReturnsDefaultString()
    {
      StrRef strRef = new StrRef(int.MaxValue);
      Assert.That(strRef.ToString(), Is.EqualTo($"BadStrRef({strRef.Id})"));
    }

    [Test(Description = "Creating a StrRef from a custom tlk uses the correct index.")]
    [TestCase(0u, 0x01000000 + 0u)]
    [TestCase(5u, 0x01000000 + 5u)]
    public void CreateFromCustomTokenReturnsCorrectIndex(uint customStrId, uint expectedStrId)
    {
      StrRef strRef = StrRef.FromCustomTlk(customStrId);

      Assert.That(strRef.Id, Is.EqualTo(expectedStrId));
      Assert.That(strRef.CustomId, Is.EqualTo(customStrId));
    }

    [Test(Description = "Overriding a StrRef with a custom value correctly applies the string override.")]
    [TestCase(473u, "Strength Information", "Test 473")]
    [TestCase(8296u, "This object is locked.", "Test 8296")]
    [TestCase(8307u, "locked", "Test 8307")]
    [TestCase(7035u, "Inventory", "Test 7035")]
    [TestCase(8301u, "You may only have <CUSTOM0> <CUSTOM1> at a time.", "Test 8301")]
    public void StringOverrideIsApplied(uint strId, string defaultString, string overrideString)
    {
      StrRef strRef = new StrRef(strId);
      Assert.That(strRef.ToString(), Is.EqualTo(defaultString));

      strRef.Override = overrideString;
      Assert.That(strRef.ToString(), Is.EqualTo(overrideString));
    }

    [Test(Description = "Overriding a StrRef with a custom value, then clearing it correctly restores the default string.")]
    [TestCase(473u, "Strength Information", "Test 473")]
    [TestCase(8296u, "This object is locked.", "Test 8296")]
    [TestCase(8307u, "locked", "Test 8307")]
    [TestCase(7035u, "Inventory", "Test 7035")]
    [TestCase(8301u, "You may only have <CUSTOM0> <CUSTOM1> at a time.", "Test 8301")]
    public void StringOverrideIsCleared(uint strId, string defaultString, string overrideString)
    {
      StrRef strRef = new StrRef(strId);
      Assert.That(strRef.ToString(), Is.EqualTo(defaultString));

      strRef.Override = overrideString;
      strRef.ClearOverride();

      Assert.That(strRef.ToString(), Is.EqualTo(defaultString));
    }

    [TearDown]
    public void CleanupOverrides()
    {
      NWNXLib.TlkTable().ClearOverrides();
    }
  }
}
