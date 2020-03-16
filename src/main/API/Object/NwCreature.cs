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