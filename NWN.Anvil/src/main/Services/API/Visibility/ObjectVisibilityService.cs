using System;
using Anvil.API;
using NWN.Native.API;

namespace Anvil.Services
{
  [ServiceBinding(typeof(ObjectVisibilityService))]
  [ServiceBindingOptions(Lazy = true)]
  internal sealed unsafe class ObjectVisibilityService
  {
    private const string VisibilityVar = "VISIBILITY_OVERRIDE";

    private readonly FunctionHook<TestObjectVisibleHook> testObjectVisibleHook;

    public ObjectVisibilityService(HookService hookService)
    {
      Console.WriteLine("Service initialized");
      testObjectVisibleHook = hookService.RequestHook<TestObjectVisibleHook>(OnTestObjectVisible, FunctionsLinux._ZN11CNWSMessage17TestObjectVisibleEP10CNWSObjectS1_, HookOrder.Late);
    }

    private delegate int TestObjectVisibleHook(void* pMessage, void* pAreaObject, void* pPlayerGameObject);

    public VisibilityMode GetGlobalOverride(NwGameObject target)
    {
      PersistentVariableInt.Internal value = target.GetObjectVariable<PersistentVariableInt.Internal>(VisibilityVar);
      return value.HasValue ? (VisibilityMode)value.Value : VisibilityMode.Default;
    }

    public VisibilityMode GetPersonalOverride(NwPlayer player, NwObject target)
    {
      PersistentVariableInt.Internal value = player.ControlledCreature.GetObjectVariable<PersistentVariableInt.Internal>(VisibilityVar + target);
      return value.HasValue ? (VisibilityMode)value.Value : VisibilityMode.Default;
    }

    public void SetGlobalOverride(NwGameObject target, VisibilityMode visibilityMode)
    {
      PersistentVariableInt.Internal value = target.GetObjectVariable<PersistentVariableInt.Internal>(VisibilityVar);
      value.Value = (int)visibilityMode;
    }

    public void SetPersonalOverride(NwPlayer player, NwObject target, VisibilityMode visibilityMode)
    {
      PersistentVariableInt.Internal value = player.ControlledCreature.GetObjectVariable<PersistentVariableInt.Internal>(VisibilityVar + target);
      value.Value = (int)visibilityMode;
    }

    private int OnTestObjectVisible(void* pMessage, void* pAreaObject, void* pPlayerGameObject)
    {
      NwGameObject areaObject = CNWSObject.FromPointer(pAreaObject).ToNwObjectSafe<NwGameObject>();
      NwCreature playerGameObject = CNWSObject.FromPointer(pPlayerGameObject).ToNwObjectSafe<NwCreature>();

      if (areaObject == null || playerGameObject == null || areaObject == playerGameObject)
      {
        return testObjectVisibleHook.CallOriginal(pMessage, pAreaObject, pPlayerGameObject);
      }

      NwPlayer controllingPlayer = playerGameObject.ControllingPlayer;
      VisibilityMode personalOverride = GetPersonalOverride(controllingPlayer, areaObject);
      VisibilityMode globalOverride = GetGlobalOverride(areaObject);
      VisibilityMode visibilityOverride = personalOverride != VisibilityMode.Default ? personalOverride : globalOverride != VisibilityMode.Default ? globalOverride : VisibilityMode.Default;

      int retVal;
      switch (visibilityOverride)
      {
        case VisibilityMode.Hidden:
          retVal = false.ToInt();
          break;
        case VisibilityMode.DMOnly:
          retVal = controllingPlayer.IsDM ? testObjectVisibleHook.CallOriginal(pMessage, pAreaObject, pPlayerGameObject) : false.ToInt();
          break;
        case VisibilityMode.AlwaysVisible:
          retVal = true.ToInt();
          break;
        case VisibilityMode.AlwaysVisibleDMOnly:
          retVal = controllingPlayer.IsDM.ToInt();
          break;
        case VisibilityMode.Default:
        case VisibilityMode.Visible:
        default:
          retVal = testObjectVisibleHook.CallOriginal(pMessage, pAreaObject, pPlayerGameObject);
          break;
      }

      return retVal;
    }
  }
}
