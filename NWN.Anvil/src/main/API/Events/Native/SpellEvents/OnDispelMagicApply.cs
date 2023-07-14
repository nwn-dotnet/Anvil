using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  /// <summary>
  /// Called when a <see cref="API.Effect.DispelMagicAll"/> or <see cref="API.Effect.DispelMagicBest"/> effect is applied to an object.
  /// </summary>
  public sealed class OnDispelMagicApply : IEvent
  {
    /// <summary>
    /// Gets the object who is having spell effects dispelled.
    /// </summary>
    public NwGameObject Object { get; private init; } = null!;

    /// <summary>
    /// Gets the type of dispel effect.
    /// </summary>
    public DispelMagicType Type { get; private init; }

    /// <summary>
    /// Gets the dispel effect.
    /// </summary>
    public Effect Effect { get; private init; } = null!;

    /// <summary>
    /// Gets the number of effects dispelled. Only available in the after event.
    /// </summary>
    public int NumEffectsDispelled { get; private set; }

    /// <summary>
    /// Gets or sets if dispelling should be skipped.
    /// </summary>
    public bool Skip { get; set; }

    NwObject IEvent.Context => Object;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSEffectListHandler.OnApplyDispelAllMagic> ApplyDispelAllMagicHook { get; set; } = null!;
      private static FunctionHook<Functions.CNWSEffectListHandler.OnApplyDispelBestMagic> ApplyDispelBestMagic { get; set; } = null!;
      private static FunctionHook<Functions.CNWSMessage.SendServerToPlayerCCMessage> SendServerToPlayerCCMessageHook { get; set; } = null!;

      private static OnDispelMagicApply? pendingEventData;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, void*, int, int> pApplyDispelAllMagicHook = &OnApplyDispelAllMagic;
        delegate* unmanaged<void*, void*, void*, int, int> pApplyDispelBestMagicHook = &OnApplyDispelBestMagic;
        delegate* unmanaged<void*, uint, byte, void*, void*, int> pSendServerToPlayerCCMessageHook = &OnSendServerToPlayerCCMessage;

        ApplyDispelAllMagicHook = HookService.RequestHook<Functions.CNWSEffectListHandler.OnApplyDispelAllMagic>(pApplyDispelAllMagicHook, HookOrder.Early);
        ApplyDispelBestMagic = HookService.RequestHook<Functions.CNWSEffectListHandler.OnApplyDispelBestMagic>(pApplyDispelBestMagicHook, HookOrder.Early);
        SendServerToPlayerCCMessageHook = HookService.RequestHook<Functions.CNWSMessage.SendServerToPlayerCCMessage>(pSendServerToPlayerCCMessageHook, HookOrder.Early);

        return new IDisposable[] { ApplyDispelAllMagicHook, ApplyDispelBestMagic };
      }

      [UnmanagedCallersOnly]
      private static int OnApplyDispelAllMagic(void* pEffectListHandler, void* pObject, void* pEffect, int bLoadingGame)
      {
        pendingEventData = CreateEvent(pObject, pEffect, DispelMagicType.All);
        ProcessEvent(EventCallbackType.Before, pendingEventData);

        int retVal = pendingEventData.Skip ? 1 : ApplyDispelAllMagicHook.CallOriginal(pEffectListHandler, pObject, pEffect, bLoadingGame);

        ProcessEvent(EventCallbackType.After, pendingEventData);
        pendingEventData = null;

        return retVal;
      }

      [UnmanagedCallersOnly]
      private static int OnApplyDispelBestMagic(void* pEffectListHandler, void* pObject, void* pEffect, int bLoadingGame)
      {
        pendingEventData = CreateEvent(pObject, pEffect, DispelMagicType.Best);
        ProcessEvent(EventCallbackType.Before, pendingEventData);

        int retVal = pendingEventData.Skip ? 1 : ApplyDispelBestMagic.CallOriginal(pEffectListHandler, pObject, pEffect, bLoadingGame);

        ProcessEvent(EventCallbackType.After, pendingEventData);
        pendingEventData = null;

        return retVal;
      }

      [UnmanagedCallersOnly]
      private static int OnSendServerToPlayerCCMessage(void* pMessage, uint nPlayerId, byte nMinor, void* pMessageData, void* pAttackData)
      {
        if (nMinor == (int)MessageClientSideMsgMinor.DispelMagic && pendingEventData != null)
        {
          CNWCCMessageData messageData = CNWCCMessageData.FromPointer(pMessageData);
          pendingEventData.NumEffectsDispelled = messageData.GetInteger(0);
        }

        return SendServerToPlayerCCMessageHook.CallOriginal(pMessage, nPlayerId, nMinor, pMessageData, pAttackData);
      }

      private static OnDispelMagicApply CreateEvent(void* pObject, void* pEffect, DispelMagicType type)
      {
        return new OnDispelMagicApply
        {
          Object = CNWSObject.FromPointer(pObject).ToNwObject<NwGameObject>()!,
          Effect = CGameEffect.FromPointer(pEffect).ToEffect(false)!,
          Type = type,
        };
      }
    }
  }
}

namespace Anvil.API
{
  public abstract partial class NwGameObject
  {
    /// <inheritdoc cref="Events.OnDispelMagicApply"/>
    public event Action<OnDispelMagicApply> OnDispelMagicApply
    {
      add => EventService.Subscribe<OnDispelMagicApply, OnDispelMagicApply.Factory>(this, value);
      remove => EventService.Unsubscribe<OnDispelMagicApply, OnDispelMagicApply.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnDispelMagicApply"/>
    public event Action<OnDispelMagicApply> OnDispelMagicApply
    {
      add => EventService.SubscribeAll<OnDispelMagicApply, OnDispelMagicApply.Factory>(value);
      remove => EventService.UnsubscribeAll<OnDispelMagicApply, OnDispelMagicApply.Factory>(value);
    }
  }
}
