using System;
using NWN.API.Constants;
using NWN.API.EngineStructures;
using NWN.Native.API;

namespace NWN.API
{
  public abstract class EffectBase : EngineStructure
  {
    protected readonly CGameEffect Effect;

    public static implicit operator CGameEffect(EffectBase effect)
    {
      return effect.Effect;
    }

    private protected EffectBase(IntPtr handle, CGameEffect effect) : base(handle)
    {
      Effect = effect;
    }

    /// <summary>
    /// Gets or sets the creator of this effect/item property.
    /// </summary>
    public NwObject Creator
    {
      get => Effect.m_oidCreator.ToNwObject();
      set => Effect.m_oidCreator = value;
    }

    /// <summary>
    /// Gets or sets the associated spell for this effect/item property.
    /// </summary>
    public Spell Spell
    {
      get => (Spell)Effect.m_nSpellId;
      set => Effect.m_nSpellId = (uint)value;
    }
  }
}
