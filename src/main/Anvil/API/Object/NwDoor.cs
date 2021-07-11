using System;
using System.Threading.Tasks;
using Anvil.API;
using NWN.Core;
using NWN.Native.API;
using SavingThrow = Anvil.API.SavingThrow;

namespace NWN.API
{
  [NativeObjectInfo(ObjectTypes.Door, ObjectType.Door)]
  public sealed partial class NwDoor : NwStationary
  {
    internal readonly CNWSDoor Door;

    internal NwDoor(CNWSDoor door) : base(door)
    {
      Door = door;
    }

    public static implicit operator CNWSDoor(NwDoor door)
    {
      return door?.Door;
    }

    public override bool KeyAutoRemoved
    {
      get => Door.m_bAutoRemoveKey.ToBool();
      set => Door.m_bAutoRemoveKey = value.ToInt();
    }

    /// <summary>
    /// Gets or sets the dialog ResRef for this door.
    /// </summary>
    public string DialogResRef
    {
      get => Door.GetDialogResref().ToString();
      set => Door.m_cDialog = new CResRef(value);
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
    {
      return NWScript.GetIsDoorActionPossible(this, (int)action).ToBool();
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

    public override byte[] Serialize()
    {
      return NativeUtils.SerializeGff("UTD", (resGff, resStruct) =>
      {
        Door.SaveObjectState(resGff, resStruct);
        return Door.SaveDoor(resGff, resStruct).ToBool();
      });
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
