using NWM.API.Constants;

namespace NWM.API
{
  public sealed class ChatMessage
  {
    public string Message;
    public TalkVolume Volume;

    public ChatMessage(string message, TalkVolume volume)
    {
      this.Message = message;
      this.Volume = volume;
    }
  }
}