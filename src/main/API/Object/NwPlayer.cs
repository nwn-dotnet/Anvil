using NWN;

namespace NWM.API
{
  public class NwPlayer : NwCreature
  {
    protected internal NwPlayer(uint objectId) : base(objectId) {}

    public void SendServerMessage(string message, Color color)
    {
      NWScript.SendMessageToPC(this, message.ColorString(color));
    }

    public void SendServerMessage(string message)
    {
      NWScript.SendMessageToPC(this, message);
    }

    public bool IsDM => NWScript.GetIsDM(ObjectId).ToBool();
    public string PlayerName => NWScript.GetPCPlayerName(this);
  }
}