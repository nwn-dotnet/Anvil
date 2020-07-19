using System;
using System.Collections.Generic;
using NWN.API.Constants;
using NWN.Core;

namespace NWN.API
{
  public partial class Effect
  {
    private readonly List<Effect> childEffects = new List<Effect>();

    /// <summary>
    /// Gets the object that created this effect.
    /// </summary>
    public NwObject Creator => NWScript.GetEffectCreator(this).ToNwObject();

    /// <summary>
    /// Gets the type of this effect.
    /// </summary>
    public EffectType EffectType => (EffectType) NWScript.GetEffectType(this);

    /// <summary>
    /// Gets the duration type (Temporary, Instant, Permanent) of this effect.
    /// </summary>
    public EffectDuration DurationType => (EffectDuration) NWScript.GetEffectDurationType(this);

    /// <summary>
    /// Gets the total duration of this effect in seconds. Returns 0 if the duration type is not <see cref="EffectDuration.Temporary"/>.
    /// </summary>
    public float TotalDuration => NWScript.GetEffectDuration(this);

    /// <summary>
    /// Gets the remaining duration of this effect in seconds. Returns 0 if the duration type is not <see cref="EffectDuration.Temporary"/>.
    /// </summary>
    public float DurationRemaining => NWScript.GetEffectDurationRemaining(this);

    /// <summary>
    /// Gets or sets the tag for this effect.
    /// </summary>
    public string Tag
    {
      get => NWScript.GetEffectTag(this);
      set => ModifyEffect(() => NWScript.TagEffect(this, value));
    }

    /// <summary>
    /// Gets or sets the subtype of this effect.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public EffectSubType SubType
    {
      get => (EffectSubType) NWScript.GetEffectSubType(this);
      set
      {
        switch (value)
        {
          case EffectSubType.Magical:
            ModifyEffect(() => NWScript.MagicalEffect(this));
            break;
          case EffectSubType.Supernatural:
            ModifyEffect(() => NWScript.SupernaturalEffect(this));
            break;
          case EffectSubType.Extraordinary:
            ModifyEffect(() => NWScript.ExtraordinaryEffect(this));
            break;
          default:
          {
            throw new ArgumentOutOfRangeException(nameof(value), value, null);
          }
        }
      }
    }

    /// <summary>
    /// Links the specified effects, that will also be applied when
    /// </summary>
    /// <param name="effects"></param>
    private void LinkEffects(params Effect[] effects)
    {
      if (effects == null)
      {
        return;
      }

      foreach (Effect effect in effects)
      {
        ModifyEffect(() => NWScript.EffectLinkEffects(effect, this), false);
        childEffects.Add(effect);
      }
    }

    /// <summary>
    /// Creates a new copy of this effect.
    /// </summary>
    public Effect Clone() => new Effect(NWScript.TagEffect(this, Tag));

    private void ModifyEffect(Func<IntPtr> modification, bool freeEffect = true)
    {
      IntPtr modifiedEffect = modification();

      // Handle is unchanged.
      if (modifiedEffect == handle)
      {
        return;
      }

      if (freeEffect)
      {
        Internal.NativeFunctions.FreeEffect(handle);
      }

      handle = modifiedEffect;
    }
  }
}