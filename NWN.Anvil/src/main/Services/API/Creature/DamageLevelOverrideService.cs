using System.Linq;
using Anvil.API;
using NLog;
using NWN.Native.API;

namespace Anvil.Services
{
  [ServiceBinding(typeof(DamageLevelOverrideService))]
  [ServiceBindingOptions(InternalBindingPriority.API, Lazy = true)]
  internal sealed unsafe class DamageLevelOverrideService
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly FunctionHook<GetDamageLevel> damageLevelHook;

    public DamageLevelOverrideService(HookService hookService)
    {
      Log.Info($"Initialising optional service {nameof(DamageLevelOverrideService)}");
      damageLevelHook = hookService.RequestHook<GetDamageLevel>(OnGetDamageLevel, HookOrder.Late);
    }

    [NativeFunction("_ZN10CNWSObject14GetDamageLevelEv", "")]
    private delegate byte GetDamageLevel(void* pObject);

    /// <summary>
    /// Clears any override that is set for the creature's damage level.<br/>
    /// </summary>
    public void ClearDamageLevelOverride(NwCreature creature)
    {
      InternalVariables.DamageLevelOverride(creature).Delete();
    }

    /// <summary>
    /// Gets the override that is set for the creature's damage level.<br/>
    /// </summary>
    public DamageLevelEntry? GetDamageLevelOverride(NwCreature creature)
    {
      InternalVariableInt damageLevelOverride = InternalVariables.DamageLevelOverride(creature);
      if (damageLevelOverride.HasValue)
      {
        return NwGameTables.DamageLevelTable.ElementAtOrDefault(damageLevelOverride.Value);
      }

      return null;
    }

    /// <summary>
    /// Sets the override that is set for the creature's damage level.<br/>
    /// </summary>
    public void SetDamageLevelOverride(NwCreature creature, DamageLevelEntry damageLevel)
    {
      InternalVariables.DamageLevelOverride(creature).Value = damageLevel.RowIndex;
    }

    private byte OnGetDamageLevel(void* pObject)
    {
      NwCreature? creature = CNWSObject.FromPointer(pObject).ToNwObject<NwCreature>();
      if (creature != null)
      {
        DamageLevelEntry? levelOverride = GetDamageLevelOverride(creature);
        if (levelOverride != null)
        {
          return (byte)levelOverride.RowIndex;
        }
      }

      return damageLevelHook.CallOriginal(pObject);
    }
  }
}
