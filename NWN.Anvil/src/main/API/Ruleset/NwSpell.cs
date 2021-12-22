using System;
using System.Collections.Generic;
using System.Linq;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API
{
  /// <summary>
  /// A spell definition.
  /// </summary>
  public sealed unsafe class NwSpell
  {
    [Inject]
    private static TlkTable TlkTable { get; set; }

    private readonly CNWSpell spellInfo;

    internal NwSpell(uint spellId, CNWSpell spellInfo)
    {
      Id = spellId;
      this.spellInfo = spellInfo;
    }

    /// <summary>
    /// Gets the type of MetaMagic that may be used with this spell.
    /// </summary>
    public MetaMagic AllowedMetaMagic => (MetaMagic)spellInfo.m_nAllowedMetamagic;

    /// <summary>
    /// Gets the unformatted message shown in the combat log when something casts this spell.
    /// </summary>
    public string AltMessage => TlkTable.GetSimpleString(spellInfo.m_nAltMessage);

    /// <summary>
    /// Gets the animation type used to cast this spell.
    /// </summary>
    public SpellCastAnimType CastAnim => (SpellCastAnimType)spellInfo.m_nCastAnimation;

    /// <summary>
    /// Gets the ResRef of the visual effect model shown at the ground of the creature when casting this spell.
    /// </summary>
    public string CastGroundVisual => spellInfo.m_resrefCastGroundVisual.ToString();

    /// <summary>
    /// Gets the ResRef of the visual effect model shown at the creature's hands when casting this spell.
    /// </summary>
    public string CastHandVisual => spellInfo.m_resrefCastHandVisual.ToString();

    /// <summary>
    /// Gets the ResRef of the visual effect model shown above the creature's head when casting this spell.
    /// </summary>
    public string CastHeadVisual => spellInfo.m_resrefCastHeadVisual.ToString();

    /// <summary>
    /// Gets the ResRef of the sound that plays when a creature casts this spell.
    /// </summary>
    public string CastSound => spellInfo.m_resrefCastSound.ToString();

    /// <summary>
    /// Gets the amount of time to cast the spell, once it has been successfully conjured.
    /// </summary>
    public TimeSpan CastTime => TimeSpan.FromMilliseconds(spellInfo.m_nCastTime);

    /// <summary>
    /// Gets the animation type used to conjure the spell.
    /// </summary>
    public SpellConjureAnimType ConjureAnim => (SpellConjureAnimType)spellInfo.m_nConjureAnimation;

    /// <summary>
    /// Gets the ResRef of the visual effect model shown at the ground of the creature when conjuring this spell.
    /// </summary>
    public string ConjureGroundVisual => spellInfo.m_resrefConjureGroundVisual.ToString();

    /// <summary>
    /// Gets the ResRef of the visual effect model shown at the creature's hands when conjuring this spell.
    /// </summary>
    public string ConjureHandVisual => spellInfo.m_resrefConjureHandVisual.ToString();

    /// <summary>
    /// Gets the ResRef of the visual effect model shown above the creature's head when conjuring this spell.
    /// </summary>
    public string ConjureHeadVisual => spellInfo.m_resrefConjureHeadVisual.ToString();

    /// <summary>
    /// Gets the ResRef of the sound that plays when a creature conjures this spell.
    /// </summary>
    public string ConjureSound => spellInfo.m_resrefConjureSoundVFX.ToString();

    /// <summary>
    /// Gets the amount of time needed to conjure the spell. This is the animation that plays before the spell is cast.
    /// </summary>
    public TimeSpan ConjureTime => TimeSpan.FromMilliseconds(spellInfo.m_nConjureTime);

    /// <summary>
    /// Gets a list of spells that may directly counter this spell.
    /// </summary>
    public IReadOnlyList<NwSpell> CounterSpells
    {
      get
      {
        NwSpell counter1 = FromSpellId((int)spellInfo.m_nCounterSpell1);
        NwSpell counter2 = FromSpellId((int)spellInfo.m_nCounterSpell2);
        List<NwSpell> retVal = new List<NwSpell>();

        if (counter1 != null)
        {
          retVal.Add(counter1);
        }

        if (counter2 != null)
        {
          retVal.Add(counter2);
        }

        return retVal;
      }
    }

    /// <summary>
    /// Gets the description of this spell.
    /// </summary>
    public string Description => TlkTable.GetSimpleString(spellInfo.m_strrefDesc);

    /// <summary>
    /// Gets the associated feat if this spell is linked to a feat.
    /// </summary>
    public NwFeat FeatReference => NwFeat.FromFeatId(spellInfo.m_nFeatId);

    /// <summary>
    /// Gets if this spell fires a projectile once successfully cast.
    /// </summary>
    public bool HasProjectile => spellInfo.m_bHasProjectile.ToBool();

    /// <summary>
    /// Gets the spell's icon ResRef.
    /// </summary>
    public string IconResRef => spellInfo.m_resrefIcon.ToString();

    /// <summary>
    /// Gets the ID of this skill.
    /// </summary>
    public uint Id { get; }

    /// <summary>
    /// Gets the name of the script invoked when the spell impacts a target.<br/>
    /// This script is only invoked when the spell was successfully cast.
    /// </summary>
    public string ImpactScript => spellInfo.m_sImpactScript.ToString();

    /// <summary>
    /// Gets the innate level of this spell.<br/>
    /// See <see cref="GetSpellLevelForClass"/> to get the spell level for a specific class.
    /// </summary>
    public byte InnateSpellLevel => spellInfo.m_nInnateLevel;

    /// <summary>
    /// Gets if this spell is considered hostile when cast on other creatures.
    /// </summary>
    public bool IsHostileSpell => spellInfo.m_bHostile.ToBool();

    /// <summary>
    /// Gets the parent spell if this spell is a subradial spell. The reverse of RadialSpells.
    /// </summary>
    public NwSpell MasterSpell => FromSpellId((int)spellInfo.m_nMasterSpell);

    /// <summary>
    /// Gets the name of this spell.
    /// </summary>
    public string Name => TlkTable.GetSimpleString((uint)spellInfo.m_strrefName);

    /// <summary>
    /// Gets the ResRef of the projectile model.
    /// </summary>
    public string ProjectileModel => spellInfo.m_resrefProjectile.ToString();

    /// <summary>
    /// Gets the orientation of this spell's projectile.
    /// </summary>
    public SpellProjectileOrientation ProjectileOrientation => (SpellProjectileOrientation)spellInfo.m_nProjectileOrientationType;

    /// <summary>
    /// Gets the type of path that this spell's projectile follows by default.
    /// </summary>
    public ProjectilePathType ProjectilePathType => (ProjectilePathType)spellInfo.m_nProjectileType;

    /// <summary>
    /// Gets the sound of this spell's projectile.
    /// </summary>
    public string ProjectileSound => spellInfo.m_resrefProjectileSound.ToString();

    /// <summary>
    /// Gets the spawn point of this spell's projectile.
    /// </summary>
    public SpellProjectileSpawnPoint ProjectileSpawnPoint => (SpellProjectileSpawnPoint)spellInfo.m_nProjectileSpawnPoint;

    /// <summary>
    /// Gets the sub-list of spells that appear in the spell's radial menu.
    /// </summary>
    public IReadOnlyList<NwSpell> RadialSpells
    {
      get
      {
        NwSpell[] spells = new NwSpell[spellInfo.m_nSubRadialSpellCount];
        for (byte i = 0; i < spells.Length; i++)
        {
          spells[i] = FromSpellId((int)spellInfo.m_pSubRadialSpell[i]);
        }

        return spells;
      }
    }

    /// <summary>
    /// Gets the range of this spell.
    /// </summary>
    public SpellRange Range
    {
      get
      {
        return spellInfo.m_sRange.ToString() switch
        {
          "P" => SpellRange.Personal,
          "T" => SpellRange.Touch,
          "S" => SpellRange.Short,
          "M" => SpellRange.Medium,
          "L" => SpellRange.Long,
          _ => SpellRange.Unknown,
        };
      }
    }

    /// <summary>
    /// Gets the spell components needed to cast this spell.
    /// </summary>
    public SpellComponents SpellComponents
    {
      get
      {
        return spellInfo.m_sComponent.ToString() switch
        {
          "V" => SpellComponents.Verbal,
          "S" => SpellComponents.Somatic,
          "VS" => SpellComponents.VerbalSomatic,
          _ => SpellComponents.None,
        };
      }
    }

    /// <summary>
    /// Gets the spell school for this spell.
    /// </summary>
    public SpellSchool SpellSchool => (SpellSchool)spellInfo.m_nSchool;

    /// <summary>
    /// Gets the associated <see cref="Spell"/> type for this spell.
    /// </summary>
    public Spell SpellType => (Spell)Id;

    /// <summary>
    /// Gets if this spell can be spontaneously cast (e.g. cleric heal spells).
    /// </summary>
    public bool SpontaneouslyCast => spellInfo.m_bSpontaneouslyCast.ToBool();

    /// <summary>
    /// Gets the talent category that this spell is assigned to.
    /// </summary>
    public TalentCategory TalentCategory => (TalentCategory)spellInfo.m_nTalentCategory;

    /// <summary>
    /// Gets the types of targets that this spell is valid for.
    /// </summary>
    public SpellTargetTypes TargetTypes => (SpellTargetTypes)spellInfo.m_nTargetType;

    /// <summary>
    /// Gets if this spell should trigger concentration checks.
    /// </summary>
    public bool UseConcentration => spellInfo.m_bUseConcentration.ToBool();

    /// <summary>
    /// Gets the UserType for this spell.
    /// </summary>
    public SpellUserType UserType => (SpellUserType)spellInfo.m_nUserType;

    /// <summary>
    /// Resolves a <see cref="NwSpell"/> from a spell id.
    /// </summary>
    /// <param name="spellId">The id of the spell to resolve.</param>
    /// <returns>The associated <see cref="NwSpell"/> instance. Null if the spell id is invalid.</returns>
    public static NwSpell FromSpellId(int spellId)
    {
      return NwRuleset.Spells.ElementAtOrDefault(spellId);
    }

    /// <summary>
    /// Resolves a <see cref="NwSpell"/> from a <see cref="Anvil.API.Spell"/>.
    /// </summary>
    /// <param name="spellType">The spell type to resolve.</param>
    /// <returns>The associated <see cref="NwSpell"/> instance. Null if the spell type is invalid.</returns>
    public static NwSpell FromSpellType(Spell spellType)
    {
      return NwRuleset.Spells.ElementAtOrDefault((int)spellType);
    }

    public static implicit operator NwSpell(Spell spellType)
    {
      return NwRuleset.Spells.ElementAtOrDefault((int)spellType);
    }

    /// <summary>
    /// Gets the chant/voice that is played when this spell is conjured.
    /// </summary>
    /// <param name="gender">The gender of the caster.</param>
    /// <returns>The ResRef of the sound to play for the specified gender.</returns>
    public string GetConjureSound(Gender gender)
    {
      return gender switch
      {
        Gender.Male => spellInfo.m_resrefConjureSoundMale.ToString(),
        Gender.Female => spellInfo.m_resrefConjureSoundFemale.ToString(),
        _ => null,
      };
    }

    /// <summary>
    /// Gets the spell class level for the specified class.
    /// </summary>
    /// <param name="nwClass">The class to query.</param>
    /// <returns>The spell level for the specified class.</returns>
    public byte GetSpellLevelForClass(NwClass nwClass)
    {
      return spellInfo.GetSpellLevel(nwClass.Id);
    }
  }
}
