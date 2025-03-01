// ReSharper disable ArrangeMethodOrOperatorBody

using Anvil.Services;

namespace Anvil.API
{
  /// <summary>
  /// Internal object variables used by anvil.<br/>
  /// Define new definitions here instead of anvil services to prevent key conflicts.
  /// </summary>
  internal static class InternalVariables
  {
    public static InternalVariableBool AlwaysWalk(NwObject creature) => creature.GetObjectVariable<InternalVariableBool>("ALWAYS_WALK");
    public static InternalVariableInt InitiativeMod(NwObject creature) => creature.GetObjectVariable<InternalVariableInt>("INITIATIVE_MOD");
    public static InternalVariableInt DamageLevelOverride(NwCreature creature) => creature.GetObjectVariable<InternalVariableInt>("DAMAGE_LEVEL");
    public static InternalVariableInt MinEquipLevelOverride(NwItem item) => item.GetObjectVariable<InternalVariableInt>("MINIMUM_EQUIP_LEVEL_OVERRIDE");
    public static InternalVariableEnum<VisibilityMode> GlobalVisibilityOverride(NwObject gameObject) => gameObject.GetObjectVariable<InternalVariableEnum<VisibilityMode>>("VISIBILITY_OVERRIDE");
    public static InternalVariableEnum<VisibilityMode> PlayerVisibilityOverride(NwPlayer player, NwObject targetGameObject) => player.ControlledCreature!.GetObjectVariable<InternalVariableEnum<VisibilityMode>>("VISIBILITY_OVERRIDE" + targetGameObject.ObjectId);
    public static InternalVariableFloat WalkRateCap(NwObject creature) => creature.GetObjectVariable<InternalVariableFloat>("WALK_RATE_CAP");
    public static InternalVariableString ObjectNameOverride(NwPlayer player, NwGameObject gameObject) => gameObject.GetObjectVariable<InternalVariableString>("PLCNO_" + player.LoginCreature!.ObjectId);
  }
}
