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

    [Test(Description = "Creating an effect and waiting for garbage collection frees the associated memory.")]
    [Timeout(10000)]
    public void CreateAndGarbageCollectEffectFreesNativeStructure()
    {
      GetWeakEffectReference(out IntPtr effectPtr, out WeakReference effectRef);

      while (effectRef.IsAlive)
      {
        GC.Collect();
        GC.WaitForPendingFinalizers();
      }

      Assert.That(NWScript.GetIsEffectValid(effectPtr).ToBool(), "Effect was still valid after garbage collection.");
    }

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    private void GetWeakEffectReference(out IntPtr effectPtr, out WeakReference effectRef)
    {
      Effect effect = Effect.CutsceneParalyze();
      effectPtr = new IntPtr((long)(IntPtr)effect);
      effectRef = new WeakReference(effect);
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
