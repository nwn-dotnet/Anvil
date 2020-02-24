using NWN;

namespace NWM.API
{
  public class NwDoor : NwStationary
  {
    protected internal NwDoor(uint objectId) : base(objectId) {}

    public void DoDoorAction(DoorAction doorAction)
    {
      NWScript.DoDoorAction(this, (int) doorAction);
    }

    public bool IsDoorActionPossible(DoorAction doorAction)
    {
      return NWScript.GetIsDoorActionPossible(this, (int) doorAction).ToBool();
    }
  }

  public enum DoorAction
  {
    Open = 0,
    Unlock = 1,
    Bash = 2,
    Ignore = 3,
    Knock = 4
  }
}