using System.Threading.Tasks;
using NWM.API.Constants;
using NWMX.API.Constants;
using NWN.Core;

namespace NWM.API
{
  [NativeObjectInfo(ObjectType.Door, InternalObjectType.Door)]
  public sealed class NwDoor : NwStationary
  {
    internal NwDoor(uint objectId) : base(objectId) {}

    /// <summary>
    /// Gets or sets the locked state for this door.
    /// </summary>
    public bool Locked
    {
      get => NWScript.GetLocked(this).ToBool();
      set => NWScript.SetLocked(this, value.ToInt());
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
  }
}