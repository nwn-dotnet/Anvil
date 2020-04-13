namespace NWNX
{
  [NWNXPlugin(NWNX_Creature)]
  internal class CreaturePlugin
  {
    public const string NWNX_Creature = "NWNX_Creature";

    /// /< @private
    /// / @name Creature Movement Rates
    /// / @anchor creature_movement_rates
    /// /
    /// / The various types of movement rates.
    /// / @{
    public const int NWNX_CREATURE_MOVEMENT_RATE_PC = 0;
    public const int NWNX_CREATURE_MOVEMENT_RATE_IMMOBILE = 1;
    public const int NWNX_CREATURE_MOVEMENT_RATE_VERY_SLOW = 2;
    public const int NWNX_CREATURE_MOVEMENT_RATE_SLOW = 3;
    public const int NWNX_CREATURE_MOVEMENT_RATE_NORMAL = 4;
    public const int NWNX_CREATURE_MOVEMENT_RATE_FAST = 5;
    public const int NWNX_CREATURE_MOVEMENT_RATE_VERY_FAST = 6;
    public const int NWNX_CREATURE_MOVEMENT_RATE_DEFAULT = 7;
    public const int NWNX_CREATURE_MOVEMENT_RATE_DM_FAST = 8;

    /// / @}
    /// / @name Creature Movement Types
    /// / @anchor creature_movement_types
    /// /
    /// / The various types of movement types.
    /// / @{
    public const int NWNX_CREATURE_MOVEMENT_TYPE_STATIONARY = 0;
    public const int NWNX_CREATURE_MOVEMENT_TYPE_WALK = 1;
    public const int NWNX_CREATURE_MOVEMENT_TYPE_RUN = 2;
    public const int NWNX_CREATURE_MOVEMENT_TYPE_SIDESTEP = 3;
    public const int NWNX_CREATURE_MOVEMENT_TYPE_WALK_BACKWARDS = 4;

    /// / @}
    /// / @name Cleric Domains
    /// / @anchor cleric_domains
    /// /
    /// / The clerical domains.
    /// / @{
    public const int NWNX_CREATURE_CLERIC_DOMAIN_AIR = 0;
    public const int NWNX_CREATURE_CLERIC_DOMAIN_ANIMAL = 1;
    public const int NWNX_CREATURE_CLERIC_DOMAIN_DEATH = 3;
    public const int NWNX_CREATURE_CLERIC_DOMAIN_DESTRUCTION = 4;
    public const int NWNX_CREATURE_CLERIC_DOMAIN_EARTH = 5;
    public const int NWNX_CREATURE_CLERIC_DOMAIN_EVIL = 6;
    public const int NWNX_CREATURE_CLERIC_DOMAIN_FIRE = 7;
    public const int NWNX_CREATURE_CLERIC_DOMAIN_GOOD = 8;
    public const int NWNX_CREATURE_CLERIC_DOMAIN_HEALING = 9;
    public const int NWNX_CREATURE_CLERIC_DOMAIN_KNOWLEDGE = 10;
    public const int NWNX_CREATURE_CLERIC_DOMAIN_MAGIC = 13;
    public const int NWNX_CREATURE_CLERIC_DOMAIN_PLANT = 14;
    public const int NWNX_CREATURE_CLERIC_DOMAIN_PROTECTION = 15;
    public const int NWNX_CREATURE_CLERIC_DOMAIN_STRENGTH = 16;
    public const int NWNX_CREATURE_CLERIC_DOMAIN_SUN = 17;
    public const int NWNX_CREATURE_CLERIC_DOMAIN_TRAVEL = 18;
    public const int NWNX_CREATURE_CLERIC_DOMAIN_TRICKERY = 19;
    public const int NWNX_CREATURE_CLERIC_DOMAIN_WAR = 20;
    public const int NWNX_CREATURE_CLERIC_DOMAIN_WATER = 21;

    /// / @}
    /// / @name Bonus Types
    /// / @anchor bonus_types
    /// /
    /// / Used with NWNX_Creature_GetTotalEffectBonus() these are the types of temporary bonuses from effects.
    /// / @{
    public const int NWNX_CREATURE_BONUS_TYPE_ATTACK = 1;
    public const int NWNX_CREATURE_BONUS_TYPE_DAMAGE = 2;
    public const int NWNX_CREATURE_BONUS_TYPE_SAVING_THROW = 3;
    public const int NWNX_CREATURE_BONUS_TYPE_ABILITY = 4;
    public const int NWNX_CREATURE_BONUS_TYPE_SKILL = 5;
    public const int NWNX_CREATURE_BONUS_TYPE_TOUCH_ATTACK = 6;

    /// / @}
    /// / @struct NWNX_Creature_SpecialAbility
    /// / @brief A creature special ability.
    /// / @struct NWNX_Creature_MemorisedSpell
    /// / @brief A memorised spell structure.
    /// / @brief Gives the creature a feat.
    /// / @param creature The creature object.
    /// / @param feat The feat id.
    /// / @remark Consider also using NWNX_Creature_AddFeatByLevel() to properly allocate the feat to a level
    public static void AddFeat(uint creature, int feat)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "AddFeat");
      NWN.Internal.NativeFunctions.StackPushInteger(feat);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Gives the creature a feat assigned at a level
    /// / @param creature The creature object.
    /// / @param feat The feat id.
    /// / @param level The level they gained the feat.
    /// / @remark Adds the feat to the stat list at the provided level.
    public static void AddFeatByLevel(uint creature, int feat, int level)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "AddFeatByLevel");
      NWN.Internal.NativeFunctions.StackPushInteger(level);
      NWN.Internal.NativeFunctions.StackPushInteger(feat);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Removes a feat from a creature.
    /// / @param creature The creature object.
    /// / @param feat The feat id.
    public static void RemoveFeat(uint creature, int feat)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "RemoveFeat");
      NWN.Internal.NativeFunctions.StackPushInteger(feat);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Determines if the creature knows a feat.
    /// / @note This differs from native @nwn{GetHasFeat} which returns FALSE if the feat has no more uses per day.
    /// / @param creature The creature object.
    /// / @param feat The feat id.
    /// / @return TRUE if the creature has the feat, regardless if they have any usages left or not.
    public static int GetKnowsFeat(uint creature, int feat)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "GetKnowsFeat");
      NWN.Internal.NativeFunctions.StackPushInteger(feat);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopInteger();
    }

    /// / @brief Returns the count of feats learned at the provided level.
    /// / @param creature The creature object.
    /// / @param level The level.
    /// / @return The count of feats.
    public static int GetFeatCountByLevel(uint creature, int level)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "GetFeatCountByLevel");
      NWN.Internal.NativeFunctions.StackPushInteger(level);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopInteger();
    }

    /// / @brief Returns the feat learned at the level and index.
    /// / @param creature The creature object.
    /// / @param level The level.
    /// / @param index The index. Index bounds: 0 <= index < NWNX_Creature_GetFeatCountByLevel().
    /// / @return The feat id at the index.
    public static int GetFeatByLevel(uint creature, int level, int index)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "GetFeatByLevel");
      NWN.Internal.NativeFunctions.StackPushInteger(index);
      NWN.Internal.NativeFunctions.StackPushInteger(level);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopInteger();
    }

    /// / @brief Returns the creature level where the specified feat was learned.
    /// / @param creature The creature object.
    /// / @param feat The feat id.
    /// / @return The character level that the specified feat was granted, otherwise 0 if the creature does not have this feat.
    public static int GetFeatGrantLevel(uint creature, int feat)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "GetFeatGrantLevel");
      NWN.Internal.NativeFunctions.StackPushInteger(feat);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopInteger();
    }

    /// / @brief Get the total number of feats known by creature.
    /// / @param creature The creature object.
    /// / @return The total feat count for the creature.
    public static int GetFeatCount(uint creature)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "GetFeatCount");
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopInteger();
    }

    /// / @brief Returns the creature's feat at a given index
    /// / @param creature The creature object.
    /// / @param index The index. Index bounds: 0 <= index < NWNX_Creature_GetFeatCount();
    /// / @return The feat id at the index.
    public static int GetFeatByIndex(uint creature, int index)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "GetFeatByIndex");
      NWN.Internal.NativeFunctions.StackPushInteger(index);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopInteger();
    }

    /// / @brief Gets if creature meets feat requirements.
    /// / @param creature The creature object.
    /// / @param feat The feat id.
    /// / @return TRUE if creature meets all requirements to take given feat
    public static int GetMeetsFeatRequirements(uint creature, int feat)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "GetMeetsFeatRequirements");
      NWN.Internal.NativeFunctions.StackPushInteger(feat);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopInteger();
    }

    /// / @brief Gets the count of special abilities of the creature.
    /// / @param creature The creature object.
    /// / @return The total special ability count.
    public static int GetSpecialAbilityCount(uint creature)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "GetSpecialAbilityCount");
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopInteger();
    }

    /// / @brief Returns the creature's special ability at a given index.
    /// / @param creature The creature object.
    /// / @param index The index. Index bounds: 0 <= index < NWNX_Creature_GetSpecialAbilityCount().
    /// / @return An NWNX_Creature_SpecialAbility struct.
    public static SpecialAbility GetSpecialAbility(uint creature, int index)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "GetSpecialAbility");
      NWN.Internal.NativeFunctions.StackPushInteger(index);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      SpecialAbility retVal;
      retVal.level = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.ready = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.id = NWN.Internal.NativeFunctions.StackPopInteger();
      return retVal;
    }

    /// / @brief Adds a special ability to a creature.
    /// / @param creature The creature object.
    /// / @param ability An NWNX_Creature_SpecialAbility struct.
    public static void AddSpecialAbility(uint creature, SpecialAbility ability)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "AddSpecialAbility");
      NWN.Internal.NativeFunctions.StackPushInteger(ability.id);
      NWN.Internal.NativeFunctions.StackPushInteger(ability.ready);
      NWN.Internal.NativeFunctions.StackPushInteger(ability.level);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Removes a special ability from a creature.
    /// / @param creature The creature object.
    /// / @param index The index. Index bounds: 0 <= index < NWNX_Creature_GetSpecialAbilityCount().
    public static void RemoveSpecialAbility(uint creature, int index)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "RemoveSpecialAbility");
      NWN.Internal.NativeFunctions.StackPushInteger(index);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Sets a special ability at the index for the creature.
    /// / @param creature The creature object.
    /// / @param index The index. Index bounds: 0 <= index < NWNX_Creature_GetSpecialAbilityCount().
    /// / @param ability An NWNX_Creature_SpecialAbility struct.
    public static void SetSpecialAbility(uint creature, int index, SpecialAbility ability)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "SetSpecialAbility");
      NWN.Internal.NativeFunctions.StackPushInteger(ability.id);
      NWN.Internal.NativeFunctions.StackPushInteger(ability.ready);
      NWN.Internal.NativeFunctions.StackPushInteger(ability.level);
      NWN.Internal.NativeFunctions.StackPushInteger(index);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Get the class taken by the creature at the provided level.
    /// / @param creature The creature object.
    /// / @param level The level.
    /// / @return The class id.
    public static int GetClassByLevel(uint creature, int level)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "GetClassByLevel");
      NWN.Internal.NativeFunctions.StackPushInteger(level);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopInteger();
    }

    /// / @brief Sets the base AC for the creature.
    /// / @param creature The creature object.
    /// / @param ac The base AC to set for the creature.
    public static void SetBaseAC(uint creature, int ac)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "SetBaseAC");
      NWN.Internal.NativeFunctions.StackPushInteger(ac);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Get the base AC for the creature.
    /// / @param creature The creature object.
    /// / @return The base AC.
    public static int GetBaseAC(uint creature)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "GetBaseAC");
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopInteger();
    }

    /// / @brief Sets the ability score of the creature to the value.
    /// / @param creature The creature object.
    /// / @param ability The ability constant.
    /// / @param value The value to set.
    /// / @deprecated Use NWNX_Creature_SetRawAbilityScore(). This will be removed in future NWNX releases.
    public static void SetAbilityScore(uint creature, int ability, int value)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "SetAbilityScore");
      NWN.Internal.NativeFunctions.StackPushInteger(value);
      NWN.Internal.NativeFunctions.StackPushInteger(ability);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Sets the ability score of the creature to the provided value.
    /// / @note Does not apply racial bonuses/penalties.
    /// / @param creature The creature object.
    /// / @param ability The ability constant.
    /// / @param value The value to set.
    public static void SetRawAbilityScore(uint creature, int ability, int value)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "SetRawAbilityScore");
      NWN.Internal.NativeFunctions.StackPushInteger(value);
      NWN.Internal.NativeFunctions.StackPushInteger(ability);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Gets the ability score of the creature.
    /// / @note Does not apply racial bonuses/penalties.
    /// / @param creature The creature object.
    /// / @param ability The ability constant.
    /// / @return The ability score.
    public static int GetRawAbilityScore(uint creature, int ability)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "GetRawAbilityScore");
      NWN.Internal.NativeFunctions.StackPushInteger(ability);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopInteger();
    }

    /// / @brief Adjusts the ability score of a creature.
    /// / @note Does not apply racial bonuses/penalties.
    /// / @param creature The creature object.
    /// / @param ability The ability constant.
    /// / @param modifier The modifier value.
    public static void ModifyRawAbilityScore(uint creature, int ability, int modifier)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "ModifyRawAbilityScore");
      NWN.Internal.NativeFunctions.StackPushInteger(modifier);
      NWN.Internal.NativeFunctions.StackPushInteger(ability);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Gets the raw ability score a polymorphed creature had prior to polymorphing.
    /// / @note For Strength, Dexterity and Constitution only.
    /// / @param creature The creature object.
    /// / @param ability The ability constant.
    /// / @return The raw ability score.
    public static int GetPrePolymorphAbilityScore(uint creature, int ability)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "GetPrePolymorphAbilityScore");
      NWN.Internal.NativeFunctions.StackPushInteger(ability);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopInteger();
    }

    /// / @brief Gets the count of memorised spells for a creature's class at a level.
    /// / @param creature The creature object.
    /// / @param class The class id from classes.2da. (Not class index 0-2)
    /// / @param level The spell level.
    /// / @return The memorised spell count.
    public static int GetMemorisedSpellCountByLevel(uint creature, int @class, int level)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "GetMemorisedSpellCountByLevel");
      NWN.Internal.NativeFunctions.StackPushInteger(level);
      NWN.Internal.NativeFunctions.StackPushInteger(@class);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopInteger();
    }

    /// / @brief Gets the memorised spell at a class level's index.
    /// / @param creature The creature object.
    /// / @param class The class id from classes.2da. (Not class index 0-2)
    /// / @param level The spell level.
    /// / @param index The index. Index bounds: 0 <= index < NWNX_Creature_GetMemorisedSpellCountByLevel().
    /// / @return An NWNX_Creature_MemorisedSpell() struct.
    public static MemorisedSpell GetMemorisedSpell(uint creature, int @class, int level, int index)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "GetMemorisedSpell");
      NWN.Internal.NativeFunctions.StackPushInteger(index);
      NWN.Internal.NativeFunctions.StackPushInteger(level);
      NWN.Internal.NativeFunctions.StackPushInteger(@class);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      MemorisedSpell retVal;
      retVal.domain = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.meta = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.ready = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.id = NWN.Internal.NativeFunctions.StackPopInteger();
      return retVal;
    }

    /// / @brief Sets the memorised spell at a class level's index.
    /// / @param creature The creature object.
    /// / @param class The class id from classes.2da. (Not class index 0-2)
    /// / @param level The spell level.
    /// / @param index The index. Index bounds: 0 <= index < NWNX_Creature_GetMemorisedSpellCountByLevel().
    /// / @param spell An NWNX_Creature_MemorisedSpell() struct.
    public static void SetMemorisedSpell(uint creature, int @class, int level, int index, MemorisedSpell spell)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "SetMemorisedSpell");
      NWN.Internal.NativeFunctions.StackPushInteger(spell.id);
      NWN.Internal.NativeFunctions.StackPushInteger(spell.ready);
      NWN.Internal.NativeFunctions.StackPushInteger(spell.meta);
      NWN.Internal.NativeFunctions.StackPushInteger(spell.domain);
      NWN.Internal.NativeFunctions.StackPushInteger(index);
      NWN.Internal.NativeFunctions.StackPushInteger(level);
      NWN.Internal.NativeFunctions.StackPushInteger(@class);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Gets the remaining spell slots (innate casting) at a class level's index.
    /// / @param creature The creature object.
    /// / @param class The class id from classes.2da. (Not class index 0-2)
    /// / @param level The spell level.
    /// / @return The remaining spell slot count.
    public static int GetRemainingSpellSlots(uint creature, int @class, int level)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "GetRemainingSpellSlots");
      NWN.Internal.NativeFunctions.StackPushInteger(level);
      NWN.Internal.NativeFunctions.StackPushInteger(@class);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopInteger();
    }

    /// / @brief Sets the remaining spell slots (innate casting) at a class level.
    /// / @param creature The creature object.
    /// / @param class The class id from classes.2da. (Not class index 0-2)
    /// / @param level The spell level.
    /// / @param slots The remaining spell slots to set.
    public static void SetRemainingSpellSlots(uint creature, int @class, int level, int slots)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "SetRemainingSpellSlots");
      NWN.Internal.NativeFunctions.StackPushInteger(slots);
      NWN.Internal.NativeFunctions.StackPushInteger(level);
      NWN.Internal.NativeFunctions.StackPushInteger(@class);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Gets the maximum spell slots (innate casting) at a class level.
    /// / @param creature The creature object.
    /// / @param class The class id from classes.2da. (Not class index 0-2)
    /// / @param level The spell level.
    /// / @return The maximum spell slot count.
    public static int GetMaxSpellSlots(uint creature, int @class, int level)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "GetMaxSpellSlots");
      NWN.Internal.NativeFunctions.StackPushInteger(level);
      NWN.Internal.NativeFunctions.StackPushInteger(@class);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopInteger();
    }

    /// / @brief Gets the known spell count (innate casting) at a class level.
    /// / @param creature The creature object.
    /// / @param class The class id from classes.2da. (Not class index 0-2)
    /// / @param level The spell level.
    /// / @return The known spell count.
    public static int GetKnownSpellCount(uint creature, int @class, int level)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "GetKnownSpellCount");
      NWN.Internal.NativeFunctions.StackPushInteger(level);
      NWN.Internal.NativeFunctions.StackPushInteger(@class);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopInteger();
    }

    /// / @brief Gets the known spell at a class level's index.
    /// / @param creature The creature object.
    /// / @param class The class id from classes.2da. (Not class index 0-2)
    /// / @param level The spell level.
    /// / @param index The index. Index bounds: 0 <= index < NWNX_Creature_GetKnownSpellCount().
    /// / @return The spell id.
    public static int GetKnownSpell(uint creature, int @class, int level, int index)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "GetKnownSpell");
      NWN.Internal.NativeFunctions.StackPushInteger(index);
      NWN.Internal.NativeFunctions.StackPushInteger(level);
      NWN.Internal.NativeFunctions.StackPushInteger(@class);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopInteger();
    }

    /// / @brief Add a spell to a creature's spellbook for class.
    /// / @param creature The creature object.
    /// / @param class The class id from classes.2da. (Not class index 0-2)
    /// / @param level The spell level.
    /// / @param spellId The spell to remove.
    public static void AddKnownSpell(uint creature, int @class, int level, int spellId)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "AddKnownSpell");
      NWN.Internal.NativeFunctions.StackPushInteger(spellId);
      NWN.Internal.NativeFunctions.StackPushInteger(level);
      NWN.Internal.NativeFunctions.StackPushInteger(@class);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Remove a spell from creature's spellbook for class.
    /// / @param creature The creature object.
    /// / @param class The class id from classes.2da. (Not class index 0-2)
    /// / @param level The spell level.
    /// / @param spellId The spell to remove.
    public static void RemoveKnownSpell(uint creature, int @class, int level, int spellId)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "RemoveKnownSpell");
      NWN.Internal.NativeFunctions.StackPushInteger(spellId);
      NWN.Internal.NativeFunctions.StackPushInteger(level);
      NWN.Internal.NativeFunctions.StackPushInteger(@class);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Clear a specific spell from the creature's spellbook for class
    /// / @param creature The creature object.
    /// / @param class The class id from classes.2da. (Not class index 0-2)
    /// / @param spellId The spell to clear.
    public static void ClearMemorisedKnownSpells(uint creature, int @class, int spellId)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "ClearMemorisedKnownSpells");
      NWN.Internal.NativeFunctions.StackPushInteger(spellId);
      NWN.Internal.NativeFunctions.StackPushInteger(@class);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Clear the memorised spell of the creature for the class, level and index.
    /// / @param creature The creature object.
    /// / @param class The class id from classes.2da. (Not class index 0-2)
    /// / @param level The spell level.
    /// / @param index The index. Index bounds: 0 <= index < NWNX_Creature_GetMemorisedSpellCountByLevel().
    public static void ClearMemorisedSpell(uint creature, int @class, int level, int index)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "ClearMemorisedSpell");
      NWN.Internal.NativeFunctions.StackPushInteger(index);
      NWN.Internal.NativeFunctions.StackPushInteger(level);
      NWN.Internal.NativeFunctions.StackPushInteger(@class);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Gets whether or not creature has a specialist school of wizardry.
    /// / @param creature The creature object.
    /// / @return TRUE if the wizard specializes.
    /// / @deprecated Use GetSpecialization(). This will be removed in future NWNX releases.
    public static int GetWizardSpecialization(uint creature)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "GetWizardSpecialization");
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopInteger();
    }

    /// / @brief Sets creature's wizard specialist school.
    /// / @param creature The creature object.
    /// / @param school The wizard school constant.
    /// / @deprecated Use NWNX_Creature_SetSpecialization(). This will be removed in future NWNX releases.
    public static void SetWizardSpecialization(uint creature, int school)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "SetWizardSpecialization");
      NWN.Internal.NativeFunctions.StackPushInteger(school);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Gets the maximum hit points for creature for level.
    /// / @param creature The creature object.
    /// / @param level The level.
    /// / @return The maximum hit points a creature can have for the class at the provided level.
    public static int GetMaxHitPointsByLevel(uint creature, int level)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "GetMaxHitPointsByLevel");
      NWN.Internal.NativeFunctions.StackPushInteger(level);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopInteger();
    }

    /// / @brief Sets the maximum hit points for creature.
    /// / @param creature The creature object.
    /// / @param level The level.
    /// / @param value The amount to set the max hit points.
    public static void SetMaxHitPointsByLevel(uint creature, int level, int value)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "SetMaxHitPointsByLevel");
      NWN.Internal.NativeFunctions.StackPushInteger(value);
      NWN.Internal.NativeFunctions.StackPushInteger(level);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Set creature's movement rate.
    /// / @param creature The creature object.
    /// / @param rate The movement rate.
    public static void SetMovementRate(uint creature, int rate)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "SetMovementRate");
      NWN.Internal.NativeFunctions.StackPushInteger(rate);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Returns the creature's current movement rate factor.
    /// / @remark Base movement rate factor is 1.0.
    /// / @param creature The creature object.
    /// / @return The current movement rate factor.
    public static float GetMovementRateFactor(uint creature)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "GetMovementRateFactor");
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopFloat();
    }

    /// / @brief Sets the creature's current movement rate factor.
    /// / @note Base movement rate factor is 1.0.
    /// / @param creature The creature object.
    /// / @param rate The rate to set.
    public static void SetMovementRateFactor(uint creature, float rate)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "SetMovementRateFactor");
      NWN.Internal.NativeFunctions.StackPushFloat(rate);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Returns the creature's current movement type
    /// / @param creature The creature object.
    /// / @return An NWNX_CREATURE_MOVEMENT_TYPE_* constant.
    public static int GetMovementType(uint creature)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "GetMovementType");
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopInteger();
    }

    /// / @brief Sets the maximum movement rate a creature can have while walking (not running)
    /// / @remark This allows a creature with movement speed enhancements to walk at a normal rate.
    /// / @param creature The creature object.
    /// / @param fWalkRate The walk rate to apply. Setting the value to -1.0 will remove the cap.
    /// / Default value is 2000.0, which is the base human walk speed.
    public static void SetWalkRateCap(uint creature, float fWalkRate = 2000.0f)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "SetWalkRateCap");
      NWN.Internal.NativeFunctions.StackPushFloat(fWalkRate);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Set creature's raw good/evil alignment value.
    /// / @param creature The creature object.
    /// / @param value The value to set.
    public static void SetAlignmentGoodEvil(uint creature, int value)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "SetAlignmentGoodEvil");
      NWN.Internal.NativeFunctions.StackPushInteger(value);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Set creature's raw law/chaos alignment value.
    /// / @param creature The creature object.
    /// / @param value The value to set.
    public static void SetAlignmentLawChaos(uint creature, int value)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "SetAlignmentLawChaos");
      NWN.Internal.NativeFunctions.StackPushInteger(value);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Gets one of creature's cleric domains.
    /// / @param creature The creature object.
    /// / @param index The first or second domain.
    /// / @deprecated Use GetDomain(). This will be removed in future NWNX releases.
    public static int GetClericDomain(uint creature, int index)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "GetClericDomain");
      NWN.Internal.NativeFunctions.StackPushInteger(index);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopInteger();
    }

    /// / @brief Sets one of creature's cleric domains.
    /// / @param creature The creature object.
    /// / @param index The first or second domain.
    /// / @param domain The domain constant to set.
    /// / @deprecated Use NWNX_Creature_SetDomain(). This will be removed in future NWNX releases.
    public static void SetClericDomain(uint creature, int index, int domain)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "SetClericDomain");
      NWN.Internal.NativeFunctions.StackPushInteger(domain);
      NWN.Internal.NativeFunctions.StackPushInteger(index);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Get the soundset index for creature.
    /// / @param creature The creature object.
    /// / @return The soundset used by the creature.
    public static int GetSoundset(uint creature)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "GetSoundset");
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopInteger();
    }

    /// / @brief Set the soundset index for creature.
    /// / @param creature The creature object.
    /// / @param soundset The soundset index.
    public static void SetSoundset(uint creature, int soundset)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "SetSoundset");
      NWN.Internal.NativeFunctions.StackPushInteger(soundset);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Set the base ranks in a skill for creature
    /// / @param creature The creature object.
    /// / @param skill The skill id.
    /// / @param rank The value to set as the skill rank.
    public static void SetSkillRank(uint creature, int skill, int rank)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "SetSkillRank");
      NWN.Internal.NativeFunctions.StackPushInteger(rank);
      NWN.Internal.NativeFunctions.StackPushInteger(skill);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Set the class ID in a particular position for a creature.
    /// / @param creature The creature object.
    /// / @param position Should be 0, 1, or 2 depending on how many classes the creature
    /// / has and which is to be modified.
    /// / @param classID A valid ID number in classes.2da and between 0 and 255.
    public static void SetClassByPosition(uint creature, int position, int classID)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "SetClassByPosition");
      NWN.Internal.NativeFunctions.StackPushInteger(classID);
      NWN.Internal.NativeFunctions.StackPushInteger(position);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Set the level at the given position for a creature.
    /// / @note A creature should already have a class in that position.
    /// / @param creature The creature object.
    /// / @param position Should be 0, 1, or 2 depending on how many classes the creature
    /// / has and which is to be modified.
    /// / @param level The level to set.
    public static void SetLevelByPosition(uint creature, int position, int level)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "SetLevelByPosition");
      NWN.Internal.NativeFunctions.StackPushInteger(level);
      NWN.Internal.NativeFunctions.StackPushInteger(position);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Set creature's base attack bonus (BAB).
    /// / @note Modifying the BAB will also affect the creature's attacks per round and its
    /// / eligibility for feats, prestige classes, etc.
    /// / @param creature The creature object.
    /// / @param bab The BAB value. Should be between 0 and 254. Setting BAB to 0 will cause the
    /// / creature to revert to its original BAB based on its classes and levels. A creature can
    /// / never have an actual BAB of zero.
    /// / @remark The base game has a function @nwn{SetBaseAttackBonus}, which actually sets
    /// / the bonus attacks per round for a creature, not the BAB.
    public static void SetBaseAttackBonus(uint creature, int bab)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "SetBaseAttackBonus");
      NWN.Internal.NativeFunctions.StackPushInteger(bab);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Gets the creatures current attacks per round (using equipped weapon).
    /// / @param creature The creature object.
    /// / @param bBaseAPR If TRUE, will return the base attacks per round, based on BAB and
    /// / equipped weapons, regardless of overrides set by calls to @nwn{SetBaseAttackBonus} builtin function.
    /// / @return The attacks per round.
    public static int GetAttacksPerRound(uint creature, int bBaseAPR = NWN.NWScript.FALSE)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "GetAttacksPerRound");
      NWN.Internal.NativeFunctions.StackPushInteger(bBaseAPR);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopInteger();
    }

    /// / @brief Sets the creature gender.
    /// / @param creature The creature object.
    /// / @param gender The GENDER_ constant.
    public static void SetGender(uint creature, int gender)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "SetGender");
      NWN.Internal.NativeFunctions.StackPushInteger(gender);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Restore all creature feat uses.
    /// / @param creature The creature object.
    public static void RestoreFeats(uint creature)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "RestoreFeats");
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Restore all creature special ability uses.
    /// / @param creature The creature object.
    public static void RestoreSpecialAbilities(uint creature)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "RestoreSpecialAbilities");
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Restore all creature spells per day for given level.
    /// / @param creature The creature object.
    /// / @param level The level to restore. If -1, all spells are restored.
    public static void RestoreSpells(uint creature, int level = -1)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "RestoreSpells");
      NWN.Internal.NativeFunctions.StackPushInteger(level);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Restore uses for all items carried by the creature.
    /// / @param creature The creature object.
    public static void RestoreItems(uint creature)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "RestoreItems");
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Sets the creature size.
    /// / @param creature The creature object.
    /// / @param size Use CREATURE_SIZE_* constants.
    public static void SetSize(uint creature, int size)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "SetSize");
      NWN.Internal.NativeFunctions.StackPushInteger(size);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Gets the creature's remaining unspent skill points.
    /// / @param creature The creature object.
    /// / @return The remaining unspent skill points.
    public static int GetSkillPointsRemaining(uint creature)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "GetSkillPointsRemaining");
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopInteger();
    }

    /// / @brief Sets the creature's remaining unspent skill points.
    /// / @param creature The creature object.
    /// / @param skillpoints The value to set.
    public static void SetSkillPointsRemaining(uint creature, int skillpoints)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "SetSkillPointsRemaining");
      NWN.Internal.NativeFunctions.StackPushInteger(skillpoints);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Sets the creature's racial type
    /// / @param creature The creature object.
    /// / @param racialtype The racial type to set.
    public static void SetRacialType(uint creature, int racialtype)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "SetRacialType");
      NWN.Internal.NativeFunctions.StackPushInteger(racialtype);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Sets the creature's gold without sending a feedback message
    /// / @param creature The creature object.
    /// / @param gold The amount of gold to set for their creature.
    public static void SetGold(uint creature, int gold)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "SetGold");
      NWN.Internal.NativeFunctions.StackPushInteger(gold);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Sets corpse decay time in milliseconds
    /// / @param creature The creature object.
    /// / @param nDecayTime The corpse decay time.
    public static void SetCorpseDecayTime(uint creature, int nDecayTime)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "SetCorpseDecayTime");
      NWN.Internal.NativeFunctions.StackPushInteger(nDecayTime);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Gets the creature's base save.
    /// / @param creature The creature object.
    /// / @param which One of SAVING_THROW_FORT, SAVING_THROW_REFLEX or SAVING_THROW_WILL
    /// / @return The base save value.
    /// / @note This will include any modifiers set in the toolset.
    public static int GetBaseSavingThrow(uint creature, int which)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "GetBaseSavingThrow");
      NWN.Internal.NativeFunctions.StackPushInteger(which);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopInteger();
    }

    /// / @brief Sets the creature's base save.
    /// / @param creature The creature object.
    /// / @param which One of SAVING_THROW_FORT, SAVING_THROW_REFLEX or SAVING_THROW_WILL
    /// / @param value The base save value.
    public static void SetBaseSavingThrow(uint creature, int which, int value)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "SetBaseSavingThrow");
      NWN.Internal.NativeFunctions.StackPushInteger(value);
      NWN.Internal.NativeFunctions.StackPushInteger(which);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Add levels of class to the creature, bypassing all validation
    /// / @param creature The creature object.
    /// / @param class The class id.
    /// / @param count The amount of levels of class to add.
    /// / @note This will not work on player characters.
    public static void LevelUp(uint creature, int @class, int count = 1)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "LevelUp");
      NWN.Internal.NativeFunctions.StackPushInteger(count);
      NWN.Internal.NativeFunctions.StackPushInteger(@class);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Remove last levels from a creature.
    /// / @param creature The creature object.
    /// / @param count The amount of levels to decrement.
    /// / @note This will not work on player characters.
    public static void LevelDown(uint creature, int count = 1)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "LevelDown");
      NWN.Internal.NativeFunctions.StackPushInteger(count);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Sets the creature's challenge rating
    /// / @param creature The creature object.
    /// / @param fCR The challenge rating.
    public static void SetChallengeRating(uint creature, float fCR)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "SetChallengeRating");
      NWN.Internal.NativeFunctions.StackPushFloat(fCR);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Returns the creature's highest attack bonus based on its own stats.
    /// / @note AB vs. Type and +AB on Gauntlets are excluded
    /// / @param creature The creature object.
    /// / @param isMelee
    /// /   * TRUE: Get Melee/Unarmed Attack Bonus
    /// /   * FALSE: Get Ranged Attack Bonus
    /// /   * -1: Get Attack Bonus depending on the weapon creature has equipped in its right hand
    /// /       Defaults to Melee Attack Bonus if weapon is invalid or no weapon
    /// / @param isTouchAttack If the attack was a touch attack.
    /// / @param isOffhand If the attack was with the offhand.
    /// / @param includeBaseAttackBonus Should the result include the base attack bonus.
    /// / @return The highest attack bonus.
    public static int GetAttackBonus(uint creature, int isMelee = -1, int isTouchAttack = NWN.NWScript.FALSE, int isOffhand = NWN.NWScript.FALSE, int includeBaseAttackBonus = NWN.NWScript.TRUE)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "GetAttackBonus");
      NWN.Internal.NativeFunctions.StackPushInteger(includeBaseAttackBonus);
      NWN.Internal.NativeFunctions.StackPushInteger(isOffhand);
      NWN.Internal.NativeFunctions.StackPushInteger(isTouchAttack);
      NWN.Internal.NativeFunctions.StackPushInteger(isMelee);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopInteger();
    }

    /// / @brief Get highest level version of feat possessed by creature.
    /// / @remark For feats that increment in power, for example, barbarian rage.
    /// / @param creature The creature object.
    /// / @param feat The feat id.
    /// / @return The highest level version of the feat.
    public static int GetHighestLevelOfFeat(uint creature, int feat)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "GetHighestLevelOfFeat");
      NWN.Internal.NativeFunctions.StackPushInteger(feat);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopInteger();
    }

    /// / @brief Get feat remaining uses.
    /// / @param creature The creature object.
    /// / @param feat The feat id.
    /// / @return The amount of remaining uses.
    public static int GetFeatRemainingUses(uint creature, int feat)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "GetFeatRemainingUses");
      NWN.Internal.NativeFunctions.StackPushInteger(feat);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopInteger();
    }

    /// / @brief Get feat total uses.
    /// / @param creature The creature object.
    /// / @param feat The feat id.
    /// / @return The total uses.
    public static int GetFeatTotalUses(uint creature, int feat)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "GetFeatTotalUses");
      NWN.Internal.NativeFunctions.StackPushInteger(feat);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopInteger();
    }

    /// / @brief Set feat remaining uses.
    /// / @param creature The creature object.
    /// / @param feat The feat id.
    /// / @param uses The amount of remaining uses.
    public static void SetFeatRemainingUses(uint creature, int feat, int uses)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "SetFeatRemainingUses");
      NWN.Internal.NativeFunctions.StackPushInteger(uses);
      NWN.Internal.NativeFunctions.StackPushInteger(feat);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Get total effect bonus
    /// / @remark This exposes the actual bonus value beyond a player's base scores to attack, damage bonus, saves,
    /// / skills, ability scores, and touch attack provided by spells, equipment, potions etc.
    /// / @param creature The creature object.
    /// / @param bonusType A @ref bonus_types "Bonus Type"
    /// / @param target A target object. Used to calculate bonuses versus specific races, alignments, etc.
    /// / @param isElemental If a damage bonus includes elemental damage.
    /// / @param isForceMax If the bonus should return the maximum possible.
    /// / @param savetype A SAVING_THROW_* constant.
    /// / @param saveSpecificType A SAVING_THROW_TYPE_* constant.
    /// / @param skill A skill id.
    /// / @param abilityScore An ABILITY_* constant.
    /// / @param isOffhand Whether the attack is an offhand attack.
    /// / @return The bonus value.
    public static int GetTotalEffectBonus(uint creature, int bonusType = NWNX_CREATURE_BONUS_TYPE_ATTACK, uint target = NWN.NWScript.OBJECT_INVALID, int isElemental = 0, int isForceMax = 0, int savetype = -1, int saveSpecificType = -1, int skill = -1, int abilityScore = -1, int isOffhand = NWN.NWScript.FALSE)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "GetTotalEffectBonus");
      NWN.Internal.NativeFunctions.StackPushInteger(isOffhand);
      NWN.Internal.NativeFunctions.StackPushInteger(abilityScore);
      NWN.Internal.NativeFunctions.StackPushInteger(skill);
      NWN.Internal.NativeFunctions.StackPushInteger(saveSpecificType);
      NWN.Internal.NativeFunctions.StackPushInteger(savetype);
      NWN.Internal.NativeFunctions.StackPushInteger(isForceMax);
      NWN.Internal.NativeFunctions.StackPushInteger(isElemental);
      NWN.Internal.NativeFunctions.StackPushObject(target);
      NWN.Internal.NativeFunctions.StackPushInteger(bonusType);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopInteger();
    }

    /// / @brief Set the original first or last name of creature
    /// / @param creature The creature object.
    /// / @param name The name to give the creature.
    /// / @param isLastName TRUE to change their last name, FALSE for first.
    /// / @note For PCs this will persist to the .bic file if saved. Requires a relog to update.
    public static void SetOriginalName(uint creature, string name, int isLastName)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "SetOriginalName");
      NWN.Internal.NativeFunctions.StackPushInteger(isLastName);
      NWN.Internal.NativeFunctions.StackPushString(name);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Get the original first or last name of creature
    /// / @param creature The creature object.
    /// / @param isLastName TRUE to get last name, FALSE for first name.
    /// / @return The original first or last name of the creature.
    public static string GetOriginalName(uint creature, int isLastName)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "GetOriginalName");
      NWN.Internal.NativeFunctions.StackPushInteger(isLastName);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopString();
    }

    /// / @brief Set creature's spell resistance
    /// / @param creature The creature object.
    /// / @param sr The spell resistance.
    /// / @warning This setting will be overwritten by effects and once those effects fade the old setting (typically 0) will be set.
    public static void SetSpellResistance(uint creature, int sr)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "SetSpellResistance");
      NWN.Internal.NativeFunctions.StackPushInteger(sr);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Set creature's animal companion creature type
    /// / @param creature The master creature object.
    /// / @param type The type from ANIMAL_COMPANION_CREATURE_TYPE_*.
    public static void SetAnimalCompanionCreatureType(uint creature, int type)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "SetAnimalCompanionCreatureType");
      NWN.Internal.NativeFunctions.StackPushInteger(type);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Set creature's familiar creature type
    /// / @param creature The master creature object.
    /// / @param type The type from FAMILIAR_CREATURE_TYPE_*.
    public static void SetFamiliarCreatureType(uint creature, int type)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "SetFamiliarCreatureType");
      NWN.Internal.NativeFunctions.StackPushInteger(type);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Set creature's animal companion's name
    /// / @param creature The master creature object.
    /// / @param name The name to give their animal companion.
    public static void SetAnimalCompanionName(uint creature, string name)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "SetAnimalCompanionName");
      NWN.Internal.NativeFunctions.StackPushString(name);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Set creature's familiar's name
    /// / @param creature The master creature object.
    /// / @param name The name to give their familiar.
    public static void SetFamiliarName(uint creature, string name)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "SetFamiliarName");
      NWN.Internal.NativeFunctions.StackPushString(name);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Get whether the creature can be disarmed.
    /// / @param creature The creature object.
    /// / @return TRUE if the creature can be disarmed.
    public static int GetDisarmable(uint creature)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "GetDisarmable");
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopInteger();
    }

    /// / @brief Set whether a creature can be disarmed.
    /// / @param creature The creature object.
    /// / @param disarmable Set to TRUE if the creature can be disarmed.
    public static void SetDisarmable(uint creature, int disarmable)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "SetDisarmable");
      NWN.Internal.NativeFunctions.StackPushInteger(disarmable);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Gets one of creature's domains.
    /// / @param creature The creature object.
    /// / @param class The class id from classes.2da. (Not class index 0-2)
    /// / @param index The first or second domain.
    /// / @deprecated Use GetDomain(). This will be removed in future NWNX releases.
    public static int GetDomain(uint creature, int @class, int index)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "GetDomain");
      NWN.Internal.NativeFunctions.StackPushInteger(index);
      NWN.Internal.NativeFunctions.StackPushInteger(@class);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopInteger();
    }

    /// / @brief Sets one of creature's domains.
    /// / @param creature The creature object.
    /// / @param class The class id from classes.2da. (Not class index 0-2)
    /// / @param index The first or second domain.
    /// / @param domain The domain constant to set.
    public static void SetDomain(uint creature, int @class, int index, int domain)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "SetDomain");
      NWN.Internal.NativeFunctions.StackPushInteger(domain);
      NWN.Internal.NativeFunctions.StackPushInteger(index);
      NWN.Internal.NativeFunctions.StackPushInteger(@class);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Gets the creature's specialist school.
    /// / @param creature The creature object.
    /// / @param class The class id from classes.2da. (Not class index 0-2)
    /// / @deprecated Use GetSpecialization(). This will be removed in future NWNX releases.
    public static int GetSpecialization(uint creature, int @class)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "GetSpecialization");
      NWN.Internal.NativeFunctions.StackPushInteger(@class);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopInteger();
    }

    /// / @brief Sets creature's specialist school.
    /// / @param creature The creature object.
    /// / @param class The class id from classes.2da. (Not class index 0-2)
    /// / @param school The school constant.
    public static void SetSpecialization(uint creature, int @class, int school)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "SetSpecialization");
      NWN.Internal.NativeFunctions.StackPushInteger(school);
      NWN.Internal.NativeFunctions.StackPushInteger(@class);
      NWN.Internal.NativeFunctions.StackPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Sets oCreatures faction to be the faction with id nFactionId.
    /// / @param oCreature The creature.
    /// / @param nFactionId The faction id we want the creature to join.
    public static void SetFaction(uint oCreature, int nFactionId)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "SetFaction");
      NWN.Internal.NativeFunctions.StackPushInteger(nFactionId);
      NWN.Internal.NativeFunctions.StackPushObject(oCreature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Gets the faction id from oCreature
    /// / @param oCreature the creature we wish to query against
    /// / @return faction id as an integer, -1 when used against invalid creature or invalid object.
    public static int GetFaction(uint oCreature)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Creature, "GetFaction");
      NWN.Internal.NativeFunctions.StackPushObject(oCreature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopInteger();
    }

    /// / @}
  }

  public struct SpecialAbility
  {
    public int id;
    public int ready;
    public int level;
  }

  public struct MemorisedSpell
  {
    public int id;
    public int ready;
    public int meta;
    public int domain;
  }
}
