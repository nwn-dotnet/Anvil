using Anvil.API;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  [TestFixture(Category = "API.Tlk")]
  public sealed class StrRefTests
  {
    [Test(Description = "A valid StrRef token resolves to the correct string")]
    [TestCase(473u, "Strength Information")]
    [TestCase(8296u, "This object is locked.")]
    [TestCase(8307u, "locked")]
    [TestCase(7035u, "Inventory")]
    [TestCase(8301u, "You may only have <CUSTOM0> <CUSTOM1> at a time.")]
    public void ResolveValidStrRefReturnsValidString(uint token, string expected)
    {
      StrRef strRef = new StrRef(token);
      Assert.That(strRef.ToString(), Is.EqualTo(expected));
    }

    [Test(Description = "An invalid StrRef token resolves to a default placeholder string")]
    public void ResolveInvalidStrRefReturnsDefaultString()
    {
      StrRef strRef = new StrRef(int.MaxValue);
      Assert.That(strRef.ToString(), Is.EqualTo($"BadStrRef({strRef.TokenNumber})"));
    }
  }
}
