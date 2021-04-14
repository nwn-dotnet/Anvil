using System;
using NWN.API.Constants;
using NWN.Native.API;
using NWN.Services;
using Feat = NWN.API.Constants.Feat;

namespace NWN.API.Events
{
  public sealed class OnSpellBroadcast : IEvent
  {
    public bool PreventSpellCast { get; set; }

    public NwGameObject Caster { get; private init; }

    public Spell Spell { get; private init; }

    public int ClassIndex { get; private init; }

    public Feat Feat { get; private init; }

    NwObject IEvent.Context => Caster;

    [NativeFunction(NWNXLib.Functions._ZN12CNWSCreature18BroadcastSpellCastEjht)]
    internal delegate void BroadcastSpellCastHook(IntPtr pCreature, uint nSpellId, byte nMultiClass, ushort nFeat);

    internal class Factory : NativeEventFactory<BroadcastSpellCastHook>
    {
      public Factory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<BroadcastSpellCastHook> RequestHook(HookService hookService)
        => hookService.RequestHook<BroadcastSpellCastHook>(OnBroadcastSpellCast, HookOrder.Early);

      private void OnBroadcastSpellCast(IntPtr pCreature, uint nSpellId, byte nMultiClass, ushort nFeat)
      {
        CNWSCreature creature = new CNWSCreature(pCreature, false);

        OnSpellBroadcast eventData = ProcessEvent(new OnSpellBroadcast
        {
          Caster = creature.m_idSelf.ToNwObject<NwCreature>(),
          Spell = (Spell)nSpellId,
          ClassIndex = nMultiClass,
          Feat = (Feat)nFeat
        });

        if (!eventData.PreventSpellCast)
        {
          Hook.CallOriginal(pCreature, nSpellId, nMultiClass, nFeat);
        }
      }
    }
  }
}
