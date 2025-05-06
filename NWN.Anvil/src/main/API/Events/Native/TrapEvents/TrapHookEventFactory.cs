using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public abstract unsafe class TrapHookEventFactory : HookEventFactory
  {
    protected static T HandleExistingTrapEvent<T>(void* pCreature, void* pNode) where T : TrapEvent, new()
    {
      CNWSCreature creature = CNWSCreature.FromPointer(pCreature);
      CNWSObjectActionNode node = CNWSObjectActionNode.FromPointer(pNode);

      NwGameObject targetObject = ((uint)node.m_pParameter[0]).ToNwObject<NwGameObject>()!;

      bool inRange = creature.GetIsInUseRange(targetObject).ToBool();
      if (!inRange && !creature.m_bTrapAnimationPlayed.ToBool())
      {
        return ProcessEvent(EventCallbackType.Before, new T
        {
          Creature = creature.ToNwObject<NwCreature>()!,
          Trap = targetObject,
          InRange = inRange,
        });
      }

      T eventData = ProcessEvent(EventCallbackType.After, new T
      {
        Creature = creature.ToNwObject<NwCreature>()!,
        Trap = targetObject,
        InRange = inRange,
      });

      eventData.ResultOverride = null; // Cannot skip after events.
      return eventData;
    }
  }
}
