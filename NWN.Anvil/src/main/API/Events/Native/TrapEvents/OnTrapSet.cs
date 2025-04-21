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
  /// Called when a creature attempts to set a trap.
  /// </summary>
  public sealed class OnTrapSet : IEvent
  {
    /// <summary>
    /// Gets if the creature is in range of the trap.
    /// </summary>
    public bool InRange { get; internal init; }

    /// <summary>
    /// Gets the creature performing the trap action.
    /// </summary>
    public NwCreature Creature { get; internal init; } = null!;

    /// <summary>
    /// Gets the target object for the trap if the trap is set on a door/placeable.
    /// </summary>
    /// <remarks>This property is null if the trap is set on the ground as a new trigger.</remarks>
    public NwGameObject? TargetObject { get; internal init; }

    /// <summary>
    /// Gets the location where the trap will be set.
    /// </summary>
    public Location TargetLocation { get; internal init; } = null!;

    /// <summary>
    /// Gets or sets a value to override the trap action result, skipping the default game behaviour.<br/>
    /// Supported values: <see cref="ActionState.Complete"/>, <see cref="ActionState.Failed"/>.
    /// </summary>
    public ActionState? ResultOverride { get; set; }

    /// <summary>
    /// Gets the result of this trap event. This value is only valid in the after event.
    /// </summary>
    public ActionState Result { get; internal set; }

    NwObject IEvent.Context => Creature;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSCreature.AIActionSetTrap> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, uint> pHook = &OnAIActionSetTrap;
        Hook = HookService.RequestHook<Functions.CNWSCreature.AIActionSetTrap>(pHook, HookOrder.Early);
        return [Hook];
      }

      [UnmanagedCallersOnly]
      private static uint OnAIActionSetTrap(void* pCreature, void* pNode)
      {
        OnTrapSet eventData = HandleTrapSetEvent(pCreature, pNode);
        if (eventData.ResultOverride is not (ActionState.Complete or ActionState.Failed))
        {
          return Hook.CallOriginal(pCreature, pNode);
        }

        eventData.Result = eventData.ResultOverride.Value;
        ProcessEvent(EventCallbackType.After, eventData);

        return (uint)eventData.Result;
      }

      private static OnTrapSet HandleTrapSetEvent(void* pCreature, void* pNode)
      {
        CNWSCreature cCreature = CNWSCreature.FromPointer(pCreature);
        CNWSObjectActionNode node = CNWSObjectActionNode.FromPointer(pNode);
        NwCreature creature = cCreature.ToNwObject<NwCreature>()!;
        NativeArray<long> nodeParams = node.m_pParameter;

        bool inRange;
        NwObject? targetObject = ((uint)nodeParams[1]).ToNwObject<NwGameObject>();
        Location targetLocation;

        if (targetObject is NwGameObject gameObject)
        {
          inRange = cCreature.GetIsInUseRange(gameObject, 0.5f, false.ToInt()).ToBool();
          targetLocation = gameObject.Location!;
        }
        else
        {
          Vector3 targetPosition = new Vector3(BitConverter.Int32BitsToSingle((int)nodeParams[2]),
            BitConverter.Int32BitsToSingle((int)nodeParams[3]),
            BitConverter.Int32BitsToSingle((int)nodeParams[4]));

          inRange = Vector3.DistanceSquared(creature.Position, targetPosition) < 1.5f * 1.5f;
          targetLocation = Location.Create(targetObject as NwArea ?? creature.Area!, targetPosition, creature.Rotation);
        }

        if (!inRange && !cCreature.m_bTrapAnimationPlayed.ToBool())
        {
          return ProcessEvent(EventCallbackType.Before, new OnTrapSet
          {
            Creature = creature,
            TargetObject = targetObject as NwGameObject,
            TargetLocation = targetLocation,
            InRange = inRange,
          });
        }

        OnTrapSet eventData = ProcessEvent(EventCallbackType.After, new OnTrapSet
        {
          Creature = creature,
          TargetObject = targetObject as NwGameObject,
          InRange = inRange,
        });

        eventData.ResultOverride = null; // Cannot skip after events.
        return eventData;
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="Events.OnTrapSet"/>
    public event Action<OnTrapSet> OnTrapSet
    {
      add => EventService.Subscribe<OnTrapSet, OnTrapSet.Factory>(this, value);
      remove => EventService.Unsubscribe<OnTrapSet, OnTrapSet.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnTrapSet"/>
    public event Action<OnTrapSet> OnTrapSet
    {
      add => EventService.SubscribeAll<OnTrapSet, OnTrapSet.Factory>(value);
      remove => EventService.UnsubscribeAll<OnTrapSet, OnTrapSet.Factory>(value);
    }
  }
}
