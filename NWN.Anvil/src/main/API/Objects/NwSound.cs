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
    private readonly CNWSSoundObject soundObject;

    internal CNWSSoundObject SoundObject
    {
      get
      {
        AssertObjectValid();
        return soundObject;
      }
    }

    public NwSound(CNWSSoundObject soundObject) : base(soundObject)
    {
      this.soundObject = soundObject;
    }

    /// <summary>
    /// Sets the volume for this sound object (0-127).
    /// </summary>
    public sbyte Volume
    {
      set => NWScript.SoundObjectSetVolume(this, value);
    }

    public static NwSound? Create(string template, Location location, string? newTag = null)
    {
      if (string.IsNullOrEmpty(template))
      {
        return default;
      }

      CNWSArea area = location.Area.Area;
      Vector position = location.Position.ToNativeVector();
      Vector orientation = location.Rotation.ToVectorOrientation().ToNativeVector();

      CNWSSoundObject? soundObject = null;
      bool result = NativeUtils.CreateFromResRef(ResRefType.UTS, template, (resGff, resStruct) =>
      {
        soundObject = new CNWSSoundObject();
        GC.SuppressFinalize(soundObject);
        soundObject.m_sTemplate = template.ToExoString();
        soundObject.Load(resGff, resStruct);
        soundObject.LoadVarTable(resGff, resStruct);

        soundObject.SetPosition(position);
        soundObject.SetOrientation(orientation);

        if (!string.IsNullOrEmpty(newTag))
        {
          soundObject.m_sTag = newTag.ToExoString();
          NwModule.Instance.Module.AddObjectToLookupTable(soundObject.m_sTag, soundObject.m_idSelf);
        }

        soundObject.AddToArea(area);
        soundObject.ChangePosition(position);
      });

      return result && soundObject != null ? soundObject.ToNwObject<NwSound>() : null;
    }

    public static NwSound? Deserialize(byte[] serialized)
    {
      CNWSSoundObject? soundObject = null;

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
          soundObject.m_oidArea = Invalid;
          GC.SuppressFinalize(soundObject);
          return true;
        }

        soundObject.Dispose();
        return false;
      });

      return result && soundObject != null ? soundObject.ToNwObject<NwSound>() : null;
    }

    public static implicit operator CNWSSoundObject?(NwSound? sound)
    {
      return sound?.SoundObject;
    }

    public override NwGameObject Clone(Location location, string? newTag = null, bool copyLocalState = true)
    {
      throw new NotSupportedException("Sound objects may not be cloned.");
    }

    /// <summary>
    /// Plays this sound object.
    /// </summary>
    public void Play()
    {
      NWScript.SoundObjectPlay(this);
    }

    public override byte[]? Serialize()
    {
      return NativeUtils.SerializeGff("UTS", (resGff, resStruct) =>
      {
        SoundObject.SaveObjectState(resGff, resStruct);
        SoundObject.Save(resGff, resStruct);
        return true;
      });
    }

    /// <summary>
    /// Stops this sound object from playing.
    /// </summary>
    public void Stop()
    {
      NWScript.SoundObjectStop(this);
    }

    internal override void RemoveFromArea()
    {
      SoundObject.RemoveFromArea();
    }

    private protected override void AddToArea(CNWSArea area, float x, float y, float z)
    {
      SoundObject.AddToArea(area, true.ToInt());
      SoundObject.ChangePosition(new Vector(x, y, z));
    }
  }
}
