using System;
using NWN.API.Constants;
using NWN.Native.API;

namespace NWN.API
{
  public abstract class EffectBase : IDisposable
  {
    protected readonly IntPtr Handle;
    internal readonly CGameEffect Effect;

    public static implicit operator CGameEffect(EffectBase effect) => effect.Effect;

    private protected EffectBase(IntPtr handle, CGameEffect effect)
    {
      this.Handle = handle;
      this.Effect = effect;
    }

    ~EffectBase()
    {
      ReleaseUnmanagedResources();
    }

    public void Dispose()
    {
      ReleaseUnmanagedResources();
      GC.SuppressFinalize(this);
    }

    private protected abstract void ReleaseUnmanagedResources();

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
