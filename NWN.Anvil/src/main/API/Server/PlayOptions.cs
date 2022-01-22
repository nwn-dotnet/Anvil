using NWN.Native.API;

namespace Anvil.API
{
  public sealed class PlayOptions
  {
    private readonly CPlayOptions playOptions;

    internal PlayOptions(CPlayOptions playOptions)
    {
      this.playOptions = playOptions;
    }

    public bool AllKillable
    {
      get => playOptions.bAllKillable.ToBool();
      set => playOptions.bAllKillable = value.ToInt();
    }

    public bool AutoFailSaveOn1
    {
      get => playOptions.bAutoFailSaveOn1.ToBool();
      set => playOptions.bAutoFailSaveOn1 = value.ToInt();
    }

    public bool BackupSavedCharacters
    {
      get => playOptions.bBackupSavedCharacters.ToBool();
      set => playOptions.bBackupSavedCharacters = value.ToInt();
    }

    public bool CdKeyBanlistAllowlist
    {
      get => playOptions.bCDKeyBanListAllowList.ToBool();
      set => playOptions.bCDKeyBanListAllowList = value.ToInt();
    }

    public bool DisallowShouting
    {
      get => playOptions.bDisallowShouting.ToBool();
      set => playOptions.bDisallowShouting = value.ToInt();
    }

    public bool EnforceLegalCharacters
    {
      get => playOptions.bEnforceLegalCharacters.ToBool();
      set => playOptions.bEnforceLegalCharacters = value.ToInt();
    }

    public bool ExamineChallengeRating
    {
      get => playOptions.bExamineChallengeRating.ToBool();
      set => playOptions.bExamineChallengeRating = value.ToInt();
    }

    public bool ExamineEffects
    {
      get => playOptions.bExamineEffects.ToBool();
      set => playOptions.bExamineEffects = value.ToInt();
    }

    public bool HideHitpointsGained
    {
      get => playOptions.bHideHitPointsGained.ToBool();
      set => playOptions.bHideHitPointsGained = value.ToInt();
    }

    public bool ItemLevelRestrictions
    {
      get => playOptions.bItemLevelRestrictions.ToBool();
      set => playOptions.bItemLevelRestrictions = value.ToInt();
    }

    public bool LoseExp
    {
      get => playOptions.bLoseExp.ToBool();
      set => playOptions.bLoseExp = value.ToInt();
    }

    public int LoseExpNum
    {
      get => playOptions.nLoseExpNum;
      set => playOptions.nLoseExpNum = value;
    }

    public bool LoseGold
    {
      get => playOptions.bLoseGold.ToBool();
      set => playOptions.bLoseGold = value.ToInt();
    }

    public int LoseGoldNum
    {
      get => playOptions.nLoseGoldNum;
      set => playOptions.nLoseGoldNum = value;
    }

    public bool LoseItems
    {
      get => playOptions.bLoseItems.ToBool();
      set => playOptions.bLoseItems = value.ToInt();
    }

    public int LoseItemsNum
    {
      get => playOptions.nLoseItemsNum;
      set => playOptions.nLoseItemsNum = value;
    }

    public bool LoseStolenItems
    {
      get => playOptions.bLoseStolenItems.ToBool();
      set => playOptions.bLoseStolenItems = value.ToInt();
    }

    public bool NonPartyKillable
    {
      get => playOptions.bNonPartyKillable.ToBool();
      set => playOptions.bNonPartyKillable = value.ToInt();
    }

    public bool OnPartyOnly
    {
      get => playOptions.bOnePartyOnly.ToBool();
      set => playOptions.bOnePartyOnly = value.ToInt();
    }

    public bool PauseAndPlay
    {
      get => playOptions.bPauseAndPlay.ToBool();
      set => playOptions.bPauseAndPlay = value.ToInt();
    }

    public PvPSetting PvPSetting
    {
      get => (PvPSetting)playOptions.nPVPSetting;
      set => playOptions.nPVPSetting = (int)value;
    }

    public bool RequireResurrection
    {
      get => playOptions.bRequireResurrection.ToBool();
      set => playOptions.bRequireResurrection = value.ToInt();
    }

    public bool ResetEncounterSpawnPool
    {
      get => playOptions.bResetEncounterSpawnPool.ToBool();
      set => playOptions.bResetEncounterSpawnPool = value.ToInt();
    }

    public bool RestoreSpellUses
    {
      get => playOptions.bRestoreSpellsUses.ToBool();
      set => playOptions.bRestoreSpellsUses = value.ToInt();
    }

    public bool ShowDMJoinMessage
    {
      get => playOptions.bShowDMJoinMessage.ToBool();
      set => playOptions.bShowDMJoinMessage = value.ToInt();
    }

    public bool UseMaxHPOnLevelUp
    {
      get => playOptions.bUseMaxHitPoints.ToBool();
      set => playOptions.bUseMaxHitPoints = value.ToInt();
    }

    public bool ValidateSpells
    {
      get => playOptions.bValidateSpells.ToBool();
      set => playOptions.bValidateSpells = value.ToInt();
    }

    public bool PlayerPartyControl
    {
      get => playOptions.bPlayerPartyControl.ToBool();
      set => playOptions.bPlayerPartyControl = value.ToInt();
    }
  }
}
