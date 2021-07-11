using NWN.Native.API;

namespace Anvil.Services
{
  internal static class DialogExtensions
  {
    public static CNWSDialogLinkReply GetReply(this CNWSDialogEntry entry, uint index)
    {
      return CNWSDialogLinkReplyArray.FromPointer(entry.m_pReplies)[(int)index];
    }

    public static CNWSDialogLinkEntry GetEntry(this CNWSDialogReply reply, uint index)
    {
      return CNWSDialogLinkEntryArray.FromPointer(reply.m_pEntries)[(int)index];
    }

    public static CNWSDialogLinkReply GetReply(this CNWSDialogEntry entry, int index)
    {
      return CNWSDialogLinkReplyArray.FromPointer(entry.m_pReplies)[index];
    }

    public static CNWSDialogLinkEntry GetEntry(this CNWSDialogReply reply, int index)
    {
      return CNWSDialogLinkEntryArray.FromPointer(reply.m_pEntries)[index];
    }
  }
}
