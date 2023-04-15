using System.Collections.Generic;
using Anvil.API;
using Anvil.Native;
using NLog;
using NWN.Native.API;

namespace Anvil.Services
{
  [ServiceBinding(typeof(PlayerLoopingVisualEffectService))]
  [ServiceBindingOptions(InternalBindingPriority.API, Lazy = true)]
  internal sealed unsafe class PlayerLoopingVisualEffectService
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly FunctionHook<Functions.CNWSMessage.ComputeGameObjectUpdateForObject> computeGameObjectUpdateForObjectHook;
    private readonly Dictionary<(NwPlayer, NwGameObject), List<VisualEffectTableEntry>> loopingEffects = new Dictionary<(NwPlayer, NwGameObject), List<VisualEffectTableEntry>>();

    public PlayerLoopingVisualEffectService(HookService hookService)
    {
      Log.Info($"Initialising optional service {nameof(PlayerLoopingVisualEffectService)}");
      computeGameObjectUpdateForObjectHook = hookService.RequestHook<Functions.CNWSMessage.ComputeGameObjectUpdateForObject>(OnComputeGameObjectUpdateForObject, HookOrder.Early);
    }

    public List<VisualEffectTableEntry>? GetLoopingVisualEffects(NwPlayer player, NwGameObject gameObject)
    {
      loopingEffects.TryGetValue((player, gameObject), out List<VisualEffectTableEntry>? entries);
      return entries;
    }

    public void AddLoopingVisualEffect(NwPlayer player, NwGameObject gameObject, VisualEffectTableEntry visualEffect)
    {
      loopingEffects.AddElement((player, gameObject), visualEffect);
    }

    public void ClearLoopingVisualEffects(NwPlayer player, NwGameObject gameObject)
    {
      loopingEffects.Remove((player, gameObject));
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

      List<VisualEffectTableEntry>? effects = GetLoopingVisualEffects(player, gameObject);
      if (effects == null)
      {
        computeGameObjectUpdateForObjectHook.CallOriginal(pMessage, pPlayer, pPlayerGameObject, pGameObjectArray, oidObjectToUpdate);
        return;
      }

      foreach (VisualEffectTableEntry effect in effects)
      {
        gameObject.GameObject.AddLoopingVisualEffect((ushort)effect.RowIndex, NwObject.Invalid, 0);
      }

      computeGameObjectUpdateForObjectHook.CallOriginal(pMessage, pPlayer, pPlayerGameObject, pGameObjectArray, oidObjectToUpdate);

      foreach (VisualEffectTableEntry effect in effects)
      {
        gameObject.GameObject.RemoveLoopingVisualEffect((ushort)effect.RowIndex);
      }
    }
  }
}
