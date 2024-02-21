using System;
using System.Numerics;
using System.Threading.Tasks;
using NWN.Core;
using NWN.Native.API;

namespace Anvil.API
{
  /// <summary>
  /// A stationary game object entity.
  /// </summary>
  [ObjectFilter(ObjectTypes.Door | ObjectTypes.Placeable)]
  public abstract class NwStationary : NwTrappable
  {
    internal NwStationary(CNWSObject gameObject) : base(gameObject) {}

    /// <summary>
    /// Gets or sets the hardness of this stationary object. This is the amount of damage deducted from each hit.
    /// </summary>
    public int Hardness
    {
      get => NWScript.GetHardness(this);
      set => NWScript.SetHardness(value, this);
    }

    /// <summary>
    /// Gets a value indicating whether this stationary object is currently open.
    /// </summary>
    public bool IsOpen => NWScript.GetIsOpen(this).ToBool();

    /// <summary>
    /// Gets or sets a value indicating whether the key for this lock should "break"/be removed from the creature's inventory when used on this lock.
    /// </summary>
    public abstract bool KeyAutoRemoved { get; set; }

    /// <summary>
    /// Gets or sets the feedback message that will be displayed when trying to unlock this stationary object.
    /// </summary>
    public string KeyRequiredFeedback
    {
      get => NWScript.GetKeyRequiredFeedback(this);
      set => NWScript.SetKeyRequiredFeedback(this, value);
    }

    public override Location Location => Location.Create(Area!, Position, Rotation);

    /// <summary>
    /// Gets or sets a value indicating whether this stationary object is lockable.
    /// </summary>
    public bool Lockable
    {
      get => NWScript.GetLockLockable(this).ToBool();
      set => NWScript.SetLockLockable(this, value.ToInt());
    }

    /// <summary>
    /// Gets or sets the skill DC required to lock this stationary object.
    /// </summary>
    public int LockDC
    {
      get => NWScript.GetLockLockDC(this);
      set => NWScript.SetLockLockDC(this, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether this stationary object is locked.
    /// </summary>
    public bool Locked
    {
      get => NWScript.GetLocked(this).ToBool();
      set => NWScript.SetLocked(this, value.ToInt());
    }

    /// <summary>
    /// Gets or sets a value indicating whether a specific key with the tag <see cref="LockKeyTag"/> is required to open this stationary object.
    /// </summary>
    public bool LockKeyRequired
    {
      get => NWScript.GetLockKeyRequired(this).ToBool();
      set => NWScript.SetLockKeyRequired(this, value.ToInt());
    }

    /// <summary>
    /// Gets or sets the tag of the key that will open this stationary object.
    /// </summary>
    public string LockKeyTag
    {
      get => NWScript.GetLockKeyTag(this);
      set => NWScript.SetLockKeyTag(this, value);
    }

    /// <summary>
    /// Gets or sets the skill DC required to unlock this stationary object.
    /// </summary>
    public int UnlockDC
    {
      get => NWScript.GetLockUnlockDC(this);
      set => NWScript.SetLockUnlockDC(this, value);
    }

    /// <summary>
    /// Creates the specified trap.
    /// </summary>
    /// <param name="trap">The base type of trap.</param>
    /// <param name="disarm">The script that will fire when the trap is disarmed. If no value set, defaults to an empty string and no script will fire.</param>
    /// <param name="triggered">The script that will fire when the trap is triggered. If no value set, defaults to an empty string and the default OnTrapTriggered script for the trap type specified will fire instead (as specified in the traps.2da).</param>
    public void CreateTrap(TrapBaseType trap, string disarm = "", string triggered = "")
    {
      NWScript.CreateTrapOnObject((int)trap, this, sOnDisarmScript: disarm, sOnTrapTriggeredScript: triggered);
    }

    public override Task FaceToPoint(Vector3 point)
    {
      Vector3 direction = Vector3.Normalize(point - Position);
      return base.FaceToPoint(Position - direction);
    }

    /// <summary>
    /// Gets the object that last locked this stationary object.
    /// </summary>
    public async Task<NwGameObject?> GetLastLockedBy()
    {
      await WaitForObjectContext();
      return NWScript.GetLastLocked().ToNwObject<NwGameObject>();
    }

    /// <summary>
    /// Sets the saving throw value for this stationary object.
    /// </summary>
    /// <param name="savingThrow">The type of saving throw to set.</param>
    /// <param name="amount">The new saving throw value (0-250).</param>
    public void SetSavingThrow(SavingThrow savingThrow, int amount)
    {
      switch (savingThrow)
      {
        case SavingThrow.Fortitude:
          NWScript.SetFortitudeSavingThrow(this, amount);
          break;
        case SavingThrow.Reflex:
          NWScript.SetReflexSavingThrow(this, amount);
          break;
        case SavingThrow.Will:
          NWScript.SetWillSavingThrow(this, amount);
          break;
        case SavingThrow.All:
          NWScript.SetFortitudeSavingThrow(this, amount);
          NWScript.SetReflexSavingThrow(this, amount);
          NWScript.SetWillSavingThrow(this, amount);
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(savingThrow), savingThrow, null);
      }
    }
  }
}
