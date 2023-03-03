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

    private delegate void ComputeGameObjectUpdateForObjectHook(void* pPlayer, void* pPlayerGameObject, void* pGameObjectArray, uint oidObjectToUpdate);

    private readonly FunctionHook<ComputeGameObjectUpdateForObjectHook> computeGameObjectUpdateForObjectHook;

    public PlayerObjectNameOverrideService(HookService hookService)
    {
      Log.Info($"Initialising optional service {nameof(PlayerObjectNameOverrideService)}");
      computeGameObjectUpdateForObjectHook = hookService.RequestHook<ComputeGameObjectUpdateForObjectHook>(OnComputeGameObjectUpdateForObject, FunctionsLinux._ZN11CNWSMessage32ComputeGameObjectUpdateForObjectEP10CNWSPlayerP10CNWSObjectP16CGameObjectArrayj, HookOrder.Early);
    }

    private void OnComputeGameObjectUpdateForObject(void* pPlayer, void* pPlayerGameObject, void* pGameObjectArray, uint oidObjectToUpdate)
    {
      NwGameObject? gameObject = oidObjectToUpdate.ToNwObjectSafe<NwGameObject>();
      NwPlayer? player = CNWSPlayer.FromPointer(pPlayer).ToNwPlayer();

      if (player == null || gameObject == null)
      {
        computeGameObjectUpdateForObjectHook.CallOriginal(pPlayer, pPlayerGameObject, pGameObjectArray, oidObjectToUpdate);
        return;
      }

      InternalVariableString nameOverride = InternalVariables.ObjectNameOverride(player, gameObject);
      if (nameOverride.HasNothing)
      {
        computeGameObjectUpdateForObjectHook.CallOriginal(pPlayer, pPlayerGameObject, pGameObjectArray, oidObjectToUpdate);
        return;
      }

      string originalName = gameObject.Name;

      gameObject.Name = nameOverride.Value!;
      computeGameObjectUpdateForObjectHook.CallOriginal(pPlayer, pPlayerGameObject, pGameObjectArray, oidObjectToUpdate);
      gameObject.Name = originalName;
    }

    public void SetObjectNameOverride(NwPlayer player, NwGameObject gameObject, string name)
    {
      InternalVariables.ObjectNameOverride(player, gameObject).Value = name;
    }

    public void ClearObjectNameOverride(NwPlayer player, NwGameObject gameObject)
    {
      InternalVariables.ObjectNameOverride(player, gameObject).Delete();
    }
  }
}
