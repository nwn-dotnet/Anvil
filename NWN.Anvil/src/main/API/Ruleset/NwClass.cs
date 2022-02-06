using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API
{
  /// <summary>
  /// A creature/character class definition.
  /// </summary>
  public sealed unsafe class NwClass
  {
    private readonly CNWClass classInfo;

    internal NwClass(byte classId, CNWClass classInfo)
    {
      Id = classId;
      this.classInfo = classInfo;
    }

    /// <summary>
    /// Gets a list containing the ability score progression granted by this class (e.g. Dragon Disciple).<br/>
    /// The list is 0-indexed, with level 1 starting at element 0.
    /// </summary>
    public IReadOnlyList<ClassAbilityGainList> AbilityGainTable
    {
      get
      {
        TwoDimNativeArray<sbyte> array = classInfo.m_lstAbilityGainTable;
        ClassAbilityGainList[] retVal = new ClassAbilityGainList[array.Length];

        for (int i = 0; i < retVal.Length; i++)
        {
          retVal[i] = new ClassAbilityGainList(array[i].ToArray());
        }

        return retVal;
      }
    }

    /// <summary>
    /// If positive, this specifies the number of levels in this class that together add one level to an arcane class when determining the spell slots based on class level.
    /// </summary>
    public int ArcaneSpellUsePerDayLevel => classInfo.m_nArcSpellUsePerDayLevel;

    /// <summary>
    /// Gets a list containing the base attack bonus progression for this class.<br/>
    /// The list is 0-indexed, with level 1 starting at element 0.
    /// </summary>
    /// <example>
    /// byte babLevel15 = nwClass.AttackBonusTable[14];
    /// </example>
    public IReadOnlyList<byte> AttackBonusTable => classInfo.m_lstBaseAttackBonus.ToArray();

    /// <summary>
    /// Gets a list containing the bonus feat progression for this class (e.g. Fighters).<br/>
    /// The list is 0-indexed, with level 1 starting at element 0.
    /// </summary>
    public IReadOnlyList<byte> BonusFeatsTable => classInfo.m_lstBonusFeatsTable.ToArray();

    /// <summary>
    /// Gets if this class can spontaneously cast certain domain spells (e.g. Cleric).
    /// </summary>
    public bool CanCastSpontaneously => classInfo.m_bCanCastSpontaneously.ToBool();

    /// <summary>
    /// Gets if this class can learn spells from reading scrolls (e.g. Wizards)
    /// </summary>
    public bool CanLearnFromScrolls => classInfo.m_bCanLearnFromScrolls.ToBool();

    /// <summary>
    /// Gets the Caster Level multiplier for this class.
    /// </summary>
    public float CasterLevelMultiplier => classInfo.m_fCasterLevelMultiplier;

    /// <summary>
    /// Gets the associated <see cref="Id"/> for this class.
    /// </summary>
    public ClassType ClassType => (ClassType)Id;

    /// <summary>
    /// Gets the description name of this class.
    /// </summary>
    public StrRef Description => new StrRef(classInfo.m_nDescription);

    /// <summary>
    /// If positive, this specifies the number of levels in this class that together add one level to a divine class when determining the spell slots based on class level.
    /// </summary>
    public int DivineSpellUsePerDayLevel => classInfo.m_nDivSpellUsePerDayLevel;

    /// <summary>
    /// Gets a list containing the CR progression for this class.<br/>
    /// This value is used for purposes of encounter challenge rating calculations.<br/>
    /// The list is 0-indexed, with level 1 starting at element 0.
    /// </summary>
    public IReadOnlyList<byte> EffectiveCRTable => classInfo.m_pnEffectiveCRForLevel.ToArray();

    /// <summary>
    /// Gets the maximum amount of levels that can be taken into this class pre-epic (before level 21).
    /// </summary>
    public byte EpicLevel => classInfo.m_nEpicLevel;

    /// <summary>
    /// Gets a list of the feats associated with this class.
    /// </summary>
    public IReadOnlyList<ClassFeat> Feats
    {
      get
      {
        CNWClass_FeatArray array = CNWClass_FeatArray.FromPointer(classInfo.m_lstFeatTable);
        ClassFeat[] retVal = new ClassFeat[classInfo.m_nNumFeats];

        for (int i = 0; i < retVal.Length; i++)
        {
          retVal[i] = new ClassFeat(array[i]);
        }

        return retVal;
      }
    }

    /// <summary>
    /// Gets if this class is subject to the effects of arcane spell failure.
    /// </summary>
    public bool HasArcaneSpellFailure => classInfo.m_bSuffersArcaneSpellFailure.ToBool();

    /// <summary>
    /// Gets if this class can choose divine domains as a part of their class (e.g. Clerics)
    /// </summary>
    public bool HasDomains => classInfo.m_bHasDomains.ToBool();

    /// <summary>
    /// Gets if this class gains spells by memorization (e.g. Wizards)
    /// </summary>
    public bool HasMemorizedSpells => classInfo.m_bNeedsToMemorizeSpells.ToBool();

    /// <summary>
    /// Gets if this class counts towards the muliclass penalty for experience.
    /// </summary>
    public bool HasMulticlassPenalty => classInfo.m_bXPPenalty.ToBool();

    /// <summary>
    /// Gets if this class can choose a spell specialization/school as a part of their class (e.g. Wizards)
    /// </summary>
    public bool HasSpecialization => classInfo.m_bHasSpecialization.ToBool();

    /// <summary>
    /// Gets what size of die is used to roll hit points on level-up. (4, 6, 8, 10, 12, etc.)
    /// </summary>
    public byte HitDie => classInfo.m_nHitDie;

    /// <summary>
    /// Gets the ResRef specifying the icon displayed in the game for this class
    /// </summary>
    public string IconResRef => classInfo.m_sIcon.ToString();

    /// <summary>
    /// Gets the id of this class.
    /// </summary>
    public byte Id { get; }

    /// <summary>
    /// Gets if the alignments specified in <see cref="Restrictions"/> and <see cref="RestrictionTypes"/>
    /// are the only alignments allowed to gain levels in this class (true),
    /// rather than being the alignments prohibited from gaining them (false).
    /// </summary>
    public bool InvertRestrictions => classInfo.m_nClassRestrictionsInversed.ToBool();

    /// <summary>
    /// Gets if this class is considered an arcane caster. False indicates that the caster is considered a divine caster.
    /// </summary>
    public bool IsArcaneCaster => classInfo.m_bIsArcane.ToBool();

    /// <summary>
    /// Gets if this class may be selected by a player.
    /// </summary>
    public bool IsPlayerClass => classInfo.m_bIsPlayerClass.ToBool();

    /// <summary>
    /// Gets if this class is restricted to selecting spells from a spell book (e.g. Wizards)
    /// </summary>
    public bool IsSpellbookRestricted => classInfo.m_bSpellbookRestricted.ToBool();

    /// <summary>
    /// Gets if this class is a s
    /// </summary>
    public bool IsSpellCaster => classInfo.m_bIsSpellCasterClass.ToBool();

    /// <summary>
    /// Gets the maximum amount of levels that can be taken into this class.<br/>
    /// 0 means there is no limit.
    /// </summary>
    public byte MaxLevel => classInfo.m_nMaxLevel;

    /// <summary>
    /// Gets the minimum level required to receive the associate for the class.
    /// </summary>
    public byte MinAssociateLevel => classInfo.m_nMinAssociateLevel;

    /// <summary>
    /// Gets the minimum level required before this class may cast spells. (e.g. Paladin/Ranger)
    /// </summary>
    public int MinCastingLevel => classInfo.m_nMinCastingLevel;

    /// <summary>
    /// Gets the name of this class as shown on the character sheet.
    /// </summary>
    public StrRef Name => new StrRef(classInfo.m_nName);

    /// <summary>
    /// Gets the name of this class, in lowercase.
    /// </summary>
    public StrRef NameLower => new StrRef(classInfo.m_nNameLower);

    /// <summary>
    /// Gets the name of this class, in plural form.
    /// </summary>
    public StrRef NamePlural => new StrRef(classInfo.m_nNamePlural);

    /// <summary>
    /// Gets a list containing the natural AC progression granted by this class (e.g. Pale Masters).<br/>
    /// The list is 0-indexed, with level 1 starting at element 0.
    /// </summary>
    public IReadOnlyList<sbyte> NaturalACGainTable => classInfo.m_lstNaturalACGainTable.ToArray();

    /// <summary>
    /// Gets the associated packages.2da row index for this class.
    /// </summary>
    public uint PackageIndex => classInfo.m_nDefaultPackage;

    /// <summary>
    /// Gets the ResRef of the 2da table defining the prerequisites for this class (making this class a prestige class).
    /// </summary>
    public string PreReqTable => classInfo.m_sPreReqTable.ToString();

    /// <summary>
    /// Gets the primary ability score for this class.<br/>
    /// Not used for spellcasting. See <see cref="SpellCastingAbility"/>.
    /// </summary>
    public Ability PrimaryAbility => (Ability)classInfo.m_nPrimaryAbility;

    /// <summary>
    /// Gets the alignment restrictions that must be met to take this class.<br/>
    /// This acts as a black/whitelist depending on <see cref="RestrictionTypes"/> and <see cref="InvertRestrictions"/>.
    /// </summary>
    public ClassRestrictions Restrictions => (ClassRestrictions)classInfo.m_nClassRestrictions;

    /// <summary>
    /// Gets the type of restrictions that <see cref="Restrictions"/> applies to.
    /// </summary>
    public ClassRestrictionTypes RestrictionTypes => (ClassRestrictionTypes)classInfo.m_nClassRestrictionType;

    /// <summary>
    /// Gets the number of skill points gained per level from this class before modifiers.
    /// </summary>
    public byte SkillPointBase => classInfo.m_nSkillPointBase;

    /// <summary>
    /// Gets the list of skills available to this class.
    /// </summary>
    public IReadOnlyList<ClassSkill> Skills
    {
      get
      {
        CNWClass_SkillArray array = CNWClass_SkillArray.FromPointer(classInfo.m_lstSkillTable);
        ClassSkill[] retVal = new ClassSkill[classInfo.m_nNumSkills];

        for (int i = 0; i < retVal.Length; i++)
        {
          retVal[i] = new ClassSkill(array[i]);
        }

        return retVal;
      }
    }

    /// <summary>
    /// Gets the ability score used for spell casting - save DCs, extra spell slots.
    /// </summary>
    public Ability SpellCastingAbility => (Ability)classInfo.m_nSpellcastingAbility;

    /// <summary>
    /// Gets a list containing the spell slot progression for this class.<br/>
    /// The list is 0-indexed, with level 1 starting at element 0.
    /// </summary>
    public IReadOnlyList<ClassSpellGainList> SpellGainTable
    {
      get
      {
        NativeArray<byte> spellsPerLevel = classInfo.m_lstSpellLevelsPerLevel;
        ClassSpellGainList[] retVal = new ClassSpellGainList[spellsPerLevel.Length];
        for (int i = 0; i < retVal.Length; i++)
        {
          retVal[i] = new ClassSpellGainList(new NativeArray<byte>(classInfo.m_lstSpellGainTable[i], spellsPerLevel[i]));
        }

        return retVal;
      }
    }

    /// <summary>
    /// Gets a list containing the known spell progression for this class.<br/>
    /// The list is 0-indexed, with level 1 starting at element 0.
    /// </summary>
    public IReadOnlyList<ClassSpellGainList> SpellKnownTable
    {
      get
      {
        NativeArray<byte> spellsPerLevel = classInfo.m_lstSpellLevelsPerLevel;
        ClassSpellGainList[] retVal = new ClassSpellGainList[spellsPerLevel.Length];
        for (int i = 0; i < retVal.Length; i++)
        {
          retVal[i] = new ClassSpellGainList(new NativeArray<byte>(classInfo.m_lstSpellKnownTable[i], spellsPerLevel[i]));
        }

        return retVal;
      }
    }

    /// <summary>
    /// Gets the name of the column that this class references in spells.2da
    /// </summary>
    public string SpellTableColumn => classInfo.m_sSpellsTableColumn.ToString();

    /// <summary>
    /// Resolves a <see cref="NwClass"/> from a class id.
    /// </summary>
    /// <param name="classId">The id of the class to resolve.</param>
    /// <returns>The associated <see cref="NwClass"/> instance. Null if the class id is invalid.</returns>
    public static NwClass FromClassId(int classId)
    {
      return NwRuleset.Classes.ElementAtOrDefault(classId);
    }

    /// <summary>
    /// Resolves a <see cref="NwClass"/> from a <see cref="Anvil.API.ClassType"/>.
    /// </summary>
    /// <param name="classType">The class type to resolve.</param>
    /// <returns>The associated <see cref="NwClass"/> instance. Null if the class type is invalid.</returns>
    public static NwClass FromClassType(ClassType classType)
    {
      return NwRuleset.Classes.ElementAtOrDefault((int)classType);
    }

    public static implicit operator NwClass(ClassType classType)
    {
      return NwRuleset.Classes.ElementAtOrDefault((int)classType);
    }

    /// <summary>
    /// Gets the ability score for the specified ability that is shown during character creation.
    /// </summary>
    /// <param name="ability">The ability score to query.</param>
    /// <returns>The ability score recommended during character creation.</returns>
    public byte GetRecommendedAbilityScore(Ability ability)
    {
      return classInfo.m_pnRecommendedAbilities[(int)ability];
    }

    /// <summary>
    /// Gets a list containing the saving throw progression granted by this class.<br/>
    /// The list is 0-indexed, with level 1 starting at element 0.
    /// </summary>
    /// <param name="savingThrow">The saving throw to query. <see cref="SavingThrow.All"/> is not supported.</param>
    /// <example>
    /// byte reflexSaveLevel15 = nwClass.GetSavingThrowTable(SavingThrow.Reflex)[14];
    /// </example>
    public IReadOnlyList<byte> GetSavingThrowTable(SavingThrow savingThrow)
    {
      return savingThrow switch
      {
        SavingThrow.Fortitude => classInfo.m_lstFortificationSaveThrowBonus.ToArray(),
        SavingThrow.Reflex => classInfo.m_lstReflexSaveThrowBonus.ToArray(),
        SavingThrow.Will => classInfo.m_lstWillSaveThrowBonus.ToArray(),
        _ => ImmutableArray<byte>.Empty,
      };
    }
  }
}
