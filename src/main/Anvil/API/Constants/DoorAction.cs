using NWN.Core;

namespace Anvil.API
{
  public enum DoorAction
  {
    Open = NWScript.DOOR_ACTION_OPEN,
    Unlock = NWScript.DOOR_ACTION_UNLOCK,
    Bash = NWScript.DOOR_ACTION_BASH,
    Ignore = NWScript.DOOR_ACTION_IGNORE,
    Knock = NWScript.DOOR_ACTION_KNOCK,
  }
}
