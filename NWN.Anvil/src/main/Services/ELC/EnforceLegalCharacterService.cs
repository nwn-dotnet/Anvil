using System;
using System.Collections.Generic;
using System.Linq;
using Anvil.API;
using Anvil.Internal;
using Anvil.Native;
using NWN.Native.API;
using Ability = NWN.Native.API.Ability;
using Feat = NWN.Native.API.Feat;
using MovementRate = NWN.Native.API.MovementRate;

namespace Anvil.Services
{
  [ServiceBinding(typeof(EnforceLegalCharacterService))]
  [ServiceBindingOptions(Lazy = true)]
  public sealed unsafe class EnforceLegalCharacterService : IDisposable
  {
    private const int AbilityMax = 5;
    private const int CharacterEpicLevel = 21;

    // Magic Numbers
    private const int InventorySlotMax = 17;
    private const int NumCreatureItemSlots = 4;
    private const int NumMultiClass = 8;
    private const int NumSpellLevels = 10;

    // Validation Failure STRREFs
    private static readonly StrRef StrRefCharacterDoesNotExist = new StrRef(63767);
    private static readonly StrRef StrRefCharacterDungeonMaster = new StrRef(67641);
    private static readonly StrRef StrRefCharacterInvalidAbilityScores = new StrRef(63761);
    private static readonly StrRef StrRefCharacterNumberMulticlasses = new StrRef(63764);
    private static readonly StrRef StrRefCharacterLevelRestriction = new StrRef(57924);
    private static readonly StrRef StrRefCharacterNonPlayer = new StrRef(63760);
    private static readonly StrRef StrRefCharacterNonPlayerClass = new StrRef(66167);
    private static readonly StrRef StrRefCharacterNonPlayerRace = new StrRef(66166);
    private static readonly StrRef StrRefCharacterSavingThrow = new StrRef(8066);
    private static readonly StrRef StrRefCharacterTooManyHitpoints = new StrRef(3109);
    private static readonly StrRef StrRefCustom = new StrRef(164);
    private static readonly StrRef StrRefFeatInvalid = new StrRef(76383);
    private static readonly StrRef StrRefFeatReqAbility = new StrRef(66175);
    private static readonly StrRef StrRefFeatReqFeat = new StrRef(66182);
    private static readonly StrRef StrRefFeatReqSkill = new StrRef(66183);
    private static readonly StrRef StrRefFeatReqSpellLevel = new StrRef(66176);
    private static readonly StrRef StrRefFeatTooMany = new StrRef(66222);
    private static readonly StrRef StrRefItemLevelRestriction = new StrRef(68521);
    private static readonly StrRef StrRefSkillInvalidNumSkillpoints = new StrRef(66155);
    private static readonly StrRef StrRefSkillInvalidRanks = new StrRef(66165);
    private static readonly StrRef StrRefSkillUnuseable = new StrRef(63815);
    private static readonly StrRef StrRefSpellIllegalLevel = new StrRef(68627);
    private static readonly StrRef StrRefSpellIllegalNumSpells = new StrRef(68630);
    private static readonly StrRef StrRefSpellIllegalRemovedSpells = new StrRef(68631);
    private static readonly StrRef StrRefSpellInvalidSpell = new StrRef(66499);
    private static readonly StrRef StrRefSpellLearnedTwice = new StrRef(68629);
    private static readonly StrRef StrRefSpellOppositeSpellSchool = new StrRef(66500);
    private static readonly StrRef StrRefSpellReqAbility = new StrRef(68628);
    private static readonly StrRef StrRefSpellReqSpellLevel = new StrRef(66498);
    private readonly int abilityCostIncrement2;
    private readonly int abilityCostIncrement3;
    private readonly int charGenBaseAbilityMax;
    private readonly int charGenBaseAbilityMin;
    private readonly CNWClassArray classes;

    private readonly int epicGreatStatBonus;
    private readonly CNWFeatArray feats;

    private readonly CNWRules pRules;
    private readonly CNWRaceArray races;
    private readonly int skillMaxLevel1Bonus;
    private readonly CNWSkillArray skills;

    private readonly FunctionHook<Functions.CNWSPlayer.ValidateCharacter> validateCharacterHook;

    // Dependencies
    private readonly VirtualMachine virtualMachine;

    public EnforceLegalCharacterService(VirtualMachine virtualMachine, HookService hookService)
    {
      this.virtualMachine = virtualMachine;
      validateCharacterHook = hookService.RequestHook<Functions.CNWSPlayer.ValidateCharacter>(OnValidateCharacter, HookOrder.Final);

      pRules = NWNXLib.Rules();
      races = CNWRaceArray.FromPointer(pRules.m_lstRaces);
      classes = CNWClassArray.FromPointer(pRules.m_lstClasses);
      skills = CNWSkillArray.FromPointer(pRules.m_lstSkills);
      feats = CNWFeatArray.FromPointer(pRules.m_lstFeats);

      epicGreatStatBonus = pRules.GetRulesetIntEntry(RulesetKeys.EPIC_GREAT_STAT_BONUS, 1);
      charGenBaseAbilityMin = pRules.GetRulesetIntEntry(RulesetKeys.CHARGEN_BASE_ABILITY_MIN, 8);
      charGenBaseAbilityMax = pRules.GetRulesetIntEntry(RulesetKeys.CHARGEN_BASE_ABILITY_MAX, 18);
      abilityCostIncrement2 = pRules.GetRulesetIntEntry(RulesetKeys.CHARGEN_ABILITY_COST_INCREMENT2, 14);
      abilityCostIncrement3 = pRules.GetRulesetIntEntry(RulesetKeys.CHARGEN_ABILITY_COST_INCREMENT3, 16);
      skillMaxLevel1Bonus = pRules.GetRulesetIntEntry(RulesetKeys.CHARGEN_SKILL_MAX_LEVEL_1_BONUS, 3);
    }

    public event Action<OnELCCustomCheck>? OnCustomCheck;

    public event Action<OnELCValidationBefore>? OnValidationBefore;

    public event Action<OnELCValidationFailure>? OnValidationFailure;

    public event Action<OnELCValidationSuccess>? OnValidationSuccess;

    public bool EnforceDefaultEventScripts { get; set; }
    public bool EnforceEmptyDialog { get; set; }

    public void Dispose()
    {
      validateCharacterHook.Dispose();
    }

    private int[] GetStatBonusesFromFeats(CExoArrayListUInt16 lstFeats, bool subtractBonuses)
    {
      int[] abilityMods = new int[6];

      HashSet<Feat> creatureFeats = new HashSet<Feat>();
      foreach (ushort nFeat in lstFeats)
      {
        creatureFeats.Add((Feat)nFeat);
      }

      int GetFeatCount(params Feat[] epicFeats)
      {
        return epicFeats.Count(feat => creatureFeats.Contains(feat));
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

    private bool HandleValidationFailure(out int strRefFailure, OnELCValidationFailure eventData)
    {
      virtualMachine.ExecuteInScriptContext(() =>
      {
        OnValidationFailure?.Invoke(eventData);
      });

      strRefFailure = (int)eventData.StrRef.Id;

      return !eventData.IgnoreFailure;
    }

    private bool InvokeCustomCheck(NwPlayer player)
    {
      OnELCCustomCheck eventData = new OnELCCustomCheck
      {
        Player = player,
      };

      virtualMachine.ExecuteInScriptContext(() =>
      {
        OnCustomCheck?.Invoke(eventData);
      });

      return !eventData.IsFailed;
    }

    private void InvokeSuccessEvent(NwPlayer player)
    {
      OnELCValidationSuccess eventData = new OnELCValidationSuccess
      {
        Player = player,
      };

      virtualMachine.ExecuteInScriptContext(() =>
      {
        OnValidationSuccess?.Invoke(eventData);
      });
    }

    private void InvokeValidationBeforeEvent(NwPlayer player)
    {
      OnELCValidationBefore eventData = new OnELCValidationBefore
      {
        Player = player,
      };

      virtualMachine.ExecuteInScriptContext(() =>
      {
        OnValidationBefore?.Invoke(eventData);
      });
    }

    // ReSharper disable once FunctionComplexityOverflow
    private int OnValidateCharacter(void* player, int* bFailedServerRestriction)
    {
      CNWSPlayer pPlayer = CNWSPlayer.FromPointer(player);

      // *** Sanity Checks ****************************************************************************************************
      if (pPlayer == null)
      {
        return (int)StrRefCharacterDoesNotExist.Id;
      }

      ICGameObject pGameObject = LowLevel.ServerExoApp.GetGameObject(pPlayer.m_oidNWSObject);
      if (pGameObject == null)
      {
        return (int)StrRefCharacterDoesNotExist.Id;
      }

      CNWSCreature pCreature = pGameObject.AsNWSCreature();
      if (pCreature == null)
      {
        return (int)StrRefCharacterDoesNotExist.Id;
      }

      CNWSCreatureStats pCreatureStats = pCreature.m_pStats;
      if (pCreatureStats == null)
      {
        return (int)StrRefCharacterDoesNotExist.Id;
      }

      CNWSInventory pInventory = pCreature.m_pInventory;
      if (pInventory == null)
      {
        return (int)StrRefCharacterDoesNotExist.Id;
      }
      // **********************************************************************************************************************

      NwPlayer nwPlayer = pPlayer.ToNwPlayer()!;
      InvokeValidationBeforeEvent(nwPlayer);

      // *** Server Restrictions **********************************************************************************************
      CServerInfo pServerInfo = NWNXLib.AppManager().m_pServerExoApp.GetServerInfo();

      *bFailedServerRestriction = false.ToInt();
      byte nCharacterLevel = pCreatureStats.GetLevel(false.ToInt());

      // *** Level Restriction Check ******************************************************************************************
      if (nCharacterLevel < pServerInfo.m_JoiningRestrictions.nMinLevel ||
        nCharacterLevel > pServerInfo.m_JoiningRestrictions.nMaxLevel)
      {
        if (HandleValidationFailure(out int strRefFailure, new OnELCLevelValidationFailure
        {
          Player = nwPlayer,
          Type = ValidationFailureType.Character,
          SubType = ValidationFailureSubType.ServerLevelRestriction,
          Level = nCharacterLevel,
          StrRef = StrRefCharacterLevelRestriction,
        }))
        {
          *bFailedServerRestriction = true.ToInt();
          return strRefFailure;
        }
      }
      // **********************************************************************************************************************

      if (pCreatureStats.m_nNumMultiClasses > Math.Clamp(NWNXLib.Rules().GetRulesetIntEntry(RulesetKeys.MULTICLASS_LIMIT, 3), 1, 8))
      {
        if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
        {
          Player = nwPlayer,
          Type = ValidationFailureType.Character,
          SubType = ValidationFailureSubType.NumMulticlass,
          StrRef = StrRefCharacterNumberMulticlasses,
        }))
        {
          *bFailedServerRestriction = true.ToInt();
          return strRefFailure;
        }
      }

      // *** Level Hack Check *************************************************************************************************
      // Character level is stored in an uint8_t which means if a character has say 80/80/120 as their levels it'll wrap around
      // to level 24 (280 - 256) thus not failing the above check
      int nTotalLevels = 0;
      for (byte i = 0; i < pCreatureStats.m_nNumMultiClasses; i++)
      {
        nTotalLevels += pCreatureStats.GetClassLevel(i, false.ToInt());

        if (nTotalLevels > pServerInfo.m_JoiningRestrictions.nMaxLevel)
        {
          if (HandleValidationFailure(out int strRefFailure, new OnELCLevelValidationFailure
          {
            Player = nwPlayer,
            Type = ValidationFailureType.Character,
            SubType = ValidationFailureSubType.LevelHack,
            Level = nTotalLevels,
            StrRef = StrRefCharacterLevelRestriction,
          }))
          {
            *bFailedServerRestriction = true.ToInt();
            return strRefFailure;
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
        if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
        {
          Player = nwPlayer,
          Type = ValidationFailureType.Character,
          SubType = ValidationFailureSubType.ColoredName,
          StrRef = StrRefCharacterDoesNotExist,
        }))
        {
          *bFailedServerRestriction = true.ToInt();
          return strRefFailure;
        }
      }
      // **********************************************************************************************************************

      // *** ILR aka Inventory Checks *****************************************************************************************

      // Only check if ILR is enabled
      if (pServerInfo.m_PlayOptions.bItemLevelRestrictions.ToBool())
      {
        for (uint slot = 0; slot <= InventorySlotMax - NumCreatureItemSlots; slot++)
        {
          CNWSItem pItem = pInventory.GetItemInSlot(slot);
          if (pItem == null)
          {
            continue;
          }

          // Check for unidentified equipped items
          if (!pItem.m_bIdentified.ToBool())
          {
            if (HandleValidationFailure(out int strRefFailure, new OnELCItemValidationFailure
            {
              Player = nwPlayer,
              Item = pItem.ToNwObject<NwItem>()!,
              Type = ValidationFailureType.Item,
              SubType = ValidationFailureSubType.UnidentifiedEquippedItem,
              StrRef = StrRefItemLevelRestriction,
            }))
            {
              *bFailedServerRestriction = true.ToInt();
              return strRefFailure;
            }
          }

          // Check the minimum equip level
          if (pItem.GetMinEquipLevel() > nCharacterLevel)
          {
            if (HandleValidationFailure(out int strRefFailure, new OnELCItemValidationFailure
            {
              Player = nwPlayer,
              Item = pItem.ToNwObject<NwItem>()!,
              Type = ValidationFailureType.Item,
              SubType = ValidationFailureSubType.MinEquipLevel,
              StrRef = StrRefItemLevelRestriction,
            }))
            {
              *bFailedServerRestriction = true.ToInt();
              return strRefFailure;
            }
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
        pCreatureStats.m_cDialog = new CResRef();
      }

      // Check for non PC
      if (!pCreatureStats.m_bIsPC.ToBool())
      {
        if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
        {
          Player = nwPlayer,
          Type = ValidationFailureType.Character,
          SubType = ValidationFailureSubType.NonPCCharacter,
          StrRef = StrRefCharacterNonPlayer,
        }))
        {
          return strRefFailure;
        }
      }

      // Check for DM character file
      if (pCreatureStats.m_bIsDMCharacterFile.ToBool())
      {
        if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
        {
          Player = nwPlayer,
          Type = ValidationFailureType.Character,
          SubType = ValidationFailureSubType.DMCharacter,
          StrRef = StrRefCharacterDungeonMaster,
        }))
        {
          return strRefFailure;
        }
      }

      // Check for non player race
      CNWRace? pRace = pCreatureStats.m_nRace < pRules.m_nNumRaces ? races[pCreatureStats.m_nRace] : null;

      if (pRace == null || !pRace.m_bIsPlayerRace.ToBool())
      {
        if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
        {
          Player = nwPlayer,
          Type = ValidationFailureType.Character,
          SubType = ValidationFailureSubType.NonPlayerRace,
          StrRef = StrRefCharacterNonPlayerRace,
        }))
        {
          return strRefFailure;
        }
      }

      // Check for non player classes, class level restrictions and prestige class requirements
      // We also check class alignment restrictions for new characters only
      for (byte nMultiClass = 0; nMultiClass < pCreatureStats.m_nNumMultiClasses; nMultiClass++)
      {
        byte classId = pCreatureStats.GetClassInfo(nMultiClass).m_nClass;
        CNWClass? pClass = classId < pRules.m_nNumClasses ? classes[classId] : null;

        if (pClass == null)
        {
          if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
          {
            Player = nwPlayer,
            Type = ValidationFailureType.Character,
            SubType = ValidationFailureSubType.InvalidClass,
            StrRef = StrRefCharacterNonPlayerClass,
          }))
          {
            return strRefFailure;
          }

          // Skip further class checks if the validation was skipped.
          continue;
        }

        if (!pClass.m_bIsPlayerClass.ToBool())
        {
          if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
          {
            Player = nwPlayer,
            Type = ValidationFailureType.Character,
            SubType = ValidationFailureSubType.NonPlayerClass,
            StrRef = StrRefCharacterNonPlayerClass,
          }))
          {
            return strRefFailure;
          }
        }

        if (pClass.m_nMaxLevel > 0 && pCreatureStats.GetClassLevel(nMultiClass, false.ToInt()) > pClass.m_nMaxLevel)
        {
          if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
          {
            Player = nwPlayer,
            Type = ValidationFailureType.Character,
            SubType = ValidationFailureSubType.ClassLevelRestriction,
            StrRef = StrRefCharacterNonPlayerClass,
          }))
          {
            return strRefFailure;
          }
        }

        if (!pCreatureStats.GetMeetsPrestigeClassRequirements(pClass).ToBool())
        {
          if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
          {
            Player = nwPlayer,
            Type = ValidationFailureType.Character,
            SubType = ValidationFailureSubType.PrestigeClassRequirements,
            StrRef = StrRefCharacterNonPlayerClass,
          }))
          {
            return strRefFailure;
          }
        }

        if (nMultiClass == 0 && nCharacterLevel == 1 && pCreatureStats.m_nExperience == 0)
        {
          if (!pClass.GetIsAlignmentAllowed(pCreatureStats.GetSimpleAlignmentGoodEvil(), pCreatureStats.GetSimpleAlignmentLawChaos()).ToBool())
          {
            if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
            {
              Player = nwPlayer,
              Type = ValidationFailureType.Character,
              SubType = ValidationFailureSubType.ClassAlignmentRestriction,
              StrRef = StrRefCharacterNonPlayerClass,
            }))
            {
              return strRefFailure;
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
        if (nAbilityGain <= AbilityMax)
        {
          nAbility[nAbilityGain]--;
        }
      }

      // Check if >18 in an ability
      for (int nAbilityIndex = 0; nAbilityIndex <= AbilityMax; nAbilityIndex++)
      {
        if (nAbility[nAbilityIndex] > charGenBaseAbilityMax)
        {
          if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
          {
            Player = nwPlayer,
            Type = ValidationFailureType.Character,
            SubType = ValidationFailureSubType.StartingAbilityValueMax,
            StrRef = StrRefCharacterInvalidAbilityScores,
          }))
          {
            return strRefFailure;
          }
        }
      }

      // Point Buy System calculation
      int[] nAbilityAtLevel = new int[6];
      int nPointBuy = pRace?.m_nAbilitiesPointBuyNumber ?? 0;

      for (int nAbilityIndex = 0; nAbilityIndex <= AbilityMax; nAbilityIndex++)
      {
        nAbilityAtLevel[nAbilityIndex] = nAbility[nAbilityIndex];

        while (nAbility[nAbilityIndex] > charGenBaseAbilityMin)
        {
          if (nAbility[nAbilityIndex] > abilityCostIncrement3)
          {
            if (nPointBuy < 3)
            {
              if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
              {
                Player = nwPlayer,
                Type = ValidationFailureType.Character,
                SubType = ValidationFailureSubType.AbilityPointBuySystemCalculation,
                StrRef = StrRefCharacterInvalidAbilityScores,
              }))
              {
                return strRefFailure;
              }
            }

            nAbility[nAbilityIndex]--;
            nPointBuy -= 3;
          }
          else if (nAbility[nAbilityIndex] > abilityCostIncrement2)
          {
            if (nPointBuy < 2)
            {
              if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
              {
                Player = nwPlayer,
                Type = ValidationFailureType.Character,
                SubType = ValidationFailureSubType.AbilityPointBuySystemCalculation,
                StrRef = StrRefCharacterInvalidAbilityScores,
              }))
              {
                return strRefFailure;
              }
            }

            nAbility[nAbilityIndex]--;
            nPointBuy -= 2;
          }
          else
          {
            if (nPointBuy < 1)
            {
              if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
              {
                Player = nwPlayer,
                Type = ValidationFailureType.Character,
                SubType = ValidationFailureSubType.AbilityPointBuySystemCalculation,
                StrRef = StrRefCharacterInvalidAbilityScores,
              }))
              {
                return strRefFailure;
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
      byte[] nMultiClassLevel = new byte[NumMultiClass];
      int nSkillPointsRemaining = 0;
      byte[] listSkillRanks = new byte[pRules.m_nNumSkills];
      HashSet<ushort> listFeats = new HashSet<ushort>();
      HashSet<ushort> listChosenFeats = new HashSet<ushort>();
      // [nMultiClass][nSpellLevel] . {SpellIDs}
      List<Dictionary<uint, HashSet<uint>>> listSpells = new List<Dictionary<uint, HashSet<uint>>>();
      for (int i = 0; i < NumMultiClass; i++)
      {
        listSpells.Add(new Dictionary<uint, HashSet<uint>>());
        for (uint j = 0; j < NumSpellLevels; j++)
        {
          listSpells[i][j] = new HashSet<uint>();
        }
      }

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
            if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
            {
              Player = nwPlayer,
              Type = ValidationFailureType.Character,
              SubType = ValidationFailureSubType.ClassSpellcasterInvalidPrimaryStat,
              StrRef = StrRefCharacterInvalidAbilityScores,
            }))
            {
              return strRefFailure;
            }
          }
        }

        // Check Epic Level Flag
        if (nLevel < CharacterEpicLevel)
        {
          if (pLevelStats.m_bEpic != 0)
          {
            if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
            {
              Player = nwPlayer,
              Type = ValidationFailureType.Feat,
              SubType = ValidationFailureSubType.EpicLevelFlag,
              StrRef = StrRefFeatInvalid,
            }))
            {
              return strRefFailure;
            }
          }
        }
        else
        {
          if (pLevelStats.m_bEpic == 0)
          {
            if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
            {
              Player = nwPlayer,
              Type = ValidationFailureType.Feat,
              SubType = ValidationFailureSubType.EpicLevelFlag,
              StrRef = StrRefFeatInvalid,
            }))
            {
              return strRefFailure;
            }
          }
        }

        // Keep track of our ability values
        if (nLevel % 4 == 0)
        {
          if (pLevelStats.m_nAbilityGain < nAbilityAtLevel.Length && pLevelStats.m_nAbilityGain >= 0)
          {
            nAbilityAtLevel[pLevelStats.m_nAbilityGain]++;
          }
          else
          {
            if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
            {
              Player = nwPlayer,
              Type = ValidationFailureType.Character,
              SubType = ValidationFailureSubType.AbilityPointBuySystemCalculation,
              StrRef = StrRefCharacterInvalidAbilityScores,
            }))
            {
              return strRefFailure;
            }
          }
        }

        // Get the stat bonus from feats
        int[] nStatMods = GetStatBonusesFromFeats(pLevelStats.m_lstFeats, false);

        // Update our ability values
        for (int nAbilityIndex = 0; nAbilityIndex <= AbilityMax; nAbilityIndex++)
        {
          nAbilityAtLevel[nAbilityIndex] += nStatMods[nAbilityIndex];
        }

        for (int nAbilityIndex = 0; nAbilityIndex <= AbilityMax; nAbilityIndex++)
        {
          nAbilityAtLevel[nAbilityIndex] += pClassLeveledUpIn.GetAbilityGainForSingleLevel(nAbilityIndex, nMultiClassLevel[nMultiClassLeveledUpIn]);
        }

        // *** Check Hit Die ********************************************************************************************************
        if (pLevelStats.m_nHitDie > pCreatureStats.GetHitDie(nMultiClassLeveledUpIn, nClassLeveledUpIn))
        {
          if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
          {
            Player = nwPlayer,
            Type = ValidationFailureType.Character,
            SubType = ValidationFailureSubType.TooManyHitPoints,
            StrRef = StrRefCharacterTooManyHitpoints,
          }))
          {
            return strRefFailure;
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

        int numSkillPoints = pRace != null && pRace.m_nSkillPointModifierAbility >= 0 && pRace.m_nSkillPointModifierAbility <= AbilityMax
          ? pCreatureStats.CalcStatModifier((byte)(nAbilityAtLevel[pRace.m_nSkillPointModifierAbility] + GetSkillPointAbilityAdjust()))
          : 0;

        if (pRace == null)
        {
          nSkillPointsRemaining = 0;
        }
        else if (nLevel == 1)
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

          byte nRankChange = pLevelStats.m_lstSkillRanks[nSkill];

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
              if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
              {
                Player = nwPlayer,
                Type = ValidationFailureType.Skill,
                SubType = ValidationFailureSubType.UnusableSkill,
                StrRef = StrRefSkillUnuseable,
              }))
              {
                return strRefFailure;
              }
            }

            // Check if we have enough available points
            if (bClassSkill)
            {
              if (nRankChange > nSkillPointsRemaining)
              {
                if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
                {
                  Player = nwPlayer,
                  Type = ValidationFailureType.Skill,
                  SubType = ValidationFailureSubType.NotEnoughSkillPoints,
                  StrRef = StrRefSkillInvalidNumSkillpoints,
                }))
                {
                  return strRefFailure;
                }
              }

              nSkillPointsRemaining -= nRankChange;
            }
            else
            {
              if (nRankChange * 2 > nSkillPointsRemaining)
              {
                if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
                {
                  Player = nwPlayer,
                  Type = ValidationFailureType.Skill,
                  SubType = ValidationFailureSubType.NotEnoughSkillPoints,
                  StrRef = StrRefSkillInvalidNumSkillpoints,
                }))
                {
                  return strRefFailure;
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
                if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
                {
                  Player = nwPlayer,
                  Type = ValidationFailureType.Skill,
                  SubType = ValidationFailureSubType.InvalidNumRanksInClassSkill,
                  StrRef = StrRefSkillInvalidRanks,
                }))
                {
                  return strRefFailure;
                }
              }
            }
            else
            {
              if (listSkillRanks[nSkill] > (nLevel + skillMaxLevel1Bonus) / 2)
              {
                if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
                {
                  Player = nwPlayer,
                  Type = ValidationFailureType.Skill,
                  SubType = ValidationFailureSubType.InvalidNumRanksInNonClassSkill,
                  StrRef = StrRefSkillInvalidRanks,
                }))
                {
                  return strRefFailure;
                }
              }
            }
          }
        }

        // Compare the remaining skillpoints in LevelStats with our own calculation
        if (pLevelStats.m_nSkillPointsRemaining > nSkillPointsRemaining)
        {
          if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
          {
            Player = nwPlayer,
            Type = ValidationFailureType.Skill,
            SubType = ValidationFailureSubType.InvalidNumRemainingSkillPoints,
            StrRef = StrRefSkillInvalidNumSkillpoints,
          }))
          {
            return strRefFailure;
          }
        }

        // **************************************************************************************************************************

        // *** Check Feats **********************************************************************************************************
        // Calculate the number of normal and bonus feats for this level
        int nNumberNormalFeats = 0;

        // First and every nth level gets a normal feat
        if (pRace != null)
        {
          if (nLevel == 1 || pRace.m_nNormalFeatEveryNthLevel != 0 && nLevel % pRace.m_nNormalFeatEveryNthLevel == 0)
          {
            nNumberNormalFeats = pRace.m_nNumberNormalFeatsEveryNthLevel;
          }

          // Add any extra first level feats
          if (nLevel == 1)
          {
            nNumberNormalFeats += pRace.m_nExtraFeatsAtFirstLevel;
          }
        }

        int nNumberBonusFeats = pClassLeveledUpIn.GetBonusFeats(nMultiClassLevel[nMultiClassLeveledUpIn]);

        // Add this level's gained feats to our own list
        foreach (ushort nFeat in pLevelStats.m_lstFeats)
        {
          CNWFeat? feat = nFeat < pRules.m_nNumFeats ? feats[nFeat] : null;

          if (feat == null)
          {
            if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
            {
              Player = nwPlayer,
              Type = ValidationFailureType.Feat,
              SubType = ValidationFailureSubType.InvalidFeat,
              StrRef = StrRefFeatInvalid,
            }))
            {
              return strRefFailure;
            }
          }

          bool bGranted = false;

          // Check if this is a feat that's automatically granted at first level
          if (nLevel == 1)
          {
            if (pRace?.IsFirstLevelGrantedFeat(nFeat).ToBool() == true)
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
            if (pClassLeveledUpIn.m_bHasDomains.ToBool() && nMultiClassLevel[nMultiClassLeveledUpIn] == 1)
            {
              if (nFeat == nDomainFeat1 || nFeat == nDomainFeat2)
              {
                listFeats.Add(nFeat);
                bGranted = true;
              }
            }
          }

          // Check if it's the "EpicCharacter" feat and we're level 21
          if (!bGranted)
          {
            if (nLevel == CharacterEpicLevel && nFeat == (int)Feat.EpicCharacter)
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

        // Check the requirements of the chosen feats
        foreach (ushort nFeat in listChosenFeats)
        {
          CNWFeat? pFeat = nFeat < pRules.m_nNumFeats ? feats[nFeat] : null;
          if (pFeat == null)
          {
            if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
            {
              Player = nwPlayer,
              Type = ValidationFailureType.Feat,
              SubType = ValidationFailureSubType.InvalidFeat,
              StrRef = StrRefFeatInvalid,
            }))
            {
              return strRefFailure;
            }

            continue;
          }

          // Spell Level Requirements
          if (pFeat.m_nMinSpellLevel != 0)
          {
            bool bSpellLevelMet = false;

            for (byte nMultiClass = 0; !bSpellLevelMet && nMultiClass < pCreatureStats.m_nNumMultiClasses; nMultiClass++)
            {
              if (nMultiClassLevel[nMultiClass] != 0)
              {
                byte nClass = pCreatureStats.GetClass(nMultiClass);
                CNWClass pClass = classes[nClass];

                if (pClass.m_bIsSpellCasterClass.ToBool())
                {
                  if (!pClass.m_bNeedsToMemorizeSpells.ToBool())
                  {
                    if (pClass.GetSpellsKnownPerLevel(nMultiClassLevel[nMultiClass],
                      pFeat.m_nMinSpellLevel,
                      nClass, pCreatureStats.m_nRace,
                      (byte)nAbilityAtLevel[pClass.m_nSpellcastingAbility]) != 0)
                    {
                      bSpellLevelMet = true;
                    }
                  }
                  else
                  {
                    if (pCreatureStats.GetSpellGainWithBonus(nMultiClass, pFeat.m_nMinSpellLevel) != 0)
                    {
                      bSpellLevelMet = true;
                    }
                  }
                }
              }
            }

            if (!bSpellLevelMet)
            {
              if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
              {
                Player = nwPlayer,
                Type = ValidationFailureType.Feat,
                SubType = ValidationFailureSubType.FeatRequiredSpellLevelNotMet,
                StrRef = StrRefFeatReqSpellLevel,
              }))
              {
                return strRefFailure;
              }
            }
          }

          byte nBaseAttackBonus = 0;

          for (byte nMultiClass = 0; nMultiClass < pCreatureStats.m_nNumMultiClasses; nMultiClass++)
          {
            if (nMultiClassLevel[nMultiClass] != 0)
            {
              CNWClass pClass = classes[pCreatureStats.GetClass(nMultiClass)];
              nBaseAttackBonus += pClass.GetAttackBonus(nMultiClassLevel[nMultiClass]);
            }
          }

          if (pFeat.m_nMinAttackBonus > nBaseAttackBonus)
          {
            if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
            {
              Player = nwPlayer,
              Type = ValidationFailureType.Feat,
              SubType = ValidationFailureSubType.FeatRequiredBaseAttackBonusNotMet,
              StrRef = StrRefFeatReqAbility,
            }))
            {
              return strRefFailure;
            }
          }

          if (pFeat.m_nMinSTR > nAbilityAtLevel[(int)Ability.Strength] + (pRace?.m_nSTRAdjust ?? 0))
          {
            if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
            {
              Player = nwPlayer,
              Type = ValidationFailureType.Feat,
              SubType = ValidationFailureSubType.FeatRequiredBaseAttackBonusNotMet,
              StrRef = StrRefFeatReqAbility,
            }))
            {
              return strRefFailure;
            }
          }

          if (pFeat.m_nMinDEX > nAbilityAtLevel[(int)Ability.Dexterity] + (pRace?.m_nDEXAdjust ?? 0))
          {
            if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
            {
              Player = nwPlayer,
              Type = ValidationFailureType.Feat,
              SubType = ValidationFailureSubType.FeatRequiredBaseAttackBonusNotMet,
              StrRef = StrRefFeatReqAbility,
            }))
            {
              return strRefFailure;
            }
          }

          if (pFeat.m_nMinINT > nAbilityAtLevel[(int)Ability.Intelligence] + (pRace?.m_nINTAdjust ?? 0))
          {
            if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
            {
              Player = nwPlayer,
              Type = ValidationFailureType.Feat,
              SubType = ValidationFailureSubType.FeatRequiredBaseAttackBonusNotMet,
              StrRef = StrRefFeatReqAbility,
            }))
            {
              return strRefFailure;
            }
          }

          if (pFeat.m_nMinWIS > nAbilityAtLevel[(int)Ability.Wisdom] + (pRace?.m_nWISAdjust ?? 0))
          {
            if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
            {
              Player = nwPlayer,
              Type = ValidationFailureType.Feat,
              SubType = ValidationFailureSubType.FeatRequiredBaseAttackBonusNotMet,
              StrRef = StrRefFeatReqAbility,
            }))
            {
              return strRefFailure;
            }
          }

          if (pFeat.m_nMinCON > nAbilityAtLevel[(int)Ability.Constitution] + (pRace?.m_nCONAdjust ?? 0))
          {
            if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
            {
              Player = nwPlayer,
              Type = ValidationFailureType.Feat,
              SubType = ValidationFailureSubType.FeatRequiredBaseAttackBonusNotMet,
              StrRef = StrRefFeatReqAbility,
            }))
            {
              return strRefFailure;
            }
          }

          if (pFeat.m_nMinCHA > nAbilityAtLevel[(int)Ability.Charisma] + (pRace?.m_nCHAAdjust ?? 0))
          {
            if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
            {
              Player = nwPlayer,
              Type = ValidationFailureType.Feat,
              SubType = ValidationFailureSubType.FeatRequiredBaseAttackBonusNotMet,
              StrRef = StrRefFeatReqAbility,
            }))
            {
              return strRefFailure;
            }
          }

          // Skill Focus Feats
          StrRef? SkillFocusFeatCheck(ushort nReqSkill)
          {
            if (nReqSkill != IntegerExtensions.AsUShort(-1))
            {
              bool bSkillRequirementMet = false;
              CNWSkill pReqSkill = skills[nReqSkill];

              if (pReqSkill.m_bUntrained.ToBool())
              {
                // Make sure we have a class that can use the skill
                for (byte nMultiClass = 0; nMultiClass < pCreatureStats.m_nNumMultiClasses; nMultiClass++)
                {
                  if (classes[pCreatureStats.GetClass(nMultiClass)].IsSkillUseable(nReqSkill).ToBool())
                  {
                    bSkillRequirementMet = true;
                  }
                }

                if (!bSkillRequirementMet)
                {
                  return StrRefFeatReqSkill;
                }
              }

              if (!bSkillRequirementMet)
              {
                if (listSkillRanks[nReqSkill] == 0)
                {
                  return StrRefFeatReqSkill;
                }
              }

              ushort nSkillRanks = pFeat.m_nMinRequiredSkillRank2;

              if (listSkillRanks[nReqSkill] < nSkillRanks)
              {
                return StrRefSkillUnuseable;
              }
            }

            return null;
          }

          StrRef? retVal = SkillFocusFeatCheck(pFeat.m_nRequiredSkill);
          if (retVal != null)
          {
            if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
            {
              Player = nwPlayer,
              Type = ValidationFailureType.Feat,
              SubType = ValidationFailureSubType.FeatRequiredSkillNotMet,
              StrRef = retVal.Value,
            }))
            {
              return strRefFailure;
            }
          }

          retVal = SkillFocusFeatCheck(pFeat.m_nRequiredSkill2);
          if (retVal != null)
          {
            if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
            {
              Player = nwPlayer,
              Type = ValidationFailureType.Feat,
              SubType = ValidationFailureSubType.FeatRequiredSkillNotMet,
              StrRef = retVal.Value,
            }))
            {
              return strRefFailure;
            }
          }

          // Check Feat Prereqs
          StrRef? PrerequisitesFeatCheck(ushort nPrereqFeat)
          {
            if (nPrereqFeat != IntegerExtensions.AsUShort(-1))
            {
              if (!listFeats.Contains(nPrereqFeat) && !listChosenFeats.Contains(nPrereqFeat))
              {
                return StrRefFeatReqFeat;
              }
            }

            return null;
          }

          retVal = PrerequisitesFeatCheck(pFeat.m_lstPrereqFeats[0]);
          if (retVal != null)
          {
            if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
            {
              Player = nwPlayer,
              Type = ValidationFailureType.Feat,
              SubType = ValidationFailureSubType.FeatRequiredFeatNotMet,
              StrRef = retVal.Value,
            }))
            {
              return strRefFailure;
            }
          }

          retVal = PrerequisitesFeatCheck(pFeat.m_lstPrereqFeats[1]);
          if (retVal != null)
          {
            if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
            {
              Player = nwPlayer,
              Type = ValidationFailureType.Feat,
              SubType = ValidationFailureSubType.FeatRequiredFeatNotMet,
              StrRef = retVal.Value,
            }))
            {
              return strRefFailure;
            }
          }

          // The feat requires a "OrPrereq" feat
          bool bHasOrPrereqFeat = false;
          // The character has one of these feats
          bool bOrPrereqFeatAcquired = false;

          for (int nOrPrereqFeat = 0; !bOrPrereqFeatAcquired && nOrPrereqFeat < 5; nOrPrereqFeat++)
          {
            ushort nPrereqFeat = pFeat.m_lstOrPrereqFeats[nOrPrereqFeat];

            if (nPrereqFeat != IntegerExtensions.AsUShort(-1))
            {
              bHasOrPrereqFeat = true;
              bOrPrereqFeatAcquired = listFeats.Contains(nPrereqFeat) || listChosenFeats.Contains(nPrereqFeat);
            }
          }

          if (bHasOrPrereqFeat && !bOrPrereqFeatAcquired)
          {
            if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
            {
              Player = nwPlayer,
              Type = ValidationFailureType.Feat,
              SubType = ValidationFailureSubType.FeatRequiredFeatNotMet,
              StrRef = StrRefFeatReqFeat,
            }))
            {
              return strRefFailure;
            }
          }
        }

        // Check if we can actually pick our chosen feats this level
        if (listChosenFeats.Any() && nNumberNormalFeats == 0 && nNumberBonusFeats == 0)
        {
          if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
          {
            Player = nwPlayer,
            Type = ValidationFailureType.Feat,
            SubType = ValidationFailureSubType.TooManyFeatsThisLevel,
            StrRef = StrRefFeatTooMany,
          }))
          {
            return strRefFailure;
          }
        }

        // List to hold moved chosen feats
        List<ushort> listMovedFeats = new List<ushort>();

        foreach (ushort nFeatIndex in listChosenFeats)
        {
          int bNormalListFeat;
          int bBonusListFeat;

          pPlayer.ValidateCharacter_SetNormalBonusFlags(nFeatIndex, &bNormalListFeat, &bBonusListFeat, nClassLeveledUpIn);

          // Not available to class
          if (!bNormalListFeat.ToBool() && !bBonusListFeat.ToBool())
          {
            if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
            {
              Player = nwPlayer,
              Type = ValidationFailureType.Feat,
              SubType = ValidationFailureSubType.FeatNotAvailableToClass,
              StrRef = StrRefFeatTooMany,
            }))
            {
              return strRefFailure;
            }
          }

          // Normal Feat Only
          if (bNormalListFeat.ToBool() && !bBonusListFeat.ToBool())
          {
            if (!nNumberNormalFeats.ToBool())
            {
              if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
              {
                Player = nwPlayer,
                Type = ValidationFailureType.Feat,
                SubType = ValidationFailureSubType.FeatIsNormalFeatOnly,
                StrRef = StrRefFeatTooMany,
              }))
              {
                return strRefFailure;
              }
            }

            // Move the feat from our level list to the main list
            listFeats.Add(nFeatIndex);
            // Add the feat that's being moved to a different list because removing stuff while iterating is bad
            listMovedFeats.Add(nFeatIndex);
            nNumberNormalFeats--;
          }

          // Bonus Feat Only
          if (!bNormalListFeat.ToBool() && bBonusListFeat.ToBool())
          {
            if (!nNumberBonusFeats.ToBool())
            {
              if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
              {
                Player = nwPlayer,
                Type = ValidationFailureType.Feat,
                SubType = ValidationFailureSubType.FeatIsBonusFeatOnly,
                StrRef = StrRefFeatTooMany,
              }))
              {
                return strRefFailure;
              }
            }

            // Move the feat from our level list to the main list
            listFeats.Add(nFeatIndex);
            // Add the feat that's being moved to a different list because removing stuff while iterating is bad
            listMovedFeats.Add(nFeatIndex);
            nNumberBonusFeats--;
          }
        }

        // Remove the moved feats from the chosen feat list
        foreach (ushort remove in listMovedFeats)
        {
          listChosenFeats.Remove(remove);
        }

        listMovedFeats.Clear();

        // The feats that are left can be normal or bonus
        foreach (ushort nFeatIndex in listChosenFeats)
        {
          if (nNumberBonusFeats.ToBool())
          {
            // Move the feat from our level list to the main list
            listFeats.Add(nFeatIndex);
            // Add the feat that's being moved to a different list because removing stuff while iterating is bad
            listMovedFeats.Add(nFeatIndex);
            nNumberBonusFeats--;
          }
          else
          {
            if (nNumberNormalFeats.ToBool())
            {
              // Move the feat from our level list to the main list
              listFeats.Add(nFeatIndex);
              // Add the feat that's being moved to a different list because removing stuff while iterating is bad
              listMovedFeats.Add(nFeatIndex);
              nNumberNormalFeats--;
            }
            else
            {
              if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
              {
                Player = nwPlayer,
                Type = ValidationFailureType.Feat,
                SubType = ValidationFailureSubType.TooManyFeatsThisLevel,
                StrRef = StrRefFeatTooMany,
              }))
              {
                return strRefFailure;
              }
            }
          }
        }

        // Remove the moved feats from the chosen feat list
        foreach (ushort remove in listMovedFeats)
        {
          listChosenFeats.Remove(remove);
        }

        listMovedFeats.Clear();
        // **************************************************************************************************************************

        // *** Check Known Spells ***************************************************************************************************
        int nNumberWizardSpellsToAdd = 0;

        // Calculate the num of spells a wizard can add
        if (pClassLeveledUpIn.m_bCanLearnFromScrolls.ToBool())
        {
          if (nMultiClassLevel[nMultiClassLeveledUpIn] == 1)
          {
            nNumberWizardSpellsToAdd = 3 + Math.Max((byte)0, pCreatureStats.CalcStatModifier((byte)(nAbilityAtLevel[(int)Ability.Intelligence] + (pRace?.m_nINTAdjust ?? 0))));
          }
          else
          {
            nNumberWizardSpellsToAdd = 2;
          }
        }

        for (byte nSpellLevel = 0; nSpellLevel < NumSpellLevels; nSpellLevel++)
        {
          for (int nSpellIndex = 0; nSpellIndex < pLevelStats.m_pAddedKnownSpellList[nSpellLevel].Count; nSpellIndex++)
          {
            // Can we add spells this level?
            if (pClassLeveledUpIn.m_bSpellbookRestricted.ToBool() && pClassLeveledUpIn.m_bNeedsToMemorizeSpells.ToBool())
            {
              if (pClassLeveledUpIn.GetSpellGain(nMultiClassLevel[nMultiClassLeveledUpIn], nSpellLevel) == 0)
              {
                if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
                {
                  Player = nwPlayer,
                  Type = ValidationFailureType.Spell,
                  SubType = ValidationFailureSubType.SpellInvalidSpellGainWizard,
                  StrRef = StrRefSpellIllegalLevel,
                }))
                {
                  return strRefFailure;
                }
              }
            }
            else if (pClassLeveledUpIn.m_bSpellbookRestricted.ToBool() && !pClassLeveledUpIn.m_bNeedsToMemorizeSpells.ToBool())
            {
              if (pClassLeveledUpIn.GetSpellsKnownPerLevel(nMultiClassLevel[nMultiClassLeveledUpIn],
                nSpellLevel,
                nClassLeveledUpIn, pCreatureStats.m_nRace,
                (byte)nAbilityAtLevel[pClassLeveledUpIn.m_nSpellcastingAbility]) == 0)
              {
                if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
                {
                  Player = nwPlayer,
                  Type = ValidationFailureType.Spell,
                  SubType = ValidationFailureSubType.SpellInvalidSpellGainBardSorcerer,
                  StrRef = StrRefSpellIllegalLevel,
                }))
                {
                  return strRefFailure;
                }
              }
            }
            else
            {
              if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
              {
                Player = nwPlayer,
                Type = ValidationFailureType.Spell,
                SubType = ValidationFailureSubType.SpellInvalidSpellGainOtherClasses,
                StrRef = StrRefSpellIllegalLevel,
              }))
              {
                return strRefFailure;
              }
            }

            uint nSpellID = pLevelStats.m_pAddedKnownSpellList[nSpellLevel][nSpellIndex];
            CNWSpell pSpell = pRules.m_pSpellArray.GetSpell((int)nSpellID);

            if (pSpell == null)
            {
              if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
              {
                Player = nwPlayer,
                Type = ValidationFailureType.Spell,
                SubType = ValidationFailureSubType.InvalidSpell,
                StrRef = StrRefSpellInvalidSpell,
              }))
              {
                return strRefFailure;
              }

              continue;
            }

            // Check the spell level
            if (pSpell.GetSpellLevel(nClassLeveledUpIn) != nSpellLevel)
            {
              if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
              {
                Player = nwPlayer,
                Type = ValidationFailureType.Spell,
                SubType = ValidationFailureSubType.SpellInvalidSpellLevel,
                StrRef = StrRefSpellReqSpellLevel,
              }))
              {
                return strRefFailure;
              }
            }

            // Check for minimum ability
            if (pClassLeveledUpIn.m_bSpellbookRestricted.ToBool())
            {
              if (nAbilityAtLevel[pClassLeveledUpIn.m_nSpellcastingAbility] < 10 + nSpellLevel)
              {
                if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
                {
                  Player = nwPlayer,
                  Type = ValidationFailureType.Spell,
                  SubType = ValidationFailureSubType.SpellMinimumAbility,
                  StrRef = StrRefSpellReqAbility,
                }))
                {
                  return strRefFailure;
                }
              }
            }

            // Check Opposition School
            if (pClassLeveledUpIn.m_bSpellbookRestricted.ToBool() && pClassLeveledUpIn.m_bNeedsToMemorizeSpells.ToBool())
            {
              byte nSchool = pCreatureStats.GetSchool(nClassLeveledUpIn);

              if (nSchool != 0)
              {
                int nOppositionSchool;
                if (pRules.m_p2DArrays.GetSpellSchoolTable().GetINTEntry(nSchool, "Opposition".ToExoString(), &nOppositionSchool).ToBool())
                {
                  if (pSpell.m_nSchool == nOppositionSchool)
                  {
                    if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
                    {
                      Player = nwPlayer,
                      Type = ValidationFailureType.Spell,
                      SubType = ValidationFailureSubType.SpellRestrictedSpellSchool,
                      StrRef = StrRefSpellOppositeSpellSchool,
                    }))
                    {
                      return strRefFailure;
                    }
                  }
                }
              }
            }

            // Check if we already know the spell
            if (listSpells[nMultiClassLeveledUpIn][nSpellLevel].Contains(nSpellID))
            {
              if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
              {
                Player = nwPlayer,
                Type = ValidationFailureType.Spell,
                SubType = ValidationFailureSubType.SpellAlreadyKnown,
                StrRef = StrRefSpellLearnedTwice,
              }))
              {
                return strRefFailure;
              }
            }

            // Check if we're a wizard and haven't exceeded the number of spells we can add
            if (pClassLeveledUpIn.m_bSpellbookRestricted.ToBool() && pClassLeveledUpIn.m_bNeedsToMemorizeSpells.ToBool())
            {
              if (nSpellLevel != 0)
              {
                if (nNumberWizardSpellsToAdd == 0)
                {
                  if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
                  {
                    Player = nwPlayer,
                    Type = ValidationFailureType.Spell,
                    SubType = ValidationFailureSubType.SpellWizardExceedsNumSpellsToAdd,
                    StrRef = StrRefSpellIllegalNumSpells,
                  }))
                  {
                    return strRefFailure;
                  }
                }

                nNumberWizardSpellsToAdd--;
              }
            }

            // Add the spell to our list
            listSpells[nMultiClassLeveledUpIn][nSpellLevel].Add(nSpellID);
          }

          // Check Bard/Sorc removed spells
          for (int nSpellIndex = 0; nSpellIndex < pLevelStats.m_pRemovedKnownSpellList[nSpellLevel].Count; nSpellIndex++)
          {
            if (!pClassLeveledUpIn.m_bSpellbookRestricted.ToBool() || pClassLeveledUpIn.m_bNeedsToMemorizeSpells.ToBool() ||
              nMultiClassLevel[nMultiClassLeveledUpIn] == 1 ||
              pClassLeveledUpIn.GetSpellsKnownPerLevel(nMultiClassLevel[nMultiClassLeveledUpIn], nSpellLevel, nClassLeveledUpIn, pCreatureStats.m_nRace,
                (byte)nAbilityAtLevel[pClassLeveledUpIn.m_nSpellcastingAbility]) == 0)
            {
              if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
              {
                Player = nwPlayer,
                Type = ValidationFailureType.Spell,
                SubType = ValidationFailureSubType.IllegalRemovedSpell,
                StrRef = StrRefSpellIllegalRemovedSpells,
              }))
              {
                return strRefFailure;
              }
            }

            uint nSpellID = pLevelStats.m_pRemovedKnownSpellList[nSpellLevel][nSpellIndex];

            CNWSpell pSpell = pRules.m_pSpellArray.GetSpell((int)nSpellID);

            if (pSpell == null)
            {
              if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
              {
                Player = nwPlayer,
                Type = ValidationFailureType.Spell,
                SubType = ValidationFailureSubType.InvalidSpell,
                StrRef = StrRefSpellInvalidSpell,
              }))
              {
                return strRefFailure;
              }

              continue;
            }

            // Check if we actually know the spell
            if (!listSpells[nMultiClassLeveledUpIn][nSpellLevel].Contains(nSpellID))
            {
              if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
              {
                Player = nwPlayer,
                Type = ValidationFailureType.Spell,
                SubType = ValidationFailureSubType.RemovedNotKnownSpell,
                StrRef = StrRefSpellIllegalRemovedSpells,
              }))
              {
                return strRefFailure;
              }
            }

            // Remove the spell from our list
            listSpells[nMultiClassLeveledUpIn][nSpellLevel].Remove(nSpellID);
          }
        }

        // Check if we have the valid number of spells
        if (pClassLeveledUpIn.m_bSpellbookRestricted.ToBool() && !pClassLeveledUpIn.m_bCanLearnFromScrolls.ToBool())
        {
          for (byte nSpellLevel = 0; nSpellLevel < NumSpellLevels; nSpellLevel++)
          {
            if (listSpells[nMultiClassLeveledUpIn][nSpellLevel].Count > pClassLeveledUpIn.GetSpellsKnownPerLevel(nMultiClassLevel[nMultiClassLeveledUpIn], nSpellLevel,
              nClassLeveledUpIn, pCreatureStats.m_nRace, (byte)nAbilityAtLevel[pClassLeveledUpIn.m_nSpellcastingAbility]))
            {
              if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
              {
                Player = nwPlayer,
                Type = ValidationFailureType.Spell,
                SubType = ValidationFailureSubType.InvalidNumSpells,
                StrRef = StrRefSpellIllegalNumSpells,
              }))
              {
                return strRefFailure;
              }
            }
          }
        }
        // **************************************************************************************************************************
      }
      // All levels processed, hurray!

      // Final Spells Check
      // Check if our list of spells from LevelStats are the same as the spells the character knows
      for (byte nMultiClass = 0; nMultiClass < pCreatureStats.m_nNumMultiClasses; nMultiClass++)
      {
        CNWClass pClass = classes[pCreatureStats.GetClass(nMultiClass)];
        // We skip wizard because they can learn spells from scrolls
        if (!pClass.m_bCanLearnFromScrolls.ToBool())
        {
          for (byte nSpellLevel = 0; nSpellLevel < NumSpellLevels; nSpellLevel++)
          {
            //  NOTE: Not sure if this is still needed, removing it for now.
            /*
            if (nSpellLevel != 0 || !(pClass.m_bSpellbookRestricted && pClass.m_bCanLearnFromScrolls))
            {
            */
            for (byte nSpellIndex = 0; nSpellIndex < pCreatureStats.GetNumberKnownSpells(nMultiClass, nSpellLevel); nSpellIndex++)
            {
              if (!listSpells[nMultiClass][nSpellLevel].Any())
              {
                if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
                {
                  Player = nwPlayer,
                  Type = ValidationFailureType.Spell,
                  SubType = ValidationFailureSubType.SpellListComparison,
                  StrRef = StrRefSpellIllegalNumSpells,
                }))
                {
                  return strRefFailure;
                }
              }

              uint nSpellID = pCreatureStats.GetKnownSpell(nMultiClass, nSpellLevel, nSpellIndex);

              if (!listSpells[nMultiClass][nSpellLevel].Contains(nSpellID))
              {
                if (HandleValidationFailure(out int strRefFailure, new OnELCSpellValidationFailure
                {
                  Player = nwPlayer,
                  Type = ValidationFailureType.Spell,
                  SubType = ValidationFailureSubType.SpellListComparison,
                  Spell = NwSpell.FromSpellId((int)nSpellID),
                  StrRef = StrRefSpellIllegalNumSpells,
                }))
                {
                  return strRefFailure;
                }
              }

              listSpells[nMultiClass][nSpellLevel].Remove(nSpellID);
            }

            if (listSpells[nMultiClass][nSpellLevel].Any())
            {
              if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
              {
                Player = nwPlayer,
                Type = ValidationFailureType.Spell,
                SubType = ValidationFailureSubType.SpellListComparison,
                StrRef = StrRefSpellIllegalNumSpells,
              }))
              {
                return strRefFailure;
              }
            }
            //}
          }
        }
      }

      // Final Skills Check
      // Compare our calculated rank with the saved rank
      for (byte nSkill = 0; nSkill < pRules.m_nNumSkills; nSkill++)
      {
        if (listSkillRanks[nSkill] != pCreatureStats.GetSkillRank(nSkill, null, true.ToInt()))
        {
          if (HandleValidationFailure(out int strRefFailure, new OnELCSkillValidationFailure
          {
            Player = nwPlayer,
            Type = ValidationFailureType.Skill,
            SubType = ValidationFailureSubType.SkillListComparison,
            Skill = NwSkill.FromSkillId(nSkill),
            StrRef = StrRefSkillInvalidRanks,
          }))
          {
            return strRefFailure;
          }
        }
      }

      // Final Feats Check
      // Check if our list of feats from LevelStats are the same as the feats the character has
      foreach (ushort nFeat in pCreatureStats.m_lstFeats)
      {
        if (!listFeats.Any())
        {
          if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
          {
            Player = nwPlayer,
            Type = ValidationFailureType.Feat,
            SubType = ValidationFailureSubType.FeatListComparison,
            StrRef = StrRefFeatTooMany,
          }))
          {
            return strRefFailure;
          }
        }

        if (!listFeats.Contains(nFeat))
        {
          if (HandleValidationFailure(out int strRefFailure, new OnELCFeatValidationFailure
          {
            Player = nwPlayer,
            Type = ValidationFailureType.Feat,
            SubType = ValidationFailureSubType.FeatListComparison,
            Feat = NwFeat.FromFeatId(nFeat),
            StrRef = StrRefFeatTooMany,
          }))
          {
            return strRefFailure;
          }
        }

        listFeats.Remove(nFeat);
      }

      // Check Misc Saving Throws
      if (pCreatureStats.m_nFortSavingThrowMisc > 0 ||
        pCreatureStats.m_nReflexSavingThrowMisc > 0 ||
        pCreatureStats.m_nWillSavingThrowMisc > 0)
      {
        if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
        {
          Player = nwPlayer,
          Type = ValidationFailureType.Character,
          SubType = ValidationFailureSubType.MiscSavingThrow,
          StrRef = StrRefCharacterSavingThrow,
        }))
        {
          return strRefFailure;
        }
      }

      // Compare Feats Lists
      int nNumberOfFeats = 0;
      for (byte nLevel = 1; nLevel <= nCharacterLevel; nLevel++)
      {
        CNWLevelStats pLevelStats = pCreatureStats.GetLevelStats((byte)(nLevel - 1));
        nNumberOfFeats += pLevelStats.m_lstFeats.Count;
      }

      if (pCreatureStats.m_lstFeats.Count > nNumberOfFeats)
      {
        if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
        {
          Player = nwPlayer,
          Type = ValidationFailureType.Feat,
          SubType = ValidationFailureSubType.NumFeatComparison,
          StrRef = StrRefFeatInvalid,
        }))
        {
          return strRefFailure;
        }
      }

      // Run a custom ELC check if enabled and there is an ELC script set
      if (!InvokeCustomCheck(nwPlayer))
      {
        if (HandleValidationFailure(out int strRefFailure, new OnELCValidationFailure
        {
          Player = nwPlayer,
          Type = ValidationFailureType.Custom,
          SubType = ValidationFailureSubType.None,
          StrRef = StrRefCustom,
        }))
        {
          return strRefFailure;
        }
      }

      InvokeSuccessEvent(nwPlayer);
      return 0;
    }
  }
}
