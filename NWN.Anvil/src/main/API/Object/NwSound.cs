using System;
using NWN.Core;
using NWN.Native.API;

namespace Anvil.API
{
  /// <summary>
  /// A configurable sound object in an area.
  /// </summary>
  [NativeObjectInfo(ObjectTypes.All, ObjectType.Sound)]
  public sealed class NwSound : NwGameObject
  {
    internal readonly CNWSSoundObject SoundObject;

    public NwSound(CNWSSoundObject soundObject) : base(soundObject)
    {
      SoundObject = soundObject;
    }

    public static implicit operator CNWSSoundObject(NwSound sound)
    {
      return sound?.SoundObject;
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
    public void Play()
    {
      NWScript.SoundObjectPlay(this);
    }

    /// <summary>
    /// Stops this sound object from playing.
    /// </summary>
    public void Stop()
    {
      NWScript.SoundObjectStop(this);
    }

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

        soundObject = new CNWSSoundObject(Invalid);
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

    private protected override void AddToArea(CNWSArea area, float x, float y, float z)
    {
      SoundObject.AddToArea(area, true.ToInt());
      SoundObject.ChangePosition(new Vector(x, y, z));
    }

    internal override void RemoveFromArea()
    {
      SoundObject.RemoveFromArea();
    }
  }
}
