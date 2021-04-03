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

    [NativeFunction(NWNXLib.Functions._ZN12CNWSCreature7UseFeatEttjjP6Vector)]
    internal delegate int CreatureUseFeatHook(IntPtr pCreature, ushort nFeat, ushort nSubFeat, uint targetObj, uint targetArea, IntPtr pTargetPos);

    internal class Factory : NativeEventFactory<CreatureUseFeatHook>
    {
      public Factory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<CreatureUseFeatHook> RequestHook(HookService hookService)
        => hookService.RequestHook<CreatureUseFeatHook>(OnCreatureUseFeat, HookOrder.Earliest);

      private int OnCreatureUseFeat(IntPtr pCreature, ushort nFeat, ushort nSubFeat, uint targetObj, uint targetArea, IntPtr pTargetPos)
      {
        CNWSCreature creature = new CNWSCreature(pCreature, false);

        OnUseFeat eventData = ProcessEvent(new OnUseFeat
        {
          Creature = creature.m_idSelf.ToNwObject<NwCreature>(),
          Feat = (Feat)nFeat,
          SubFeatId = nSubFeat,
          TargetObject = targetObj.ToNwObject<NwGameObject>(),
          TargetArea = targetArea.ToNwObject<NwArea>(),
          TargetPosition = pTargetPos != IntPtr.Zero ? Marshal.PtrToStructure<Vector3>(pTargetPos) : Vector3.Zero
        });

        return !eventData.PreventFeatUse ? Hook.CallOriginal(pCreature, nFeat, nSubFeat, targetObj, targetArea, pTargetPos) : false.ToInt();
      }
    }
  }
}
