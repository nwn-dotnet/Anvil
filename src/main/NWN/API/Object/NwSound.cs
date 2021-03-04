using NWN.API.Constants;
using NWN.Core;
using NWN.Native.API;

namespace NWN.API
{
  [NativeObjectInfo(ObjectTypes.All, ObjectType.Sound)]
  public class NwSound : NwGameObject
  {
    internal readonly CNWSSoundObject SoundObject;

    public NwSound(uint objectId, CNWSSoundObject soundObject) : base(objectId, soundObject)
    {
      this.SoundObject = soundObject;
    }

    public static implicit operator CNWSSoundObject(NwSound sound)
    {
      return sound?.SoundObject;
    }

    public override Location Location
    {
      set
      {
        SoundObject.AddToArea(value.Area, true.ToInt());
        Position = value.Position;
        Rotation = value.Rotation;
      }
    }

    /// <summary>
    /// Sets the volume for this sound object (0-127).
    /// </summary>
    public sbyte Volume
    {
      set => NWScript.SoundObjectSetVolume(this, value);
    }

    /// <summary>
    /// Plays this sound object.
    /// </summary>
    public void Play() => NWScript.SoundObjectPlay(this);

    /// <summary>
    /// Stops this sound object from playing.
    /// </summary>
    public void Stop() => NWScript.SoundObjectStop(this);
  }
}
