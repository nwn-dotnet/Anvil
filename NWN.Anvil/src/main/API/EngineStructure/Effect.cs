using System;
using NWN.Core;
using NWN.Native.API;

namespace Anvil.API
{
  public sealed partial class Effect : EffectBase
  {
    internal const int DurationMask = 0x7;
    internal const int SubTypeMask = 0x18;

    internal Effect(CGameEffect effect) : base(effect) {}

    /// <summary>
    /// Gets the remaining duration of this effect in seconds. Returns 0 if the duration type is not <see cref="EffectDuration.Temporary"/>.
    /// </summary>
    public float DurationRemaining
    {
      get => NWScript.GetEffectDurationRemaining(this);
    }

    /// <summary>
    /// Gets the duration type (Temporary, Instant, Permanent) of this effect.
    /// </summary>
    public EffectDuration DurationType
    {
      get => (EffectDuration)(Effect.m_nSubType & DurationMask);
    }

    /// <summary>
    /// Gets the type of this effect.
    /// </summary>
    public EffectType EffectType
    {
      get => (EffectType)NWScript.GetEffectType(this);
    }

    /// <summary>
    /// Gets or sets the subtype of this effect.
    /// </summary>
    public EffectSubType SubType
    {
      get => (EffectSubType)(Effect.m_nSubType & SubTypeMask);
      set => Effect.m_nSubType = (ushort)(value | (EffectSubType)DurationType);
    }

    /// <summary>
    /// Gets or sets the tag for this effect.
    /// </summary>
    public string Tag
    {
      get => Effect.m_sCustomTag.ToString();
      set => Effect.SetCustomTag(value.ToExoString());
    }

    /// <summary>
    /// Gets the total duration of this effect in seconds. Returns 0 if the duration type is not <see cref="EffectDuration.Temporary"/>.
    /// </summary>
    public float TotalDuration
    {
      get => NWScript.GetEffectDuration(this);
    }

    protected override int StructureId
    {
      get => NWScript.ENGINE_STRUCTURE_EFFECT;
    }

    public static explicit operator Effect(ItemProperty itemProperty)
    {
      return new Effect(itemProperty);
    }

    public static implicit operator Effect(IntPtr intPtr)
    {
      return new Effect(CGameEffect.FromPointer(intPtr));
    }

    /// <summary>
    /// Creates a new copy of this effect.
    /// </summary>
    public Effect Clone()
    {
      CGameEffect clone = new CGameEffect(true.ToInt());
      clone.CopyEffect(this, false.ToInt());

      return new Effect(clone);
    }
  }
}
