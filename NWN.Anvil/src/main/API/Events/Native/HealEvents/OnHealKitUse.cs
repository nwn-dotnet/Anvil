using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using NWN.Native.API;
using Anvil.Services;

namespace Anvil.API.Events
{
  /// <summary>
  /// Called when a heal kit is used by a creature.
  /// </summary>
  public sealed class OnHealKitUse : IEvent
  {
    /// <summary>
    /// Gets the item property index on the heal item that was used.
    /// </summary>
    public int ItemPropertyIndex { get; private init; }

    /// <summary>
    /// Gets the heal item that was used.
    /// </summary>
    public NwItem ItemUsed { get; private init; } = null!;

    /// <summary>
    /// Gets if the creature had to move to the target to use the heal item.
    /// </summary>
    public bool MoveToTarget { get; private init; }

    /// <summary>
    /// Gets or sets whether the heal kit should be prevented from being used.
    /// </summary>
    public bool PreventUse { get; set; }

    /// <summary>
    /// Gets if the creature successfully used this healing kit.
    /// </summary>
    public Lazy<ActionState> Result { get; private set; } = null!;

    /// <summary>
    /// Gets the object that was targetted with this healing item.
    /// </summary>
    public NwGameObject Target { get; private init; } = null!;

    /// <summary>
    /// Gets the creature that used the heal item.
    /// </summary>
    public NwCreature UsedBy { get; private init; } = null!;

    NwObject IEvent.Context => UsedBy;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSCreature.AIActionHeal> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, uint> pHook = &OnAIActionHeal;
        Hook = HookService.RequestHook<Functions.CNWSCreature.AIActionHeal>(pHook, HookOrder.Early);
        return [Hook];
      }

      [UnmanagedCallersOnly]
      private static uint OnAIActionHeal(void* pCreature, void* pNode)
      {
        CNWSObjectActionNode actionNode = CNWSObjectActionNode.FromPointer(pNode);

        OnHealKitUse eventData = new OnHealKitUse
        {
          UsedBy = CNWSCreature.FromPointer(pCreature).ToNwObject<NwCreature>()!,
          Target = ((uint)actionNode.m_pParameter[0].AsULong()).ToNwObject<NwGameObject>()!,
          ItemUsed = ((uint)actionNode.m_pParameter[1].AsULong()).ToNwObject<NwItem>()!,
          ItemPropertyIndex = (int)actionNode.m_pParameter[2],
          MoveToTarget = ((int)actionNode.m_pParameter[2]).ToBool(),
        };

        eventData.Result = new Lazy<ActionState>(() => !eventData.PreventUse ? (ActionState)Hook.CallOriginal(pCreature, pNode) : ActionState.Failed);

        ProcessEvent(EventCallbackType.Before, eventData);
        uint retVal = (uint)eventData.Result.Value;
        ProcessEvent(EventCallbackType.After, eventData);

        return retVal;
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="Events.OnHealKitUse"/>
    public event Action<OnHealKitUse> OnHealKitUse
    {
      add => EventService.Subscribe<OnHealKitUse, OnHealKitUse.Factory>(this, value);
      remove => EventService.Unsubscribe<OnHealKitUse, OnHealKitUse.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnHealKitUse"/>
    public event Action<OnHealKitUse> OnHealKitUse
    {
      add => EventService.SubscribeAll<OnHealKitUse, OnHealKitUse.Factory>(value);
      remove => EventService.UnsubscribeAll<OnHealKitUse, OnHealKitUse.Factory>(value);
    }
  }
}
