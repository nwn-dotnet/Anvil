using NWN.Core.NWNX;

namespace NWNX.API.Constants
{
  public enum NodeType
  {
    Invalid = DialogPlugin.NWNX_DIALOG_NODE_TYPE_INVALID,
    StartingNode = DialogPlugin.NWNX_DIALOG_NODE_TYPE_STARTING_NODE,
    EntryNode = DialogPlugin.NWNX_DIALOG_NODE_TYPE_ENTRY_NODE,
    ReplyNode = DialogPlugin.NWNX_DIALOG_NODE_TYPE_REPLY_NODE,
  }

  public enum ScriptType
  {
    Other = DialogPlugin.NWNX_DIALOG_SCRIPT_TYPE_OTHER,
    StartingConditional = DialogPlugin.NWNX_DIALOG_SCRIPT_TYPE_STARTING_CONDITIONAL,
    ActionTaken = DialogPlugin.NWNX_DIALOG_SCRIPT_TYPE_ACTION_TAKEN
  }

  public enum Language
  {
    English = DialogPlugin.NWNX_DIALOG_LANGUAGE_ENGLISH,
    French = DialogPlugin.NWNX_DIALOG_LANGUAGE_FRENCH,
    German = DialogPlugin.NWNX_DIALOG_LANGUAGE_GERMAN,
    Italian = DialogPlugin.NWNX_DIALOG_LANGUAGE_ITALIAN,
    Spanish = DialogPlugin.NWNX_DIALOG_LANGUAGE_SPANISH,
    Polish = DialogPlugin.NWNX_DIALOG_LANGUAGE_POLISH,
    Korean = DialogPlugin.NWNX_DIALOG_LANGUAGE_KOREAN,
    ChineseTraditional = DialogPlugin.NWNX_DIALOG_LANGUAGE_CHINESE_TRADITIONAL,
    ChineseSimplified = DialogPlugin.NWNX_DIALOG_LANGUAGE_CHINESE_SIMPLIFIED,
    Japanese = DialogPlugin.NWNX_DIALOG_LANGUAGE_JAPANESE
  }
}