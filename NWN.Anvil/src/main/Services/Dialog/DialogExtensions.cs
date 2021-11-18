using NWN.Native.API;

namespace Anvil.Services
{
  internal static class DialogExtensions
  {
    public static CNWSDialogEntryArray ToArray(this CNWSDialogEntry pointer)
    {
      return CNWSDialogEntryArray.FromPointer(pointer);
    }

    public static CNWSDialogReplyArray ToArray(this CNWSDialogReply pointer)
    {
      return CNWSDialogReplyArray.FromPointer(pointer);
    }

    public static CNWSDialogLinkEntryArray ToArray(this CNWSDialogLinkEntry pointer)
    {
      return CNWSDialogLinkEntryArray.FromPointer(pointer);
    }

    public static CNWSDialogLinkReplyArray ToArray(this CNWSDialogLinkReply pointer)
    {
      return CNWSDialogLinkReplyArray.FromPointer(pointer);
    }
  }
}
