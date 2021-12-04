using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
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
    public NwItem ItemUsed { get; private init; }

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
    public Lazy<ActionState> Result { get; private set; }

    /// <summary>
    /// Gets the object that was targetted with this healing item.
    /// </summary>
    public NwGameObject Target { get; private init; }

    /// <summary>
    /// Gets the creature that used the heal item.
    /// </summary>
    public NwCreature UsedBy { get; private init; }

    NwObject IEvent.Context
    {
      get => UsedBy;
    }

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.AIActionHealHook>
    {
      internal delegate uint AIActionHealHook(void* pCreature, void* pNode);

      protected override FunctionHook<AIActionHealHook> RequestHook()
      {
        delegate* unmanaged<void*, void*, uint> pHook = &OnAIActionHeal;
        return HookService.RequestHook<AIActionHealHook>(pHook, FunctionsLinux._ZN12CNWSCreature12AIActionHealEP20CNWSObjectActionNode, HookOrder.Early);
      }

      [UnmanagedCallersOnly]
      private static uint OnAIActionHeal(void* pCreature, void* pNode)
      {
        CNWSObjectActionNode actionNode = CNWSObjectActionNode.FromPointer(pNode);

        OnHealKitUse eventData = new OnHealKitUse
        {
          UsedBy = CNWSCreature.FromPointer(pCreature).ToNwObject<NwCreature>(),
          Target = ((uint)actionNode.m_pParameter[0].AsULong()).ToNwObject<NwGameObject>(),
          ItemUsed = ((uint)actionNode.m_pParameter[1].AsULong()).ToNwObject<NwItem>(),
          ItemPropertyIndex = (int)actionNode.m_pParameter[2],
          MoveToTarget = ((int)actionNode.m_pParameter[2]).ToBool(),
        };

        eventData.Result = new Lazy<ActionState>(() => !eventData.PreventUse ? (ActionState)Hook.CallOriginal(pCreature, pNode) : ActionState.Failed);
        ProcessEvent(eventData);

        return (uint)eventData.Result.Value;
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
