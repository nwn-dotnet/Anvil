using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnUseFeat : IEvent
  {
    public NwCreature Creature { get; private init; } = null!;

    public NwFeat Feat { get; private init; } = null!;
    public bool PreventFeatUse { get; set; }

    public int SubFeatId { get; private init; }

    public NwArea TargetArea { get; private init; } = null!;

    public NwGameObject TargetObject { get; private init; } = null!;

    public Vector3 TargetPosition { get; private init; }

    NwObject? IEvent.Context => Creature;

    internal sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<CreatureUseFeatHook> Hook { get; set; } = null!;

      private delegate int CreatureUseFeatHook(void* pCreature, ushort nFeat, ushort nSubFeat, uint oidTarget, uint oidArea, void* pTargetPos);

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, ushort, ushort, uint, uint, void*, int> pHook = &OnCreatureUseFeat;
        Hook = HookService.RequestHook<CreatureUseFeatHook>(pHook, FunctionsLinux._ZN12CNWSCreature7UseFeatEttjjP6Vector, HookOrder.Earliest);
        return new IDisposable[] { Hook };
      }

      [UnmanagedCallersOnly]
      private static int OnCreatureUseFeat(void* pCreature, ushort nFeat, ushort nSubFeat, uint oidTarget, uint oidArea, void* pTargetPos)
      {
        CNWSCreature creature = CNWSCreature.FromPointer(pCreature);

        OnUseFeat eventData = ProcessEvent(new OnUseFeat
        {
          Creature = creature.ToNwObject<NwCreature>()!,
          Feat = NwFeat.FromFeatId(nFeat)!,
          SubFeatId = nSubFeat,
          TargetObject = oidTarget.ToNwObject<NwGameObject>()!,
          TargetArea = oidArea.ToNwObject<NwArea>()!,
          TargetPosition = pTargetPos != null ? Marshal.PtrToStructure<Vector3>((IntPtr)pTargetPos) : Vector3.Zero,
        });

        return !eventData.PreventFeatUse ? Hook.CallOriginal(pCreature, nFeat, nSubFeat, oidTarget, oidArea, pTargetPos) : false.ToInt();
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="Events.OnUseFeat"/>
    public event Action<OnUseFeat> OnUseFeat
    {
      add => EventService.Subscribe<OnUseFeat, OnUseFeat.Factory>(this, value);
      remove => EventService.Unsubscribe<OnUseFeat, OnUseFeat.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnUseFeat"/>
    public event Action<OnUseFeat> OnUseFeat
    {
      add => EventService.SubscribeAll<OnUseFeat, OnUseFeat.Factory>(value);
      remove => EventService.UnsubscribeAll<OnUseFeat, OnUseFeat.Factory>(value);
    }
  }
}
