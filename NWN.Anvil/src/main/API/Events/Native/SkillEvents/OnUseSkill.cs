using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using NWN.Native.API;
using Anvil.Services;

namespace Anvil.API.Events
{
  /// <summary>
  /// Called when a creature attempts to use a skill.
  /// </summary>
  public sealed class OnUseSkill : IEvent
  {
    /// <summary>
    /// Gets the area that the skill was used.
    /// </summary>
    public NwArea Area { get; private init; } = null!;

    /// <summary>
    /// Gets the creature using the skill.
    /// </summary>
    public NwCreature Creature { get; private init; } = null!;

    /// <summary>
    /// Gets or sets whether usage of this skill should be prevented.
    /// </summary>
    public bool PreventSkillUse { get; set; }

    /// <summary>
    /// Gets the skill that is being used.
    /// </summary>
    public NwSkill Skill { get; private init; } = null!;

    /// <summary>
    /// Gets the SubSkill (if any) that is being used.
    /// </summary>
    public SubSkill SubSkill { get; private init; }

    /// <summary>
    /// Gets the target object for this skill usage.
    /// </summary>
    public NwGameObject Target { get; private init; } = null!;

    /// <summary>
    /// Gets the target position for this skill usage.
    /// </summary>
    public Vector3 TargetPosition { get; private init; }

    /// <summary>
    /// Gets the item that is being used, if any.
    /// </summary>
    public NwItem UsedItem { get; private init; } = null!;

    NwObject IEvent.Context => Creature;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSCreature.UseSkill> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, byte, byte, uint, Vector3, uint, uint, int, int> pHook = &OnUseSkill;
        Hook = HookService.RequestHook<Functions.CNWSCreature.UseSkill>(pHook, HookOrder.Early);
        return new IDisposable[] { Hook };
      }

      [UnmanagedCallersOnly]
      private static int OnUseSkill(void* pCreature, byte nSkill, byte nSubSkill, uint oidTarget, Vector3 vTargetPosition, uint oidArea, uint oidUsedItem, int nActivePropertyIndex)
      {
        OnUseSkill eventData = ProcessEvent(EventCallbackType.Before, new OnUseSkill
        {
          Creature = CNWSCreature.FromPointer(pCreature).ToNwObject<NwCreature>()!,
          Skill = NwSkill.FromSkillId(nSkill)!,
          SubSkill = (SubSkill)nSubSkill,
          Target = oidTarget.ToNwObject<NwGameObject>()!,
          Area = oidArea.ToNwObject<NwArea>()!,
          UsedItem = oidUsedItem.ToNwObject<NwItem>()!,
          TargetPosition = vTargetPosition,
        });

        int retVal = !eventData.PreventSkillUse ? Hook.CallOriginal(pCreature, nSkill, nSubSkill, oidTarget, vTargetPosition, oidArea, oidUsedItem, nActivePropertyIndex) : false.ToInt();
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
    /// <inheritdoc cref="Events.OnUseSkill"/>
    public event Action<OnUseSkill> OnUseSkill
    {
      add => EventService.Subscribe<OnUseSkill, OnUseSkill.Factory>(this, value);
      remove => EventService.Unsubscribe<OnUseSkill, OnUseSkill.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnUseSkill"/>
    public event Action<OnUseSkill> OnUseSkill
    {
      add => EventService.SubscribeAll<OnUseSkill, OnUseSkill.Factory>(value);
      remove => EventService.UnsubscribeAll<OnUseSkill, OnUseSkill.Factory>(value);
    }
  }
}
