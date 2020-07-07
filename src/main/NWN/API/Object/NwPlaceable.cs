using System.Threading.Tasks;
using NWN.API.Constants;
using NWN.Core;
using NWNX.API.Constants;

namespace NWN.API
{
  [NativeObjectInfo(ObjectType.Placeable, InternalObjectType.Placeable)]
  public sealed class NwPlaceable : NwStationary
  {
    internal NwPlaceable(uint objectId) : base(objectId) {}

    public bool Occupied => NWScript.GetSittingCreature(this) != INVALID;

    public NwCreature SittingCreature => NWScript.GetSittingCreature(this).ToNwObject<NwCreature>();

    /// <summary>
    /// Gets or sets the locked state for this placeable.
    /// </summary>
    public bool Locked
    {
      get => NWScript.GetLocked(this).ToBool();
      set => NWScript.SetLocked(this, value.ToInt());
    }

    /// <summary>
    /// Gets or sets whether a specific key with the tag <see cref="LockKeyTag"/> is required to open this placeable.
    /// </summary>
    public bool LockKeyRequired
    {
      get => NWScript.GetLockKeyRequired(this).ToBool();
      set => NWScript.SetLockKeyRequired(this, value.ToInt());
    }

    /// <summary>
    /// Gets or sets the tag of the key that will open this placeable.
    /// </summary>
    public string LockKeyTag
    {
      get => NWScript.GetLockKeyTag(this);
      set => NWScript.SetLockKeyTag(this, value);
    }

    /// <summary>
    /// Gets or sets whether this placeable should illuminate
    /// </summary>
    public bool Illumination
    {
      get => NWScript.GetPlaceableIllumination(this).ToBool();
      set => NWScript.SetPlaceableIllumination(this, value.ToInt());
    }

    /// <summary>
    /// Gets or sets whether this placeable should be useable (clickable)
    /// </summary>
    public bool Useable
    {
      get => NWScript.GetUseableFlag(this).ToBool();
      set => NWScript.SetUseableFlag(this, value.ToInt());
    }

    /// <summary>
    /// Moves the specified item to the placeable's inventory.
    /// </summary>
    /// <param name="item">The item to add.</param>
    public async Task GiveItem(NwItem item)
    {
      NwObject assignTarget;
      if (item.Possessor != null)
      {
        assignTarget = item.Possessor;
      }
      else
      {
        assignTarget = item.Area;
      }

      if (assignTarget != this)
      {
        await assignTarget.WaitForObjectContext();
        NWScript.ActionGiveItem(item, this);
      }
    }

    public static NwPlaceable Create(string template, Location location, bool useAppearAnim = false, string newTag = "")
    {
      location = Location.Create(location.Area, location.Position, location.FlippedRotation);
      return NwObjectFactory.CreateInternal<NwPlaceable>(template, location, useAppearAnim, newTag);
    }
  }
}