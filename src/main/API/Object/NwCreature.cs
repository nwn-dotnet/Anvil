using System;
using System.Collections.Generic;
using NWM.API.Constants;
using NWN;

namespace NWM.API
{
  public class NwCreature : NwGameObject
  {
    protected internal NwCreature(uint objectId) : base(objectId) {}
    public static NwCreature Create(string template, Location location, bool useAppearAnim = false, string newTag = "")
    {
      return CreateInternal<NwCreature>(ObjectType.Item, template, location, useAppearAnim, newTag);
    }

    public bool IsDMPossessed
    {
      get => NWScript.GetIsDMPossessed(this).ToBool();
    }

    public NwCreature Master
    {
      get => NWScript.GetMaster(this).ToNwObject<NwCreature>();
    }

    public int Xp
    {
      get => NWScript.GetXP(this);
      set => NWScript.SetXP(this, value);
    }

    public int Gold
    {
      get => NWScript.GetGold(this);
      set
      {
        int diff = value - Gold;
        if (diff == 0)
        {
          return;
        }

        if (diff > 0)
        {
          NWScript.GiveGoldToCreature(this, diff);
        }
        else
        {
          NWScript.TakeGoldFromCreature(Math.Abs(diff), this, true.ToInt());
        }
      }
    }

    public IEnumerable<Effect> ActiveEffects
    {
      get
      {
        for (Effect effect = NWScript.GetFirstEffect(this); NWScript.GetIsEffectValid(effect) == true.ToInt(); effect = NWScript.GetNextEffect(this))
        {
          yield return effect;
        }
      }
    }

    /// <summary>
    /// Applies the specified effect to this creature.
    /// </summary>
    /// <param name="durationType"></param>
    /// <param name="effect"></param>
    /// <param name="duration"></param>
    public void ApplyEffect(EffectDuration durationType, Effect effect, float duration = 0f)
    {
      NWScript.ApplyEffectToObject((int)durationType, effect, this, duration);
    }

    /// <summary>
    /// The creature will generate a random location near its current location
    /// and pathfind to it. This repeats and never ends, which means it is necessary
    /// to call <see cref="NwObject.ClearActionQueue"/> in order to allow a creature to perform any other action
    /// once BeginRandomWalking has been called.
    /// </summary>
    public void StartRandomPatrol()
    {
      ExecuteOnSelf(NWScript.ActionRandomWalk);
    }

    /// <summary>
    /// Causes the calling creature to start attacking the target using whichever weapon is current equipped.
    /// </summary>
    /// <param name="target">The target object to attack.</param>
    /// <param name="passive">If TRUE, the attacker will not move to attack the target. If we have a melee weapon equipped, we will just stand still.</param>
    public void AttackTarget(NwGameObject target, bool passive = false)
    {
      ExecuteOnSelf(() => NWScript.ActionAttack(target, passive.ToInt()));
    }

    /// <summary>
    /// Commands this creature to walk/run to the specified destination. If the location is invalid or a path cannot be found to it, the command does nothing.
    /// </summary>
    /// <param name="destination">The location to move towards.</param>
    /// <param name="run">If this is TRUE, the creature will run rather than walk</param>
    public void MoveToLocation(Location destination, bool run = false)
    {
      ExecuteOnSelf(() => NWScript.ActionMoveToLocation(destination, run.ToInt()));
    }

    /// <summary>
    /// Commands this creature to move to a certain distance from the target object.
    /// If there is no path to the object, this command will do nothing.
    /// </summary>
    /// <param name="target">The object we wish the creature to move to</param>
    /// <param name="run">If this is TRUE, the action subject will run rather than walk</param>
    /// <param name="range">This is the desired distance between the creature and the target object</param>
    public void MoveToObject(NwObject target, bool run = false, float range = 1.0f)
    {
      ExecuteOnSelf(() => NWScript.ActionMoveToObject(target, run.ToInt(), range));
    }

    /// <summary>
    /// Commands this creature to move to a certain distance away from fleeFrom
    /// </summary>
    /// <param name="fleeFrom">The target object we wish the creature to move away from. If fleeFrom is not in the same area as the creature, nothing will happen.</param>
    /// <param name="run">If this is TRUE, the creature will run rather than walk</param>
    /// <param name="range">This is the distance we wish the creature to put between themselves and target</param>
    public void MoveAwayFromObject(NwObject fleeFrom, bool run, float range = 40.0f)
    {
      ExecuteOnSelf(() => NWScript.ActionMoveAwayFromObject(fleeFrom, run.ToInt(), range));
    }

    /// <summary>
    /// Causes the creature to move away or flee from location.
    /// </summary>
    public void MoveAwayFromLocation(Location location, bool run, float range = 40.0f)
    {
      ExecuteOnSelf(() => NWScript.ActionMoveAwayFromLocation(location, run.ToInt(), range));
    }

    public NwCreature Clone(Location location = null, string newTag = null)
    {
      if (location == null)
      {
        location = Location;
      }

      return NWScript.CopyObject(this, location, sNewTag: newTag).ToNwObject<NwCreature>();
    }

    /// <summary>
    /// Adds the specified item to the creature's inventory.
    /// </summary>
    /// <param name="item">The item to add.</param>
    public void GiveItem(NwItem item)
    {
      NWScript.ActionGiveItem(item, this);
    }

    /// <summary>
    /// Get the item possessed by this creature with the tag itemTag
    /// </summary>
    public NwItem FindItemWithTag(string itemTag)
    {
      return NWScript.GetItemPossessedBy(this, itemTag).ToNwObject<NwItem>();
    }

    /// <summary>
    /// Commands the creature to equip the specified item into the given inventory slot.<br/>
    /// Note: If the creature already has an item equipped in the slot specified, it will be unequipped automatically
    /// by the call to EquipItem, and dropped if the creature lacks inventory space.<br/>
    /// In order for EquipItem to succeed the creature must be able to equip the item normally. This means that:<br/>
    /// 1) The item is in the creature's inventory.<br/>
    /// 2) The item must already be identified (if magical).<br/>
    /// 3) The creature has the level required to equip the item (if magical and ILR is on).<br/>
    /// 4) The creature possesses the required feats to equip the item (such as weapon proficiencies).
    /// </summary>
    public void EquipItem(NwItem item, InventorySlot slot)
    {
      ExecuteOnSelf(() => NWScript.ActionEquipItem(item, (int) slot));
    }

    /// <summary>
    /// Commands this creature to unequip the specified item from whatever slot it is currently in.
    /// </summary>
    public void UnequipItem(NwItem item)
    {
      ExecuteOnSelf(() => NWScript.ActionUnequipItem(item));
    }

    /// <summary>
    /// Commands this creature to walk over, and pick up the specified item on the ground.
    /// </summary>
    /// <param name="item">The item to pick up.</param>
    public void PickUpItem(NwItem item)
    {
      ExecuteOnSelf(() => NWScript.ActionPickUpItem(item));
    }

    /// <summary>
    /// Commands this creature to begin placing down an item at its feet.
    /// </summary>
    /// <param name="item">The item to drop.</param>
    public void PutDownItem(NwItem item)
    {
      ExecuteOnSelf(() => NWScript.ActionPutDownItem(item));
    }
  }
}