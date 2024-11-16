using NWN.Core;

namespace Anvil.API
{
  public enum GuiEventType
  {
    ChatBarFocus = NWScript.GUIEVENT_CHATBAR_FOCUS,
    ChatBarUnFocus = NWScript.GUIEVENT_CHATBAR_UNFOCUS,
    CharacterSheetSkillClick = NWScript.GUIEVENT_CHARACTERSHEET_SKILL_CLICK,
    CharacterSheetFeatClick = NWScript.GUIEVENT_CHARACTERSHEET_FEAT_CLICK,
    EffectIconClick = NWScript.GUIEVENT_EFFECTICON_CLICK,
    DeathPanelWaitForHelpClick = NWScript.GUIEVENT_DEATHPANEL_WAITFORHELP_CLICK,
    MinimapMapPinClick = NWScript.GUIEVENT_MINIMAP_MAPPIN_CLICK,
    MinimapOpen = NWScript.GUIEVENT_MINIMAP_OPEN,
    MinimapClose = NWScript.GUIEVENT_MINIMAP_CLOSE,
    JournalOpen = NWScript.GUIEVENT_JOURNAL_OPEN,
    JournalClose = NWScript.GUIEVENT_JOURNAL_CLOSE,
    PlayerListPlayerClick = NWScript.GUIEVENT_PLAYERLIST_PLAYER_CLICK,
    PartyBarPortraitClick = NWScript.GUIEVENT_PARTYBAR_PORTRAIT_CLICK,
    DisabledPanelAttemptOpen = NWScript.GUIEVENT_DISABLED_PANEL_ATTEMPT_OPEN,
    CompassClick = NWScript.GUIEVENT_COMPASS_CLICK,
    LevelUpCancelled = NWScript.GUIEVENT_LEVELUP_CANCELLED,
    AreaLoadScreenFinished = NWScript.GUIEVENT_AREA_LOADSCREEN_FINISHED,
    QuickChatActivate = NWScript.GUIEVENT_QUICKCHAT_ACTIVATE,
    QuickChatSelect = NWScript.GUIEVENT_QUICKCHAT_SELECT,
    QuickChatClose = NWScript.GUIEVENT_QUICKCHAT_CLOSE,
    SelectCreature = NWScript.GUIEVENT_SELECT_CREATURE,
    UnselectCreature = NWScript.GUIEVENT_UNSELECT_CREATURE,
    ExamineObject = NWScript.GUIEVENT_EXAMINE_OBJECT,
    OptionsOpen = NWScript.GUIEVENT_OPTIONS_OPEN,
    OptionsClose = NWScript.GUIEVENT_OPTIONS_CLOSE,
    RadialOpen = NWScript.GUIEVENT_RADIAL_OPEN,
    ChatlogPortraitClick = NWScript.GUIEVENT_CHATLOG_PORTRAIT_CLICK,
    PlayerlistPlayerTell = NWScript.GUIEVENT_PLAYERLIST_PLAYER_TELL,
  }
}
