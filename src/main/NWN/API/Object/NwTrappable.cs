using NWN.API.Constants;
using NWN.Core;

namespace NWN.API
{
  public abstract class NwTrappable : NwGameObject
  {
    internal NwTrappable(uint objectId) : base(objectId) {}

    /// <summary>
    /// Gets a value indicating whether this object is trapped.
    /// </summary>
    public bool IsTrapped => NWScript.GetIsTrapped(this).ToBool();

    /// <summary>
    /// Gets or sets a value indicating whether this trap is an active trap. An inactive trap will not trigger when a creature steps on it.
    /// <remarks>Setting a trap as inactive will not make the trap disappear if it has already been detected. Use <see cref="TrapDetectable"/>.</remarks>
    /// </summary>
    public bool TrapActive
    {
      get => NWScript.GetTrapActive(this).ToBool();
      set => NWScript.SetTrapActive(this, value.ToInt());
    }

    /// <summary>
    /// Gets or sets a value indicating whether this trap can be detected.
    /// </summary>
    public bool TrapDetectable
    {
      get => NWScript.GetTrapDetectable(this).ToBool();
      set => NWScript.SetTrapDetectable(this, value.ToInt());
    }

    /// <summary>
    /// Gets or sets a value indicating whether this trap can be disarmed.
    /// </summary>
    public bool TrapDisarmable
    {
      get => NWScript.GetTrapDisarmable(this).ToBool();
      set => NWScript.SetTrapDisarmable(this, value.ToInt());
    }

    /// <summary>
    /// Gets or sets a value indicating whether this trap can be recovered.
    /// </summary>
    public bool TrapRecoverable
    {
      get => NWScript.GetTrapRecoverable(this).ToBool();
      set => NWScript.SetTrapRecoverable(this, value.ToInt());
    }

    /// <summary>
    /// Gets or sets a value indicating whether this trap should not reset after firing (true = don't reset).
    /// </summary>
    public bool OneShotTrap
    {
      get => NWScript.GetTrapOneShot(this).ToBool();
      set => NWScript.SetTrapOneShot(this, value.ToInt());
    }

    /// <summary>
    /// Gets or sets the skill DC required to disarm this trap.
    /// </summary>
    public int TrapDisarmDC
    {
      get => NWScript.GetTrapDisarmDC(this);
      set => NWScript.SetTrapDisarmDC(this, value);
    }

    /// <summary>
    /// Gets or sets the skill DC required to detect this trap.
    /// </summary>
    public int TrapDetectDC
    {
      get => NWScript.GetTrapDetectDC(this);
      set => NWScript.SetTrapDetectDC(this, value);
    }

    /// <summary>
    /// Gets or sets the tag of the key that will disarm this trap.
    /// </summary>
    public string TrapKeyTag
    {
      get => NWScript.GetTrapKeyTag(this);
      set => NWScript.SetTrapKeyTag(this, value);
    }

    /// <summary>
    /// Gets the player that created this trap. If the trap was placed in the toolset, this returns null.
    /// </summary>
    public NwPlayer TrapCreator => NWScript.GetTrapCreator(this).ToNwObject<NwPlayer>();

    /// <summary>
    /// Gets a value indicating whether this trap has been flagged as visible to all creatures in the game.
    /// </summary>
    public bool IsTrapFlagged => NWScript.GetTrapFlagged(this).ToBool();

    /// <summary>
    /// Gets the base type of this trap.
    /// </summary>
    public TrapBaseType TrapBaseType => (TrapBaseType) NWScript.GetTrapBaseType(this);

    /// <summary>
    /// Gets if the specified creature can see this trap.
    /// </summary>
    /// <param name="creature">The creature to check.</param>
    /// <returns>true if the creature can detect this trap, otherwise false.</returns>
    public bool IsTrapDetectedBy(NwCreature creature) => NWScript.GetTrapDetectedBy(this, creature).ToBool();

    /// <summary>
    /// Sets the detected state for this trap for the given creatures.
    /// </summary>
    /// <param name="detected">The new detected state.</param>
    /// <param name="creatures">The creatures to update.</param>
    public void SetTrapDetectedBy(bool detected, params NwCreature[] creatures)
    {
      int iDetected = detected.ToInt();

      foreach (NwCreature creature in creatures)
      {
        NWScript.SetTrapDetectedBy(this, creature, iDetected);
      }
    }

    /// <summary>
    /// Disables this trap as if a creature disarmed it (calling the OnDisarm event respectively).
    /// </summary>
    public void DisableTrap() => NWScript.SetTrapDisabled(this);

    /// <summary>
    /// Determine who last disarmed a trapped trigger, door or placeable object.
    /// </summary>
    public NwTrappable LastDisarmed => (NwTrappable)NWScript.GetLastDisarmed().ToNwObject<NwGameObject>();
  }
}
