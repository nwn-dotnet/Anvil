using System.Numerics;
using System.Runtime.InteropServices;
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
    /// Gets the creature using the skill.
    /// </summary>
    public NwCreature Creature { get; private init; }

    /// <summary>
    /// Gets the skill that is being used.
    /// </summary>
    public Skill Skill { get; private init; }

    /// <summary>
    /// Gets the SubSkill (if any) that is being used.
    /// </summary>
    public SubSkill SubSkill { get; private init; }

    /// <summary>
    /// Gets the item that is being used, if any.
    /// </summary>
    public NwItem UsedItem { get; private init; }

    /// <summary>
    /// Gets the target object for this skill usage.
    /// </summary>
    public NwGameObject Target { get; private init; }

    /// <summary>
    /// Gets the target position for this skill usage.
    /// </summary>
    public Vector3 TargetPosition { get; private init; }

    /// <summary>
    /// Gets or sets whether usage of this skill should be prevented.
    /// </summary>
    public bool PreventSkillUse { get; set; }

    NwObject IEvent.Context
    {
      get => Creature;
    }

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.UseSkillHook>
    {
      internal delegate int UseSkillHook(void* pCreature, byte nSkill, byte nSubSkill, uint oidTarget, Vector3 vTargetPosition, uint oidArea, uint oidUsedItem, int nActivePropertyIndex);

      protected override FunctionHook<UseSkillHook> RequestHook()
      {
        delegate* unmanaged<void*, byte, byte, uint, Vector3, uint, uint, int, int> pHook = &OnUseSkill;
        return HookService.RequestHook<UseSkillHook>(pHook, FunctionsLinux._ZN12CNWSCreature8UseSkillEhhj6Vectorjji, HookOrder.Early);
      }

      [UnmanagedCallersOnly]
      private static int OnUseSkill(void* pCreature, byte nSkill, byte nSubSkill, uint oidTarget, Vector3 vTargetPosition, uint oidArea, uint oidUsedItem, int nActivePropertyIndex)
      {
        OnUseSkill eventData = ProcessEvent(new OnUseSkill
        {
          // Event Data goes here.
        });

        return Hook.CallOriginal(pCreature, nSkill, nSubSkill, oidTarget, vTargetPosition, oidArea, oidUsedItem, nActivePropertyIndex);
      }
    }
  }
}
