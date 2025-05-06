using Anvil.API;
using NUnit.Framework;

namespace Anvil.Tests.API
{
  [TestFixture]
  public sealed class StrTokenTests
  {
    [TestCase(8301u, "You may only have {0} {1} at a time.", 0, 1, "Token 0 Test", "Token 1 Test")]
    public void SetCustomTokenValueIsTokenApplied(uint strId, string format, int tokenId1, int tokenId2, string token1Value, string token2Value)
    {
      StrRef strRef = new StrRef(strId);
      StrTokenCustom token1 = new StrTokenCustom(tokenId1);
      StrTokenCustom token2 = new StrTokenCustom(tokenId2);

      token1.Value = token1Value;
      token2.Value = token2Value;

      Assert.That(token1.Value, Is.EqualTo(token1Value));
      Assert.That(token2.Value, Is.EqualTo(token2Value));

      Assert.That(strRef.ToParsedString(), Is.EqualTo(string.Format(format, token1Value, token2Value)));
    }
  }
}
