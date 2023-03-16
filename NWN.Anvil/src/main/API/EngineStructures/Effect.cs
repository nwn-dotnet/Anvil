using System;
using NWN.Core;
using NWN.Native.API;

namespace Anvil.API
{
  //! ## Examples
  //! @include EffectExamples.cs

  /// <summary>
  /// Represents an effect engine structure.
  /// </summary>
  public sealed partial class Effect : EffectBase
  {
    internal Effect(CGameEffect effect, bool memoryOwn) : base(effect, memoryOwn) {}

    /// <summary>
    /// Gets the remaining duration of this effect in seconds. Returns 0 if the duration type is not <see cref="EffectDuration.Temporary"/>.
    /// </summary>
    public float DurationRemaining => NWScript.GetEffectDurationRemaining(this);

    /// <summary>
    /// Gets or sets the duration type (Temporary, Instant, Permanent) of this effect.
    /// </summary>
    public EffectDuration DurationType
    {
      get => (EffectDuration)Effect.GetDurationType();
      set => Effect.SetDurationType((ushort)value);
    }

    /// <summary>
    /// Gets the type of this effect.
    /// </summary>
    public EffectType EffectType => (EffectType)NWScript.GetEffectType(this);

    /// <summary>
    /// Gets or sets the subtype of this effect.
    /// </summary>
    public EffectSubType SubType
    {
      get => (EffectSubType)Effect.GetSubType();
      set => Effect.SetSubType((ushort)value);
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
    /// Gets the link id of this effect, if it is a linked effect. There is no guarantees about this identifier other than it is unique and the same for all effects linked to it.
    /// </summary>
    public string LinkId => NWScript.GetEffectLinkId(this);

    /// <summary>
    /// Gets or sets if this effect should ignore immunities.
    /// </summary>
    public bool IgnoreImmunity
    {
      get => Effect.m_bIgnoreImmunity.ToBool();
      set => Effect.m_bIgnoreImmunity = value.ToInt();
    }

    /// <summary>
    /// Gets the total duration of this effect in seconds. Returns 0 if the duration type is not <see cref="EffectDuration.Temporary"/>.
    /// </summary>
    public float TotalDuration => NWScript.GetEffectDuration(this);

    protected override int StructureId => NWScript.ENGINE_STRUCTURE_EFFECT;

    public static implicit operator Effect?(IntPtr intPtr)
    {
      return intPtr != IntPtr.Zero ? new Effect(CGameEffect.FromPointer(intPtr), true) : null;
    }

    /// <summary>
    /// Creates a new copy of this effect.
    /// </summary>
    public Effect Clone()
    {
      CGameEffect clone = new CGameEffect(true.ToInt());
      clone.CopyEffect(this, false.ToInt());

      return new Effect(clone, true);
    }
  }
}
