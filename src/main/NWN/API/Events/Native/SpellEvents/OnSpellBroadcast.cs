using System.Runtime.InteropServices;
using NWN.API.Constants;
using NWN.Native.API;
using NWN.Services;
using Feat = NWN.API.Constants.Feat;

namespace NWN.API.Events
{
  public sealed class OnSpellBroadcast : IEvent
  {
    public bool PreventSpellCast { get; set; }

    public NwCreature Caster { get; private init; }

    public Spell Spell { get; private init; }

    public int ClassIndex { get; private init; }

    public Feat Feat { get; private init; }

    NwObject IEvent.Context
    {
      get => Caster;
    }

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.BroadcastSpellCastHook>
    {
      internal delegate void BroadcastSpellCastHook(void* pCreature, uint nSpellId, byte nMultiClass, ushort nFeat);

      protected override FunctionHook<BroadcastSpellCastHook> RequestHook()
      {
        delegate* unmanaged<void*, uint, byte, ushort, void> pHook = &OnBroadcastSpellCast;
        return HookService.RequestHook<BroadcastSpellCastHook>(pHook, FunctionsLinux._ZN12CNWSCreature18BroadcastSpellCastEjht, HookOrder.Early);
      }

      [UnmanagedCallersOnly]
      private static void OnBroadcastSpellCast(void* pCreature, uint nSpellId, byte nMultiClass, ushort nFeat)
      {
        CNWSCreature creature = CNWSCreature.FromPointer(pCreature);

        OnSpellBroadcast eventData = ProcessEvent(new OnSpellBroadcast
        {
          Caster = creature.ToNwObject<NwCreature>(),
          Spell = (Spell)nSpellId,
          ClassIndex = nMultiClass,
          Feat = (Feat)nFeat,
        });

        if (!eventData.PreventSpellCast)
        {
          Hook.CallOriginal(pCreature, nSpellId, nMultiClass, nFeat);
        }
      }
    }
  }
}
