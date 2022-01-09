// ReSharper disable ArrangeMethodOrOperatorBody

using Anvil.API.Events;
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
    public static InternalVariableFloat WalkRateCap(NwObject creature) => creature.GetObjectVariable<InternalVariableFloat>("WALK_RATE_CAP");
    public static InternalVariableEnum<VisibilityMode> GlobalVisibilityOverride(NwObject gameObject) => gameObject.GetObjectVariable<InternalVariableEnum<VisibilityMode>>("VISIBILITY_OVERRIDE");
    public static InternalVariableEnum<VisibilityMode> PlayerVisibilityOverride(NwPlayer player, NwObject targetGameObject) => player.ControlledCreature.GetObjectVariable<InternalVariableEnum<VisibilityMode>>("VISIBILITY_OVERRIDE" + targetGameObject.ObjectId);
  }
}
