using System;
using NWN.Native.API;
using NWN.Services;
using Feat = NWN.API.Constants.Feat;

namespace NWN.API.Events
{
  public class OnDisarm : IEvent
  {
    public bool PreventDisarm { get; set; }

    public NativeEventType EventType { get; private set; }

    public bool Result { get; private set; }

    public NwGameObject DisarmedObject { get; private init; }

    public NwGameObject DisarmedBy { get; private init; }

    public Feat Feat { get; private init; }

    NwObject IEvent.Context => DisarmedObject;

    [NativeFunction(NWNXLib.Functions._ZN21CNWSEffectListHandler13OnApplyDisarmEP10CNWSObjectP11CGameEffecti)]
    internal delegate int ApplyDisarmHook(IntPtr pEffectHandler, IntPtr pObject, IntPtr pEffect, int bLoadingGame);

    internal class Factory : NativeEventFactory<ApplyDisarmHook>
    {
      public Factory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<ApplyDisarmHook> RequestHook(HookService hookService)
        => hookService.RequestHook<ApplyDisarmHook>(OnApplyDisarm, HookOrder.Early);

      private int OnApplyDisarm(IntPtr pEffectHandler, IntPtr pObject, IntPtr pEffect, int bLoadingGame)
      {
        CNWSObject gameObject = new CNWSObject(pObject, false);
        CGameEffect gameEffect = new CGameEffect(pEffect, false);

        OnDisarm eventData = ProcessEvent(new OnDisarm
        {
          EventType = NativeEventType.Before,
          DisarmedObject = gameObject.m_idSelf.ToNwObject<NwGameObject>(),
          DisarmedBy = gameEffect.m_oidCreator.ToNwObject<NwGameObject>(),
          Feat = gameEffect.GetInteger(0) == 1 ? Feat.ImprovedDisarm : Feat.Disarm
        });

        eventData.EventType = NativeEventType.After;
        eventData.Result = !eventData.PreventDisarm && Hook.CallOriginal(pEffectHandler, pObject, pEffect, bLoadingGame).ToBool();
        ProcessEvent(eventData);

        return eventData.Result.ToInt();
      }
    }
  }
}
