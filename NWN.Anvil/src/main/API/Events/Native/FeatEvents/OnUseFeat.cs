using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  /// <summary>
  /// Triggered when a creature attempts to use a feat.
  /// </summary>
  public sealed class OnUseFeat : IEvent
  {
    /// <summary>
    /// Gets the creature attempting to use the feat.
    /// </summary>
    public NwCreature Creature { get; private init; } = null!;

    /// <summary>
    /// Gets the feat being used.
    /// </summary>
    public NwFeat Feat { get; private init; } = null!;

    /// <summary>
    /// Set to true to prevent the feat from being used.
    /// </summary>
    public bool PreventFeatUse { get; set; }

    /// <summary>
    /// Gets the sub-feat id, if applicable.
    /// </summary>
    public int SubFeatId { get; private init; }

    /// <summary>
    /// Gets the targeted area, if applicable.
    /// </summary>
    public NwArea? TargetArea { get; private init; }

    /// <summary>
    /// Gets the targeted object, if applicable.
    /// </summary>
    public NwGameObject? TargetObject { get; private init; }

    /// <summary>
    /// Gets the targeted position, if applicable.
    /// </summary>
    public Vector3 TargetPosition { get; private init; }

    NwObject IEvent.Context => Creature;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSCreature.UseFeat> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, ushort, ushort, uint, uint, void*, int> pHook = &OnCreatureUseFeat;
        Hook = HookService.RequestHook<Functions.CNWSCreature.UseFeat>(pHook, HookOrder.Earliest);
        return [Hook];
      }

      [UnmanagedCallersOnly]
      private static int OnCreatureUseFeat(void* pCreature, ushort nFeat, ushort nSubFeat, uint oidTarget, uint oidArea, void* pTargetPos)
      {
        CNWSCreature creature = CNWSCreature.FromPointer(pCreature);

        OnUseFeat eventData = ProcessEvent(EventCallbackType.Before, new OnUseFeat
        {
          Creature = creature.ToNwObject<NwCreature>()!,
          Feat = NwFeat.FromFeatId(nFeat)!,
          SubFeatId = nSubFeat,
          TargetObject = oidTarget.ToNwObject<NwGameObject>(),
          TargetArea = oidArea.ToNwObject<NwArea>(),
          TargetPosition = pTargetPos != null ? Marshal.PtrToStructure<Vector3>((IntPtr)pTargetPos) : Vector3.Zero,
        });

        int retVal = !eventData.PreventFeatUse ? Hook.CallOriginal(pCreature, nFeat, nSubFeat, oidTarget, oidArea, pTargetPos) : false.ToInt();
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
    /// <inheritdoc cref="Events.OnUseFeat"/>
    public event Action<OnUseFeat> OnUseFeat
    {
      add => EventService.Subscribe<OnUseFeat, OnUseFeat.Factory>(this, value);
      remove => EventService.Unsubscribe<OnUseFeat, OnUseFeat.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnUseFeat"/>
    public event Action<OnUseFeat> OnUseFeat
    {
      add => EventService.SubscribeAll<OnUseFeat, OnUseFeat.Factory>(value);
      remove => EventService.UnsubscribeAll<OnUseFeat, OnUseFeat.Factory>(value);
    }
  }
}
