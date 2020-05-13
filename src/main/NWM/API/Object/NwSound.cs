using NWM.API.Constants;
using NWMX.API.Constants;
using NWN;

namespace NWM.API
{
  [NativeObjectInfo(ObjectType.All, InternalObjectType.Sound)]
  public class NwSound : NwGameObject
  {
    public NwSound(uint objectId) : base(objectId) {}

    public void Play()
    {
      NWScript.SoundObjectPlay(this);
    }
  }
}