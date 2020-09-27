using System;
using NWN.API.Constants;
using NWN.Core;

namespace NWN.API
{
  public partial class Effect
  {
    /// <summary>
    /// Gets the object that created this effect.
    /// </summary>
    public NwObject Creator
    {
      get => NWScript.GetEffectCreator(this).ToNwObject();
    }

    /// <summary>
    /// Gets the type of this effect.
    /// </summary>
    public EffectType EffectType
    {
      get => (EffectType) NWScript.GetEffectType(this);
    }

    /// <summary>
    /// Gets the duration type (Temporary, Instant, Permanent) of this effect.
    /// </summary>
    public EffectDuration DurationType
    {
      get => (EffectDuration) NWScript.GetEffectDurationType(this);
    }

    /// <summary>
    /// Gets the total duration of this effect in seconds. Returns 0 if the duration type is not <see cref="EffectDuration.Temporary"/>.
    /// </summary>
    public float TotalDuration
    {
      get => NWScript.GetEffectDuration(this);
    }

    /// <summary>
    /// Gets the remaining duration of this effect in seconds. Returns 0 if the duration type is not <see cref="EffectDuration.Temporary"/>.
    /// </summary>
    public float DurationRemaining
    {
      get => NWScript.GetEffectDurationRemaining(this);
    }

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
    /// <exception cref="ArgumentOutOfRangeException">Thrown if value is not <see cref="EffectSubType.Magical"/>, <see cref="EffectSubType.Supernatural"/> or <see cref="EffectSubType.Extraordinary"/>.</exception>
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
        VM.FreeGameDefinedStructure(NWScript.ENGINE_STRUCTURE_EFFECT, handle);
      }

      handle = modifiedEffect;
    }
  }
}
