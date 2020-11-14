using System.Threading.Tasks;
using NWN.API.Constants;
using NWN.Core;
using NWNX.API.Constants;

namespace NWN.API
{
  [NativeObjectInfo(ObjectTypes.Door, InternalObjectType.Door)]
  public sealed class NwDoor : NwStationary
  {
    internal NwDoor(uint objectId) : base(objectId) {}

    /// <summary>
    /// Opens this door.
    /// </summary>
    public async Task Open()
    {
      await WaitForObjectContext();
      NWScript.ActionOpenDoor(this);
    }

    /// <summary>
    /// Closes this door.
    /// </summary>
    public async Task Close()
    {
      await WaitForObjectContext();
      NWScript.ActionCloseDoor(this);
    }

    /// <summary>
    /// Determines whether an action can be used on a door.
    /// </summary>
    public bool IsDoorActionPossible(DoorAction action)
      => NWScript.GetIsDoorActionPossible(this, (int)action).ToBool();

    /// <summary>
    /// Perform DoorAction on this (door).
    /// </summary>
    public async Task DoDoorAction(DoorAction action)
    {
      await WaitForObjectContext();
      NWScript.DoDoorAction(this, (int)action);
    }
  }
}
