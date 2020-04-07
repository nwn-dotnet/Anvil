using NWN;

namespace NWM.API
{
  public sealed class NwPlayer : NwCreature
  {
    internal NwPlayer(uint objectId) : base(objectId) {}

    public bool IsDM => NWScript.GetIsDM(ObjectId).ToBool();
    public string PlayerName => NWScript.GetPCPlayerName(this);

    public void SendServerMessage(string message, Color color)
    {
      NWScript.SendMessageToPC(this, message.ColorString(color));
    }

    public void SendServerMessage(string message)
    {
      NWScript.SendMessageToPC(this, message);
    }

    public void ActionStartConversation(NwGameObject converseWith, string dialogResRef, bool isPrivate = false, bool playHello = true)
    {
      ExecuteOnSelf(() => NWScript.ActionStartConversation(converseWith, dialogResRef, isPrivate.ToInt(), playHello.ToInt()));
    }

    public void ExportCharacter()
    {
      NWScript.ExportSingleCharacter(this);
    }
  }
}