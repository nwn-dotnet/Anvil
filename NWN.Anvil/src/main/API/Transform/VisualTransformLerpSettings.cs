using System;

namespace Anvil.API
{
  public sealed class VisualTransformLerpSettings
  {
    public TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(2);
    public VisualTransformLerpType LerpType { get; set; } = VisualTransformLerpType.Linear;

    public bool PauseWithGame { get; set; } = true;

    public bool ReturnDestinationTransform { get; set; } = false;

    public ObjectVisualTransformBehavior BehaviorFlags { get; set; } = ObjectVisualTransformBehavior.Default;

    public int Repeats { get; set; } = 0;
  }
}
