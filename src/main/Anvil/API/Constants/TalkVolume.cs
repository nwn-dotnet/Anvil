using NWN.Core;

namespace Anvil.API
{
  public enum TalkVolume
  {
    Talk = NWScript.TALKVOLUME_TALK,
    Whisper = NWScript.TALKVOLUME_WHISPER,
    Shout = NWScript.TALKVOLUME_SHOUT,
    SilentTalk = NWScript.TALKVOLUME_SILENT_TALK,
    SilentShout = NWScript.TALKVOLUME_SILENT_SHOUT,
    Party = NWScript.TALKVOLUME_PARTY,
    Tell = NWScript.TALKVOLUME_TELL,
  }
}
