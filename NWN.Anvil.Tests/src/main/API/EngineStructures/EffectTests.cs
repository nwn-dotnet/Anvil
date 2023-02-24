using System;
using System.Runtime.CompilerServices;
using Anvil.API;
using NUnit.Framework;
using NWN.Core;
using NWN.Native.API;

namespace Anvil.Tests.API
{
  [TestFixture(Category = "API.EngineStructure")]
  public sealed class EffectTests
  {
    [Test(Description = "Creating an effect and disposing the effect explicitly frees the associated memory.")]
    public void CreateAndDisposeEffectFreesNativeStructure()
    {
      Effect effect = Effect.CutsceneParalyze();
      Assert.That(effect.IsValid, Is.True, "Effect was not valid after creation.");
      effect.Dispose();
      Assert.That(effect.IsValid, Is.False, "Effect was still valid after disposing.");
    }

    [Test(Description = "A soft effect reference created from a native object does not cause the original effect to be deleted.")]
    public void CreateSoftEffectReferencAndDisposeDoesNotFreeMemory()
    {
      Effect effect = Effect.Blindness();
      Assert.That(effect.IsValid, Is.True, "Effect was not valid after creation.");

      CGameEffect gameEffect = effect;
      Assert.That(gameEffect, Is.Not.Null, "Native effect was not valid after implicit cast.");

      Effect softReference = gameEffect.ToEffect(false)!;
      softReference.Dispose();
      Assert.That(softReference.IsValid, Is.True, "The soft reference disposed the memory of the original effect.");
    }
  }
}
