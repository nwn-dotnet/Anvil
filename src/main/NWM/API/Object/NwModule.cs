using NWM.Core;
using NWN;

namespace NWM.API
{
  [Service]
  public sealed class NwModule : NwObject
  {
    internal NwModule(uint objectId) : base(objectId) {}
    public NwModule() : this(NWScript.GetModule()) {}

    public void SendMessageToAllDMs(string message, Color color)
    {
      NWScript.SendMessageToAllDMs(message.ColorString(color));
    }

    public void SendMessageToAllDMs(string message)
    {
      NWScript.SendMessageToAllDMs(message);
    }
  }
}