using NWM.API.Constants;
using NWN;

namespace NWM.API
{
  public sealed class NwDoor : NwStationary
  {
    internal NwDoor(uint objectId) : base(objectId) {}

    public bool IsDoorActionPossible(DoorAction doorAction)
    {
      return NWScript.GetIsDoorActionPossible(this, (int) doorAction).ToBool();
    }
  }
}