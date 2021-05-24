using System;
using NWN.API.Constants;
using NWN.Core;
using NWN.Native.API;

namespace NWN.API
{
  [NativeObjectInfo(ObjectTypes.All, ObjectType.Sound)]
  public class NwSound : NwGameObject
  {
    internal readonly CNWSSoundObject SoundObject;

    public NwSound(CNWSSoundObject soundObject) : base(soundObject)
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
        if (value.Area != Area)
        {
          SoundObject.AddToArea(value.Area, true.ToInt());
        }

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

    public override byte[] Serialize()
    {
      return NativeUtils.SerializeGff("UTS", (resGff, resStruct) =>
      {
        SoundObject.SaveObjectState(resGff, resStruct);
        SoundObject.Save(resGff, resStruct);
        return true;
      });
    }

    public static NwSound Deserialize(byte[] serialized)
    {
      CNWSSoundObject soundObject = null;

      bool result = NativeUtils.DeserializeGff(serialized, (resGff, resStruct) =>
      {
        if (!resGff.IsValidGff("UTS"))
        {
          return false;
        }

        soundObject = new CNWSSoundObject(INVALID);
        if (soundObject.Load(resGff, resStruct).ToBool())
        {
          soundObject.LoadObjectState(resGff, resStruct);
          GC.SuppressFinalize(soundObject);
          return true;
        }

        soundObject.Dispose();
        return false;
      });

      return result && soundObject != null ? soundObject.ToNwObject<NwSound>() : null;
    }
  }
}
