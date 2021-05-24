using System;
using NWN.API.Constants;
using NWN.Core;
using NWN.Native.API;
using EffectSubType = NWN.API.Constants.EffectSubType;

namespace NWN.API
{
  public sealed partial class Effect : EffectBase
  {
    internal Effect(IntPtr handle, CGameEffect effect) : base(handle, effect) {}

    public static implicit operator IntPtr(Effect effect) => effect.Handle;

    public static implicit operator Effect(IntPtr intPtr) => new Effect(intPtr, CGameEffect.FromPointer(intPtr));

    public static explicit operator Effect(ItemProperty itemProperty) => new Effect(itemProperty, itemProperty);

    private protected override void ReleaseUnmanagedResources()
    {
      VM.FreeGameDefinedStructure(NWScript.ENGINE_STRUCTURE_EFFECT, Handle);
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
      get => Effect.m_sCustomTag.ToString();
      set => Effect.SetCustomTag(value.ToExoString());
    }

    /// <summary>
    /// Gets or sets the subtype of this effect.
    /// </summary>
    public EffectSubType SubType
    {
      get => (EffectSubType)Effect.m_nSubType;
      set => Effect.m_nSubType = (ushort)value;
    }

    /// <summary>
    /// Creates a new copy of this effect.
    /// </summary>
    public Effect Clone()
    {
      CGameEffect clone = new CGameEffect(true.ToInt());
      clone.CopyEffect(this, false.ToInt());

      return new Effect(clone.Pointer, clone);
    }
  }
}
