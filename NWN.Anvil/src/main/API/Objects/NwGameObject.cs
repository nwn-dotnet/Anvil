using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Anvil.Services;
using NWN.Core;
using NWN.Native.API;
using Vector3 = System.Numerics.Vector3;
using Vector4 = System.Numerics.Vector4;

namespace Anvil.API
{
  /// <summary>
  /// Base class for all entities in areas.
  /// </summary>
  [ObjectFilter(ObjectTypes.All)]
  public abstract partial class NwGameObject : NwObject
  {
    private readonly CNWSObject gameObject;

    internal CNWSObject GameObject
    {
      get
      {
        AssertObjectValid();
        return gameObject;
      }
    }

    internal NwGameObject(CNWSObject gameObject) : base(gameObject)
    {
      this.gameObject = gameObject;
      VisualTransform = new VisualTransform(this);
    }

    /// <summary>
    /// Gets all effects (permanent and temporary) that are active on this game object.
    /// </summary>
    public IEnumerable<Effect> ActiveEffects
    {
      get
      {
        for (Effect? effect = NWScript.GetFirstEffect(this); effect != null && NWScript.GetIsEffectValid(effect) == true.ToInt(); effect = NWScript.GetNextEffect(this))
        {
          yield return effect;
        }
      }
    }

    /// <summary>
    /// Gets the current animation state of this object.
    /// </summary>
    public AnimationState AnimationState => (AnimationState)gameObject.m_nAnimation;

    /// <summary>
    /// Gets the area this object is currently in.
    /// </summary>
    public NwArea? Area => GameObject.GetArea().ToNwObject<NwArea>();

    /// <summary>
    /// Gets or sets the appearance of this creature.
    /// </summary>
    [Obsolete("Use NwCreature.Appearance instead.")]
    public AppearanceType CreatureAppearanceType
    {
      get => (AppearanceType)NWScript.GetAppearanceType(this);
      set => NWScript.SetCreatureAppearanceType(this, (int)value);
    }

    /// <summary>
    /// Gets or sets the highlight color of this object.
    /// </summary>
    public Color HighlightColor
    {
      get => GameObject.m_vHiliteColor.ToColor();
      set => NWScript.SetObjectHiliteColor(this, value.ToRGBA());
    }

    /// <summary>
    /// Gets or sets the current HP for this object.
    /// </summary>
    public int HP
    {
      get => NWScript.GetCurrentHitPoints(this);
      set => GameObject.m_nCurrentHitPoints = value;
    }

    /// <summary>
    /// Gets a value indicating whether this object is in a conversation.
    /// </summary>
    public bool IsInConversation => NWScript.IsInConversation(this).ToBool();

    /// <summary>
    /// Gets or sets the location of this object.
    /// </summary>
    public virtual Location? Location
    {
      get => NWScript.GetLocation(this);
      set
      {
        if (value == null)
        {
          return;
        }

        NwArea area = value.Area;
        if (area == Area)
        {
          Position = value.Position;
          Rotation = value.Rotation;
        }
        else
        {
          AddToArea(area.Area, value.Position.X, value.Position.Y, value.Position.Z);
          Rotation = value.Rotation;
        }
      }
    }

    /// <summary>
    /// Gets or sets if this object should be listening for chat patterns, via the listen pattern system.
    /// </summary>
    public bool IsListening
    {
      get => NWScript.GetIsListening(this).ToBool();
      set => NWScript.SetListening(this, value.ToInt());
    }

    /// <summary>
    /// Gets or sets the maximum HP for this object. Returns 0 if this object has no defined HP.
    /// </summary>
    public int MaxHP
    {
      get => NWScript.GetMaxHitPoints(this);
      set => GameObject.m_nBaseHitPoints = value;
    }

    /// <summary>
    /// Gets or sets the mouse cursor for this object.
    /// </summary>
    public MouseCursor MouseCursor
    {
      get => (MouseCursor)GameObject.m_nMouseCursor;
      set => NWScript.SetObjectMouseCursor(this, (int)value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the plot flag is enabled.
    /// </summary>
    public bool PlotFlag
    {
      get => NWScript.GetPlotFlag(this).ToBool();
      set => NWScript.SetPlotFlag(this, value.ToInt());
    }

    /// <summary>
    /// Gets or sets the PortraitId of this (game object).
    /// </summary>
    public int PortraitId
    {
      get => NWScript.GetPortraitId(this);
      set => NWScript.SetPortraitId(this, value);
    }

    /// <summary>
    /// Gets or sets the Portrait ResRef for this object.
    /// </summary>
    public string PortraitResRef
    {
      get => NWScript.GetPortraitResRef(this);
      set => NWScript.SetPortraitResRef(this, value);
    }

    /// <summary>
    /// Gets or sets the local area position of this GameObject.
    /// </summary>
    public virtual Vector3 Position
    {
      get => GameObject.m_vPosition.ToManagedVector();
      set => GameObject.SetPosition(value.ToNativeVector(), false.ToInt());
    }

    /// <summary>
    /// Gets or sets the world rotation for this object.
    /// </summary>
    public virtual float Rotation
    {
      get => NWScript.GetFacing(this) % 360;
      set
      {
        float radians = value % 360 * NwMath.DegToRad;
        Vector3 orientation = new Vector3(MathF.Cos(radians), MathF.Sin(radians), 0.0f);
        GameObject.SetOrientation(orientation.ToNativeVector());
      }
    }

    /// <summary>
    /// Gets or sets the transition target for this object.
    /// </summary>
    public NwGameObject? TransitionTarget
    {
      get => NWScript.GetTransitionTarget(this).ToNwObject<NwGameObject>();
      set => NWScript.SetTransitionTarget(this, value);
    }

    /// <summary>
    /// Gets or sets the global visiblity override for this object.
    /// </summary>
    public VisibilityMode VisibilityOverride
    {
      get => ObjectVisibilityService.Value.GetGlobalOverride(this);
      set => ObjectVisibilityService.Value.SetGlobalOverride(this, value);
    }

    /// <summary>
    /// Gets the visual transform for this object.
    /// </summary>
    public VisualTransform VisualTransform { get; }

    internal override CNWSScriptVarTable ScriptVarTable => GameObject.m_ScriptVars;

    /// <summary>
    /// Casts a spell at an object.
    /// </summary>
    /// <param name="spell">The spell to cast.</param>
    /// <param name="target">The target for the spell.</param>
    /// <param name="metaMagic">Metamagic that should be applied to the spell.</param>
    /// <param name="cheat">If true, this object doesn't have to be able to cast the spell.</param>
    /// <param name="domainLevel">Specifies the spell level if the spell is to be cast as a domain spell.</param>
    /// <param name="projectilePathType">The type of projectile path to use for this spell.</param>
    /// <param name="instant">If true, the spell is cast immediately.</param>
    public async Task ActionCastSpellAt(NwSpell spell, NwGameObject target, MetaMagic metaMagic = MetaMagic.Any, bool cheat = false, int domainLevel = 0, ProjectilePathType projectilePathType = ProjectilePathType.Default, bool instant = false)
    {
      await WaitForObjectContext();
      NWScript.ActionCastSpellAtObject(spell.Id, target, (int)metaMagic, cheat.ToInt(), domainLevel, (int)projectilePathType, instant.ToInt());
    }

    /// <summary>
    /// Casts a spell at an location.
    /// </summary>
    /// <param name="spell">The spell to cast.</param>
    /// <param name="target">The target for the spell.</param>
    /// <param name="metaMagic">Metamagic that should be applied to the spell.</param>
    /// <param name="cheat">If true, this object doesn't have to be able to cast the spell.</param>
    /// <param name="projectilePathType">The type of projectile path to use for this spell.</param>
    /// <param name="instant">If true, the spell is cast immediately.</param>
    public async Task ActionCastSpellAt(NwSpell spell, Location target, MetaMagic metaMagic = MetaMagic.Any, bool cheat = false, ProjectilePathType projectilePathType = ProjectilePathType.Default, bool instant = false)
    {
      await WaitForObjectContext();
      NWScript.ActionCastSpellAtLocation(spell.Id, target, (int)metaMagic, cheat.ToInt(), (int)projectilePathType, instant.ToInt());
    }

    /// <summary>
    /// Jumps this object to the specified location.<br/>
    /// Unlike the <see cref="Location"/> setter, this method will compute a safe location when teleporting, and may not equal the location specified.
    /// </summary>
    /// <param name="location">The location to attempt to jump to.</param>
    public async Task ActionJumpToLocation(Location location)
    {
      await WaitForObjectContext();
      NWScript.ActionJumpToLocation(location);
    }

    /// <summary>
    /// Instructs this object to do nothing for the specified duration, before continuing with the next item in the action queue.
    /// </summary>
    /// <param name="duration">The time to wait.</param>
    public async Task ActionWait(TimeSpan duration)
    {
      await WaitForObjectContext();
      NWScript.ActionWait((float)duration.TotalSeconds);
    }

    /// <summary>
    /// Applies the specified effect to this game object.
    /// </summary>
    /// <param name="durationType">The duration type to apply with this effect.</param>
    /// <param name="effect">The effect to apply.</param>
    /// <param name="duration">If duration type is <see cref="EffectDuration.Temporary"/>, the duration of this effect.</param>
    public void ApplyEffect(EffectDuration durationType, Effect effect, TimeSpan duration = default)
    {
      NWScript.ApplyEffectToObject((int)durationType, effect, this, (float)duration.TotalSeconds);
    }

    /// <summary>
    /// Creates a copy of this game object.
    /// </summary>
    /// <param name="location">The location to create the cloned object.</param>
    /// <param name="newTag">A new tag to assign the cloned object.</param>
    /// <param name="copyLocalState">If true, will clone all local variables, effects, action queue and transition info (triggers, doors) for the object.</param>
    /// <returns>The newly cloned copy of the item.</returns>
    public abstract NwGameObject Clone(Location location, string? newTag = null, bool copyLocalState = true);

    /// <summary>
    /// Destroys this object (irrevocably).
    /// </summary>
    /// <param name="delay">Time in seconds until this object should be destroyed.</param>
    [Obsolete("Use the non-delay overload instead, in combination with the scheduler service or async/await.")]
    public void Destroy(float delay)
    {
      NWScript.DestroyObject(this, delay);
    }

    /// <summary>
    /// Destroys this object (irrevocably).
    /// </summary>
    public virtual void Destroy()
    {
      NWScript.DestroyObject(this);
    }

    /// <summary>
    /// Returns the distance to the target.<br/>
    /// If you only need to compare the distance, you can compare the squared distance using <see cref="DistanceSquared"/> to avoid a costly sqrt operation.
    /// </summary>
    /// <param name="target">The other object to calculate distance from.</param>
    /// <returns>The distance in game units, or -1 if the target is in a different area.</returns>
    public float Distance(NwGameObject target)
    {
      if (target.Area != Area)
      {
        return -1.0f;
      }

      return Vector3.Distance(target.Position, Position);
    }

    /// <summary>
    /// Returns the squared distance to the target.
    /// </summary>
    /// <param name="target">The other object to calculate distance from.</param>
    /// <returns>The squared distance in game units, or -1 if the target is in a different area.</returns>
    public float DistanceSquared(NwGameObject target)
    {
      if (target.Area != Area)
      {
        return -1.0f;
      }

      return Vector3.DistanceSquared(target.Position, Position);
    }

    /// <summary>
    /// Immediately ends this GameObject's current conversation.
    /// </summary>
    public async void EndConversation()
    {
      await NwTask.NextFrame();
      GameObject.StopDialog();
    }

    /// <summary>
    /// Rotates this object to face towards target.
    /// </summary>
    /// <param name="target">The target object to face.</param>
    public async Task FaceToObject(NwGameObject target)
    {
      await FaceToPoint(target.Position);
    }

    /// <summary>
    /// Rotates this object to face a position.
    /// </summary>
    /// <param name="point">The position to face towards.</param>
    public virtual async Task FaceToPoint(Vector3 point)
    {
      await WaitForObjectContext();
      NWScript.SetFacingPoint(point);
    }

    /// <summary>
    /// Gets the color for the specified color channel.
    /// </summary>
    /// @note A chart of available colors can be found here: https://nwnlexicon.com/index.php?title=Color_Charts
    /// <param name="colorChannel">The color channel that you want to get the color value of.</param>
    /// <returns>The current color index value of the specified channel.</returns>
    public int GetColor(ColorChannel colorChannel)
    {
      return NWScript.GetColor(this, (int)colorChannel);
    }

    /// <summary>
    /// Returns the creatures closest to this object.
    /// </summary>
    public IEnumerable<NwCreature> GetNearestCreatures()
    {
      return GetNearestCreatures(CreatureTypeFilter.None, CreatureTypeFilter.None, CreatureTypeFilter.None);
    }

    /// <summary>
    /// Returns the creatures closest to this object, matching the specified criteria.
    /// </summary>
    /// <param name="filter1">A filter created using <see cref="CreatureTypeFilter"/>.</param>
    public IEnumerable<NwCreature> GetNearestCreatures(CreatureTypeFilter filter1)
    {
      return GetNearestCreatures(filter1, CreatureTypeFilter.None, CreatureTypeFilter.None);
    }

    /// <summary>
    /// Returns the creatures closest to this object, matching all of the specified criteria.
    /// </summary>
    /// <param name="filter1">A filter created using <see cref="CreatureTypeFilter"/>.</param>
    /// <param name="filter2">A 2nd filter created using <see cref="CreatureTypeFilter"/>.</param>
    public IEnumerable<NwCreature> GetNearestCreatures(CreatureTypeFilter filter1, CreatureTypeFilter filter2)
    {
      return GetNearestCreatures(filter1, filter2, CreatureTypeFilter.None);
    }

    /// <summary>
    /// Returns the creatures closest to this object, matching all of the specified criteria.
    /// </summary>
    /// <param name="filter1">A filter created using <see cref="CreatureTypeFilter"/>.</param>
    /// <param name="filter2">A 2nd filter created using <see cref="CreatureTypeFilter"/>.</param>
    /// <param name="filter3">A 3rd filter created using <see cref="CreatureTypeFilter"/>.</param>
    public IEnumerable<NwCreature> GetNearestCreatures(CreatureTypeFilter filter1, CreatureTypeFilter filter2, CreatureTypeFilter filter3)
    {
      int i;
      uint current;

      for (i = 1, current = NWScript.GetNearestCreature(
          filter1.Key,
          filter1.Value,
          this,
          i,
          filter2.Key,
          filter2.Value,
          filter3.Key,
          filter3.Value);
        current != Invalid;
        i++, current = NWScript.GetNearestCreature(
          filter1.Key,
          filter1.Value,
          this,
          i,
          filter2.Key,
          filter2.Value,
          filter3.Key,
          filter3.Value))
      {
        yield return current.ToNwObject<NwCreature>()!;
      }
    }

    /// <summary>
    /// Gets the nearest object that is of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of object to search.</typeparam>
    public IEnumerable<T> GetNearestObjectsByType<T>() where T : NwGameObject
    {
      int typeFilter = (int)GetObjectFilter<T>();
      int i;
      uint current;

      for (i = 1, current = NWScript.GetNearestObject(typeFilter, this, i); current != Invalid; i++, current = NWScript.GetNearestObject(typeFilter, this, i))
      {
        T? obj = current.ToNwObjectSafe<T>();
        if (obj != null)
        {
          yield return obj;
        }
      }
    }

    /// <summary>
    /// Gets whether this object has a direct line of sight to the specified object (not blocked by any geometry).<br/>
    /// @note This is an expensive function and may degrade performance if used frequently.
    /// </summary>
    /// <param name="target">The target object to perform the line of sight check against.</param>
    /// <returns>true if this object has line of sight on the target, otherwise false.</returns>
    public bool HasLineOfSight(NwGameObject target)
    {
      return NWScript.LineOfSightObject(this, target).ToBool();
    }

    public override Guid? PeekUUID()
    {
      CNWSUUID uid = GameObject.m_pUUID;
      if (!uid.CanCarryUUID())
      {
        return null;
      }

      CExoString uidString = uid.m_uuid;
      return uidString != null ? Guid.Parse(uidString.ToString()) : null;
    }

    /// <summary>
    /// Plays the specified animation.
    /// </summary>
    /// <param name="animation">Constant value representing the animation to play.</param>
    /// <param name="animSpeed">Speed to play the animation.</param>
    /// <param name="queueAsAction">If true, enqueues animation playback in the object's action queue.</param>
    /// <param name="duration">Duration to keep animating. Not used in fire and forget animations.</param>
    public async Task PlayAnimation(Animation animation, float animSpeed, bool queueAsAction = false, TimeSpan duration = default)
    {
      await WaitForObjectContext();
      if (!queueAsAction)
      {
        NWScript.PlayAnimation((int)animation, animSpeed, (float)duration.TotalSeconds);
      }
      else
      {
        NWScript.ActionPlayAnimation((int)animation, animSpeed, (float)duration.TotalSeconds);
      }
    }

    /// <summary>
    /// Plays the specified sound as mono audio from the location of this object.
    /// </summary>
    /// <param name="soundName">The name of the sound to play.</param>
    public async Task PlaySound(string soundName)
    {
      await WaitForObjectContext();
      NWScript.PlaySound(soundName);
    }

    [Obsolete("Use the StrRef overload instead.")]
    public async Task PlaySoundByStrRef(int strRef, bool runAsAction = true)
    {
      await PlaySoundByStrRef(new StrRef(strRef), runAsAction);
    }

    /// <summary>
    /// Plays a sound associated with a string reference (strRef).<br/>
    /// The sound comes out as a mono sound sourcing from the location of the object running the command.<br/>
    /// If runAsAction is False, then the sound is played instantly.
    /// </summary>
    /// <param name="strRef">String reference number of the sound to play.</param>
    /// <param name="runAsAction">Determines if this is an action that can be stacked on the action queue.</param>
    /// <remarks>The strRef values for sounds can be found in the file dialog.tlk in the NWN install directory.</remarks>
    public async Task PlaySoundByStrRef(StrRef strRef, bool runAsAction = true)
    {
      await WaitForObjectContext();
      NWScript.PlaySoundByStrRef((int)strRef.Id, runAsAction.ToInt());
    }

    /// <summary>
    /// Removes the specified effect from this game object.
    /// </summary>
    /// <param name="effect">The existing effect instance.</param>
    public void RemoveEffect(Effect effect)
    {
      NWScript.RemoveEffect(this, effect);
    }

    /// <summary>
    /// Replaces the specified texture with a new texture on this object only.
    /// </summary>
    /// <param name="texture">The texture to be replaced.</param>
    /// <param name="newTexture">The replacement texture.</param>
    public void ReplaceObjectTexture(string texture, string newTexture)
    {
      NWScript.ReplaceObjectTexture(this, texture, newTexture);
    }

    /// <summary>
    /// Resets all material shader parameter overrides on this object.
    /// </summary>
    public void ResetMaterialShaderUniforms()
    {
      NWScript.ResetMaterialShaderUniforms(this);
    }

    /// <summary>
    /// Resets all material shader parameter overrides for the specified material on this object.
    /// <param name="material">The material on the object to be reset.</param>
    /// </summary>
    public void ResetMaterialShaderUniforms(string material)
    {
      NWScript.ResetMaterialShaderUniforms(this, material);
    }

    /// <summary>
    /// Resets the specified material shader parameter override for the specified material.
    /// <param name="material">The material on the object to be reset.</param>
    /// <param name="param">The parameter override to reset.</param>
    /// </summary>
    public void ResetMaterialShaderUniforms(string material, string param)
    {
      NWScript.ResetMaterialShaderUniforms(this, material, param);
    }

    /// <summary>
    /// Performs a saving throw against the given dc.
    /// </summary>
    /// <param name="savingThrow">The type of saving throw to make (Fortitude/Reflex/Will).</param>
    /// <param name="dc">Difficulty class.</param>
    /// <param name="saveType">The sub-type of this save (Mind effect, etc).</param>
    /// <param name="saveVs">The creature this object is making the save against.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if savingThrow is not Fortitude, Reflex, or Will.</exception>
    /// <returns>The result of the saving throw.</returns>
    public SavingThrowResult RollSavingThrow(SavingThrow savingThrow, int dc, SavingThrowType saveType, NwGameObject? saveVs = null)
    {
      return savingThrow switch
      {
        SavingThrow.Fortitude => (SavingThrowResult)NWScript.FortitudeSave(this, dc, (int)saveType, saveVs),
        SavingThrow.Reflex => (SavingThrowResult)NWScript.ReflexSave(this, dc, (int)saveType, saveVs),
        SavingThrow.Will => (SavingThrowResult)NWScript.WillSave(this, dc, (int)saveType, saveVs),
        _ => throw new ArgumentOutOfRangeException(nameof(savingThrow), savingThrow, null),
      };
    }

    public abstract byte[]? Serialize();

    /// <summary>
    /// Sets the color for the specified color channel.
    /// </summary>
    /// @note A chart of available colors can be found here: https://nwnlexicon.com/index.php?title=Color_Charts
    /// <param name="colorChannel">The color channel to modify.</param>
    /// <param name="newColor">The color channel's new color index.</param>
    public void SetColor(ColorChannel colorChannel, int newColor)
    {
      NWScript.SetColor(this, (int)colorChannel, newColor);
    }

    /// <summary>
    /// Rotates this object to face the specified facing angle.
    /// </summary>
    /// <param name="facing">The angle to face.</param>
    public async Task SetFacing(float facing)
    {
      await WaitForObjectContext();
      NWScript.SetFacing(facing);
    }

    /// <summary>
    /// Sets whether this object is destroyable.
    /// </summary>
    /// <param name="destroyable">If false, this creature does not fade out on death, but sticks around as a corpse.</param>
    /// <param name="raiseable">If true, this creature can be raised via resurrection.</param>
    /// <param name="selectableWhenDead">If true, this creature is selectable after death.</param>
    public async Task SetIsDestroyable(bool destroyable, bool raiseable = true, bool selectableWhenDead = false)
    {
      await WaitForObjectContext();
      NWScript.SetIsDestroyable(destroyable.ToInt(), raiseable.ToInt(), selectableWhenDead.ToInt());
    }

    /// <summary>
    /// Sets a listen pattern for this object, assigning a pattern number that is accessible from the related object's OnConversation event.
    /// </summary>
    /// <param name="pattern">The pattern to match the message against. This is not regex, but a custom pattern format. See the remarks for more info.</param>
    /// <param name="patternNumber">The pattern number to assign, on a successful match.</param>
    /// <remarks>
    /// Pattern Format:<br/>
    /// ** will match zero or more characters<br/>
    /// *w one or more whitespace<br/>
    /// *n one or more numeric<br/>
    /// *p one or more punctuation<br/>
    /// *a one or more alphabetic<br/>
    /// | is or<br/>
    /// ( and ) can be used for block.<br/>
    /// <example>
    /// Setting a creature to listen for "**" will match any string<br/>
    /// Telling him to listen for "**funk**" will match any string that contains the word "funk".<br/>
    /// "**(bash|open|unlock)**(chest|door)**" will match strings like "open the door please" or "he just bashed that chest!"
    /// </example>
    /// </remarks>
    public void SetListenPattern(string pattern, int patternNumber)
    {
      NWScript.SetListenPattern(this, pattern, patternNumber);
    }

    /// <summary>
    /// Sets a material shader uniform override.
    /// </summary>
    /// <param name="material">The material on the object to modify.</param>
    /// <param name="param">The parameter to override.</param>
    /// <param name="value">The new parameter value.</param>
    public void SetMaterialShaderUniform(string material, string param, int value)
    {
      NWScript.SetMaterialShaderUniformInt(this, material, param, value);
    }

    /// <summary>
    /// Sets a material shader uniform override.
    /// </summary>
    /// <param name="material">The material on the object to modify.</param>
    /// <param name="param">The parameter to override.</param>
    /// <param name="value">The new parameter value.</param>
    public void SetMaterialShaderUniform(string material, string param, Vector4 value)
    {
      NWScript.SetMaterialShaderUniformVec4(this, material, param, value.X, value.Y, value.Z, value.W);
    }

    /// <summary>
    /// Sets a material shader uniform override.
    /// </summary>
    /// <param name="material">The material on the object to modify.</param>
    /// <param name="param">The parameter to override.</param>
    /// <param name="value">The new parameter value.</param>
    public void SetMaterialShaderUniform(string material, string param, float value)
    {
      NWScript.SetMaterialShaderUniformVec4(this, material, param, value);
    }

    internal abstract void RemoveFromArea();

    private protected abstract void AddToArea(CNWSArea area, float x, float y, float z);

    private protected T CloneInternal<T>(Location location, string? newTag, bool copyLocalState) where T : NwGameObject
    {
      return NWScript.CopyObject(this, location, Invalid, newTag ?? string.Empty, copyLocalState.ToInt()).ToNwObject<T>()!;
    }
  }
}
