using System.Collections.Generic;
using System.Threading.Tasks;
using NWM.API.Constants;
using NWN.Core;
using NWN.Core.NWNX;
using Vector3 = System.Numerics.Vector3;

namespace NWM.API
{
  public abstract class NwGameObject : NwObject
  {
    internal NwGameObject(uint objectId) : base(objectId) {}

    /// <summary>
    /// Gets the location of this object.
    /// </summary>
    public virtual Location Location
    {
      get => NWScript.GetLocation(this);
      set => ObjectPlugin.AddToArea(this, value.Area, value.Position);
    }

    /// <summary>
    /// Gets the area this object is currently in.
    /// </summary>
    public NwArea Area
    {
      get => NWScript.GetArea(this).ToNwObject<NwArea>();
    }

    /// <summary>
    /// The local area position of this GameObject.
    /// </summary>
    public Vector3 Position
    {
      get => NWScript.GetPosition(this);
      set => ObjectPlugin.AddToArea(this, Area, Position);
    }

    /// <summary>
    /// The world rotation for this object
    /// </summary>
    public virtual float Rotation
    {
      get => NWScript.GetFacing(this) % 360;
    }

    /// <summary>
    /// Gets or sets the visual transform for this object.
    /// </summary>
    public VisualTransform VisualTransform
    {
      get => new VisualTransform(this);
      set => value?.Apply(this);
    }

    /// <summary>
    /// Sets the rotation of this object.
    /// </summary>
    public virtual async Task SetRotation(float value)
    {
      await WaitForObjectContext();
      NWScript.SetFacing(value % 360);
    }

    /// <summary>
    /// Returns the distance to the target. <br/>
    /// If you only need to compare the distance, you can compare the squared distance using <see cref="SqrDistanceToObject"/>. (calculating squared distance is faster)
    /// </summary>
    /// <param name="target">The other object to calculate distance from.</param>
    /// <returns>The distance in game units, or -1 if this target is in a different area.</returns>
    public float DistanceToObject(NwGameObject target)
    {
      if (target.Area != Area)
      {
        return -1.0f;
      }

      return (target.Position - Position).Length();
    }

    /// <summary>
    /// Returns the squared distance to the target.
    /// </summary>
    /// <param name="target">The other object to calculate distance from.</param>
    /// <returns>The squared distance in game units, or -1 if this target is in a different area.</returns>
    public float SqrDistanceToObject(NwGameObject target)
    {
      if (target.Area != Area)
      {
        return -1.0f;
      }

      return (target.Position - Position).LengthSquared();
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
    /// Gets the current HP for this object.
    /// </summary>
    public int HP => NWScript.GetCurrentHitPoints(this);

    /// <summary>
    /// Gets the maximum HP for this object. Returns 0 if this object has no defined HP.
    /// </summary>
    public int MaxHP => NWScript.GetMaxHitPoints(this);

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
    /// <param name="point"></param>
    public virtual async Task FaceToPoint(Vector3 point)
    {
      await WaitForObjectContext();
      NWScript.SetFacingPoint(point);
    }

    /// <summary>
    /// Gets or sets this object's plot status.
    /// </summary>
    public bool PlotFlag
    {
      get => NWScript.GetPlotFlag(this).ToBool();
      set => NWScript.SetPlotFlag(this, value.ToInt());
    }

    /// <summary>
    /// Gets all items belonging to this object's inventory.
    /// </summary>
    public IEnumerable<NwItem> Items
    {
      get
      {
        for (uint item = NWScript.GetFirstItemInInventory(this); item != INVALID; item = NWScript.GetNextItemInInventory(this))
        {
          yield return item.ToNwObject<NwItem>();
        }
      }
    }

    /// <summary>
    /// Plays the specified animation.
    /// </summary>
    /// <param name="animation">Constant value representing the animation to play.</param>
    /// <param name="animSpeed">Speed to play the animation.</param>
    /// <param name="queueAsAction">If true, enqueues animation playback in the object's action queue.</param>
    /// <param name="duration">Duration to keep animating. Not used in fire and forget animations.</param>
    public async Task PlayAnimation(Animation animation, float animSpeed, bool queueAsAction = false, float duration = 0.0f)
    {
      await WaitForObjectContext();
      if (!queueAsAction)
      {
        NWScript.PlayAnimation((int) animation, animSpeed, duration);
      }
      else
      {
        NWScript.ActionPlayAnimation((int) animation, animSpeed, duration);
      }
    }

    /// <summary>
    /// Instructs this object to speak.
    /// </summary>
    /// <param name="message">The message the object should speak.</param>
    /// <param name="talkVolume">The channel/volume of this message.</param>
    /// <param name="queueAsAction">Whether the object should speak immediately (false), or be queued in the object's action queue (true).</param>
    public async Task SpeakString(string message, TalkVolume talkVolume = TalkVolume.Talk, bool queueAsAction = false)
    {
      await WaitForObjectContext();
      if (!queueAsAction)
      {
        NWScript.SpeakString(message, (int) talkVolume);
      }
      else
      {
        NWScript.ActionSpeakString(message, (int) talkVolume);
      }
    }

    /// <summary>
    /// Returns the creatures closest to this object.
    /// </summary>
    public IEnumerable<NwCreature> GetNearestCreatures() =>
      GetNearestCreatures(CreatureTypeFilter.None, CreatureTypeFilter.None, CreatureTypeFilter.None);

    /// <summary>
    /// Returns the creatures closest to this object, matching the specified criteria.
    /// </summary>
    /// <param name="filter1">A filter created using <see cref="CreatureTypeFilter"/>.*</param>
    public IEnumerable<NwCreature> GetNearestCreatures(CreatureTypeFilter filter1) =>
      GetNearestCreatures(filter1, CreatureTypeFilter.None, CreatureTypeFilter.None);

    /// <summary>
    /// Returns the creatures closest to this object, matching all of the specified criteria.
    /// </summary>
    /// <param name="filter1">A filter created using <see cref="CreatureTypeFilter"/>.*</param>
    /// <param name="filter2">A filter created using <see cref="CreatureTypeFilter"/>.*</param>
    public IEnumerable<NwCreature> GetNearestCreatures(CreatureTypeFilter filter1, CreatureTypeFilter filter2) =>
      GetNearestCreatures(filter1, filter2, CreatureTypeFilter.None);

    /// <summary>
    /// Returns the creatures closest to this object, matching all of the specified criteria.
    /// </summary>
    /// <param name="filter1">A filter created using <see cref="CreatureTypeFilter"/>.*</param>
    /// <param name="filter2">A filter created using <see cref="CreatureTypeFilter"/>.*</param>
    /// <param name="filter3">A filter created using <see cref="CreatureTypeFilter"/>.*</param>
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
        current != INVALID;
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
        yield return current.ToNwObject<NwCreature>();
      }
    }

    /// <summary>
    /// Gets the nearest object that is of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of object to search.</typeparam>
    public IEnumerable<T> GetNearestObjectsByType<T>() where T : NwGameObject
    {
      int objType = (int) NwObjectFactory.GetObjectType<T>();
      int i;
      uint current;

      for (i = 1, current = NWScript.GetNearestObject(objType, this, i); current != INVALID; i++, current = NWScript.GetNearestObject(objType, this, i))
      {
        T obj = current.ToNwObject<T>();
        if (obj != null)
        {
          yield return obj;
        }
      }
    }

    /// <summary>
    /// Plays the specified sound as mono audio from the location of this object.
    /// </summary>
    /// <param name="soundName"></param>
    public async Task PlaySound(string soundName)
    {
      await WaitForObjectContext();
      NWScript.PlaySound(soundName);
    }

    /// <summary>
    /// Do a Fortitude Save check for the given dc.
    /// </summary>
    /// <param name="dc">Difficulty class.</param>
    /// <param name="saveType">The type of save.</param>
    /// <param name="saveVs"></param>
    /// <returns>The result of the saving throw.</returns>
    public SavingThrowResult FortitudeSave(int dc, SavingThrowType saveType, NwGameObject saveVs = null) =>
      (SavingThrowResult) NWScript.FortitudeSave(this, dc, (int) saveType, saveVs);

    /// <summary>
    /// Do a Reflex Save check for the given dc.
    /// </summary>
    /// <param name="dc">Difficulty class.</param>
    /// <param name="saveType">The type of save.</param>
    /// <param name="saveVs"></param>
    /// <returns>The result of the saving throw.</returns>
    public SavingThrowResult ReflexSave(int dc, SavingThrowResult saveType, NwGameObject saveVs = null) =>
      (SavingThrowResult) NWScript.ReflexSave(this, dc, (int) saveType, saveVs);

    /// <summary>
    /// Do a Will Save check for the given dc.
    /// </summary>
    /// <param name="dc">Difficulty class.</param>
    /// <param name="saveType">The type of save.</param>
    /// <param name="saveVs"></param>
    /// <returns>The result of the saving throw.</returns>
    public SavingThrowResult WillSave(int dc, SavingThrowResult saveType, NwGameObject saveVs = null) =>
      (SavingThrowResult) NWScript.WillSave(this, dc, (int) saveType, saveVs);

    /// <summary>
    /// Casts a spell at an object.
    /// </summary>
    /// <param name="spell">The spell to cast.</param>
    /// <param name="target">The target for the spell.</param>
    /// <param name="metaMagic">Metamagic that should be applied to the spell.</param>
    /// <param name="cheat">If true, this object doesn't have to be able to cast the spell.</param>
    /// <param name="domainLevel">Specifies the spell level if the spell is to be cast as a domain spell.</param>
    /// <param name="projectilePathType"></param>
    /// <param name="instant">If true, the spell is cast immediately.</param>
    public async Task ActionCastSpellAt(Spell spell, NwGameObject target, MetaMagic metaMagic = MetaMagic.Any, bool cheat = false, int domainLevel = 0, ProjectilePathType projectilePathType = ProjectilePathType.Default, bool instant = false)
    {
      await WaitForObjectContext();
      NWScript.ActionCastSpellAtObject((int) spell, target, (int) metaMagic, cheat.ToInt(), domainLevel, (int) projectilePathType, instant.ToInt());
    }

    /// <summary>
    /// Casts a spell at an location.
    /// </summary>
    /// <param name="spell">The spell to cast.</param>
    /// <param name="target">The target for the spell.</param>
    /// <param name="metaMagic">Metamagic that should be applied to the spell.</param>
    /// <param name="cheat">If true, this object doesn't have to be able to cast the spell.</param>
    /// <param name="projectilePathType"></param>
    /// <param name="instant">If true, the spell is cast immediately.</param>
    public async Task ActionCastSpellAt(Spell spell, Location target, MetaMagic metaMagic = MetaMagic.Any, bool cheat = false, ProjectilePathType projectilePathType = ProjectilePathType.Default, bool instant = false)
    {
      await WaitForObjectContext();
      NWScript.ActionCastSpellAtLocation((int) spell, target, (int) metaMagic, cheat.ToInt(), (int) projectilePathType, instant.ToInt());
    }

    /// <summary>
    /// Destroys this object (irrevocably)
    /// </summary>
    /// <param name="delay">Time in seconds until this object should be destroyed.</param>
    public void Destroy(float delay = 0.0f)
    {
      NWScript.DestroyObject(this, delay);
    }
  }
}