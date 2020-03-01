using System.Collections.Generic;
using NWN;

namespace NWM.API
{
  public class NwCreature : NwGameObject
  {
    protected internal NwCreature(uint objectId) : base(objectId) {}

    public int Xp
    {
      get => NWScript.GetXP(this);
      set => NWScript.SetXP(this, value);
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
  }
}