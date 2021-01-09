using System.Threading.Tasks;
using NWN.API.Constants;
using NWN.Core;
using NWN.Native.API;

namespace NWN.API
{
  [NativeObjectInfo(ObjectTypes.Door, ObjectType.Door)]
  public sealed class NwDoor : NwStationary
  {
    private readonly CNWSDoor door;

    internal NwDoor(uint objectId, CNWSDoor door) : base(objectId, door)
    {
      this.door = door;
    }

    public static implicit operator CNWSDoor(NwDoor door)
    {
      return door?.door;
    }

    public override Location Location
    {
      set
      {
        door.AddToArea(value.Area, value.Position.X, value.Position.Y, value.Position.Z);
        Rotation = value.Rotation;
      }
    }

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
    /// Determines whether the specified action can be performed on this door.
    /// </summary>
    /// <param name="action">The action to check.</param>
    /// <returns>true if the specified action can be performed, otherwise false.</returns>
    public bool IsDoorActionPossible(DoorAction action)
      => NWScript.GetIsDoorActionPossible(this, (int)action).ToBool();
  }
}
