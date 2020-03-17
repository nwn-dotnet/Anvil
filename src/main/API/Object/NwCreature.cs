using System;
using System.Collections.Generic;
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
    public void BeginRandomWalking()
    {
      ExecuteOnSelf(NWScript.ActionRandomWalk);
    }

    /// <summary>
    /// Tells the creature to walk/run to the specified destination. If the location is invalid or a path cannot be found to it, the command does nothing.
    /// </summary>
    /// <param name="destination">The location to move towards.</param>
    /// <param name="run">If this is true, the creature will run rather than walk</param>
    public void MoveToLocation(Location destination, bool run = false)
    {
      ExecuteOnSelf(() => NWScript.ActionMoveToLocation(destination, run.ToInt()));
    }

    public void MoveToObject(NwObject target, bool run = false, float range = 1.0f)
    {
      ExecuteOnSelf(() => NWScript.ActionMoveToObject(target, run.ToInt(), range));
    }

    public NwCreature Clone(Location location = null, string newTag = null)
    {
      if (location == null)
      {
        location = Location;
      }

      return NWScript.CopyObject(this, location, sNewTag: newTag).ToNwObject<NwCreature>();
    }

    public void GiveItem(NwItem item)
    {
      NWScript.ActionGiveItem(item, this);
    }
  }
}