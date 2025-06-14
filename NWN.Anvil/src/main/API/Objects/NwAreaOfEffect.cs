using System;
using System.Collections.Generic;
using Anvil.Native;
using NWN.Core;
using NWN.Native.API;

namespace Anvil.API
{
  /// <summary>
  /// The area/region of an ongoing spell effect or ability.
  /// </summary>
  [ObjectType(ObjectTypes.AreaOfEffect)]
  [ObjectFilter(ObjectTypes.AreaOfEffect)]
  public sealed partial class NwAreaOfEffect : NwGameObject
  {
    private readonly CNWSAreaOfEffectObject areaOfEffect;

    internal CNWSAreaOfEffectObject AreaOfEffect
    {
      get
      {
        AssertObjectValid();
        return areaOfEffect;
      }
    }

    internal NwAreaOfEffect(CNWSAreaOfEffectObject areaOfEffect) : base(areaOfEffect)
    {
      this.areaOfEffect = areaOfEffect;
    }

    public override bool IsValid => NWNXUtils.AsNWSAreaOfEffectObject(NWNXUtils.GetGameObject(ObjectId)) == areaOfEffect.Pointer;

    /// <summary>
    /// Gets the creator of this Area of Effect.
    /// </summary>
    public NwGameObject? Creator => NWScript.GetAreaOfEffectCreator(this).ToNwObject<NwGameObject>();

    /// <summary>
    /// Gets the radius of this area of effect.
    /// </summary>
    public float Radius => AreaOfEffect.m_fRadius;

    /// <summary>
    /// Gets the Area Of Effect duration.
    /// </summary>
    public TimeSpan RemainingDuration => TimeSpan.FromMilliseconds(AreaOfEffect.m_nDuration);

    /// <summary>
    /// Gets the spell from which the Area Of Effect was created.
    /// </summary>
    public NwSpell? Spell => NwSpell.FromSpellId((int)AreaOfEffect.m_nSpellId);

    public static implicit operator CNWSAreaOfEffectObject?(NwAreaOfEffect? areaOfEffect)
    {
      return areaOfEffect?.AreaOfEffect;
    }

    public override NwAreaOfEffect Clone(Location location, string? newTag = null, bool copyLocalState = true)
    {
      throw new NotSupportedException("Area of Effect objects may not be cloned.");
    }

    /// <summary>
    /// Gets all objects of the given type that are currently in this area of effect.
    /// </summary>
    /// <typeparam name="T">The type of objects to return.</typeparam>
    /// <returns>An enumerable containing all objects currently in the effect area.</returns>
    public IEnumerable<T> GetObjectsInEffectArea<T>() where T : NwGameObject
    {
      int typeFilter = (int)GetObjectFilter<T>();
      for (uint obj = NWScript.GetFirstInPersistentObject(this, typeFilter); obj != Invalid; obj = NWScript.GetNextInPersistentObject(this, typeFilter))
      {
        T? gameObject = obj.ToNwObjectSafe<T>();
        if (gameObject != null)
        {
          yield return gameObject;
        }
      }
    }

    /// <summary>
    /// Gets all objects of the given types that are currently in this area of effect.
    /// </summary>
    /// <param name="objectTypes">The types of object to return.</param>
    /// <returns>An enumerable containing all objects currently in the effect area.</returns>
    public IEnumerable<NwGameObject> GetObjectsInEffectArea(ObjectTypes objectTypes)
    {
      int objType = (int)objectTypes;
      for (uint obj = NWScript.GetFirstInPersistentObject(this, objType); obj != Invalid; obj = NWScript.GetNextInPersistentObject(this, objType))
      {
        yield return obj.ToNwObject<NwGameObject>()!;
      }
    }

    public override byte[] Serialize()
    {
      throw new NotSupportedException();
    }

    internal override void RemoveFromArea()
    {
      AreaOfEffect.RemoveFromArea();
    }

    private protected override void AddToArea(CNWSArea area, float x, float y, float z)
    {
      AreaOfEffect.AddToArea(area, x, y, z, true.ToInt());
    }

    /// <summary>
    /// Set the radius of this area of effect.
    /// </summary>
    /// <param name="radius">The new radius of the area of effect.</param>
    public void SetRadius(float radius)
    {
      if (radius > 0 && AreaOfEffect.m_nShape == 0)
      {
        AreaOfEffect.SetShape(0, radius);
      }
    }
  }
}
