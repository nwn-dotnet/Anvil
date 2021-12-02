using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API
{
  /// <summary>
  /// The race type of a creature. Contains additional ruleset information about the race.
  /// </summary>
  public sealed class NwRace
  {
    [Inject]
    private static RulesetService RulesetService { get; set; }

    private readonly CNWRace raceInfo;

    public NwRace(RacialType racialType, CNWRace raceInfo)
    {
      RacialType = racialType;
      this.raceInfo = raceInfo;
    }

    /// <summary>
    /// Gets the amount of points granted to this race during ability score point buy in character creation.
    /// </summary>
    public int AbilityPointBuyAmount
    {
      get => raceInfo.m_nAbilitiesPointBuyNumber;
    }

    /// <summary>
    /// Gets the default age used during character creation.
    /// </summary>
    public int DefaultAge
    {
      get => raceInfo.m_nAge;
    }

    /// <summary>
    /// Gets the description used by default for the character's description.
    /// </summary>
    public string DefaultCharacterDescription
    {
      get => raceInfo.GetDefaultBiographyText().ToString();
    }

    /// <summary>
    /// Gets the description of this race.
    /// </summary>
    public string Description
    {
      get => raceInfo.GetDescriptionText().ToString();
    }

    public byte Endurance
    {
      get => raceInfo.m_nEndurance;
    }

    /// <summary>
    /// Gets the number of extra feats that this race grants characters at first level.
    /// </summary>
    public int ExtraFeatsAtFirstLevel
    {
      get => raceInfo.m_nExtraFeatsAtFirstLevel;
    }

    /// <summary>
    /// Gets the number of extra skill points that this race grants characters per level.
    /// </summary>
    public int ExtraSkillPointsPerLevel
    {
      get => raceInfo.m_nExtraSkillPointsPerLevel;
    }

    /// <summary>
    /// Gets the favoured class for this race, for the purposes of multi-classing.
    /// </summary>
    public ClassType FavoredClass
    {
      get => (ClassType)raceInfo.m_nFavoredClass;
    }

    /// <summary>
    /// Gets the initial muliplier for skill points given at first level.
    /// </summary>
    public int FirstLevelSkillPointsMultiplier
    {
      get => raceInfo.m_nFirstLevelSkillPointsMultiplier;
    }

    /// <summary>
    /// Gets if this race can be chosen/used by players.
    /// </summary>
    public bool IsPlayerRace
    {
      get => raceInfo.m_bIsPlayerRace.ToBool();
    }

    /// <summary>
    /// Gets the name of this race.
    /// </summary>
    public string Name
    {
      get => raceInfo.GetNameText().ToString();
    }

    /// <summary>
    /// Gets the level period that feats are granted from this race.
    /// </summary>
    public int NormalFeatEveryNthLevel
    {
      get => raceInfo.m_nNormalFeatEveryNthLevel;
    }

    /// <summary>
    /// Gets how many feats are given to the character every <see cref="NormalFeatEveryNthLevel"/>.
    /// </summary>
    public int NumberNormalFeatsEveryNthLevel
    {
      get => raceInfo.m_nNumberNormalFeatsEveryNthLevel;
    }

    /// <summary>
    /// Gets the plural name of this race.
    /// </summary>
    public string PluralName
    {
      get => raceInfo.GetNamePluralText().ToString();
    }

    /// <summary>
    /// Gets the RacialType of this race.
    /// </summary>
    public RacialType RacialType { get; }

    /// <summary>
    /// Gets the ability score used to determine bonus skill points at level up.
    /// </summary>
    public Ability SkillPointModifierAbility
    {
      get => (Ability)raceInfo.m_nSkillPointModifierAbility;
    }

    public static NwRace FromRacialType(RacialType racialType)
    {
      return RulesetService.Races[(int)racialType];
    }

    /// <summary>
    /// Gets the ability score adjustment for the specified ability.
    /// </summary>
    /// <param name="ability">The ability to query.</param>
    /// <returns>A signed byte of the adjustment to the creature's ability score.</returns>
    public sbyte GetAbilityAdjustment(Ability ability)
    {
      return raceInfo.GetAbilityAdjust((byte)ability).AsSByte();
    }

    /// <summary>
    /// Gets if the specified feat is one that is automatically granted by this race at first level.
    /// </summary>
    /// <param name="feat">The feat to query.</param>
    /// <returns>True if this is a default granted feat for this race, otherwise false.</returns>
    public bool IsFirstLevelGrantedFeat(Feat feat)
    {
      return raceInfo.IsFirstLevelGrantedFeat((ushort)feat).ToBool();
    }
  }
}
