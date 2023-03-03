using System;
using Anvil.API;
using NLog;
using NWN.Native.API;

namespace Anvil.Services
{
  [ServiceBinding(typeof(PlayerObjectNameOverrideService))]
  [ServiceBindingOptions(InternalBindingPriority.API, Lazy = true)]
  internal sealed unsafe class PlayerObjectNameOverrideService
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private delegate void ComputeGameObjectUpdateForObjectHook(void* pMessage, void* pPlayer, void* pPlayerGameObject, void* pGameObjectArray, uint oidObjectToUpdate);

    private readonly FunctionHook<ComputeGameObjectUpdateForObjectHook> computeGameObjectUpdateForObjectHook;

    public PlayerObjectNameOverrideService(HookService hookService)
    {
      Log.Info($"Initialising optional service {nameof(PlayerObjectNameOverrideService)}");
      computeGameObjectUpdateForObjectHook = hookService.RequestHook<ComputeGameObjectUpdateForObjectHook>(OnComputeGameObjectUpdateForObject, FunctionsLinux._ZN11CNWSMessage32ComputeGameObjectUpdateForObjectEP10CNWSPlayerP10CNWSObjectP16CGameObjectArrayj, HookOrder.Early);
    }

    public string? GetObjectNameOverride(NwPlayer player, NwGameObject gameObject)
    {
      InternalVariableString nameOverride = InternalVariables.ObjectNameOverride(player, gameObject);
      return nameOverride.HasValue ? nameOverride.Value : null;
    }

    public void SetObjectNameOverride(NwPlayer player, NwGameObject gameObject, string name)
    {
      if (!IsSupportedObjectType(gameObject))
      {
        throw new ArgumentException($"Object type {gameObject.GetType()} is not supported for name overrides.", nameof(gameObject));
      }

      InternalVariables.ObjectNameOverride(player, gameObject).Value = name;
      CLastUpdateObject? lastUpdateObject = player.Player.GetLastUpdateObject(gameObject.ObjectId);
      if (lastUpdateObject != null)
      {
        lastUpdateObject.m_nUpdateDisplayNameSeq--;
      }
    }

    public void ClearObjectNameOverride(NwPlayer player, NwGameObject gameObject)
    {
      InternalVariables.ObjectNameOverride(player, gameObject).Delete();
      CLastUpdateObject? lastUpdateObject = player.Player.GetLastUpdateObject(gameObject.ObjectId);
      if (lastUpdateObject != null)
      {
        lastUpdateObject.m_nUpdateDisplayNameSeq--;
      }
    }

    private void OnComputeGameObjectUpdateForObject(void* pMessage, void* pPlayer, void* pPlayerGameObject, void* pGameObjectArray, uint oidObjectToUpdate)
    {
      NwGameObject? gameObject = oidObjectToUpdate.ToNwObjectSafe<NwGameObject>();
      NwPlayer? player = CNWSPlayer.FromPointer(pPlayer).ToNwPlayer();

      if (player == null || gameObject == null)
      {
        computeGameObjectUpdateForObjectHook.CallOriginal(pMessage, pPlayer, pPlayerGameObject, pGameObjectArray, oidObjectToUpdate);
        return;
      }

      InternalVariableString nameOverride = InternalVariables.ObjectNameOverride(player, gameObject);
      if (nameOverride.HasNothing)
      {
        computeGameObjectUpdateForObjectHook.CallOriginal(pMessage, pPlayer, pPlayerGameObject, pGameObjectArray, oidObjectToUpdate);
        return;
      }

      CExoString? originalName = GetName(gameObject);
      if (originalName == null)
      {
        computeGameObjectUpdateForObjectHook.CallOriginal(pMessage, pPlayer, pPlayerGameObject, pGameObjectArray, oidObjectToUpdate);
        return;
      }

      originalName = new CExoString(originalName);

      SetName(gameObject, nameOverride.Value!.ToExoString());
      computeGameObjectUpdateForObjectHook.CallOriginal(pMessage, pPlayer, pPlayerGameObject, pGameObjectArray, oidObjectToUpdate);
      SetName(gameObject, originalName);
    }

    private CExoString? GetName(NwGameObject gameObject)
    {
      return gameObject switch
      {
        NwCreature creature => creature.Creature.m_sDisplayName,
        NwDoor door => door.Door.m_sDisplayName,
        NwItem item => item.Item.m_sDisplayName,
        NwPlaceable placeable => placeable.Placeable.m_sDisplayName,
        _ => null,
      };
    }

    private void SetName(NwGameObject gameObject, CExoString name)
    {
      if (gameObject is NwCreature creature)
      {
        creature.Creature.m_sDisplayName = name;
      }
      else if (gameObject is NwDoor door)
      {
        door.Door.m_sDisplayName = name;
      }
      else if (gameObject is NwItem item)
      {
        item.Item.m_sDisplayName = name;
      }
      else if (gameObject is NwPlaceable placeable)
      {
        placeable.Placeable.m_sDisplayName = name;
      }
    }

    private bool IsSupportedObjectType(NwGameObject gameObject)
    {
      return gameObject is NwCreature or NwDoor or NwItem or NwPlaceable;
    }
  }
}
