using NWN;

namespace NWM.API
{
  public class NwSound : NwGameObject
  {
    public NwSound(uint objectId) : base(objectId) {}

    public void Play()
    {
      NWScript.SoundObjectPlay(this);
    }
  }
}