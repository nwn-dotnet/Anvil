using System.Numerics;
using NWN.Native.API;

namespace Anvil.API
{
  public abstract class EffectBase : EngineStructure
  {
    /// <summary>
    /// Gets the integer parameters of this effect/item property.
    /// </summary>
    public EffectParams<int> IntParams { get; }

    /// <summary>
    /// Gets the float parameters of this effect/item property.
    /// </summary>
    public EffectParams<float> FloatParams { get; }

    /// <summary>
    /// Gets the string parameters of this effect/item property.
    /// </summary>
    public EffectParams<string> StringParams { get; }

    /// <summary>
    /// Gets the vector parameters of this effect/item property.
    /// </summary>
    public EffectParams<Vector3> VectorParams { get; }

    /// <summary>
    /// Gets the object parameters of this effect/item property.
    /// </summary>
    public EffectParams<NwObject> ObjectParams { get; }

    protected readonly CGameEffect Effect;

    public static implicit operator CGameEffect(EffectBase effect)
    {
      return effect.Effect;
    }

    private protected unsafe EffectBase(CGameEffect effect) : base(effect.Pointer)
    {
      Effect = effect;

      IntParams = new EffectParams<int>(effect.m_nNumIntegers,
        i => effect.m_nParamInteger[i],
        (i, value) => effect.m_nParamInteger[i] = value);
      FloatParams = new EffectParams<float>(effect.m_nParamFloat.Length,
        i => effect.m_nParamFloat[i],
        (i, value) => effect.m_nParamFloat[i] = value);
      StringParams = new EffectParams<string>(6,
        i => effect.m_sParamString[i].ToString(),
        (i, value) => effect.m_sParamString[i] = value.ToExoString());
      VectorParams = new EffectParams<Vector3>(2,
        i => effect.m_vParamVector[i].ToManagedVector(),
        (i, value) => effect.m_vParamVector[i] = value.ToNativeVector());
      ObjectParams = new EffectParams<NwObject>(effect.m_oidParamObjectID.Length,
        i => effect.m_oidParamObjectID[i].ToNwObject(),
        (i, value) => effect.m_oidParamObjectID[i] = value);
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

    /// <summary>
    /// Gets or sets the caster level for this effect/item property.
    /// </summary>
    public int CasterLevel
    {
      get => Effect.m_nCasterLevel;
      set => Effect.m_nCasterLevel = value;
    }

    public bool Expose
    {
      get => Effect.m_bExpose.ToBool();
      set => Effect.m_bExpose = value.ToInt();
    }

    public bool ShowIcon
    {
      get => Effect.m_bShowIcon.ToBool();
      set => Effect.m_bShowIcon = value.ToInt();
    }
  }
}
