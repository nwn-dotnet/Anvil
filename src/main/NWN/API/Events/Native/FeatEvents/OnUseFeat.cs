using System;
using System.Numerics;
using System.Runtime.InteropServices;
using NWN.Native.API;
using NWN.Services;
using Feat = NWN.API.Constants.Feat;

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

    NwObject IEvent.Context => Creature;

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
        CNWSCreature creature = new CNWSCreature(pCreature, false);

        OnUseFeat eventData = ProcessEvent(new OnUseFeat
        {
          Creature = creature.m_idSelf.ToNwObject<NwCreature>(),
          Feat = (Feat)nFeat,
          SubFeatId = nSubFeat,
          TargetObject = oidTarget.ToNwObject<NwGameObject>(),
          TargetArea = oidArea.ToNwObject<NwArea>(),
          TargetPosition = pTargetPos != null ? Marshal.PtrToStructure<Vector3>((IntPtr)pTargetPos) : Vector3.Zero
        });

        return !eventData.PreventFeatUse ? Hook.CallOriginal(pCreature, nFeat, nSubFeat, oidTarget, oidArea, pTargetPos) : false.ToInt();
      }
    }
  }
}
