using System.Collections.Generic;
using NWM.API.Constants;
using NWN;
using Vector3 = System.Numerics.Vector3;

namespace NWM.API
{
  public abstract class NwGameObject : NwObject
  {
    internal NwGameObject(uint objectId) : base(objectId) {}

    /// <summary>
    /// Gets or sets the location of this object.
    /// </summary>
    public virtual Location Location
    {
      get => NWScript.GetLocation(this);
      set => ExecuteOnSelf(() => NWScript.JumpToLocation(value));
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
      set => ExecuteOnSelf(() => NWScript.JumpToLocation(NWScript.Location(Area, value, Rotation)));
    }

    /// <summary>
    /// The world rotation for this object
    /// </summary>
    public virtual float Rotation
    {
      get => NWScript.GetFacing(this) % 360;
      set => ExecuteOnSelf(() => NWScript.SetFacing(value % 360));
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
    /// Gets or sets the Portrait ResRef for this object.
    /// </summary>
    public string PortraitResRef
    {
      get => NWScript.GetPortraitResRef(this);
      set => NWScript.SetPortraitResRef(this, value);
    }

    /// <summary>
    /// Gets the current Hitpoints for this object.
    /// </summary>
    public int HP
    {
      get => NWScript.GetCurrentHitPoints(this);
    }

    /// <summary>
    /// Rotates this object to face towards target.
    /// </summary>
    /// <param name="target">The target object to face.</param>
    public void FaceToObject(NwGameObject target)
    {
      FaceToPoint(target.Position);
    }

    /// <summary>
    /// Rotates this object to face a position.
    /// </summary>
    /// <param name="point"></param>
    public virtual void FaceToPoint(Vector3 point)
    {
      AssignCommand(() => NWScript.SetFacingPoint(point));
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
    public void PlayAnimation(Animation animation, float animSpeed, bool queueAsAction = false, float duration = 0.0f)
    {
      if (!queueAsAction)
      {
        ExecuteOnSelf(() => NWScript.PlayAnimation((int) animation, animSpeed, duration));
      }
      else
      {
        ExecuteOnSelf(() => NWScript.ActionPlayAnimation((int) animation, animSpeed, duration));
      }
    }

    /// <summary>
    /// Instructs this object to speak.
    /// </summary>
    /// <param name="message">The message the object should speak.</param>
    /// <param name="talkVolume">The channel/volume of this message.</param>
    /// <param name="queueAsAction">Whether the object should speak immediately (false), or be queued in the object's action queue (true).</param>
    public void SpeakString(string message, TalkVolume talkVolume = TalkVolume.Talk, bool queueAsAction = false)
    {
      if (!queueAsAction)
      {
        ExecuteOnSelf(() => NWScript.SpeakString(message, (int) talkVolume));
      }
      else
      {
        ExecuteOnSelf(() => NWScript.ActionSpeakString(message, (int) talkVolume));
      }
    }

    /// <summary>
    /// Returns the creatures closest to this object.
    /// </summary>
    public IEnumerable<NwCreature> GetNearestCreatures() => GetNearestCreatures(CreatureTypeFilter.None, CreatureTypeFilter.None, CreatureTypeFilter.None);

    /// <summary>
    /// Returns the creatures closest to this object, matching the specified criteria.
    /// </summary>
    /// <param name="filter1">A filter created using <see cref="CreatureTypeFilter"/>.*</param>
    public IEnumerable<NwCreature> GetNearestCreatures(CreatureTypeFilter filter1) => GetNearestCreatures(filter1, CreatureTypeFilter.None, CreatureTypeFilter.None);

    /// <summary>
    /// Returns the creatures closest to this object, matching all of the specified criteria.
    /// </summary>
    /// <param name="filter1">A filter created using <see cref="CreatureTypeFilter"/>.*</param>
    /// <param name="filter2">A filter created using <see cref="CreatureTypeFilter"/>.*</param>
    public IEnumerable<NwCreature> GetNearestCreatures(CreatureTypeFilter filter1, CreatureTypeFilter filter2) => GetNearestCreatures(filter1, filter2, CreatureTypeFilter.None);

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
    /// Destroys this object (irrevocably)
    /// </summary>
    /// <param name="delay">Time in seconds until this object should be destroyed.</param>
    public void Destroy(float delay = 0.0f)
    {
      NWScript.DestroyObject(this, delay);
    }
  }
}