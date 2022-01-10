using System;
using System.Threading.Tasks;
using NWN.Core;
using NWN.Native.API;

namespace Anvil.API
{
  /// <summary>
  /// A tile-based door.
  /// </summary>
  [NativeObjectInfo(ObjectTypes.Door, ObjectType.Door)]
  public sealed partial class NwDoor : NwStationary
  {
    internal readonly CNWSDoor Door;

    internal NwDoor(CNWSDoor door) : base(door)
    {
      Door = door;
    }

    /// <summary>
    /// Gets or sets the dialog ResRef for this door.
    /// </summary>
    public string DialogResRef
    {
      get => Door.GetDialogResref().ToString();
      set => Door.m_cDialog = new CResRef(value);
    }

    public override bool KeyAutoRemoved
    {
      get => Door.m_bAutoRemoveKey.ToBool();
      set => Door.m_bAutoRemoveKey = value.ToInt();
    }

    public static NwDoor Deserialize(byte[] serialized)
    {
      CNWSDoor door = null;

      bool result = NativeUtils.DeserializeGff(serialized, (resGff, resStruct) =>
      {
        if (!resGff.IsValidGff("UTD"))
        {
          return false;
        }

        door = new CNWSDoor(Invalid);
        if (door.LoadDoor(resGff, resStruct).ToBool())
        {
          door.LoadObjectState(resGff, resStruct);
          GC.SuppressFinalize(door);
          return true;
        }

        door.Dispose();
        return false;
      });

      return result && door != null ? door.ToNwObject<NwDoor>() : null;
    }

    public static implicit operator CNWSDoor(NwDoor door)
    {
      return door?.Door;
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
    /// Gets this door's base save value for the specified saving throw.
    /// </summary>
    /// <param name="savingThrow">The type of saving throw.</param>
    /// <returns>The creature's base saving throw value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if savingThrow is not Fortitude, Reflex, or Will.</exception>
    public int GetBaseSavingThrow(SavingThrow savingThrow)
    {
      return savingThrow switch
      {
        SavingThrow.Fortitude => Door.m_nFortitudeSave.AsSByte(),
        SavingThrow.Reflex => Door.m_nReflexSave.AsSByte(),
        SavingThrow.Will => Door.m_nWillSave.AsSByte(),
        _ => throw new ArgumentOutOfRangeException(nameof(savingThrow), savingThrow, null),
      };
    }

    /// <summary>
    /// Determines whether the specified action can be performed on this door.
    /// </summary>
    /// <param name="action">The action to check.</param>
    /// <returns>true if the specified action can be performed, otherwise false.</returns>
    public bool IsDoorActionPossible(DoorAction action)
    {
      return NWScript.GetIsDoorActionPossible(this, (int)action).ToBool();
    }

    /// <summary>
    /// Opens this door.
    /// </summary>
    public async Task Open()
    {
      await WaitForObjectContext();
      NWScript.ActionOpenDoor(this);
    }

    public override byte[] Serialize()
    {
      return NativeUtils.SerializeGff("UTD", (resGff, resStruct) =>
      {
        Door.SaveObjectState(resGff, resStruct);
        return Door.SaveDoor(resGff, resStruct).ToBool();
      });
    }

    public override NwDoor Clone(Location location, string newTag = null, bool copyLocalState = true)
    {
      return NWScript.CopyObject(this, location, sNewTag: newTag ?? string.Empty, bCopyLocalState: copyLocalState.ToInt()).ToNwObject<NwDoor>();
    }

    /// <summary>
    /// Sets this door's base save value for the specified saving throw.
    /// </summary>
    /// <param name="savingThrow">The type of saving throw.</param>
    /// <param name="newValue">The new base saving throw.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if savingThrow is not Fortitude, Reflex, or Will.</exception>
    public void SetBaseSavingThrow(SavingThrow savingThrow, sbyte newValue)
    {
      switch (savingThrow)
      {
        case SavingThrow.Fortitude:
          Door.m_nFortitudeSave = newValue.AsByte();
          break;
        case SavingThrow.Reflex:
          Door.m_nReflexSave = newValue.AsByte();
          break;
        case SavingThrow.Will:
          Door.m_nWillSave = newValue.AsByte();
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(savingThrow), savingThrow, null);
      }
    }

    /// <summary>
    /// Creates a door at the specified location.
    /// </summary>
    /// <param name="template">The door resref template from the toolset palette.</param>
    /// <param name="location">The location where this door will spawn.</param>
    /// <param name="newTag">The new tag to assign this door. Leave uninitialized/as null to use the template's tag.</param>
    public static NwDoor Create(string template, Location location, string newTag = null)
    {
      if (string.IsNullOrEmpty(template))
      {
        return default;
      }

      CNWSArea area = location.Area.Area;
      Vector position = location.Position.ToNativeVector();
      Vector orientation = location.Rotation.ToVectorOrientation().ToNativeVector();

      CNWSDoor door = null;
      bool result = NativeUtils.CreateFromResRef(ResRefType.UTD, template, (resGff, resStruct) =>
      {
        door = new CNWSDoor();
        GC.SuppressFinalize(door);

        door.m_sTemplate = template.ToExoString();
        door.LoadDoor(resGff, resStruct);
        door.LoadVarTable(resGff, resStruct);

        door.SetPosition(position);
        door.SetOrientation(orientation);

        if (!string.IsNullOrEmpty(newTag))
        {
          door.m_sTag = newTag.ToExoString();
          NwModule.Instance.Module.AddObjectToLookupTable(door.m_sTag, door.m_idSelf);
        }

        door.AddToArea(area, position.x, position.y, area.ComputeHeight(position));
      });

      return result && door != null ? door.ToNwObject<NwDoor>() : null;
    }

    internal override void RemoveFromArea()
    {
      Door.RemoveFromArea();
    }

    private protected override void AddToArea(CNWSArea area, float x, float y, float z)
    {
      Door.AddToArea(area, x, y, z, true.ToInt());

      // If the door is trapped it needs to be added to the area's trap list for it to be detectable by players.
      if (IsTrapped)
      {
        area.m_pTrapList.Add(this);
      }
    }
  }
}
