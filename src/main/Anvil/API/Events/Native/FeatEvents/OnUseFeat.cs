using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Anvil.API;
using Anvil.Services;
using NWN.API.Events;
using NWN.Native.API;
using Feat = Anvil.API.Feat;

namespace NWN.API.Events
{
  public sealed class OnUseFeat : IEvent
  {
    public bool PreventFeatUse { get; set; }

    public NwCreature Creature { get; private init; }

    public Feat Feat { get; private init; }

    public int SubFeatId { get; private init; }

    public NwGameObject TargetObject { get; private init; }

    public NwArea TargetArea { get; private init; }

    public Vector3 TargetPosition { get; private init; }

    NwObject IEvent.Context
    {
      get => Creature;
    }

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.CreatureUseFeatHook>
    {
      internal delegate int CreatureUseFeatHook(void* pCreature, ushort nFeat, ushort nSubFeat, uint oidTarget, uint oidArea, void* pTargetPos);

      protected override FunctionHook<CreatureUseFeatHook> RequestHook()
      {
        delegate* unmanaged<void*, ushort, ushort, uint, uint, void*, int> pHook = &OnCreatureUseFeat;
        return HookService.RequestHook<CreatureUseFeatHook>(pHook, FunctionsLinux._ZN12CNWSCreature7UseFeatEttjjP6Vector, HookOrder.Earliest);
      }

      [UnmanagedCallersOnly]
      private static int OnCreatureUseFeat(void* pCreature, ushort nFeat, ushort nSubFeat, uint oidTarget, uint oidArea, void* pTargetPos)
      {
        CNWSCreature creature = CNWSCreature.FromPointer(pCreature);

        OnUseFeat eventData = ProcessEvent(new OnUseFeat
        {
          Creature = creature.ToNwObject<NwCreature>(),
          Feat = (Feat)nFeat,
          SubFeatId = nSubFeat,
          TargetObject = oidTarget.ToNwObject<NwGameObject>(),
          TargetArea = oidArea.ToNwObject<NwArea>(),
          TargetPosition = pTargetPos != null ? Marshal.PtrToStructure<Vector3>((IntPtr)pTargetPos) : Vector3.Zero,
        });

        return !eventData.PreventFeatUse ? Hook.CallOriginal(pCreature, nFeat, nSubFeat, oidTarget, oidArea, pTargetPos) : false.ToInt();
      }
    }
  }
}

namespace NWN.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="NWN.API.Events.OnUseFeat"/>
    public event Action<OnUseFeat> OnUseFeat
    {
      add => EventService.Subscribe<OnUseFeat, OnUseFeat.Factory>(this, value);
      remove => EventService.Unsubscribe<OnUseFeat, OnUseFeat.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="NWN.API.Events.OnUseFeat"/>
    public event Action<OnUseFeat> OnUseFeat
    {
      add => EventService.SubscribeAll<OnUseFeat, OnUseFeat.Factory>(value);
      remove => EventService.UnsubscribeAll<OnUseFeat, OnUseFeat.Factory>(value);
    }
  }
}
