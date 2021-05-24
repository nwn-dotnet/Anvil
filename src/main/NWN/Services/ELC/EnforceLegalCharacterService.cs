using System;
using Anvil.Internal;
using NWN.API;
using NWN.Native.API;
using NWNX.API.Constants;

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
    private const int NumCreatureItemSlots = 4;
    private const int NumMulticlass = 3;
    private const int CharacterEpicLevel = 21;
    private const int NumSpellLevels = 10;

    private delegate int ValidateCharacterHook(void* pPlayer, int* bFailedServerRestriction);
    private readonly FunctionHook<ValidateCharacterHook> validateCharacterHook;

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
          Type = ElcFailureType.Character,
          SubType = ElcFailureSubType.LevelRestriction,
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
            Type = ElcFailureType.Character,
            SubType = ElcFailureSubType.LevelRestriction,
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
