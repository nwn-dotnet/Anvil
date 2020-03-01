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

  public enum TalkVolume
  {
    Talk = 0,
    Whisper = 1,
    Shout = 2,
    SilentTalk = 3,
    SilentShout = 4,
    Party = 5,
    Tell = 6
  }
}