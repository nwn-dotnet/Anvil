using Anvil.API;
using NUnit.Framework;
using NWN.Native.API;

namespace Anvil.Tests.API
{
  [TestFixture(Category = "API.EngineStructure")]
  public sealed class EffectTests
  {
    [Test(Description = "Creating an effect and disposing the effect explicitly frees the associated memory.")]
    public void CreateAndDisposeEffectValidPropertyUpdated()
    {
      Effect effect = Effect.CutsceneParalyze();
      Assert.IsTrue(effect.IsValid, "Effect was not valid after creation.");
      effect.Dispose();
      Assert.IsFalse(effect.IsValid, "Effect was still valid after disposing.");
    }

    [Test(Description = "A soft effect reference created from a native object does not cause the original effect to be deleted.")]
    public void CreateSoftEffectReferencAndDisposeDoesNotFreeMemory()
    {
      Effect effect = Effect.Blindness();
      Assert.IsTrue(effect.IsValid, "Effect was not valid after creation.");

      CGameEffect gameEffect = effect;
      Assert.IsNotNull(gameEffect, "Native effect was not valid after implicit cast.");
      Effect softReference = gameEffect.ToEffect(false);
      softReference.Dispose();
      Assert.IsTrue(softReference.IsValid, "The soft reference disposed the memory of the original effect.");
    }
  }
}
