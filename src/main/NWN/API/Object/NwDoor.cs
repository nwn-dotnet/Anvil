using System;
using System.Threading.Tasks;
using NWN.API.Constants;
using NWN.Core;
using NWN.Native.API;
using SavingThrow = NWN.API.Constants.SavingThrow;

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

    public override Location Location
    {
      set
      {
        if (value.Area != Area)
        {
          Door.AddToArea(value.Area, value.Position.X, value.Position.Y, value.Position.Z, true.ToInt());

          // If the door is trapped it needs to be added to the area's trap list for it to be detectable by players.
          if (IsTrapped)
          {
            value.Area.Area.m_pTrapList.Add(this);
          }
        }
        else
        {
          Position = value.Position;
        }

        Rotation = value.Rotation;
      }
    }

    public override bool KeyAutoRemoved
    {
      get => Door.m_bAutoRemoveKey.ToBool();
      set => Door.m_bAutoRemoveKey = value.ToInt();
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
        SavingThrow.Fortitude => unchecked((sbyte)Door.m_nFortitudeSave),
        SavingThrow.Reflex => unchecked((sbyte)Door.m_nReflexSave),
        SavingThrow.Will => unchecked((sbyte)Door.m_nWillSave),
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
          Door.m_nFortitudeSave = unchecked((byte)newValue);
          break;
        case SavingThrow.Reflex:
          Door.m_nReflexSave = unchecked((byte)newValue);
          break;
        case SavingThrow.Will:
          Door.m_nWillSave = unchecked((byte)newValue);
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
  }
}
