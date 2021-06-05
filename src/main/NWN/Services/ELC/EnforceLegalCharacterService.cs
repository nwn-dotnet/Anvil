using System;
using Anvil.Internal;
using NWN.API;
using NWN.Native.API;

namespace NWN.Services
{
  [ServiceBinding(typeof(EnforceLegalCharacterService))]
  [ServiceBindingOptions(Lazy = true)]
  public sealed unsafe class EnforceLegalCharacterService : IDisposable
  {
    // Validation Failure STRREFs
    private const int StrRefCharacterDoesNotExist = 63767;
    private const int StrRefCharacterLevelRestriction = 57924;
    private const int StrRefCharacterNonPlayer = 63760;
    private const int StrRefCharacterDungeonMaster = 67641;
    private const int StrRefCharacterNonPlayerRace = 66166;
    private const int StrRefCharacterNonPlayerClass = 66167;
    private const int StrRefCharacterTooManyHitpoints = 3109;
    private const int StrRefCharacterSavingThrow = 8066;
    private const int StrRefCharacterInvalidAbilityScores = 63761;
    private const int StrRefItemLevelRestriction = 68521;
    private const int StrRefSkillUnuseable = 63815;
    private const int StrRefSkillInvalidRanks = 66165;
    private const int StrRefSkillInvalidNumSkillpoints = 66155;
    private const int StrRefFeatInvalid = 76383;
    private const int StrRefFeatTooMany = 66222;
    private const int StrRefFeatReqAbility = 66175;
    private const int StrRefFeatReqSpellLevel = 66176;
    private const int StrRefFeatReqFeat = 66182;
    private const int StrRefFeatReqSkill = 66183;
    private const int StrRefSpellReqSpellLevel = 66498;
    private const int StrRefSpellInvalidSpell = 66499;
    private const int StrRefSpellIllegalLevel = 68627;
    private const int StrRefSpellReqAbility = 68628;
    private const int StrRefSpellLearnedTwice = 68629;
    private const int StrRefSpellIllegalNumSpells = 68630;
    private const int StrRefSpellIllegalRemovedSpells = 68631;
    private const int StrRefSpellOppositeSpellSchool = 66500;
    private const int StrRefCustom = 164;

    // Magic Numbers
    private const int NumPlayerItemSlots = 14;
    private const int NumMulticlass = 3;
    private const int CharacterEpicLevel = 21;
    private const int NumSpellLevels = 10;

    private delegate int ValidateCharacterHook(void* pPlayer, int* bFailedServerRestriction);
    private readonly FunctionHook<ValidateCharacterHook> validateCharacterHook;

    public bool EnforceDefaultEventScripts { get; set; }
    public bool EnforceEmptyDialog { get; set; }

    public EnforceLegalCharacterService(HookService hookService)
    {
      validateCharacterHook = hookService.RequestHook<ValidateCharacterHook>(OnValidateCharacter, FunctionsLinux._ZN10CNWSPlayer17ValidateCharacterEPi, HookOrder.Final);
    }

    private int OnValidateCharacter(void* pPlayer, int* bFailedServerRestriction)
    {
      CNWSPlayer player = CNWSPlayer.FromPointer(pPlayer);

      // *** Sanity Checks ****************************************************************************************************
      if (player == null)
      {
        return StrRefCharacterDoesNotExist;
      }

      ICGameObject gameObject = LowLevel.ServerExoApp.GetGameObject(player.m_oidNWSObject);
      if (gameObject == null)
      {
        return StrRefCharacterDoesNotExist;
      }

      CNWSCreature creature = gameObject.AsNWSCreature();
      if (creature == null)
      {
        return StrRefCharacterDoesNotExist;
      }

      CNWSCreatureStats creatureStats = creature.m_pStats;
      if (creatureStats == null)
      {
        return StrRefCharacterDoesNotExist;
      }

      CNWSInventory inventory = creature.m_pInventory;
      if (inventory == null)
      {
        return StrRefCharacterDoesNotExist;
      }
      // **********************************************************************************************************************

      // *** Server Restrictions **********************************************************************************************
      JoiningRestrictions joinRestrictions = NwServer.Instance.ServerInfo.JoiningRestrictions;

      int characterLevel = creatureStats.GetLevel(false.ToInt());

      if (characterLevel > joinRestrictions.MaxLevel || characterLevel < joinRestrictions.MinLevel)
      {
        OnELCLevelValidationFailure failure = new OnELCLevelValidationFailure
        {
          Type = ValidationFailureType.Character,
          SubType = ValidationFailureSubType.ServerLevelRestriction,
          Level = characterLevel,
        };

        if (HandleValidationFailure(failure))
        {
          *bFailedServerRestriction = true.ToInt();
          return failure.StrRef;
        }
      }
      // **********************************************************************************************************************

      // *** Level Hack Check *************************************************************************************************
      // Character level is stored in an uint8_t which means if a character has say 80/80/120 as their levels it'll wrap around
      // to level 24 (280 - 256) thus not failing the above check
      int totalLevels = 0;
      for (byte i = 0; i < creatureStats.m_nNumMultiClasses; i++)
      {
        totalLevels += creatureStats.GetClassLevel(i, false.ToInt());

        if (totalLevels > joinRestrictions.MaxLevel)
        {
          OnELCLevelValidationFailure failure = new OnELCLevelValidationFailure
          {
            Type = ValidationFailureType.Character,
            SubType = ValidationFailureSubType.LevelHack,
            Level = totalLevels,
          };

          if (HandleValidationFailure(failure))
          {
            *bFailedServerRestriction = true.ToInt();
            return failure.StrRef;
          }
        }
      }
      // **********************************************************************************************************************

      // *** Colored Name Checking ********************************************************************************************
      static bool CheckColoredName(CExoLocString lsName)
      {
        CExoString sName = new CExoString();
        CExoString colorTag = "<c".ToExoString();

        for (uint i = 0; i < lsName.GetStringCount(); i++)
        {
          int nID;
          byte nGender;

          if (lsName.GetString(i, &nID, sName, &nGender).ToBool())
          {
            if (sName.Find(colorTag, 0) >= 0)
            {
              return true;
            }
          }
        }

        return false;
      }

      if (CheckColoredName(creatureStats.m_lsFirstName) || CheckColoredName(creatureStats.m_lsLastName))
      {
        OnELCValidationFailure failure = new OnELCValidationFailure
        {
          Type = ValidationFailureType.Character,
          SubType = ValidationFailureSubType.ColoredName,
          StrRef = StrRefCharacterDoesNotExist,
        };

        if (HandleValidationFailure(failure))
        {
          *bFailedServerRestriction = true.ToInt();
          return failure.StrRef;
        }
      }
      // **********************************************************************************************************************

      // *** ILR aka Inventory Checks *****************************************************************************************
      if (NwServer.Instance.ServerInfo.PlayOptions.ItemLevelRestrictions)
      {
        for (uint slot = 0; slot < NumPlayerItemSlots; slot++)
        {
          CNWSItem item = inventory.GetItemInSlot(slot);
          if (item == null)
          {
            continue;
          }

          OnELCValidationFailure failure = null;

          // Check for unidentified equipped items
          if (!item.m_bIdentified.ToBool())
          {
            failure = new OnELCItemValidationFailure
            {
              Item = item.ToNwObject<NwItem>(),
              Type = ValidationFailureType.Item,
              SubType = ValidationFailureSubType.UnidentifiedEquippedItem,
              StrRef = StrRefItemLevelRestriction,
            };
          }

          // Check the minimum equip level
          if (item.GetMinEquipLevel() > characterLevel)
          {
            failure = new OnELCItemValidationFailure
            {
              Item = item.ToNwObject<NwItem>(),
              Type = ValidationFailureType.Item,
              SubType = ValidationFailureSubType.MinEquipLevel,
              StrRef = StrRefItemLevelRestriction,
            };
          }

          if (failure != null && HandleValidationFailure(failure))
          {
            *bFailedServerRestriction = true.ToInt();
            return failure.StrRef;
          }
        }
      }

      // Strip invalid item properties for local vault servers
      if (NwServer.Instance.ServerInfo.JoiningRestrictions.AllowLocalVaultCharacters)
      {
        player.StripAllInvalidItemPropertiesInInventory(creature);
      }
      // **********************************************************************************************************************

      // *** Misc Checks ******************************************************************************************************
      // Set Plot/Immortal to false
      creature.m_bPlotObject = false.ToInt();
      creature.m_bIsImmortal = false.ToInt();
      // **********************************************************************************************************************

      // *** Character Validation (ELC) ***************************************************************************************
      // Return early if ELC is off.
      if (!NwServer.Instance.ServerInfo.PlayOptions.EnforceLegalCharacters)
      {
        return 0;
      }

      // Enforce default event scripts: default.nss
      if (EnforceDefaultEventScripts)
      {
        for (int i = 0; i < 13; i++)
        {
          creature.m_sScripts[i] = "default".ToExoString();
        }
      }

      // Enforce empty dialog resref
      if (EnforceEmptyDialog)
      {
        creatureStats.m_cDialog = new CResRef("");
      }

      // Check for non PC
      if (!creatureStats.m_bIsPC.ToBool())
      {
        OnELCValidationFailure failure = new OnELCValidationFailure
        {
          Type = ValidationFailureType.Character,
          SubType = ValidationFailureSubType.NonPCCharacter,
          StrRef = StrRefCharacterNonPlayer,
        };

        if (HandleValidationFailure(failure))
        {
          return failure.StrRef;
        }
      }

      // Check for DM character file
      if (creatureStats.m_bIsDMCharacterFile.ToBool())
      {
        OnELCValidationFailure failure = new OnELCValidationFailure
        {
          Type = ValidationFailureType.Character,
          SubType = ValidationFailureSubType.DMCharacter,
          StrRef = StrRefCharacterDungeonMaster,
        };

        if (HandleValidationFailure(failure))
        {
          return failure.StrRef;
        }
      }

      CNWRules rules = NWNXLib.Rules();

      // Check for non player race
      CNWRaceArray races = CNWRaceArray.FromPointer(rules.m_lstRaces);
      CNWRace race = creatureStats.m_nRace < rules.m_nNumRaces ? races[creatureStats.m_nRace] : null;

      if (race == null || !race.m_bIsPlayerRace.ToBool())
      {
        OnELCValidationFailure failure = new OnELCValidationFailure
        {
          Type = ValidationFailureType.Character,
          SubType = ValidationFailureSubType.NonPlayerRace,
          StrRef = StrRefCharacterNonPlayerRace,
        };

        if (HandleValidationFailure(failure))
        {
          return failure.StrRef;
        }
      }

      // Check for non player classes, class level restrictions and prestige class requirements
      // We also check class alignment restrictions for new characters only
      CNWClassArray classes = CNWClassArray.FromPointer(rules.m_lstClasses);

      for (byte multiClass = 0; multiClass < creatureStats.m_nNumMultiClasses; multiClass++)
      {
        byte classId = creatureStats.m_ClassInfo[multiClass].m_nClass;
        CNWClass classInfo = classId < rules.m_nNumClasses ? classes[classId] : null;

        if (classInfo == null)
        {
          OnELCValidationFailure failure = new OnELCValidationFailure
          {
            Type = ValidationFailureType.Character,
            SubType = ValidationFailureSubType.InvalidClass,
            StrRef = StrRefCharacterNonPlayerClass,
          };

          if (HandleValidationFailure(failure))
          {
            return failure.StrRef;
          }

          // Skip further class checks if the validation was skipped.
          continue;
        }

        if (!classInfo.m_bIsPlayerClass.ToBool())
        {
          OnELCValidationFailure failure = new OnELCValidationFailure
          {
            Type = ValidationFailureType.Character,
            SubType = ValidationFailureSubType.NonPlayerClass,
            StrRef = StrRefCharacterNonPlayerClass,
          };

          if (HandleValidationFailure(failure))
          {
            return failure.StrRef;
          }
        }

        if (classInfo.m_nMaxLevel > 0 && creatureStats.GetClassLevel(multiClass, false.ToInt()) > classInfo.m_nMaxLevel)
        {
          OnELCValidationFailure failure = new OnELCValidationFailure
          {
            Type = ValidationFailureType.Character,
            SubType = ValidationFailureSubType.ClassLevelRestriction,
            StrRef = StrRefCharacterNonPlayerClass,
          };

          if (HandleValidationFailure(failure))
          {
            return failure.StrRef;
          }
        }

        if (!creatureStats.GetMeetsPrestigeClassRequirements(classInfo).ToBool())
        {
          OnELCValidationFailure failure = new OnELCValidationFailure
          {
            Type = ValidationFailureType.Character,
            SubType = ValidationFailureSubType.PrestigeClassRequirements,
            StrRef = StrRefCharacterNonPlayerClass,
          };

          if (HandleValidationFailure(failure))
          {
            return failure.StrRef;
          }
        }

        if (multiClass == 0 && characterLevel == 1 && creatureStats.m_nExperience == 0)
        {
          if (!classInfo.GetIsAlignmentAllowed(creatureStats.GetSimpleAlignmentGoodEvil(), creatureStats.GetSimpleAlignmentLawChaos()).ToBool())
          {
            OnELCValidationFailure failure = new OnELCValidationFailure
            {
              Type = ValidationFailureType.Character,
              SubType = ValidationFailureSubType.ClassAlignmentRestriction,
              StrRef = StrRefCharacterNonPlayerClass,
            };

            if (HandleValidationFailure(failure))
            {
              return failure.StrRef;
            }
          }
        }
      }

      // Check movement rate
      if (creatureStats.m_nMovementRate != (int)MovementRate.PC)
      {
        creatureStats.SetMovementRate((int)MovementRate.PC);
      }

      // Calculate Ability Scores;
      byte[] abilityScores = new byte[6];
      byte[] abilityMods = new byte[6];

      // TODO: GetStatBonusesFromFeats

      // Get our base ability stats
      abilityScores[(int)Ability.Strength] = (byte)((creature.m_bIsPolymorphed.ToBool() ? creature.m_nPrePolymorphSTR : creatureStats.m_nStrengthBase) + abilityMods[(int)Ability.Strength]);
      abilityScores[(int)Ability.Dexterity] = (byte)((creature.m_bIsPolymorphed.ToBool() ? creature.m_nPrePolymorphDEX : creatureStats.m_nDexterityBase) + abilityMods[(int)Ability.Dexterity]);
      abilityScores[(int)Ability.Constitution] = (byte)((creature.m_bIsPolymorphed.ToBool() ? creature.m_nPrePolymorphCON : creatureStats.m_nConstitutionBase) + abilityMods[(int)Ability.Constitution]);
      abilityScores[(int)Ability.Intelligence] = (byte)(creatureStats.m_nIntelligenceBase + abilityMods[(int)Ability.Intelligence]);
      abilityScores[(int)Ability.Wisdom] = (byte)(creatureStats.m_nWisdomBase + abilityMods[(int)Ability.Wisdom]);
      abilityScores[(int)Ability.Charisma] = (byte)(creatureStats.m_nCharismaBase + abilityMods[(int)Ability.Charisma]);

      // Get the level 1 ability values
      for (int level = 4; level < characterLevel; level += 4)
      {
        byte abilityGain = creatureStats.GetLevelStats((byte)(level - 1)).m_nAbilityGain;
        if (abilityGain < abilityScores.Length)
        {
          abilityScores[abilityGain]--;
        }
      }

      int charGenBaseAbilityMin = rules.GetRulesetIntEntry("CHARGEN_BASE_ABILITY_MIN".ToExoString(), 8);
      int charGenBaseAbilityMax = rules.GetRulesetIntEntry("CHARGEN_BASE_ABILITY_MAX".ToExoString(), 18);

      // Check if >18 in an ability
      foreach (byte abilityScore in abilityScores)
      {
        if (abilityScore > charGenBaseAbilityMax)
        {
          OnELCValidationFailure failure = new OnELCValidationFailure
          {
            Type = ValidationFailureType.Character,
            SubType = ValidationFailureSubType.StartingAbilityValueMax,
            StrRef = StrRefCharacterInvalidAbilityScores,
          };

          if (HandleValidationFailure(failure))
          {
            return failure.StrRef;
          }
        }
      }

      return 0;
    }

    private bool HandleValidationFailure(OnELCValidationFailure eventData)
    {
      return true;
    }

    public void Dispose()
    {
      validateCharacterHook.Dispose();
    }
  }
}
