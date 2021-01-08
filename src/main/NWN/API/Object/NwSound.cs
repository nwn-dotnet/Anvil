using NWN.API.Constants;
using NWN.Core;
using NWN.Native.API;

namespace NWN.API
{
  [NativeObjectInfo(ObjectTypes.All, ObjectType.Sound)]
  public class NwSound : NwGameObject
  {
    public NwSound(uint objectId) : base(objectId) {}

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
