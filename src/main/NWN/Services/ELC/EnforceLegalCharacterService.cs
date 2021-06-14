using System;
using System.Collections.Generic;
using System.Linq;
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
    private const int INVENTORY_SLOT_MAX = 17;
    private const int NUM_CREATURE_ITEM_SLOTS = 4;
    private const int NUM_MULTICLASS = 3;
    private const int CHARACTER_EPIC_LEVEL = 21;
    private const int NUM_SPELL_SLOTS = 10;
    private const int ABILITY_MAX = 5;

    private delegate int ValidateCharacterHook(void* pPlayer, int* bFailedServerRestriction);

    private readonly FunctionHook<ValidateCharacterHook> validateCharacterHook;

    private readonly CNWRules pRules;
    private readonly CNWRaceArray races;
    private readonly CNWClassArray classes;
    private readonly CNWSkillArray skills;
    private readonly CNWFeatArray feats;

    private readonly int epicGreatStatBonus;
    private readonly int charGenBaseAbilityMin;
    private readonly int charGenBaseAbilityMax;
    private readonly int abilityCostIncrement2;
    private readonly int abilityCostIncrement3;
    private readonly int skillMaxLevel1Bonus;

    public bool EnforceDefaultEventScripts { get; set; }
    public bool EnforceEmptyDialog { get; set; }

    public EnforceLegalCharacterService(HookService hookService)
    {
      validateCharacterHook = hookService.RequestHook<ValidateCharacterHook>(OnValidateCharacter, FunctionsLinux._ZN10CNWSPlayer17ValidateCharacterEPi, HookOrder.Final);

      pRules = NWNXLib.Rules();
      races = CNWRaceArray.FromPointer(pRules.m_lstRaces);
      classes = CNWClassArray.FromPointer(pRules.m_lstClasses);
      skills = CNWSkillArray.FromPointer(pRules.m_lstSkills);
      feats = CNWFeatArray.FromPointer(pRules.m_lstFeats);

      epicGreatStatBonus = pRules.GetRulesetIntEntry("CRULES_EPIC_GREAT_STAT_BONUS".ToExoString(), 1);
      charGenBaseAbilityMin = pRules.GetRulesetIntEntry("CHARGEN_BASE_ABILITY_MIN".ToExoString(), 8);
      charGenBaseAbilityMax = pRules.GetRulesetIntEntry("CHARGEN_BASE_ABILITY_MAX".ToExoString(), 18);
      abilityCostIncrement2 = pRules.GetRulesetIntEntry("CHARGEN_ABILITY_COST_INCREMENT2".ToExoString(), 14);
      abilityCostIncrement3 = pRules.GetRulesetIntEntry("CHARGEN_ABILITY_COST_INCREMENT3".ToExoString(), 16);
      skillMaxLevel1Bonus = pRules.GetRulesetIntEntry("CHARGEN_SKILL_MAX_LEVEL_1_BONUS".ToExoString(), 3);
    }

    private int OnValidateCharacter(void* player, int* bFailedServerRestriction)
    {
      CNWSPlayer pPlayer = CNWSPlayer.FromPointer(player);

      // *** Sanity Checks ****************************************************************************************************
      if (pPlayer == null)
      {
        return StrRefCharacterDoesNotExist;
      }

      ICGameObject pGameObject = LowLevel.ServerExoApp.GetGameObject(pPlayer.m_oidNWSObject);
      if (pGameObject == null)
      {
        return StrRefCharacterDoesNotExist;
      }

      CNWSCreature pCreature = pGameObject.AsNWSCreature();
      if (pCreature == null)
      {
        return StrRefCharacterDoesNotExist;
      }

      CNWSCreatureStats pCreatureStats = pCreature.m_pStats;
      if (pCreatureStats == null)
      {
        return StrRefCharacterDoesNotExist;
      }

      CNWSInventory pInventory = pCreature.m_pInventory;
      if (pInventory == null)
      {
        return StrRefCharacterDoesNotExist;
      }
      // **********************************************************************************************************************

      // *** Server Restrictions **********************************************************************************************
      CServerInfo pServerInfo = NWNXLib.AppManager().m_pServerExoApp.GetServerInfo();

      *bFailedServerRestriction = false.ToInt();
      byte nCharacterLevel = pCreatureStats.GetLevel(false.ToInt());

      // *** Level Restriction Check ******************************************************************************************
      if (nCharacterLevel < pServerInfo.m_JoiningRestrictions.nMinLevel ||
        nCharacterLevel > pServerInfo.m_JoiningRestrictions.nMaxLevel)
      {
        OnELCLevelValidationFailure failure = new OnELCLevelValidationFailure
        {
          Type = ValidationFailureType.Character,
          SubType = ValidationFailureSubType.ServerLevelRestriction,
          Level = nCharacterLevel,
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
      int nTotalLevels = 0;
      for (byte i = 0; i < pCreatureStats.m_nNumMultiClasses; i++)
      {
        nTotalLevels += pCreatureStats.GetClassLevel(i, false.ToInt());

        if (nTotalLevels > pServerInfo.m_JoiningRestrictions.nMaxLevel)
        {
          OnELCLevelValidationFailure failure = new OnELCLevelValidationFailure
          {
            Type = ValidationFailureType.Character,
            SubType = ValidationFailureSubType.LevelHack,
            Level = nTotalLevels,
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

      if (CheckColoredName(pCreatureStats.m_lsFirstName) || CheckColoredName(pCreatureStats.m_lsLastName))
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

      // Only check if ILR is enabled
      if (pServerInfo.m_PlayOptions.bItemLevelRestrictions.ToBool())
      {
        for (uint slot = 0; slot <= INVENTORY_SLOT_MAX - NUM_CREATURE_ITEM_SLOTS; slot++)
        {
          CNWSItem pItem = pInventory.GetItemInSlot(slot);
          if (pItem == null)
          {
            continue;
          }

          OnELCValidationFailure failure = null;

          // Check for unidentified equipped items
          if (!pItem.m_bIdentified.ToBool())
          {
            failure = new OnELCItemValidationFailure
            {
              Item = pItem.ToNwObject<NwItem>(),
              Type = ValidationFailureType.Item,
              SubType = ValidationFailureSubType.UnidentifiedEquippedItem,
              StrRef = StrRefItemLevelRestriction,
            };
          }

          // Check the minimum equip level
          if (pItem.GetMinEquipLevel() > nCharacterLevel)
          {
            failure = new OnELCItemValidationFailure
            {
              Item = pItem.ToNwObject<NwItem>(),
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
      if (pServerInfo.m_JoiningRestrictions.bAllowLocalVaultChars.ToBool())
      {
        pPlayer.StripAllInvalidItemPropertiesInInventory(pCreature);
      }
      // **********************************************************************************************************************

      // *** Misc Checks ******************************************************************************************************
      // Set Plot/Immortal to false
      pCreature.m_bPlotObject = false.ToInt();
      pCreature.m_bIsImmortal = false.ToInt();
      // **********************************************************************************************************************

      // *** Character Validation (ELC) ***************************************************************************************
      // Return early if ELC is off.
      if (!pServerInfo.m_PlayOptions.bEnforceLegalCharacters.ToBool())
      {
        return 0;
      }

      // Enforce default event scripts: default.nss
      if (EnforceDefaultEventScripts)
      {
        for (int i = 0; i < 13; i++)
        {
          pCreature.m_sScripts[i] = "default".ToExoString();
        }
      }

      // Enforce empty dialog resref
      if (EnforceEmptyDialog)
      {
        pCreatureStats.m_cDialog = new CResRef("");
      }

      // Check for non PC
      if (!pCreatureStats.m_bIsPC.ToBool())
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
      if (pCreatureStats.m_bIsDMCharacterFile.ToBool())
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

      // Check for non player race
      CNWRace pRace = pCreatureStats.m_nRace < pRules.m_nNumRaces ? races[pCreatureStats.m_nRace] : null;

      if (pRace == null || !pRace.m_bIsPlayerRace.ToBool())
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
      for (byte nMultiClass = 0; nMultiClass < pCreatureStats.m_nNumMultiClasses; nMultiClass++)
      {
        byte classId = pCreatureStats.m_ClassInfo[nMultiClass].m_nClass;
        CNWClass pClass = classId < pRules.m_nNumClasses ? classes[classId] : null;

        if (pClass == null)
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

        if (!pClass.m_bIsPlayerClass.ToBool())
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

        if (pClass.m_nMaxLevel > 0 && pCreatureStats.GetClassLevel(nMultiClass, false.ToInt()) > pClass.m_nMaxLevel)
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

        if (!pCreatureStats.GetMeetsPrestigeClassRequirements(pClass).ToBool())
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

        if (nMultiClass == 0 && nCharacterLevel == 1 && pCreatureStats.m_nExperience == 0)
        {
          if (!pClass.GetIsAlignmentAllowed(pCreatureStats.GetSimpleAlignmentGoodEvil(), pCreatureStats.GetSimpleAlignmentLawChaos()).ToBool())
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
      if (pCreatureStats.m_nMovementRate != (int)MovementRate.PC)
      {
        pCreatureStats.SetMovementRate((int)MovementRate.PC);
      }

      // Calculate Ability Scores;
      byte[] nAbility = new byte[6];
      int[] nMods = GetStatBonusesFromFeats(pCreatureStats.m_lstFeats, true);

      // Get our base ability stats
      nAbility[(int)Ability.Strength] =
        (byte)((pCreature.m_bIsPolymorphed.ToBool() ? pCreature.m_nPrePolymorphSTR : pCreatureStats.m_nStrengthBase) + nMods[(int)Ability.Strength]);
      nAbility[(int)Ability.Dexterity] =
        (byte)((pCreature.m_bIsPolymorphed.ToBool() ? pCreature.m_nPrePolymorphDEX : pCreatureStats.m_nDexterityBase) + nMods[(int)Ability.Dexterity]);
      nAbility[(int)Ability.Constitution] =
        (byte)((pCreature.m_bIsPolymorphed.ToBool() ? pCreature.m_nPrePolymorphCON : pCreatureStats.m_nConstitutionBase) + nMods[(int)Ability.Constitution]);
      nAbility[(int)Ability.Intelligence] = (byte)(pCreatureStats.m_nIntelligenceBase + nMods[(int)Ability.Intelligence]);
      nAbility[(int)Ability.Wisdom] = (byte)(pCreatureStats.m_nWisdomBase + nMods[(int)Ability.Wisdom]);
      nAbility[(int)Ability.Charisma] = (byte)(pCreatureStats.m_nCharismaBase + nMods[(int)Ability.Charisma]);

      // Get the level 1 ability values
      for (int nLevel = 4; nLevel <= nCharacterLevel; nLevel += 4)
      {
        byte nAbilityGain = pCreatureStats.GetLevelStats((byte)(nLevel - 1)).m_nAbilityGain;
        if (nAbilityGain <= ABILITY_MAX)
        {
          nAbility[nAbilityGain]--;
        }
      }

      // Check if >18 in an ability
      for (int nAbilityIndex = 0; nAbilityIndex <= ABILITY_MAX; nAbilityIndex++)
      {
        if (nAbility[nAbilityIndex] > charGenBaseAbilityMax)
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

      // Point Buy System calculation
      int[] nAbilityAtLevel = new int[6];
      int nPointBuy = pRace.m_nAbilitiesPointBuyNumber;

      for (int nAbilityIndex = 0; nAbilityIndex <= ABILITY_MAX; nAbilityIndex++)
      {
        nAbilityAtLevel[nAbilityIndex] = nAbility[nAbilityIndex];

        while (nAbility[nAbilityIndex] > charGenBaseAbilityMin)
        {
          if (nAbility[nAbilityIndex] > abilityCostIncrement3)
          {
            if (nPointBuy < 3)
            {
              OnELCValidationFailure failure = new OnELCValidationFailure
              {
                Type = ValidationFailureType.Character,
                SubType = ValidationFailureSubType.AbilityPointBuySystemCalculation,
                StrRef = StrRefCharacterInvalidAbilityScores,
              };

              if (HandleValidationFailure(failure))
              {
                return failure.StrRef;
              }
            }

            nAbility[nAbilityIndex]--;
            nPointBuy -= 3;
          }
          else if (nAbility[nAbilityIndex] > abilityCostIncrement2)
          {
            if (nPointBuy < 2)
            {
              OnELCValidationFailure failure = new OnELCValidationFailure
              {
                Type = ValidationFailureType.Character,
                SubType = ValidationFailureSubType.AbilityPointBuySystemCalculation,
                StrRef = StrRefCharacterInvalidAbilityScores,
              };

              if (HandleValidationFailure(failure))
              {
                return failure.StrRef;
              }
            }

            nAbility[nAbilityIndex]--;
            nPointBuy -= 2;
          }
          else
          {
            if (nPointBuy < 1)
            {
              OnELCValidationFailure failure = new OnELCValidationFailure
              {
                Type = ValidationFailureType.Character,
                SubType = ValidationFailureSubType.AbilityPointBuySystemCalculation,
                StrRef = StrRefCharacterInvalidAbilityScores,
              };

              if (HandleValidationFailure(failure))
              {
                return failure.StrRef;
              }
            }

            nAbility[nAbilityIndex]--;
            nPointBuy--;
          }
        }
      }

      // Get Cleric Domain Feats
      ushort? nDomainFeat1 = null;
      ushort? nDomainFeat2 = null;

      for (byte nMultiClass = 0; nMultiClass < pCreatureStats.m_nNumMultiClasses; nMultiClass++)
      {
        if (classes[pCreatureStats.GetClass(nMultiClass)].m_bHasDomains.ToBool())
        {
          CNWDomain pDomain = pRules.GetDomain(pCreatureStats.GetDomain1(nMultiClass));

          if (pDomain != null)
          {
            nDomainFeat1 = pDomain.m_nGrantedFeat;
          }

          pDomain = pRules.GetDomain(pCreatureStats.GetDomain2(nMultiClass));
          if (pDomain != null)
          {
            nDomainFeat2 = pDomain.m_nGrantedFeat;
          }
        }
      }

      // *** Character Per-Level Checks********************************************************************************************

      // Init some vars
      byte[] nMultiClassLevel = new byte[NUM_MULTICLASS];
      int nSkillPointsRemaining = 0;
      byte[] listSkillRanks = new byte[pRules.m_nNumSkills];
      HashSet<ushort> listFeats = new HashSet<ushort>();
      HashSet<ushort> listChosenFeats = new HashSet<ushort>();
      // [nMultiClass][nSpellLevel] -> {SpellIDs}
      List<Dictionary<uint, HashSet<uint>>> listSpells = new List<Dictionary<uint, HashSet<uint>>>(NUM_MULTICLASS);

      for (int nLevel = 1; nLevel <= nCharacterLevel; nLevel++)
      {
        // Grab our level stats and figure out which class was leveled
        CNWLevelStats pLevelStats = pCreatureStats.GetLevelStats((byte)(nLevel - 1));
        byte nClassLeveledUpIn = pLevelStats.m_nClass;
        CNWClass pClassLeveledUpIn = classes[nClassLeveledUpIn];

        // Keep track of multiclass levels
        byte nMultiClassLeveledUpIn = 0;
        for (byte nMultiClass = 0; nMultiClass < pCreatureStats.m_nNumMultiClasses; nMultiClass++)
        {
          if (nClassLeveledUpIn == pCreatureStats.GetClass(nMultiClass))
          {
            nMultiClassLevel[nMultiClass]++;
            nMultiClassLeveledUpIn = nMultiClass;
            break;
          }
        }

        // Check if our first level class is a spellcaster and if their primary casting stat is >= 11
        if (nLevel == 1 && pClassLeveledUpIn.m_bIsSpellCasterClass.ToBool())
        {
          if (nAbilityAtLevel[pClassLeveledUpIn.m_nPrimaryAbility] < 11)
          {
            OnELCValidationFailure failure = new OnELCValidationFailure
            {
              Type = ValidationFailureType.Character,
              SubType = ValidationFailureSubType.ClassSpellcasterInvalidPrimaryStat,
              StrRef = StrRefCharacterInvalidAbilityScores,
            };

            if (HandleValidationFailure(failure))
            {
              return failure.StrRef;
            }
          }
        }

        // Check Epic Level Flag
        if (nLevel < CHARACTER_EPIC_LEVEL)
        {
          if (pLevelStats.m_bEpic != 0)
          {
            OnELCValidationFailure failure = new OnELCValidationFailure
            {
              Type = ValidationFailureType.Feat,
              SubType = ValidationFailureSubType.EpicLevelFlag,
              StrRef = StrRefFeatInvalid,
            };

            if (HandleValidationFailure(failure))
            {
              return failure.StrRef;
            }
          }
        }
        else
        {
          if (pLevelStats.m_bEpic == 0)
          {
            OnELCValidationFailure failure = new OnELCValidationFailure
            {
              Type = ValidationFailureType.Feat,
              SubType = ValidationFailureSubType.EpicLevelFlag,
              StrRef = StrRefFeatInvalid,
            };

            if (HandleValidationFailure(failure))
            {
              return failure.StrRef;
            }
          }
        }

        // Keep track of our ability values
        if (nLevel % 4 == 0)
        {
          nAbilityAtLevel[pLevelStats.m_nAbilityGain]++;
        }

        // Get the stat bonus from feats
        int[] nStatMods = GetStatBonusesFromFeats(pLevelStats.m_lstFeats, false);

        // Update our ability values
        for (int nAbilityIndex = 0; nAbilityIndex <= ABILITY_MAX; nAbilityIndex++)
        {
          nAbilityAtLevel[nAbilityIndex] += nStatMods[nAbilityIndex];
        }

        for (byte nMultiClass = 0; nMultiClass < NUM_MULTICLASS; nMultiClass++)
        {
          byte nClassId = pCreatureStats.GetClass(nMultiClass);
          CNWClass pClass = nClassId < pRules.m_nNumClasses ? classes[nClassId] : null;

          if (pClass != null)
          {
            for (int nAbilityIndex = 0; nAbilityIndex < ABILITY_MAX; nAbilityIndex++)
            {
              nAbilityAtLevel[nAbilityIndex] += pClass.GetAbilityGainForSingleLevel(nAbilityIndex, nMultiClassLevel[nMultiClassLeveledUpIn]);
            }
          }
        }

        // *** Check Hit Die ********************************************************************************************************
        if (pLevelStats.m_nHitDie > pCreatureStats.GetHitDie(nMultiClassLeveledUpIn, nClassLeveledUpIn))
        {
          OnELCValidationFailure failure = new OnELCValidationFailure
          {
            Type = ValidationFailureType.Character,
            SubType = ValidationFailureSubType.TooManyHitPoints,
            StrRef = StrRefCharacterTooManyHitpoints,
          };

          if (HandleValidationFailure(failure))
          {
            return failure.StrRef;
          }
        }
        // **************************************************************************************************************************

        // *** Check Skills *********************************************************************************************************
        // Calculate the skillpoints we gained this level
        int GetSkillPointAbilityAdjust()
        {
          switch ((Ability)pRace.m_nSkillPointModifierAbility)
          {
            case Ability.Strength:
              return pRace.m_nSTRAdjust;
            case Ability.Dexterity:
              return pRace.m_nDEXAdjust;
            case Ability.Constitution:
              return pRace.m_nCONAdjust;
            case Ability.Intelligence:
              return pRace.m_nINTAdjust;
            case Ability.Wisdom:
              return pRace.m_nWISAdjust;
            case Ability.Charisma:
              return pRace.m_nCHAAdjust;
            default:
              return 0;
          }
        }

        int numSkillPoints = pRace.m_nSkillPointModifierAbility >= 0 && pRace.m_nSkillPointModifierAbility <= ABILITY_MAX
          ? pCreatureStats.CalcStatModifier((byte)(nAbilityAtLevel[pRace.m_nSkillPointModifierAbility] + GetSkillPointAbilityAdjust()))
          : 0;

        if (nLevel == 1)
        {
          nSkillPointsRemaining += pRace.m_nFirstLevelSkillPointsMultiplier * Math.Max(1, pClassLeveledUpIn.m_nSkillPointBase + numSkillPoints);
          nSkillPointsRemaining += pRace.m_nFirstLevelSkillPointsMultiplier * pRace.m_nExtraSkillPointsPerLevel;
        }
        else
        {
          nSkillPointsRemaining += Math.Max(1, pClassLeveledUpIn.m_nSkillPointBase + numSkillPoints);
          nSkillPointsRemaining += pRace.m_nExtraSkillPointsPerLevel;
        }

        // Loop all the skills and check our LevelStats to see what changed.
        for (ushort nSkill = 0; nSkill < pRules.m_nNumSkills; nSkill++)
        {
          CNWSkill pSkill = skills[nSkill];
          byte nRankChange = (byte)pLevelStats.m_lstSkillRanks[nSkill];

          if (nRankChange != 0)
          {
            // Figure out if we can use the skill and if it's a class skill
            bool bCanUse = false;
            bool bClassSkill = false;

            if (pSkill.m_bAllClassesCanUse.ToBool())
            {
              bCanUse = true;
            }

            if (pClassLeveledUpIn.IsSkillUseable(nSkill).ToBool())
            {
              bCanUse = true;

              if (pClassLeveledUpIn.IsSkillClassSkill(nSkill).ToBool())
              {
                bClassSkill = true;
              }
            }

            // We must be able to use the skill
            if (!bCanUse)
            {
              OnELCValidationFailure failure = new OnELCValidationFailure
              {
                Type = ValidationFailureType.Skill,
                SubType = ValidationFailureSubType.UnusableSkill,
                StrRef = StrRefSkillUnuseable,
              };

              if (HandleValidationFailure(failure))
              {
                return failure.StrRef;
              }
            }

            // Check if we have enough available points
            if (bClassSkill)
            {
              if (nRankChange > nSkillPointsRemaining)
              {
                OnELCValidationFailure failure = new OnELCValidationFailure
                {
                  Type = ValidationFailureType.Skill,
                  SubType = ValidationFailureSubType.NotEnoughSkillPoints,
                  StrRef = StrRefSkillInvalidNumSkillpoints,
                };

                if (HandleValidationFailure(failure))
                {
                  return failure.StrRef;
                }
              }

              nSkillPointsRemaining -= nRankChange;
            }
            else
            {
              if (nRankChange * 2 > nSkillPointsRemaining)
              {
                OnELCValidationFailure failure = new OnELCValidationFailure
                {
                  Type = ValidationFailureType.Skill,
                  SubType = ValidationFailureSubType.NotEnoughSkillPoints,
                  StrRef = StrRefSkillInvalidNumSkillpoints,
                };

                if (HandleValidationFailure(failure))
                {
                  return failure.StrRef;
                }
              }

              nSkillPointsRemaining -= nRankChange * 2;
            }

            // Increase the rank for the skill
            listSkillRanks[nSkill] += nRankChange;

            // Can't have more than Level + 3 in a class skill, or (Level + 3) / 2 for a non class skill
            if (bClassSkill)
            {
              if (listSkillRanks[nSkill] > nLevel + skillMaxLevel1Bonus)
              {
                OnELCValidationFailure failure = new OnELCValidationFailure
                {
                  Type = ValidationFailureType.Skill,
                  SubType = ValidationFailureSubType.InvalidNumRanksInClassSkill,
                  StrRef = StrRefSkillInvalidRanks,
                };

                if (HandleValidationFailure(failure))
                {
                  return failure.StrRef;
                }
              }
            }
            else
            {
              if (listSkillRanks[nSkill] > (nLevel + skillMaxLevel1Bonus) / 2)
              {
                OnELCValidationFailure failure = new OnELCValidationFailure
                {
                  Type = ValidationFailureType.Skill,
                  SubType = ValidationFailureSubType.InvalidNumRanksInNonClassSkill,
                  StrRef = StrRefSkillInvalidRanks,
                };

                if (HandleValidationFailure(failure))
                {
                  return failure.StrRef;
                }
              }
            }
          }
        }

        // Compare the remaining skillpoints in LevelStats with our own calculation
        if (pLevelStats.m_nSkillPointsRemaining > nSkillPointsRemaining)
        {
          OnELCValidationFailure failure = new OnELCValidationFailure
          {
            Type = ValidationFailureType.Skill,
            SubType = ValidationFailureSubType.InvalidNumRemainingSkillPoints,
            StrRef = StrRefSkillInvalidNumSkillpoints,
          };

          if (HandleValidationFailure(failure))
          {
            return failure.StrRef;
          }
        }

        // **************************************************************************************************************************

        // *** Check Feats **********************************************************************************************************
        // Calculate the number of normal and bonus feats for this level
        int nNumberNormalFeats = 0;
        int nNumberBonusFeats = 0;

        // First and every nth level gets a normal feat
        if ((nLevel == 1) ||
          ((pRace.m_nNormalFeatEveryNthLevel != 0) && (nLevel % pRace.m_nNormalFeatEveryNthLevel == 0)))
        {
          nNumberNormalFeats = pRace.m_nNumberNormalFeatsEveryNthLevel;
        }

        // Add any extra first level feats
        if (nLevel == 1)
        {
          nNumberNormalFeats += pRace.m_nExtraFeatsAtFirstLevel;
        }

        nNumberBonusFeats = pClassLeveledUpIn.GetBonusFeats(nMultiClassLevel[nMultiClassLeveledUpIn]);

        // Add this level's gained feats to our own list
        for (int nFeatIndex = 0; nFeatIndex < pLevelStats.m_lstFeats.num; nFeatIndex++)
        {
          ushort nFeat = *pLevelStats.m_lstFeats._OpIndex(nFeatIndex);
          CNWFeat feat = nFeat < pRules.m_nNumFeats ? feats[nFeat] : null;

          if (feat == null)
          {
            OnELCValidationFailure failure = new OnELCValidationFailure
            {
              Type = ValidationFailureType.Feat,
              SubType = ValidationFailureSubType.InvalidFeat,
              StrRef = StrRefFeatInvalid,
            };

            if (HandleValidationFailure(failure))
            {
              return failure.StrRef;
            }
          }

          bool bGranted = false;

          // Check if this is a feat that's automatically granted at first level
          if (nLevel == 1)
          {
            if (pRace.IsFirstLevelGrantedFeat(nFeat).ToBool())
            {
              listFeats.Add(nFeat);
              bGranted = true;
            }
          }

          // Check if this is a feat that's automatically granted for this level
          if (!bGranted)
          {
            byte nLevelGranted;
            if (pClassLeveledUpIn.IsGrantedFeat(nFeat, &nLevelGranted).ToBool())
            {
              if (nLevelGranted == nMultiClassLevel[nMultiClassLeveledUpIn])
              {
                listFeats.Add(nFeat);
                bGranted = true;
              }
            }
          }

          // Check if it's one of our cleric domain feats
          if (!bGranted)
          {
            if (pClassLeveledUpIn.m_bHasDomains.ToBool() && (nMultiClassLevel[nMultiClassLeveledUpIn] == 1))
            {
              if ((nFeat == nDomainFeat1) || (nFeat == nDomainFeat2))
              {
                listFeats.Add(nFeat);
                bGranted = true;
              }
            }
          }

          // Check if it's the "EpicCharacter" feat and we're level 21
          if (!bGranted)
          {
            if (nLevel == CHARACTER_EPIC_LEVEL && nFeat == (int)Feat.EpicCharacter)
            {
              listFeats.Add(nFeat);
              bGranted = true;
            }
          }

          // Not a granted feat, add it to listChosenFeats
          if (!bGranted)
          {
            listChosenFeats.Add(nFeat);
          }
        }
      }

      return 0;
    }

    private bool HandleValidationFailure(OnELCValidationFailure eventData)
    {
      return true;
    }

    private int[] GetStatBonusesFromFeats(CExoArrayListUInt16 lstFeats, bool subtractBonuses)
    {
      int[] abilityMods = new int[6];

      HashSet<Feat> feats = new HashSet<Feat>();
      for (int i = 0; i < lstFeats.num; i++)
      {
        feats.Add((Feat)(*lstFeats._OpIndex(i)));
      }

      int GetFeatCount(params Feat[] epicFeats)
      {
        return epicFeats.Count(feat => feats.Contains(feat));
      }

      abilityMods[(int)Ability.Strength] += epicGreatStatBonus * GetFeatCount(
        Feat.EpicGreatStrength1,
        Feat.EpicGreatStrength2,
        Feat.EpicGreatStrength3,
        Feat.EpicGreatStrength4,
        Feat.EpicGreatStrength5,
        Feat.EpicGreatStrength6,
        Feat.EpicGreatStrength7,
        Feat.EpicGreatStrength8,
        Feat.EpicGreatStrength9,
        Feat.EpicGreatStrength10);

      abilityMods[(int)Ability.Dexterity] += epicGreatStatBonus * GetFeatCount(
        Feat.EpicGreatDexterity1,
        Feat.EpicGreatDexterity2,
        Feat.EpicGreatDexterity3,
        Feat.EpicGreatDexterity4,
        Feat.EpicGreatDexterity5,
        Feat.EpicGreatDexterity6,
        Feat.EpicGreatDexterity7,
        Feat.EpicGreatDexterity8,
        Feat.EpicGreatDexterity9,
        Feat.EpicGreatDexterity10);

      abilityMods[(int)Ability.Constitution] += epicGreatStatBonus * GetFeatCount(
        Feat.EpicGreatConstitution1,
        Feat.EpicGreatConstitution2,
        Feat.EpicGreatConstitution3,
        Feat.EpicGreatConstitution4,
        Feat.EpicGreatConstitution5,
        Feat.EpicGreatConstitution6,
        Feat.EpicGreatConstitution7,
        Feat.EpicGreatConstitution8,
        Feat.EpicGreatConstitution9,
        Feat.EpicGreatConstitution10);

      abilityMods[(int)Ability.Intelligence] += epicGreatStatBonus * GetFeatCount(
        Feat.EpicGreatIntelligence1,
        Feat.EpicGreatIntelligence2,
        Feat.EpicGreatIntelligence3,
        Feat.EpicGreatIntelligence4,
        Feat.EpicGreatIntelligence5,
        Feat.EpicGreatIntelligence6,
        Feat.EpicGreatIntelligence7,
        Feat.EpicGreatIntelligence8,
        Feat.EpicGreatIntelligence9,
        Feat.EpicGreatIntelligence10);

      abilityMods[(int)Ability.Wisdom] += epicGreatStatBonus * GetFeatCount(
        Feat.EpicGreatWisdom1,
        Feat.EpicGreatWisdom2,
        Feat.EpicGreatWisdom3,
        Feat.EpicGreatWisdom4,
        Feat.EpicGreatWisdom5,
        Feat.EpicGreatWisdom6,
        Feat.EpicGreatWisdom7,
        Feat.EpicGreatWisdom8,
        Feat.EpicGreatWisdom9,
        Feat.EpicGreatWisdom10);

      abilityMods[(int)Ability.Charisma] += epicGreatStatBonus * GetFeatCount(
        Feat.EpicGreatCharisma1,
        Feat.EpicGreatCharisma2,
        Feat.EpicGreatCharisma3,
        Feat.EpicGreatCharisma4,
        Feat.EpicGreatCharisma5,
        Feat.EpicGreatCharisma6,
        Feat.EpicGreatCharisma7,
        Feat.EpicGreatCharisma8,
        Feat.EpicGreatCharisma9,
        Feat.EpicGreatCharisma10);

      if (subtractBonuses)
      {
        abilityMods[(int)Ability.Strength] *= -1;
        abilityMods[(int)Ability.Dexterity] *= -1;
        abilityMods[(int)Ability.Constitution] *= -1;
        abilityMods[(int)Ability.Intelligence] *= -1;
        abilityMods[(int)Ability.Wisdom] *= -1;
        abilityMods[(int)Ability.Charisma] *= -1;
      }

      return abilityMods;
    }

    public void Dispose()
    {
      validateCharacterHook.Dispose();
    }
  }
}
