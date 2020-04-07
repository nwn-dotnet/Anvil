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

    public Location Location
    {
      get => NWScript.GetLocation(this);
      set => ExecuteOnSelf(() => NWScript.JumpToLocation(value));
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

    public void FaceTowards(NwGameObject nwObject)
    {
      AssignCommand(() => NWScript.SetFacingPoint(nwObject.Position));
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
  }
}