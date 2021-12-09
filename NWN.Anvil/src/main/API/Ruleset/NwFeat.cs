using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API
{
  /// <summary>
  /// A creature/character feat(ure) definition.
  /// </summary>
  public sealed class NwFeat
  {
    [Inject]
    private static TlkTable TlkTable { get; set; }

    private readonly CNWFeat featInfo;

    internal NwFeat(Feat featType, CNWFeat featInfo)
    {
      this.featInfo = featInfo;
      FeatType = featType;
    }

    /// <summary>
    /// Gets whether all classes can use this feat or not.
    /// </summary>
    public bool AllClassesCanUse
    {
      get => featInfo.m_bAllClassesCanUse.ToBool();
    }

    /// <summary>
    /// Gets the description of this feat, as shown in the in-game skill window.
    /// </summary>
    public string Description
    {
      get => TlkTable.GetSimpleString((uint)featInfo.m_nDescriptionStrref);
    }

    /// <summary>
    /// Gets the associated <see cref="Feat"/> type for this feat.
    /// </summary>
    public Feat FeatType { get; }

    /// <summary>
    /// Gets the ResRef for the icon representing this skill.
    /// </summary>
    public string IconResRef
    {
      get => featInfo.m_cIcon.ToString();
    }

    /// <summary>
    /// Gets whether the use of this feat is considered as a hostile act.
    /// </summary>
    public bool IsHostileFeat
    {
      get => featInfo.m_bHostileFeat.ToBool();
    }

    /// <summary>
    /// This number references masterfeats.2da. The master feat is used when a feat falls into a subcategory, such as the way "improved critical (longsword)" is a subcategory of improved critical.
    /// </summary>
    public byte MasterFeat
    {
      get => featInfo.m_nMasterFeat;
    }

    /// <summary>
    /// Gets the maximum character level allowed a character to be able to select this feat.
    /// </summary>
    public byte MaxLevel
    {
      get => featInfo.m_nMaxLevel;
    }

    /// <summary>
    /// Gets the minimum attack bonus a character must have to select this feat.
    /// </summary>
    public byte MinAttackBonus
    {
      get => featInfo.m_nMinAttackBonus;
    }

    /// <summary>
    /// Gets the minimum fortitude saving throw bonus a character must have to be able to select this feat.
    /// </summary>
    public byte MinFortSave
    {
      get => featInfo.m_nMinFortSave;
    }

    /// <summary>
    /// Gets the minimum level a character must have to be able to take this feat.
    /// </summary>
    public byte MinLevel
    {
      get => featInfo.m_nMinLevel;
    }

    /// <summary>
    /// Gets the corresponding class the character must have <see cref="MinLevel"/> levels in.
    /// </summary>
    public NwClass MinLevelClass
    {
      get => NwRuleset.Classes.ElementAtOrDefault(featInfo.m_nMinLevelClass);
    }

    /// <summary>
    /// The minimum spell level a spellcasting character must be able to cast in order to select this feat.<br/>
    /// To determine eligibility, a check is made in classes.2da, examining the "SpellGainTable" and either the "SpellKnownTable" (for bards and sorcerers) or the "PrimaryAbil" (for other classes).
    /// </summary>
    public byte MinSpellLevel
    {
      get => featInfo.m_nMinSpellLevel;
    }

    /// <summary>
    /// Gets the name of this feat, as shown in the in-game skill window.
    /// </summary>
    public string Name
    {
      get => TlkTable.GetSimpleString((uint)featInfo.m_nNameStrref);
    }

    /// <summary>
    /// Gets all feats that need to be selected before this one may be chosen.
    /// </summary>
    public IEnumerable<NwFeat> RequiredFeatsAll
    {
      get
      {
        return featInfo.m_lstPrereqFeats.Select(FromFeatId).Where(feat => feat != null);
      }
    }

    /// <summary>
    /// Gets feats that need to be selected before this one may be chosen.<br/>
    /// Unlike <see cref="RequiredFeatsAll"/>, the creature only needs one of these feats to be able to take this feat.
    /// </summary>
    public IReadOnlyList<NwFeat> RequiredFeatsSome
    {
      get
      {
        return featInfo.m_lstOrPrereqFeats.Select(FromFeatId).Where(feat => feat != null).ToImmutableList();
      }
    }

    /// <summary>
    /// Gets the first required skill the character must have to be able to select this feat.
    /// </summary>
    public NwSkill RequiredSkill1
    {
      get => NwSkill.FromSkillId(featInfo.m_nRequiredSkill);
    }

    /// <summary>
    /// Gets the number of skill ranks needed for <see cref="RequiredSkill1"/>.
    /// </summary>
    public ushort RequiredSkill1MinRanks
    {
      get => featInfo.m_nMinRequiredSkillRank;
    }

    /// <summary>
    /// Gets the second required skill the character must have to be able to select this feat.
    /// </summary>
    public NwSkill RequiredSkill2
    {
      get => NwSkill.FromSkillId(featInfo.m_nRequiredSkill2);
    }

    /// <summary>
    /// Gets the number of skill ranks needed for <see cref="RequiredSkill2"/>.
    /// </summary>
    public ushort RequiredSkill2MinRanks
    {
      get => featInfo.m_nMinRequiredSkillRank;
    }

    /// <summary>
    /// Gets if this feat requires a character action.<br/>
    /// If this is true and the character uses this feat, it is added to the character's action queue, instead of running instantly.
    /// </summary>
    public bool RequiresAction
    {
      get => featInfo.m_bRequiresAction.ToBool();
    }

    /// <summary>
    /// Gets if only epic characters can choose this feat.
    /// </summary>
    public bool RequiresEpic
    {
      get => featInfo.m_bRequiresEpic.ToBool();
    }

    /// <summary>
    /// The Spell associated with this feat.
    /// </summary>
    public NwSpell Spell
    {
      get => NwSpell.FromSpellId(featInfo.m_nSpellId);
    }

    /// <summary>
    /// Gets the feat which follows this feat. For example, the Disarm feat has Improved Disarm as a successor.
    /// </summary>
    public NwFeat SuccessorFeat
    {
      get => FromFeatId(featInfo.m_nSuccessor);
    }

    /// <summary>
    /// This determines how the AI treats this feat. It is an ID value in categories.2da.
    /// </summary>
    public TalentCategory TalentCategory
    {
      get => (TalentCategory)featInfo.m_nTalentCategory;
    }

    /// <summary>
    /// To do with the functions around Talents. The "Level" of the feat when we are searching for feats we want to not use if the enemy is of a certain CR.<br/>
    /// Just as a reference spells.2da entries just double the spell level (so enemy is CR5, we cast level 1 or 2 spells, but not level 3 which doubles to 6).
    /// </summary>
    public int TalentMaxCR
    {
      get => featInfo.m_nTalentMaxCR;
    }

    /// <summary>
    /// Gets whether this feat targets the character using the feat (so when using it, it doesn't pop up a selection for who to target).<br/>
    /// Overrides spells.2da targeting options for PCs.
    /// </summary>
    public bool TargetSelf
    {
      get => featInfo.m_bTargetSelf.ToBool();
    }

    /// <summary>
    /// Gets the number of uses per day that this feat can be used.
    /// </summary>
    public byte UsesPerDay
    {
      get => featInfo.m_nUsesPerDay;
    }

    /// <summary>
    /// Resolves a <see cref="NwFeat"/> from a feat id.
    /// </summary>
    /// <param name="featId">The id of the feat to resolve.</param>
    /// <returns>The associated <see cref="NwFeat"/> instance. Null if the feat id is invalid.</returns>
    public static NwFeat FromFeatId(int featId)
    {
      return NwRuleset.Feats.ElementAtOrDefault(featId);
    }

    /// <summary>
    /// Resolves a <see cref="NwFeat"/> from a <see cref="Anvil.API.Feat"/>.
    /// </summary>
    /// <param name="featType">The feat type to resolve.</param>
    /// <returns>The associated <see cref="NwFeat"/> instance. Null if the feat type is invalid.</returns>
    public static NwFeat FromFeatType(Feat featType)
    {
      return NwRuleset.Feats.ElementAtOrDefault((int)featType);
    }

    /// <summary>
    /// Gets the minimum ability score a character must have to select this feat.
    /// </summary>
    /// <param name="ability">The ability to query.</param>
    /// <returns>The ability score requirement.</returns>
    public byte GetRequiredAbilityScore(Ability ability)
    {
      return ability switch
      {
        Ability.Strength => featInfo.m_nMinSTR,
        Ability.Dexterity => featInfo.m_nMinDEX,
        Ability.Constitution => featInfo.m_nMinCON,
        Ability.Intelligence => featInfo.m_nMinINT,
        Ability.Wisdom => featInfo.m_nMinWIS,
        Ability.Charisma => featInfo.m_nMinCHA,
        _ => 0,
      };
    }

    private static NwFeat FromFeatId(ushort featId)
    {
      return NwRuleset.Feats.ElementAtOrDefault(featId);
    }
  }
}
