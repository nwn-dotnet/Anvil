namespace Anvil.API
{
  public enum DoorOpenState
  {
    /// <summary>
    /// The closed state of the door.
    /// </summary>
    Closed = 0,

    /// <summary>
    /// The forward open state of the door. This is the direction of the arrow as shown in the toolset.
    /// </summary>
    OpenForward = 1,

    /// <summary>
    /// The backward open state of the door. This is the opposite direction of the arrow as shown in the toolset.
    /// </summary>
    OpenBackward = 2,

    /// <summary>
    /// The destroyed state of the door.
    /// </summary>
    Destroyed = 3,
  }
}
