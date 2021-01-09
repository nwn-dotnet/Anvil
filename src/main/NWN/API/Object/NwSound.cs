using NWN.API.Constants;
using NWN.Core;
using NWN.Native.API;

namespace NWN.API
{
  [NativeObjectInfo(ObjectTypes.All, ObjectType.Sound)]
  public class NwSound : NwGameObject
  {
    private readonly CNWSSoundObject soundObject;

    public NwSound(uint objectId, CNWSSoundObject soundObject) : base(objectId)
    {
      this.soundObject = soundObject;
    }

    public static implicit operator CNWSSoundObject(NwSound sound)
    {
      return sound?.soundObject;
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
