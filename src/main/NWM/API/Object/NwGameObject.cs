using System.Collections.Generic;
using NWM.API.Constants;
using NWN;
using Vector3 = System.Numerics.Vector3;

namespace NWM.API
{
  public abstract class NwGameObject : NwObject
  {
    internal NwGameObject(uint objectId) : base(objectId) {}

    public VisualTransform VisualTransform
    {
      get => new VisualTransform(this);
      set => value?.Apply(this);
    }

    public virtual Location Location
    {
      get => NWScript.GetLocation(this);
      set => ExecuteOnSelf(() => NWScript.JumpToLocation(value));
    }

    public string PortraitResRef
    {
      get => NWScript.GetPortraitResRef(this);
      set => NWScript.SetPortraitResRef(this, value);
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

    public int HP
    {
      get => NWScript.GetCurrentHitPoints(this);
    }

    public void FaceToObject(NwGameObject nwObject)
    {
      FaceToPoint(nwObject.Position);
    }

    public virtual void FaceToPoint(Vector3 point)
    {
      AssignCommand(() => NWScript.SetFacingPoint(point));
    }

    public NwArea Area
    {
      get => NWScript.GetArea(this).ToNwObject<NwArea>();
    }

    public bool PlotFlag
    {
      get => NWScript.GetPlotFlag(this).ToBool();
      set => NWScript.SetPlotFlag(this, value.ToInt());
    }

    public IEnumerable<NwItem> Items
    {
      get
      {
        for (NwItem item = NWScript.GetFirstItemInInventory(this).ToNwObject<NwItem>(); item != INVALID; item = NWScript.GetNextItemInInventory(this).ToNwObject<NwItem>())
        {
          yield return item;
        }
      }
    }

    /// <summary>
    /// Plays the specified animation
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

    public IEnumerable<NwCreature> GetNearestCreatures() => GetNearestCreatures(CreatureTypeFilter.None, CreatureTypeFilter.None, CreatureTypeFilter.None);
    public IEnumerable<NwCreature> GetNearestCreatures(CreatureTypeFilter filter1) => GetNearestCreatures(filter1, CreatureTypeFilter.None, CreatureTypeFilter.None);
    public IEnumerable<NwCreature> GetNearestCreatures(CreatureTypeFilter filter1, CreatureTypeFilter filter2) => GetNearestCreatures(filter1, filter2, CreatureTypeFilter.None);

    public IEnumerable<NwCreature> GetNearestCreatures(CreatureTypeFilter filter1, CreatureTypeFilter filter2, CreatureTypeFilter filter3)
    {
      int i;
      NwCreature current;

      for (i = 1, current = NWScript.GetNearestCreature(
          filter1.Key,
          filter1.Value,
          this,
          i,
          filter2.Key,
          filter2.Value,
          filter3.Key,
          filter3.Value).ToNwObject<NwCreature>();
        current != INVALID;
        i++, NWScript.GetNearestCreature(
          filter1.Key,
          filter1.Value,
          this,
          i,
          filter2.Key,
          filter2.Value,
          filter3.Key,
          filter3.Value).ToNwObject<NwCreature>())
      {
        yield return current;
      }
    }

    public IEnumerable<T> GetNearestObjectsByType<T>() where T : NwGameObject
    {
      int objType = (int) NwObjectFactory.GetObjectType<T>();
      int i;
      NwGameObject current;

      for (i = 1, current = NWScript.GetNearestObject(objType, this, i).ToNwObject<NwGameObject>(); current != INVALID; i++, current = NWScript.GetNearestObject(objType, this, i).ToNwObject<NwGameObject>())
      {
        if (current is T gameObject)
        {
          yield return gameObject;
        }
      }
    }

    public void Destroy(float delay = 0.0f)
    {
      NWScript.DestroyObject(this, delay);
    }
  }
}