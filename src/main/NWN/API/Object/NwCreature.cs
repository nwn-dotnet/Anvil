using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anvil.Internal;
using NWN.API.Constants;
using NWN.Core;
using NWN.Native.API;
using Ability = NWN.API.Constants.Ability;
using Action = NWN.API.Constants.Action;
using Alignment = NWN.API.Constants.Alignment;
using Animation = NWN.Native.API.Animation;
using AssociateType = NWN.API.Constants.AssociateType;
using ClassType = NWN.API.Constants.ClassType;
using CombatMode = NWN.API.Constants.CombatMode;
using CreatureSize = NWN.API.Constants.CreatureSize;
using Feat = NWN.API.Constants.Feat;
using ImmunityType = NWN.API.Constants.ImmunityType;
using InventorySlot = NWN.API.Constants.InventorySlot;
using MovementRate = NWN.API.Constants.MovementRate;
using ObjectType = NWN.Native.API.ObjectType;
using RacialType = NWN.API.Constants.RacialType;
using Skill = NWN.API.Constants.Skill;

namespace NWN.API
{
  [NativeObjectInfo(ObjectTypes.Creature, ObjectType.Creature)]
  public sealed partial class NwCreature : NwGameObject
  {
    private const byte QuickBarButtonCount = 36;

    internal readonly CNWSCreature Creature;

    private NwFaction faction;

    internal NwCreature(CNWSCreature creature) : base(creature)
    {
      Creature = creature;
      faction = new NwFaction(creature.GetFaction());
      Inventory = new Inventory(this, Creature.m_pcItemRepository);
    }

    public static implicit operator CNWSCreature(NwCreature creature)
    {
      return creature?.Creature;
    }

    public override Location Location
    {
      set
      {
        if (value.Area != Area)
        {
          Creature.AddToArea(value.Area, value.Position.X, value.Position.Y, value.Position.Z, true.ToInt());
        }
        else
        {
          Position = value.Position;
        }

        Rotation = value.Rotation;
      }
    }

    /// <summary>
    /// Gets or sets the faction of this object.
    /// </summary>
    public NwFaction Faction
    {
      get => faction;
      set
      {
        if (value == null)
        {
          throw new ArgumentNullException(nameof(Faction), "New faction must not be null.");
        }

        faction = value;
        faction.AddMember(this);
      }
    }

    /// <summary>
    /// Gets the player currently controlling this creature.<br/>
    /// If this creature is a possessed familiar or is DM possessed, this will return the player or DM controlling this creature.<br/>
    /// If this creature is a player creature (the creature a played logged in with), but the player is possessing another creature, this returns null.<br/>
    /// If no player is controlling this creature, this returns null.
    /// </summary>
    public NwPlayer ControllingPlayer
    {
      get => ObjectId.ToNwPlayer();
    }

    /// <summary>
    /// Gets the player that logged in with this creature.<br/>
    /// If this creature is a NPC or familiar, regardless of possession, this will return null.
    /// </summary>
    public NwPlayer LoginPlayer
    {
      get => ObjectId.ToNwPlayer(false);
    }

    /// <summary>
    /// Gets if this creature is currently being controlled by a player/DM.<br/>
    /// If this creature is a possessed familiar or is DM possessed, this will return true.<br/>
    /// If this creature is a player creature (the creature a played logged in with), but the player is possessing another creature, this returns false.<br/>
    /// If no player is controlling this creature, this returns false.
    /// </summary>
    public bool IsPlayerControlled
    {
      get => ControllingPlayer != null;
    }

    /// <summary>
    /// Gets if this creature is a player character/DM avatar.<br/>
    /// If this creature is a NPC or familiar, regardless of possession, this will return false.
    /// </summary>
    public bool IsLoginPlayerCharacter
    {
      get => LoginPlayer != null;
    }

    /// <summary>
    /// Gets or sets the name of this creature's deity.
    /// </summary>
    public string Deity
    {
      get => NWScript.GetDeity(this);
      set => NWScript.SetDeity(this, value);
    }

    /// <summary>
    /// Gets or sets the name of this creature's sub-race.
    /// </summary>
    public string SubRace
    {
      get => NWScript.GetSubRace(this);
      set => NWScript.SetSubRace(this, value);
    }

    /// <summary>
    /// Gets or sets this creature's currently set Phenotype (body type).
    /// </summary>
    public Phenotype Phenotype
    {
      get => (Phenotype)NWScript.GetPhenoType(this);
      set => NWScript.SetPhenoType((int)value, this);
    }

    /// <summary>
    /// Gets or sets the total experience points for this creature, taking/granting levels based on progression.
    /// </summary>
    public int Xp
    {
      get => NWScript.GetXP(this);
      set => NWScript.SetXP(this, value < 0 ? 0 : value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether this creature's action queue can be modified.
    /// </summary>
    public bool Commandable
    {
      get => NWScript.GetCommandable(this).ToBool();
      set => NWScript.SetCommandable(value.ToInt(), this);
    }

    /// <summary>
    /// Gets or sets a value indicating whether this creature will leave a lootable corpse on death.<br/>
    /// This flag must be set while the creature is alive. Players are not supported.
    /// </summary>
    public bool Lootable
    {
      get => NWScript.GetLootable(this).ToBool();
      set => NWScript.SetLootable(this, value.ToInt());
    }

    /// <summary>
    /// Gets or sets a value indicating whether this creature is immortal.<br/>
    /// An immortal creature still takes damage, but cannot be killed.
    /// </summary>
    public bool Immortal
    {
      get => NWScript.GetImmortal(this).ToBool();
      set => NWScript.SetImmortal(this, value.ToInt());
    }

    /// <summary>
    /// Gets or sets the current AI level that this creature is running at.<br/>
    /// <see cref="Constants.AiLevel.Default"/> is recommended for most creatures. Too many creatures at <see cref="Constants.AiLevel.Normal"/> or higher can cause performance degradation.
    /// </summary>
    public AiLevel AiLevel
    {
      get => (AiLevel)NWScript.GetAILevel(this);
      set => NWScript.SetAILevel(this, (int)value);
    }

    /// <summary>
    /// Gets or sets the sounds to use when this creature makes a step. By default, this is based on the creature's appearance.
    /// </summary>
    public FootstepType FootstepType
    {
      get => (FootstepType)NWScript.GetFootstepType(this);
      set => NWScript.SetFootstepType((int)value, this);
    }

    /// <summary>
    /// Gets or sets the sound set index for this creature.
    /// </summary>
    public ushort SoundSet
    {
      get => Creature.m_nSoundSet;
      set => Creature.m_nSoundSet = value;
    }

    /// <summary>
    /// Gets or sets the base AC for this creature.
    /// </summary>
    public sbyte BaseAC
    {
      get => (sbyte)Creature.m_pStats.m_nACNaturalBase;
      set => Creature.m_pStats.m_nACNaturalBase = (char)value;
    }

    public sbyte ArmorCheckPenalty
    {
      get => (sbyte)Creature.m_pStats.m_nArmorCheckPenalty;
    }

    public sbyte ShieldCheckPenalty
    {
      get => (sbyte)Creature.m_pStats.m_nShieldCheckPenalty;
    }

    public byte BaseArmorArcaneSpellFailure
    {
      get => Creature.m_pStats.m_nBaseArmorArcaneSpellFailure;
    }

    public byte BaseShieldArcaneSpellFailure
    {
      get => Creature.m_pStats.m_nBaseShieldArcaneSpellFailure;
    }

    /// <summary>
    /// Gets a value indicating whether this creature is flat footed.
    /// </summary>
    public bool FlatFooted
    {
      get => Creature.GetFlatFooted().ToBool();
    }

    /// <summary>
    /// Gets a value indicating whether this creature is a dead NPC, dead PC, or dying PC.
    /// </summary>
    public bool IsDead
    {
      get => NWScript.GetIsDead(this).ToBool();
    }

    /// <summary>
    /// Gets a value indicating whether this creature is in combat.
    /// </summary>
    public bool IsInCombat
    {
      get => NWScript.GetIsInCombat(this).ToBool();
    }

    /// <summary>
    /// Gets the racial type of this creature.
    /// </summary>
    public RacialType RacialType
    {
      get => (RacialType)NWScript.GetRacialType(this);
    }

    /// <summary>
    /// Gets or sets the gender of this creature.
    /// </summary>
    public Gender Gender
    {
      get => (Gender)Creature.m_pStats.m_nGender;
      set
      {
        Creature.m_pStats.m_nGender = (byte)value;
        Creature.m_cAppearance.m_nGender = (byte)value;
      }
    }

    /// <summary>
    /// Gets the associate type of this creature, otherwise returns <see cref="NWN.API.Constants.AssociateType.None"/> if this creature is not an associate of anyone.
    /// </summary>
    public AssociateType AssociateType
    {
      get => (AssociateType)NWScript.GetAssociateType(this);
    }

    /// <summary>
    /// Gets a value indicating whether this creature is currently resting.
    /// </summary>
    public bool IsResting
    {
      get => NWScript.GetIsResting(this).ToBool();
    }

    /// <summary>
    /// Gets a value indicating whether this creature is a playable racial type.
    /// </summary>
    public bool IsPlayableRace
    {
      get => NWScript.GetIsPlayableRacialType(this).ToBool();
    }

    /// <summary>
    /// Gets the total weight of this creature, in pounds.
    /// </summary>
    public decimal TotalWeight
    {
      get => NWScript.GetWeight(this) * 0.1m;
    }

    /// <summary>
    /// Gets this creature's armour class.
    /// </summary>
    public int AC
    {
      get => NWScript.GetAC(this);
    }

    /// <summary>
    /// Gets or sets the Base Attack Bonus for this creature.
    /// </summary>
    public byte BaseAttackBonus
    {
      get => Creature.m_pStats.m_nBaseAttackBonus;
      set => Creature.m_pStats.m_nBaseAttackBonus = value;
    }

    /// <summary>
    /// Gets this creature's age, in years.
    /// </summary>
    public int Age
    {
      get => NWScript.GetAge(this);
    }

    /// <summary>
    /// Gets the size of this creature.
    /// </summary>
    public CreatureSize Size
    {
      get => (CreatureSize)NWScript.GetCreatureSize(this);
    }

    /// <summary>
    /// Gets the calculated challenge rating for this creature.
    /// </summary>
    public float ChallengeRating
    {
      get => NWScript.GetChallengeRating(this);
    }

    /// <summary>
    /// Gets the original name of this creature.
    /// </summary>
    public string OriginalName
    {
      get => $"{OriginalFirstName} {OriginalLastName}";
    }

    /// <summary>
    /// Gets the original first name of this creature.
    /// </summary>
    public string OriginalFirstName
    {
      get => Creature.m_pStats.m_lsFirstName.ExtractLocString();
    }

    /// <summary>
    /// Gets the original last name of this creature.
    /// </summary>
    public string OriginalLastName
    {
      get => Creature.m_pStats.m_lsLastName.ExtractLocString();
    }

    /// <summary>
    /// Gets or sets the movement rate of this creature.
    /// </summary>
    public MovementRate MovementRate
    {
      get => (MovementRate)Creature.m_pStats.m_nMovementRate;
      set
      {
        if (MovementRate == MovementRate.Immobile)
        {
          Creature.m_nAIState |= (ushort)AIState.CanUseLegs;
        }

        Creature.m_pStats.SetMovementRate((int)value);
      }
    }

    /// <summary>
    /// Gets or sets the creature's current movement rate factor.<br/>
    /// Base movement rate factor is 1.0.
    /// </summary>
    public float MovementRateFactor
    {
      get => Creature.GetMovementRateFactor();
      set => Creature.SetMovementRateFactor(value);
    }

    /// <summary>
    /// Gets the creature's current movement type.
    /// </summary>
    public MovementType MovementType
    {
      get
      {
        return (Animation)Creature.m_nAnimation switch
        {
          Animation.Walking => MovementType.Walk,
          Animation.WalkingForwardLeft => MovementType.Walk,
          Animation.WalkingForwardRight => MovementType.Walk,
          Animation.WalkingBackwards => MovementType.WalkBackwards,
          Animation.Running => MovementType.Run,
          Animation.RunningForwardLeft => MovementType.Run,
          Animation.RunningForwardRight => MovementType.Run,
          Animation.WalkingLeft => MovementType.Sidestep,
          Animation.WalkingRight => MovementType.Sidestep,
          _ => MovementType.Stationary,
        };
      }
    }

    /// <summary>
    /// Gets this creature's Law/Chaos Alignment.
    /// </summary>
    public Alignment LawChaosAlignment
    {
      get => (Alignment)NWScript.GetAlignmentLawChaos(this);
    }

    /// <summary>
    /// Gets this creature's Good/Evil Alignment.
    /// </summary>
    public Alignment GoodEvilAlignment
    {
      get => (Alignment)NWScript.GetAlignmentGoodEvil(this);
    }

    /// <summary>
    /// Gets a value indicating whether this creature is currently possessed by a DM avatar.
    /// </summary>
    public bool IsDMPossessed
    {
      get => NWScript.GetIsDMPossessed(this).ToBool();
    }

    /// <summary>
    /// Gets a value indicating whether this creature is a familiar currently possessed by a master.
    /// </summary>
    public bool IsPossessedFamiliar
    {
      get => NWScript.GetIsPossessedFamiliar(this).ToBool();
    }

    /// <summary>
    /// Gets the last command issued to this creature.
    /// </summary>
    public AssociateCommand LastCommandFromMaster
    {
      get => (AssociateCommand)NWScript.GetLastAssociateCommand(this);
    }

    /// <summary>
    /// Gets the possessor of this creature. This can be the master of a familiar, or the DM for a DM controlled creature.
    /// </summary>
    public NwCreature Master
    {
      get => NWScript.GetMaster(this).ToNwObject<NwCreature>();
    }

    /// <summary>
    /// Gets this creature's current attack target.
    /// </summary>
    public NwGameObject AttackTarget
    {
      get => NWScript.GetAttackTarget(this).ToNwObject<NwGameObject>();
    }

    /// <summary>
    /// Gets the current action that this creature is executing.
    /// </summary>
    public Action CurrentAction
    {
      get => (Action)NWScript.GetCurrentAction(this);
    }

    /// <summary>
    /// Gets the caster level of the last spell this creature casted.
    /// </summary>
    public int LastSpellCasterLevel
    {
      get => NWScript.GetCasterLevel(this);
    }

    /// <summary>
    /// Gets the Hit Dice/Level of this creature.
    /// </summary>
    public int Level
    {
      get => NWScript.GetHitDice(this);
    }

    /// <summary>
    /// Gets a value indicating whether this creature can be disarmed (checks disarm flag on creature, and if the creature actually has a weapon equipped in their right hand that is droppable).
    /// </summary>
    public bool Disarmable
    {
      get => NWScript.GetIsCreatureDisarmable(this).ToBool();
    }

    /// <summary>
    /// Gets or sets the wing type of this creature.
    /// </summary>
    public CreatureWingType WingType
    {
      get => (CreatureWingType)NWScript.GetCreatureWingType(this);
      set => NWScript.SetCreatureWingType((int)value, this);
    }

    /// <summary>
    /// Gets or sets the tail type of this creature.
    /// </summary>
    public CreatureTailType TailType
    {
      get => (CreatureTailType)NWScript.GetCreatureTailType(this);
      set => NWScript.SetCreatureTailType((int)value, this);
    }

    /// <summary>
    /// Gets the creature's current Combat Mode.<br/>
    /// Can be used in the <see cref="OnCombatModeToggle"/> event to determine which combat mode is being toggled off.
    /// </summary>
    public CombatMode CombatMode
    {
      get => (CombatMode)Creature.m_nCombatMode;
    }

    /// <summary>
    /// Gets this creature's animal companion creature type.
    /// </summary>
    public AnimalCompanionCreatureType AnimalCompanionType
    {
      get => (AnimalCompanionCreatureType)NWScript.GetAnimalCompanionCreatureType(this);
    }

    /// <summary>
    /// Gets or sets a value indicating whether this creature will auto-explore the minimap as it walks around.
    /// </summary>
    public bool ExploresMinimap
    {
      get => NWScript.GetCreatureExploresMinimap(this).ToBool();
      set => NWScript.SetCreatureExploresMinimap(this, value.ToInt());
    }

    /// <summary>
    /// Gets this creature's default level up package.
    /// </summary>
    public PackageType StartingPackage
    {
      get => (PackageType)NWScript.GetCreatureStartingPackage(this);
    }

    /// <summary>
    /// Gets the type of familiar that this creature can summon.
    /// </summary>
    public FamiliarCreatureType FamiliarType
    {
      get => (FamiliarCreatureType)NWScript.GetFamiliarCreatureType(this);
    }

    /// <summary>
    /// Gets a value indicating whether this creature was spawned from an encounter.
    /// </summary>
    public bool IsEncounterCreature
    {
      get => NWScript.GetIsEncounterCreature(this).ToBool();
    }

    /// <summary>
    /// Gets this creature's animal companion name.
    /// </summary>
    public string AnimalCompanionName
    {
      get => NWScript.GetAnimalCompanionName(this);
    }

    /// <summary>
    /// Gets the arcane spell failure factor for this creature.
    /// </summary>
    public int ArcaneSpellFailure
    {
      get => NWScript.GetArcaneSpellFailure(this);
    }

    /// <summary>
    /// Gets a value indicating whether this creature is currently in Defensive Casting Mode.
    /// </summary>
    public bool DefensiveCastingModeActive
    {
      get => NWScript.GetDefensiveCastingMode(this).ToBool();
    }

    /// <summary>
    /// Gets a value indicating whether this creature is currently in Detect Mode.
    /// </summary>
    public bool DetectModeActive
    {
      get => NWScript.GetDetectMode(this).ToBool();
    }

    /// <summary>
    /// Gets this creature's familiar name.
    /// </summary>
    public string FamiliarName
    {
      get => NWScript.GetFamiliarName(this);
    }

    /// <summary>
    /// Gets the attack mode used during this creature's last attack.
    /// </summary>
    public LastAttackMode LastAttackMode
    {
      get => (LastAttackMode)NWScript.GetLastAttackMode(this);
    }

    /// <summary>
    /// Gets the special attack type used in the last physical attack against this creature.
    /// </summary>
    public SpecialAttack LastSpecialAttackType
    {
      get => (SpecialAttack)NWScript.GetLastAttackType(this);
    }

    /// <summary>
    /// Gets the spell resistance of this creature.<br/>
    /// Returns 0 if this creature has no spell resistance.
    /// </summary>
    public int SpellResistance
    {
      get => NWScript.GetSpellResistance(this);
    }

    /// <summary>
    /// Gets a value indicating whether this creature is currently in stealth mode.
    /// </summary>
    public bool StealthModeActive
    {
      get => NWScript.GetStealthMode(this).ToBool();
    }

    /// <summary>
    /// Gets the number of hit dice worth of Turn Resistance this creature has.
    /// </summary>
    public int TurnResistanceHitDice
    {
      get => NWScript.GetTurnResistanceHD(this);
    }

    /// <summary>
    /// Gets the inventory of this creature.
    /// </summary>
    public Inventory Inventory { get; }

    /// <summary>
    /// Gets or sets the amount of gold carried by this creature.<br/>
    /// This property does not display feedback to the creature. See <see cref="GiveGold"/> and <see cref="TakeGold"/> for options that provide feedback.
    /// </summary>
    public uint Gold
    {
      get => Creature.m_nGold;
      set => Creature.m_nGold = value;
    }

    /// <summary>
    /// Gets or sets the corpse decay time for this creature.
    /// </summary>
    public TimeSpan CorpseDecayTime
    {
      get => TimeSpan.FromMilliseconds(Creature.m_nDecayTime);
      set => Creature.m_nDecayTime = (uint)value.TotalMilliseconds;
    }

    /// <summary>
    /// Sets the number of base attacks for this creature.<br/>
    /// The range of values accepted are from 1 to 6.<br/>
    /// @note Each successive attack per round suffers a -5 penalty.<br/>
    /// If the character has levels in Monk and fights with Unarmed Strike, each successive attack per round suffers a -3 penalty.<br/>
    /// These additional attacks are not shown on the player's character sheet.
    /// </summary>
    public int BaseAttackCount
    {
      set => NWScript.SetBaseAttackBonus(value, this);
    }

    /// <summary>
    /// Gets all henchmen associated with this creature.
    /// </summary>
    public IEnumerable<NwCreature> Henchmen
    {
      get
      {
        int i;
        uint current;
        const int henchmenType = (int)AssociateType.Henchman;

        for (i = 1, current = NWScript.GetAssociate(henchmenType, this, i); current != Invalid; i++, current = NWScript.GetAssociate(henchmenType, this, i))
        {
          yield return current.ToNwObject<NwCreature>();
        }
      }
    }

    /// <summary>
    /// Gets this creature's classes.
    /// </summary>
    public IReadOnlyList<ClassType> Classes
    {
      get
      {
        int classCount = Creature.m_pStats.m_nNumMultiClasses;
        List<ClassType> classes = new List<ClassType>(classCount);

        for (byte i = 0; i < classCount; i++)
        {
          ClassType classType = (ClassType)Creature.m_pStats.GetClass(i);
          if (classType == ClassType.Invalid)
          {
            break;
          }

          classes.Add(classType);
        }

        return classes.AsReadOnly();
      }
    }

    /// <summary>
    /// Gets this creature's classes, and associated class info.
    /// </summary>
    public IReadOnlyList<CreatureClassInfo> ClassInfo
    {
      get
      {
        int classCount = Creature.m_pStats.m_nNumMultiClasses;
        List<CreatureClassInfo> classes = new List<CreatureClassInfo>(classCount);

        for (byte i = 0; i < classCount; i++)
        {
          CNWSCreatureStats_ClassInfo classInfo = Creature.m_pStats.GetClassInfo(i);

          if (classInfo.m_nClass == (int)ClassType.Invalid)
          {
            break;
          }

          classes.Add(new CreatureClassInfo(classInfo));
        }

        return classes.AsReadOnly();
      }
    }

    /// <summary>
    /// Gets an enumerable containing information about this creature's levels (feats, skills, class taken, etc).
    /// </summary>
    public unsafe IReadOnlyList<CreatureLevelInfo> LevelInfo
    {
      get
      {
        int statCount = Creature.m_pStats.m_lstLevelStats.num;
        List<CreatureLevelInfo> retVal = new List<CreatureLevelInfo>(statCount);

        for (int i = 0; i < statCount; i++)
        {
          CNWLevelStats levelStats = CNWLevelStats.FromPointer(*Creature.m_pStats.m_lstLevelStats._OpIndex(i));
          retVal.Add(new CreatureLevelInfo(this, levelStats));
        }

        return retVal;
      }
    }

    /// <summary>
    /// Gets the feats known by this character.
    /// </summary>
    public unsafe IReadOnlyList<Feat> Feats
    {
      get
      {
        Feat[] feats = new Feat[FeatCount];

        for (int i = 0; i < feats.Length; i++)
        {
          feats[i] = (Feat)Creature.m_pStats.m_lstFeats.element[i];
        }

        return feats;
      }
    }

    /// <summary>
    /// Gets the number of feats known by this creature.
    /// </summary>
    public int FeatCount
    {
      get => Creature.m_pStats.m_lstFeats.num;
    }

    /// <summary>
    /// Gets all effects (permanent and temporary) that are active on this creature.
    /// </summary>
    public IEnumerable<Effect> ActiveEffects
    {
      get
      {
        for (Effect effect = NWScript.GetFirstEffect(this); NWScript.GetIsEffectValid(effect) == true.ToInt(); effect = NWScript.GetNextEffect(this))
        {
          yield return effect;
        }
      }
    }

    /// <summary>
    /// Gets or sets this creature's Chaos (0) - Lawful (100) alignment value.
    /// </summary>
    public int LawChaosValue
    {
      get => NWScript.GetLawChaosValue(this);
      set
      {
        int current = LawChaosValue;
        if (value == current)
        {
          return;
        }

        Alignment alignment = value < current ? Alignment.Chaotic : Alignment.Lawful;
        int shift = Math.Abs(value - current);
        NWScript.AdjustAlignment(this, (int)alignment, shift, false.ToInt());
      }
    }

    /// <summary>
    /// Gets or sets this creature's Evil (0) - Good (100) alignment value.
    /// </summary>
    public int GoodEvilValue
    {
      get => NWScript.GetGoodEvilValue(this);
      set
      {
        int current = GoodEvilValue;
        if (value == current)
        {
          return;
        }

        Alignment alignment = value < current ? Alignment.Evil : Alignment.Good;
        int shift = Math.Abs(value - current);
        NWScript.AdjustAlignment(this, (int)alignment, shift, false.ToInt());
      }
    }

    /// <summary>
    /// Forces this creature to join the specified standard faction. This will NOT work on players.
    /// </summary>
    /// <param name="newFaction">The NPCs new faction.</param>
    public void ChangeToStandardFaction(StandardFaction newFaction)
    {
      NWScript.ChangeToStandardFaction(this, (int)newFaction);
    }

    /// <summary>
    /// Gets whether this creature has a specific immunity.
    /// </summary>
    /// <param name="immunityType">The immunity type to check.</param>
    /// <param name="verses">If specified, the race and alignment of verses will be considered when determining immunities.</param>
    /// <returns>True if the creature has the specified immunity, otherwise false.</returns>
    public bool IsImmuneTo(ImmunityType immunityType, NwGameObject verses = null)
    {
      return NWScript.GetIsImmune(this, (int)immunityType, verses).ToBool();
    }

    /// <summary>
    /// Gives gold to this creature.
    /// </summary>
    /// <param name="amount">The amount of gold to give.</param>
    /// <param name="showFeedback">If true, shows "Acquired xgp" feedback to the creature.</param>
    public void GiveGold(int amount, bool showFeedback = true)
    {
      Creature.AddGold(amount, showFeedback.ToInt());
    }

    /// <summary>
    /// Takes gold away from this creature.
    /// </summary>
    /// <param name="amount">The amount of gold to take.</param>
    /// <param name="showFeedback">If true, shows "Lost xgp" feedback to the creature.</param>
    public void TakeGold(int amount, bool showFeedback = true)
    {
      Creature.RemoveGold(amount, showFeedback.ToInt());
    }

    /// <summary>
    /// Creates a creature at the specified location.
    /// </summary>
    /// <param name="template">The creature resref template from the toolset palette.</param>
    /// <param name="location">The location where this creature will spawn.</param>
    /// <param name="useAppearAnim">If true, plays EffectAppear when created.</param>
    /// <param name="newTag">The new tag to assign this creature. Leave uninitialized/as null to use the template's tag.</param>
    public static NwCreature Create(string template, Location location, bool useAppearAnim = false, string newTag = "")
    {
      return CreateInternal<NwCreature>(template, location, useAppearAnim, newTag);
    }

    /// <summary>
    /// Gets the item that is equipped in the specified inventory slot.
    /// </summary>
    /// <param name="slot">The inventory slot to check.</param>
    /// <returns>The item in the inventory slot, otherwise null if it is unpopulated.</returns>
    public NwItem GetItemInSlot(InventorySlot slot)
    {
      return NWScript.GetItemInSlot((int)slot, this).ToNwObject<NwItem>();
    }

    /// <summary>
    /// Attempts to perform a melee touch attack on target. This is not a creature action, and assumes that this creature is already within range of the target.
    /// </summary>
    /// <param name="target">The target of this touch attack.</param>
    public async Task<TouchAttackResult> TouchAttackMelee(NwGameObject target)
    {
      await WaitForObjectContext();
      return (TouchAttackResult)NWScript.TouchAttackMelee(target);
    }

    /// <summary>
    /// Attempts to perform a ranged touch attack on the target. This is not a creature action, and simply does a roll to see if the target was hit.
    /// </summary>
    /// <param name="target">The target of this touch attack.</param>
    /// <param name="displayFeedback">If true, displays combat feedback in the chat window.</param>
    public async Task<TouchAttackResult> TouchAttackRanged(NwGameObject target, bool displayFeedback)
    {
      await WaitForObjectContext();
      return (TouchAttackResult)NWScript.TouchAttackRanged(target, displayFeedback.ToInt());
    }

    /// <summary>
    /// Adjusts the alignment of this creature and associated party members by the specified value.<br/>
    /// Use the <see cref="LawChaosValue"/> and <see cref="GoodEvilValue"/> setters to only affect this creature.
    /// </summary>
    /// <param name="alignment">The alignment to shift towards.</param>
    /// <param name="shift">The amount of alignment shift.</param>
    public void AdjustPartyAlignment(Alignment alignment, int shift)
    {
      NWScript.AdjustAlignment(this, (int)alignment, shift);
    }

    /// <summary>
    /// Gets the specified ability score from this creature.
    /// </summary>
    /// <param name="ability">The type of ability.</param>
    /// <param name="baseOnly">If true, will return the creature's base ability score without bonuses or penalties.</param>
    public int GetAbilityScore(Ability ability, bool baseOnly = false)
    {
      return NWScript.GetAbilityScore(this, (int)ability, baseOnly.ToInt());
    }

    /// <summary>
    /// Gets this creature's ability modifier for the specified ability.
    /// </summary>
    /// <param name="ability">The ability to resolve.</param>
    /// <returns>An int representing the creature's ability modifier.</returns>
    public int GetAbilityModifier(Ability ability)
    {
      return NWScript.GetAbilityModifier((int)ability, this);
    }

    /// <summary>
    /// Gets the last target this creature tried to attack.
    /// </summary>
    public NwGameObject AttemptedAttackTarget
    {
      get => Creature.m_oidAttemptedAttackTarget.ToNwObject<NwGameObject>();
    }

    /// <summary>
    /// Gets the target this creature attempted to cast a spell at.
    /// </summary>
    public NwGameObject AttemptedSpellTarget
    {
      get => Creature.m_oidAttemptedSpellTarget.ToNwObject<NwGameObject>();
    }

    /// <summary>
    /// Gets the <see cref="CreatureClassInfo"/> associated with the specified class type.
    /// </summary>
    /// <param name="classType">The class type to query.</param>
    /// <returns>The <see cref="CreatureClassInfo"/> for the specified class, otherwise null if this creature does not have any levels in the class.</returns>
    public CreatureClassInfo GetClassInfo(ClassType classType)
    {
      return ClassInfo.FirstOrDefault(classInfo => classInfo.Type == classType);
    }

    /// <summary>
    /// Gets if this creature has the specified spell available to cast.
    /// </summary>
    /// <param name="spell">The spell to check.</param>
    /// <returns>True if this creature can immediately cast the spell.</returns>
    public bool HasSpellUse(Spell spell)
    {
      return NWScript.GetHasSpell((int)spell, this) > 0;
    }

    /// <summary>
    /// Gets the number of ranks this creature has in the specified skill.
    /// </summary>
    /// <param name="skill">The skill to check.</param>
    /// <param name="ranksOnly">If true, returns the base amount of skill ranks without any ability modifiers.</param>
    /// <returns>-1 if the creature does not have this skill, 0 if untrained, otherwise the number of skill ranks.</returns>
    public int GetSkillRank(Skill skill, bool ranksOnly = false)
    {
      return NWScript.GetSkillRank((int)skill, this, ranksOnly.ToInt());
    }

    /// <summary>
    /// Returns true if this creature has the skill specified, and is useable.
    /// </summary>
    /// <param name="skill">The skill to check.</param>
    /// <returns>True if the creature has this skill.</returns>
    public bool HasSkill(Skill skill)
    {
      return NWScript.GetHasSkill((int)skill, this).ToBool();
    }

    /// <summary>
    /// Returns true if 1d20 + skill rank is greater than, or equal to difficultyClass.
    /// </summary>
    /// <param name="skill">The type of skill check.</param>
    /// <param name="difficultyClass">The DC of this skill check.</param>
    public bool DoSkillCheck(Skill skill, int difficultyClass)
    {
      return NWScript.GetIsSkillSuccessful(this, (int)skill, difficultyClass).ToBool();
    }

    /// <summary>
    /// Returns true if this creature knows the specified <see cref="Constants.Feat"/>, and can use it.<br/>
    /// Use <see cref="KnowsFeat"/> to simply check if a creature knows <see cref="Constants.Feat"/>, but may or may not have uses remaining.
    /// </summary>
    public bool HasFeatPrepared(Feat feat)
    {
      return NWScript.GetHasFeat((int)feat, this).ToBool();
    }

    /// <summary>
    /// Determines whether this creature has the specified talent.
    /// </summary>
    /// <param name="talent">The talent to check.</param>
    /// <returns>True if this creature has talent, otherwise false.</returns>
    public bool HasTalent(Talent talent)
    {
      return NWScript.GetCreatureHasTalent(talent, this).ToBool();
    }

    /// <summary>
    /// The creature will generate a random location near its current location
    /// and pathfind to it. This repeats and never ends, which means it is necessary
    /// to call <see cref="NwObject.ClearActionQueue"/> in order to allow a creature to perform any other action
    /// once BeginRandomWalking has been called.
    /// </summary>
    public async Task ActionRandomWalk()
    {
      await WaitForObjectContext();
      NWScript.ActionRandomWalk();
    }

    /// <summary>
    /// Instructs this creature to start attacking the target using whichever weapon they currently have equipped.
    /// </summary>
    /// <param name="target">The target object to attack.</param>
    /// <param name="passive">If TRUE, the attacker will not move to attack the target. If we have a melee weapon equipped, we will just stand still.</param>
    public async Task ActionAttackTarget(NwGameObject target, bool passive = false)
    {
      await WaitForObjectContext();
      NWScript.ActionAttack(target, passive.ToInt());
    }

    /// <summary>
    /// Instructs this creature to walk/run to the specified target location.
    /// </summary>
    /// <param name="target">The location to move towards.</param>
    /// <param name="run">If true, the creature will run rather than walk.</param>
    public async Task ActionMoveTo(Location target, bool run = false)
    {
      await WaitForObjectContext();
      NWScript.ActionMoveToLocation(target, run.ToInt());
    }

    /// <summary>
    /// Instructs this creature to walk/run to the specified target object.
    /// </summary>
    /// <param name="target">The target object to move towards.</param>
    /// <param name="run">If true, the creature will run rather than walk.</param>
    /// <param name="range">The desired distance between the creature and the target object.</param>
    public async Task ActionMoveTo(NwObject target, bool run = false, float range = 1.0f)
    {
      await WaitForObjectContext();
      NWScript.ActionMoveToObject(target, run.ToInt(), range);
    }

    /// <summary>
    /// Instructs this creature to walk/run to the specified target location.
    /// </summary>
    /// <param name="target">The location to move towards.</param>
    /// <param name="run">If true, the creature will run rather than walk.</param>
    /// <param name="timeOut">The amount of time to search for a path before jumping to the location (Default: 30 seconds).</param>
    public async Task ActionForceMoveTo(Location target, bool run = false, TimeSpan? timeOut = null)
    {
      timeOut ??= TimeSpan.FromSeconds(30);
      await WaitForObjectContext();
      NWScript.ActionForceMoveToLocation(target, run.ToInt(), (float)timeOut.Value.TotalSeconds);
    }

    /// <summary>
    /// Instructs this creature to walk/run to the specified target object.
    /// </summary>
    /// <param name="target">The target object to move towards.</param>
    /// <param name="run">If true, the creature will run rather than walk.</param>
    /// <param name="range">The desired distance between the creature and the target object.</param>
    /// <param name="timeOut">The amount of time to search for a path before jumping to the object. (Default: 30 seconds).</param>
    public async Task ActionForceMoveTo(NwObject target, bool run = false, float range = 1.0f, TimeSpan? timeOut = null)
    {
      timeOut ??= TimeSpan.FromSeconds(30);
      await WaitForObjectContext();
      NWScript.ActionForceMoveToObject(target, run.ToInt(), range, (float)timeOut.Value.TotalSeconds);
    }

    /// <summary>
    /// Instructs this creature to move a certain distance away from the specified target.
    /// </summary>
    /// <param name="target">The target object this creature should move away from. If the target object is not in the same area as this creature, nothing will happen.</param>
    /// <param name="run">If set to true, the creature will run rather than walk.</param>
    /// <param name="range">How much distance this creature should put between themselves and the object.</param>
    public async Task ActionMoveAwayFrom(NwObject target, bool run, float range = 40.0f)
    {
      await WaitForObjectContext();
      NWScript.ActionMoveAwayFromObject(target, run.ToInt(), range);
    }

    /// <summary>
    /// Instructs this creature to move to a certain distance away from the specified location.
    /// </summary>
    /// <param name="location">The target location this creature should move away from. If the location is not in the same area as this creature, nothing will happen.</param>
    /// <param name="run">If set true, the creature will run rather than walk.</param>
    /// <param name="range">How much distance this creature should put between themselves and the location.</param>
    public async Task ActionMoveAwayFrom(Location location, bool run, float range = 40.0f)
    {
      await WaitForObjectContext();
      NWScript.ActionMoveAwayFromLocation(location, run.ToInt(), range);
    }

    /// <summary>
    /// Forces this creature to follow the specified target until <see cref="NwObject.ClearActionQueue"/> is called.
    /// </summary>
    /// <param name="target">The target to follow.</param>
    /// <param name="distance">The distance to follow the creature at.</param>
    public async Task ActionForceFollowObject(NwGameObject target, float distance)
    {
      await WaitForObjectContext();
      NWScript.ActionForceFollowObject(target, distance);
    }

    /// <summary>
    /// Begins the casting animation and spell fx for the specified spell, without any spell effects.
    /// </summary>
    /// <param name="spell">The spell to cast.</param>
    /// <param name="location">The target location for the fake spell to be cast at.</param>
    /// <param name="pathType">An optional path type for this spell to use.</param>
    public async Task ActionCastFakeSpellAt(Spell spell, Location location, ProjectilePathType pathType = ProjectilePathType.Default)
    {
      await WaitForObjectContext();
      NWScript.ActionCastFakeSpellAtLocation((int)spell, location, (int)pathType);
    }

    /// <summary>
    /// Begins the casting animation and spell fx for the specified spell, without any spell effects.
    /// </summary>
    /// <param name="spell">The spell to cast.</param>
    /// <param name="target">The target object for the fake spell to be cast at.</param>
    /// <param name="pathType">An optional path type for this spell to use.</param>
    public async Task ActionCastFakeSpellAt(Spell spell, NwGameObject target, ProjectilePathType pathType = ProjectilePathType.Default)
    {
      await WaitForObjectContext();
      NWScript.ActionCastFakeSpellAtObject((int)spell, target, (int)pathType);
    }

    /// <summary>
    /// Intructs this creature to enter counterspell combat mode against the specified creature.
    /// </summary>
    /// <param name="counterSpellTarget">The target object to enter counterspell mode against.</param>
    public async Task ActionCounterspell(NwGameObject counterSpellTarget)
    {
      await WaitForObjectContext();
      NWScript.ActionCounterSpell(counterSpellTarget);
    }

    /// <summary>
    /// Instructs this creature to equip its most damaging melee weapon. If no valid melee weapon is found, it will equip the most damaging ranged weapon.<p/>
    /// </summary>
    /// <param name="verses">If set, finds the most effective melee weapon for attacking this object.</param>
    /// <param name="offhand">Determines if an off-hand weapon is equipped.</param>
    public async Task ActionEquipMostDamagingMelee(NwGameObject verses = null, bool offhand = false)
    {
      await WaitForObjectContext();
      NWScript.ActionEquipMostDamagingMelee(verses, offhand.ToInt());
    }

    /// <summary>
    /// Instructs this creature to equip its most damaging ranged weapon. If no valid ranged weapon is found, it will equip the most damaging melee weapon.<p/>
    /// </summary>
    /// <param name="verses">If set, finds the most effective ranged weapon for attacking this object.</param>
    public async Task ActionEquipMostDamagingRanged(NwGameObject verses = null)
    {
      await WaitForObjectContext();
      NWScript.ActionEquipMostDamagingRanged(verses);
    }

    /// <summary>
    /// Instructs this creature to equip the best armor in its inventory.
    /// <remarks>This function will do nothing if this creature is in combat (<see cref="IsInCombat"/>).<br/>
    /// It will also not equip clothing items with a base AC of 0, even if the item has AC bonuses applied to it.</remarks>
    /// </summary>
    public async Task ActionEquipMostEffectiveArmor()
    {
      await WaitForObjectContext();
      NWScript.ActionEquipMostEffectiveArmor();
    }

    /// <summary>
    /// Instructs this creature to use the specified placeable.
    /// </summary>
    /// <param name="placeable">The placeable object to interact with.</param>
    public async Task ActionInteractObject(NwPlaceable placeable)
    {
      await WaitForObjectContext();
      NWScript.ActionInteractObject(placeable);
    }

    /// <summary>
    /// Instructs this creature to rest.
    /// </summary>
    /// <param name="enemyLineOfSightCheck">If true, allows this creature to rest if enemies are nearby as long as they are not visible to this creature.</param>
    public async Task ActionRest(bool enemyLineOfSightCheck = false)
    {
      await WaitForObjectContext();
      NWScript.ActionRest(enemyLineOfSightCheck.ToInt());
    }

    /// <summary>
    /// Creates a copy of this creature.
    /// </summary>
    /// <param name="location">The location to place the new creature. Defaults to the current creature's location.</param>
    /// <param name="newTag">A new tag to assign to the creature.</param>
    /// <returns>The cloned creature.</returns>
    public NwCreature Clone(Location location = null, string newTag = null)
    {
      location ??= Location;
      return NWScript.CopyObject(this, location, sNewTag: newTag ?? string.Empty).ToNwObject<NwCreature>();
    }

    /// <summary>
    /// Moves the specified item/item stack to this creature's inventory.
    /// </summary>
    /// <param name="item">The item to add.</param>
    public async Task GiveItem(NwItem item)
    {
      NwObject assignTarget;
      if (item.Possessor != null)
      {
        assignTarget = item.Possessor;
      }
      else
      {
        assignTarget = item.Area;
      }

      if (assignTarget != this)
      {
        await assignTarget.WaitForObjectContext();
        NWScript.ActionGiveItem(item, this);
      }
    }

    /// <summary>
    /// Moves a specified amount of items from an item stack to this creature's inventory.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <param name="amount">The number of items from the item stack to take.</param>
    public async Task GiveItem(NwItem item, int amount)
    {
      if (amount > item.StackSize)
      {
        amount = item.StackSize;
      }

      if (amount == item.StackSize)
      {
        await GiveItem(item);
        return;
      }

      NwItem clone = item.Clone(this);
      clone.StackSize = amount;
      item.StackSize -= amount;
    }

    /// <summary>
    /// Get the item possessed by this creature with the tag itemTag.
    /// </summary>
    public NwItem FindItemWithTag(string itemTag)
    {
      return NWScript.GetItemPossessedBy(this, itemTag).ToNwObject<NwItem>();
    }

    /// <summary>
    /// Instructs this creature to equip the specified item into the given inventory slot.<br/>
    /// Note: If the creature already has an item equipped in the slot specified, it will be unequipped automatically
    /// by the call to EquipItem, and dropped if the creature lacks inventory space.<br/>
    /// In order for EquipItem to succeed the creature must be able to equip the item normally. This means that:<br/>
    /// 1) The item is in the creature's inventory.<br/>
    /// 2) The item must already be identified (if magical).<br/>
    /// 3) The creature has the level required to equip the item (if magical and ILR is on).<br/>
    /// 4) The creature possesses the required feats to equip the item (such as weapon proficiencies).
    /// </summary>
    public async Task ActionEquipItem(NwItem item, InventorySlot slot)
    {
      await WaitForObjectContext();
      NWScript.ActionEquipItem(item, (int)slot);
    }

    /// <summary>
    /// Instructs this creature to unequip the specified item from whatever slot it is currently in.
    /// </summary>
    public async Task ActionUnequipItem(NwItem item)
    {
      await WaitForObjectContext();
      NWScript.ActionUnequipItem(item);
    }

    /// <summary>
    /// Instructs this creature to walk over and pick up the specified item on the ground.
    /// </summary>
    /// <param name="item">The item to pick up.</param>
    public async Task ActionPickUpItem(NwItem item)
    {
      await WaitForObjectContext();
      NWScript.ActionPickUpItem(item);
    }

    /// <summary>
    /// Instructs this creature to begin placing down an item at its feet.
    /// </summary>
    /// <param name="item">The item to drop.</param>
    public async Task ActionPutDownItem(NwItem item)
    {
      await WaitForObjectContext();
      NWScript.ActionPutDownItem(item);
    }

    /// <summary>
    /// Instructs the creature to sit in the specified placeable.
    /// </summary>
    /// <param name="sitPlaceable">The placeable to sit in. Must be marked useable, empty, and support sitting (e.g. chairs).</param>
    /// <param name="alignToPlaceable">If true, auto-aligns the creature to the placeable's rotation. Otherwise, this creature will face East (0).</param>
    public async Task ActionSit(NwPlaceable sitPlaceable, bool alignToPlaceable = true)
    {
      await WaitForObjectContext();
      if (alignToPlaceable)
      {
        NWScript.SetFacing(sitPlaceable.Rotation);
      }

      NWScript.ActionSit(sitPlaceable);
    }

    /// <summary>
    /// Instructs this creature to approach and lock the specified door.
    /// </summary>
    /// <param name="door">The door to lock.</param>
    public async Task ActionLockObject(NwDoor door)
    {
      await DoActionLockObject(door);
    }

    /// <summary>
    /// Instructs this creature to approach and lock the specified placeable.
    /// </summary>
    /// <param name="placeable">The placeable to lock.</param>
    public async Task ActionLockObject(NwPlaceable placeable)
    {
      await DoActionLockObject(placeable);
    }

    private async Task DoActionLockObject(NwGameObject target)
    {
      await WaitForObjectContext();
      NWScript.ActionLockObject(target);
    }

    /// <summary>
    /// Instructs this creature to approach and unlock the specified door.
    /// </summary>
    /// <param name="door">The door to unlock.</param>
    public async Task ActionUnlockObject(NwDoor door)
    {
      await DoActionUnlockObject(door);
    }

    /// <summary>
    /// Instructs this creature to approach and unlock the specified placeable.
    /// </summary>
    /// <param name="placeable">The placeable to unlock.</param>
    public async Task ActionUnlockObject(NwPlaceable placeable)
    {
      await DoActionUnlockObject(placeable);
    }

    private async Task DoActionUnlockObject(NwGameObject target)
    {
      await WaitForObjectContext();
      NWScript.ActionUnlockObject(target);
    }

    /// <summary>
    /// Instructs this creature to use the specified feat on the target object.
    /// </summary>
    /// <remarks>This action cannot be used on PCs.</remarks>
    /// <param name="feat">The feat to use.</param>
    /// <param name="target">The target object for the feat.</param>
    public async Task ActionUseFeat(Feat feat, NwGameObject target)
    {
      await WaitForObjectContext();
      NWScript.ActionUseFeat((int)feat, target);
    }

    /// <summary>
    /// Instructs this creature to use the specified item property of an item in their inventory.
    /// </summary>
    /// <param name="item">The item to use.</param>
    /// <param name="itemProperty">The item property on the item to use.</param>
    /// <param name="location">The target location for the item property action.</param>
    /// <param name="decrementCharges">If true, decrements item charges as configured for the item property action.</param>
    /// <param name="subPropertyIndex">Specifies the index to use if this item has sub-properties (such as sub-radial spells).</param>
    public async Task ActionUseItem(NwItem item, ItemProperty itemProperty, Location location, bool decrementCharges = true, int subPropertyIndex = 0)
    {
      await WaitForObjectContext();
      NWScript.ActionUseItemAtLocation(item, itemProperty, location, subPropertyIndex, decrementCharges.ToInt());
    }

    /// <summary>
    /// Instructs this creature to use the specified item property of an item in their inventory.
    /// </summary>
    /// <param name="item">The item to use.</param>
    /// <param name="itemProperty">The item property on the item to use.</param>
    /// <param name="gameObject">The target object for the item property action.</param>
    /// <param name="decrementCharges">If true, decrements item charges as configured for the item property action.</param>
    /// <param name="subPropertyIndex">Specifies the index to use if this item has sub-properties (such as sub-radial spells).</param>
    public async Task ActionUseItem(NwItem item, ItemProperty itemProperty, NwGameObject gameObject, bool decrementCharges = true, int subPropertyIndex = 0)
    {
      await WaitForObjectContext();
      NWScript.ActionUseItemOnObject(item, itemProperty, gameObject, subPropertyIndex, decrementCharges.ToInt());
    }

    /// <summary>
    /// Instructs this creature to attempt to use a skill on another object.
    /// </summary>
    /// <param name="skill">The skill to use.</param>
    /// <param name="target">The target to use the skill on.</param>
    /// <param name="subSkill">A specific subskill to use.</param>
    /// <param name="itemUsed">An item to use in conjunction with this skill.</param>
    public async Task ActionUseSkill(Skill skill, NwGameObject target, SubSkill subSkill = SubSkill.None, NwItem itemUsed = null)
    {
      await WaitForObjectContext();
      NWScript.ActionUseSkill((int)skill, target, (int)subSkill, itemUsed);
    }

    /// <summary>
    /// Uses the specified talent.
    /// </summary>
    /// <param name="talent">The talent to use.</param>
    /// <param name="target">The target location for the talent.</param>
    public async Task ActionUseTalent(Talent talent, Location target)
    {
      await WaitForObjectContext();
      NWScript.ActionUseTalentAtLocation(talent, target);
    }

    /// <summary>
    /// Uses the specified talent.
    /// </summary>
    /// <param name="talent">The talent to use.</param>
    /// <param name="target">The target object for the talent.</param>
    public async Task ActionUseTalent(Talent talent, NwGameObject target)
    {
      await WaitForObjectContext();
      NWScript.ActionUseTalentOnObject(talent, target);
    }

    /// <summary>
    /// Instructs this creature to perform the specified action on a door.
    /// </summary>
    /// <param name="door">The door to interact with.</param>
    /// <param name="doorAction">The action to perform on the door.</param>
    public async Task DoDoorAction(NwDoor door, DoorAction doorAction)
    {
      await WaitForObjectContext();
      NWScript.DoDoorAction(door, (int)doorAction);
    }

    /// <summary>
    /// Instructs this creature to perform the specified action on a placeable.
    /// </summary>
    /// <param name="placeable">The placeable to interact with.</param>
    /// <param name="placeableAction">The action to perform on the placeable.</param>
    public async Task DoPlaceableAction(NwPlaceable placeable, PlaceableAction placeableAction)
    {
      await WaitForObjectContext();
      NWScript.DoPlaceableObjectAction(placeable, (int)placeableAction);
    }

    /// <summary>
    /// Gets a value indicating whether this creature considers the target as a friend.
    /// </summary>
    /// <param name="target">The target creature.</param>
    /// <returns>true if target is an enemy, otherwise false.</returns>
    public bool IsEnemy(NwCreature target)
    {
      return NWScript.GetIsEnemy(target, this).ToBool();
    }

    /// <summary>
    /// Gets a value indicating whether this creature considers the target as neutral.
    /// </summary>
    /// <param name="target">The target creature.</param>
    /// <returns>true if this creature considers the target as neutral, otherwise false.</returns>
    public bool IsNeutral(NwCreature target)
    {
      return NWScript.GetIsNeutral(target, this).ToBool();
    }

    /// <summary>
    /// Gets a value indicating whether this creature considers the target as a enemy.
    /// </summary>
    /// <param name="target">The target creature.</param>
    /// <returns>true if target is a friend, otherwise false.</returns>
    public bool IsFriend(NwCreature target)
    {
      return NWScript.GetIsFriend(target, this).ToBool();
    }

    /// <summary>
    /// Returns this creature's spell school specialization in the specified class.<br/>
    /// Unless custom content is used, only Wizards have spell schools.
    /// </summary>
    /// <param name="classType">The class to query for specialized spell schools.</param>
    /// <returns>The creature's selected spell specialization.</returns>
    public SpellSchool GetSpecialization(ClassType classType = ClassType.Wizard)
    {
      return (SpellSchool)NWScript.GetSpecialization(this, (int)classType);
    }

    /// <summary>
    /// Returns this creature's domains in the specified class. Unless custom content is used, only clerics have domains.
    /// </summary>
    /// <param name="classType">The class with domains.</param>
    /// <returns>An enumeration of this creature's domains.</returns>
    public IEnumerable<Domain> GetClassDomains(ClassType classType = ClassType.Cleric)
    {
      const int error = (int)Domain.Error;
      int classT = (int)classType;

      int i;
      int current;

      for (i = 1, current = NWScript.GetDomain(this, i, classT); current != error; i++, current = NWScript.GetDomain(this, i, classT))
      {
        yield return (Domain)current;
      }
    }

    /// <summary>
    /// Gets if this creature is using the specified action mode.
    /// </summary>
    /// <param name="actionMode">The action mode to query.</param>
    /// <returns>True if the specified action mode is currently active, otherwise false.</returns>
    public bool GetActionMode(ActionMode actionMode)
    {
      return NWScript.GetActionMode(this, (int)actionMode).ToBool();
    }

    /// <summary>
    /// Instructs this creature to enable/disable the specified action mode (parry, power attack, expertise, etc).
    /// </summary>
    /// <param name="actionMode">The action mode to toggle.</param>
    /// <param name="status">The new state of the action mode.</param>
    public void SetActionMode(ActionMode actionMode, bool status)
    {
      NWScript.SetActionMode(this, (int)actionMode, status.ToInt());
    }

    /// <summary>
    /// Instantly gives this creature the benefits of a rest (restored hitpoints, spells, feats, etc...).
    /// </summary>
    public void ForceRest()
    {
      NWScript.ForceRest(this);
    }

    /// <summary>
    /// Returns the model number being used for the body part and creature.
    /// </summary>
    public CreatureModelType GetCreatureBodyPart(CreaturePart creaturePart)
    {
      return (CreatureModelType)NWScript.GetCreatureBodyPart((int)creaturePart, this);
    }

    /// <summary>
    /// Sets the body part model to be used on the creature.
    /// </summary>
    public void SetCreatureBodyPart(CreaturePart creaturePart, CreatureModelType creatureModel)
    {
      NWScript.SetCreatureBodyPart((int)creaturePart, (int)creatureModel, this);
    }

    /// <summary>
    /// Gets the associate of this creature with the matching associate type.<br/>
    /// See <see cref="Henchmen"/> for getting a list of all henchmen associated with this creature.
    /// </summary>
    /// <param name="associateType">The type of associate to locate.</param>
    /// <returns>The associated creature, otherwise null if this creature does not have an associate of the specified type.</returns>
    public NwCreature GetAssociate(AssociateType associateType)
    {
      return NWScript.GetAssociate((int)associateType, this).ToNwObject<NwCreature>();
    }

    /// <summary>
    /// Gets whether this creature is under the effects of the specified feat.
    /// </summary>
    /// <param name="feat">The feat to check.</param>
    public bool HasFeatEffect(Feat feat)
    {
      return NWScript.GetHasFeatEffect((int)feat, this).ToBool();
    }

    /// <summary>
    /// Gets whether this creature is under the effects of the specified spell.
    /// </summary>
    /// <param name="spell">The spell to check.</param>
    public bool HasSpellEffect(Spell spell)
    {
      return NWScript.GetHasSpellEffect((int)spell, this).ToBool();
    }

    /// <summary>
    /// Gets whether this creature has a friendly reaction towards another given creature.
    /// </summary>
    /// <param name="creature">The target creature to test.</param>
    public bool IsReactionTypeFriendly(NwCreature creature)
    {
      return NWScript.GetIsReactionTypeFriendly(creature, this).ToBool();
    }

    /// <summary>
    /// Gets whether this creature can see the specified creature.
    /// </summary>
    /// <param name="creature">The creature to test visibility.</param>
    /// <returns>True if the creature is visible by this creature, otherwise false.</returns>
    public bool IsCreatureSeen(NwCreature creature)
    {
      return NWScript.GetObjectSeen(creature, this).ToBool();
    }

    /// <summary>
    /// Gets whether this creature can hear the specified creature.
    /// </summary>
    /// <param name="creature">The creature that may be heard.</param>
    /// <returns>True if the creature has been heard, otherwise false.</returns>
    public bool IsCreatureHeard(NwCreature creature)
    {
      return NWScript.GetObjectHeard(creature, this).ToBool();
    }

    /// <summary>
    /// Gets whether this creature has a hostile reaction towards another given creature.
    /// </summary>
    /// <param name="creature">The target creature to test.</param>
    public bool IsReactionTypeHostile(NwCreature creature)
    {
      return NWScript.GetIsReactionTypeHostile(creature, this).ToBool();
    }

    /// <summary>
    /// Gets whether this creature has a neutral reaction towards another given creature.
    /// </summary>
    /// <param name="creature">The target creature to test.</param>
    public bool IsReactionTypeNeutral(NwCreature creature)
    {
      return NWScript.GetIsReactionTypeNeutral(creature, this).ToBool();
    }

    /// <summary>
    /// Gets how one creature feels toward this creature.
    /// </summary>
    /// <param name="creature">The creature whose feelings we wish to know.</param>
    public int Reputation(NwCreature creature)
    {
      return NWScript.GetReputation(this, creature);
    }

    /// <summary>
    /// Gets the best talent from a group of talents.
    /// </summary>
    /// <param name="category">The category of talents to pick from.</param>
    /// <param name="maxCr">The maximum Challenge Rating of the talent.</param>
    public TalentCategory TalentBest(TalentCategory category, int maxCr)
    {
      return (TalentCategory)NWScript.GetCreatureTalentBest((int)category, maxCr, this);
    }

    /// <summary>
    /// Gets a random talent from a group of talents possessed by this creature.
    /// </summary>
    /// <param name="category">The category of talents to pick from.</param>
    public TalentCategory TalentRandom(TalentCategory category)
    {
      return (TalentCategory)NWScript.GetCreatureTalentRandom((int)category, this);
    }

    /// <summary>
    /// Increment the remaining uses per day for this (creature) by the specified amount.<br/>
    /// Total number of feats uses per-day cannot exceed the creature's standard maximum.
    /// </summary>
    /// <param name="feat">The n/day feat to add uses.</param>
    /// <param name="amount">The amount of uses to add.</param>
    public void IncrementRemainingFeatUses(Feat feat, int amount = 1)
    {
      for (int i = 0; i < amount; i++)
      {
        NWScript.IncrementRemainingFeatUses(this, (int)feat);
      }
    }

    /// <summary>
    /// Decrements the remaining number of uses of a particular feat for this creature by the specified amount.<br/>
    /// You must have at least one feat use remaining to be able to decrement it.<br/>
    /// Passive feats, and feats that can be used unlimited times per day, cannot be decremented or suppressed.
    /// </summary>
    /// <param name="feat">The n/day feat to decrement uses.</param>
    /// <param name="amount">The amount of uses to decrement.</param>
    public void DecrementRemainingFeatUses(Feat feat, int amount = 1)
    {
      for (int i = 0; i < amount; i++)
      {
        NWScript.DecrementRemainingFeatUses(this, (int)feat);
      }
    }

    /// <summary>
    /// Levels up this creature using the default settings.<br/>
    /// You can assign a new class to level up <br/>
    /// If an invalid class combination is chosen the default class is leveled up.<br/>
    /// Package determines which package to level up with.<br/>
    /// If package is omitted it will use the starting package assigned to that class or just the class package.<br/>
    /// </summary>
    /// <param name="classType">Constant matching the class to level the creature in.</param>
    /// <param name="package"> Constant matching the package used to select skills and feats for the henchman.</param>
    /// <param name="spellsReady">Determines if all memorable spell slots will be filled without requiring rest.</param>
    /// <returns>Returns the new level if successful, or 0 if the function fails.</returns>
    public int LevelUpHenchman(ClassType classType, PackageType package, bool spellsReady = false)
    {
      return NWScript.LevelUpHenchman(this, (int)classType, (int)package, spellsReady.ToInt());
    }

    /// <summary>
    /// Instructs this creature to speak/play the specified voice chat.
    /// </summary>
    /// <param name="voiceChatType">The <see cref="NWN.API.Constants.VoiceChatType"/> for this creature to speak.</param>
    public void PlayVoiceChat(VoiceChatType voiceChatType)
    {
      NWScript.PlayVoiceChat((int)voiceChatType, this);
    }

    /// <summary>
    /// Restores the number of base attacks back to it's original state on this (creature).
    /// </summary>
    public void RestoreBaseAttackBonus()
    {
      NWScript.RestoreBaseAttackBonus(this);
    }

    /// <summary>
    /// Gets whether the given area tile is visible on the map for this creature.<br/>
    /// Tile exploration also controls object visibility in areas and the fog of war for interior and underground areas.
    /// </summary>
    /// <param name="area">The area containing the tile.</param>
    /// <param name="x">The location of the tile on the x axis.</param>
    /// <param name="y">The location of the tile on the y axis.</param>
    /// <returns>True if this creature has explored this tile, otherwise false.</returns>
    public bool GetTileExplored(NwArea area, int x, int y)
    {
      return NWScript.GetTileExplored(this, area, x, y).ToBool();
    }

    /// <summary>
    /// Gets whether the given area tile is visible on the map for this creature.<br/>
    /// Tile exploration also controls object visibility in areas and the fog of war for interior and underground areas.
    /// </summary>
    /// <param name="area">The area containing the tile.</param>
    /// <param name="x">The location of the tile on the x axis.</param>
    /// <param name="y">The location of the tile on the y axis.</param>
    /// <param name="newState">The new exploration state for this tile (true = explored, false = unexplored).</param>
    /// <returns>The exploration state before newState. True if this creature has explored this tile, otherwise false.</returns>
    public bool SetTileExplored(NwArea area, int x, int y, bool newState)
    {
      return NWScript.SetTileExplored(this, area, x, y, newState.ToInt()).ToBool();
    }

    /// <summary>
    /// Check whether this creature can damage the specified object using their current weapon/s.
    /// </summary>
    /// <param name="target">The object to test this creature's weapon against.</param>
    /// <param name="offHand">If true, checks the creature's off-hand weapon.</param>
    public async Task<bool> IsWeaponEffective(NwGameObject target, bool offHand = false)
    {
      await WaitForObjectContext();
      return NWScript.GetIsWeaponEffective(target, offHand.ToInt()).ToBool();
    }

    /// <summary>
    /// Teleports this creature to the nearest valid location by the target.<br/>
    /// This action produces no visual effect.
    /// </summary>
    /// <param name="gameObject">The target object to jump to.</param>
    /// <param name="walkStraightLineToPoint">Unknown.</param>
    /// <remarks>Does not affect dead or dying creatures.</remarks>
    public async Task JumpToObject(NwGameObject gameObject, bool walkStraightLineToPoint = true)
    {
      await WaitForObjectContext();
      NWScript.JumpToObject(gameObject, walkStraightLineToPoint.ToInt());
    }

    /// <summary>
    /// Instructs this creature to immediately speak the first non-branching conversation line in their dialog.
    /// </summary>
    /// <param name="dialogResRef">The dialog resource reference to use.</param>
    /// <param name="tokenTarget">The object to use if there are object-specific tokens in the string.</param>
    public async Task SpeakOneLinerConversation(string dialogResRef = "", NwGameObject tokenTarget = null)
    {
      await WaitForObjectContext();
      NWScript.SpeakOneLinerConversation(dialogResRef, tokenTarget);
    }

    /// <summary>
    /// Gives this creature the specified feat.<br/>
    /// Consider using the <see cref="AddFeat(NWN.API.Constants.Feat, int)"/> overload to properly allocate the feat to a level.
    /// </summary>
    /// <param name="feat">The feat to give.</param>
    public void AddFeat(Feat feat)
    {
      Creature.m_pStats.AddFeat((ushort)feat);
    }

    /// <summary>
    /// Gives this creature the specified feat at a level.<br/>
    /// Consider using the <see cref="AddFeat(NWN.API.Constants.Feat, int)"/> overload to properly allocate the feat to a level.
    /// </summary>
    /// <param name="feat">The feat to give.</param>
    /// <param name="level">The level the feat was gained.</param>
    public unsafe void AddFeat(Feat feat, int level)
    {
      if (level == 0 || level > Creature.m_pStats.m_lstLevelStats.num)
      {
        throw new ArgumentOutOfRangeException(nameof(level), "Level must be from 1 to the creature's max level.");
      }

      CNWLevelStats levelStats = CNWLevelStats.FromPointer(*Creature.m_pStats.m_lstLevelStats._OpIndex(level - 1));

      levelStats.AddFeat((ushort)feat);
      Creature.m_pStats.AddFeat((ushort)feat);
    }

    /// <summary>
    /// Gets if this creature knows the specified feat.
    /// </summary>
    /// <param name="feat">The feat to check.</param>
    /// <returns>True if the creature knows the feat, otherwise false.</returns>
    public bool KnowsFeat(Feat feat)
    {
      return Creature.m_pStats.HasFeat((ushort)feat).ToBool();
    }

    /// <summary>
    /// Removes the specified feat from this creature.
    /// </summary>
    /// <param name="feat">The feat to remove.</param>
    public void RemoveFeat(Feat feat)
    {
      Creature.m_pStats.RemoveFeat((ushort)feat);
    }

    /// <summary>
    /// Gets the level a feat was gained.
    /// </summary>
    /// <param name="feat">The feat to query.</param>
    /// <returns>The character level a feat was gained, otherwise 0 if the character does not have the feat.</returns>
    public int GetFeatGainLevel(Feat feat)
    {
      IReadOnlyList<CreatureLevelInfo> levelInfo = LevelInfo;
      for (int i = 0; i < levelInfo.Count; i++)
      {
        if (levelInfo[i].Feats.Contains(feat))
        {
          return i + 1;
        }
      }

      return 0;
    }

    public bool MeetsFeatRequirements(Feat feat)
    {
      using CExoArrayListUInt16 unused = new CExoArrayListUInt16();
      return Creature.m_pStats.FeatRequirementsMet((ushort)feat, unused).ToBool();
    }

    /// <summary>
    /// Gets the special abilities available to this creature.
    /// </summary>
    public IReadOnlyList<SpecialAbility> SpecialAbilities
    {
      get
      {
        List<SpecialAbility> retVal = new List<SpecialAbility>();
        CExoArrayListCNWSStatsSpellLikeAbility specialAbilities = Creature.m_pStats.m_pSpellLikeAbilityList;

        for (int i = 0; i < specialAbilities.num; i++)
        {
          CNWSStats_SpellLikeAbility ability = specialAbilities._OpIndex(i);
          if (ability.m_nSpellId != ~0u)
          {
            retVal.Add(new SpecialAbility((Spell)ability.m_nSpellId, ability.m_nCasterLevel, ability.m_bReadied.ToBool()));
          }
        }

        return retVal;
      }
    }

    /// <summary>
    /// Adds the specified ability to this creature.
    /// </summary>
    /// <param name="ability">The ability to add.</param>
    public void AddSpecialAbility(SpecialAbility ability)
    {
      CExoArrayListCNWSStatsSpellLikeAbility specialAbilities = Creature.m_pStats.m_pSpellLikeAbilityList;
      specialAbilities.Add(new CNWSStats_SpellLikeAbility
      {
        m_nSpellId = (uint)ability.Spell,
        m_bReadied = ability.Ready.ToInt(),
        m_nCasterLevel = ability.CasterLevel,
      });
    }

    /// <summary>
    /// Removes the specified ability at the given index.
    /// </summary>
    /// <param name="index">The ability index to remove.</param>
    public void RemoveSpecialAbilityAt(int index)
    {
      CExoArrayListCNWSStatsSpellLikeAbility specialAbilities = Creature.m_pStats.m_pSpellLikeAbilityList;
      if (index < specialAbilities.num)
      {
        specialAbilities._OpIndex(index).m_nSpellId = ~0u;
      }
    }

    /// <summary>
    /// Updates the specified ability at the given index.
    /// </summary>
    /// <param name="index">The ability index to update.</param>
    /// <param name="ability">The new state for the ability.</param>
    public void SetSpecialAbilityAt(int index, SpecialAbility ability)
    {
      CExoArrayListCNWSStatsSpellLikeAbility specialAbilities = Creature.m_pStats.m_pSpellLikeAbilityList;
      if (index < specialAbilities.num)
      {
        CNWSStats_SpellLikeAbility specialAbility = specialAbilities._OpIndex(index);
        specialAbility.m_nSpellId = (uint)ability.Spell;
        specialAbility.m_bReadied = ability.Ready.ToInt();
        specialAbility.m_nCasterLevel = ability.CasterLevel;
      }
    }

    /// <summary>
    /// Gets the base (raw) ability score for the specified ability, without racial modfiers.
    /// </summary>
    /// <param name="ability">The ability score type to query.</param>
    /// <returns>An integer representing the creature's ability score without modifiers.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if an invalid ability is specified.</exception>
    public byte GetRawAbilityScore(Ability ability)
    {
      return ability switch
      {
        Ability.Strength => Creature.m_pStats.m_nStrengthBase,
        Ability.Dexterity => Creature.m_pStats.m_nDexterityBase,
        Ability.Constitution => Creature.m_pStats.m_nConstitutionBase,
        Ability.Intelligence => Creature.m_pStats.m_nIntelligenceBase,
        Ability.Wisdom => Creature.m_pStats.m_nWisdomBase,
        Ability.Charisma => Creature.m_pStats.m_nCharismaBase,
        _ => throw new ArgumentOutOfRangeException(nameof(ability), ability, null),
      };
    }

    /// <summary>
    /// Sets the base (raw) ability score for the specified ability, without racial modifiers.
    /// </summary>
    /// <param name="ability">The ability score type to query.</param>
    /// <param name="value">The new ability score to set.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if an invalid ability is specified.</exception>
    public void SetsRawAbilityScore(Ability ability, byte value)
    {
      switch (ability)
      {
        case Ability.Strength:
          Creature.m_pStats.SetSTRBase(value);
          break;
        case Ability.Dexterity:
          Creature.m_pStats.SetDEXBase(value);
          break;
        case Ability.Constitution:
          Creature.m_pStats.SetCONBase(value);
          break;
        case Ability.Intelligence:
          Creature.m_pStats.SetINTBase(value);
          break;
        case Ability.Wisdom:
          Creature.m_pStats.SetWISBase(value);
          break;
        case Ability.Charisma:
          Creature.m_pStats.SetCHABase(value);
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(ability), ability, null);
      }
    }

    /// <summary>
    /// Gets the raw ability score a polymorphed creature had prior to polymorphing.
    /// </summary>
    /// <param name="ability">The ability score to query. Works for strength, dexterity and constitution only.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public byte GetPrePolymorphAbilityScore(Ability ability)
    {
      return ability switch
      {
        Ability.Strength => Creature.m_nPrePolymorphSTR,
        Ability.Dexterity => Creature.m_nPrePolymorphDEX,
        Ability.Constitution => Creature.m_nPrePolymorphCON,
        _ => throw new ArgumentOutOfRangeException(nameof(ability), ability, null),
      };
    }

    /// <summary>
    /// Restore all <see cref="NwCreature" /> spells per day for given level.
    /// </summary>
    public void RestoreSpells(byte level)
    {
      if (level > 9)
      {
        throw new ArgumentOutOfRangeException(nameof(level), "Level must be a spell level between 0 and 9.");
      }

      Creature.m_pStats.ReadySpellLevel(level);
    }

    /// <summary>
    /// Restore all <see cref="NwCreature" /> spells per day for all levels.
    /// </summary>
    public void RestoreAllSpells()
    {
      for (byte i = 0; i <= 9; i++)
      {
        Creature.m_pStats.ReadySpellLevel(i);
      }
    }

    /// <summary>
    /// Gets the level stat info for the specified level (feat, class, skills, etc.).
    /// </summary>
    /// <param name="level">The level to lookup.</param>
    /// <returns>A <see cref="LevelInfo"/> object containing level info.</returns>
    public unsafe CreatureLevelInfo GetLevelStats(int level)
    {
      if (level == 0 || level > Creature.m_pStats.m_lstLevelStats.num)
      {
        throw new ArgumentOutOfRangeException(nameof(level), "Level must be from 1 to the creature's max level.");
      }

      CNWLevelStats levelStats = CNWLevelStats.FromPointer(*Creature.m_pStats.m_lstLevelStats._OpIndex(level - 1));
      return new CreatureLevelInfo(this, levelStats);
    }

    public unsafe void AcquireItem(NwItem item, bool displayFeedback = true)
    {
      if (item == null)
      {
        throw new ArgumentNullException(nameof(item), "Item cannot be null.");
      }

      void* itemPtr = item.Item;
      Creature.AcquireItem(&itemPtr, Invalid, Invalid, 0xFF, 0xFF, true.ToInt(), displayFeedback.ToInt());
    }

    public byte[] SerializeQuickbar()
    {
      return NativeUtils.SerializeGff("GFF", (resGff, resStruct) =>
      {
        Creature.SaveQuickButtons(resGff, resStruct);
        return true;
      });
    }

    public bool DeserializeQuickbar(byte[] serialized)
    {
      bool result = NativeUtils.DeserializeGff(serialized, (resGff, resStruct) =>
      {
        if (!resGff.IsValidGff("GFF"))
        {
          return false;
        }

        Creature.LoadQuickButtons(resGff, resStruct);
        return true;
      });

      NwPlayer player = ControllingPlayer;

      if (result && player != null)
      {
        CNWSMessage message = LowLevel.ServerExoApp.GetNWSMessage();
        message?.SendServerToPlayerGuiQuickbar_SetButton(player, 0, true.ToInt());
      }

      return result;
    }

    public override byte[] Serialize()
    {
      return NativeUtils.SerializeGff("BIC", (resGff, resStruct) =>
      {
        Creature.SaveObjectState(resGff, resStruct);
        return Creature.SaveCreature(resGff, resStruct).ToBool();
      });
    }

    public static NwCreature Deserialize(byte[] serialized)
    {
      CNWSCreature creature = null;

      bool result = NativeUtils.DeserializeGff(serialized, (resGff, resStruct) =>
      {
        if (!resGff.IsValidGff(new[] { "BIC", "GFF", "UTC" }, new[] { "V3.2" }))
        {
          return false;
        }

        creature = new CNWSCreature(Invalid, 0, 1);
        if (creature.LoadCreature(resGff, resStruct, 0, 0, 0, 0).ToBool())
        {
          creature.LoadObjectState(resGff, resStruct);
          GC.SuppressFinalize(creature);
          return true;
        }

        creature.Dispose();
        return false;
      });

      return result && creature != null ? creature.ToNwObject<NwCreature>() : null;
    }

    public PlayerQuickBarButton[] GetQuickBarButtons()
    {
      if (Creature.m_pQuickbarButton == null)
      {
        Creature.InitializeQuickbar();
      }

      PlayerQuickBarButton[] retVal = new PlayerQuickBarButton[QuickBarButtonCount];
      for (byte i = 0; i < QuickBarButtonCount; i++)
      {
        retVal[i] = InternalGetQuickBarButton(i);
      }

      return retVal;
    }

    public void SetQuickBarButtons(PlayerQuickBarButton[] buttons)
    {
      if (Creature.m_pQuickbarButton == null)
      {
        Creature.InitializeQuickbar();
      }

      for (byte i = 0; i < buttons.Length && i < QuickBarButtonCount; i++)
      {
        InternalSetQuickBarButton(i, buttons[i]);
      }
    }

    public PlayerQuickBarButton GetQuickBarButton(byte index)
    {
      if (index >= QuickBarButtonCount)
      {
        throw new ArgumentOutOfRangeException(nameof(index), "Index must be < 36");
      }

      if (Creature.m_pQuickbarButton == null)
      {
        Creature.InitializeQuickbar();
      }

      return InternalGetQuickBarButton(index);
    }

    public void SetQuickBarButton(byte index, PlayerQuickBarButton data)
    {
      if (index >= QuickBarButtonCount)
      {
        throw new ArgumentOutOfRangeException(nameof(index), "Index must be < 36");
      }

      if (Creature.m_pQuickbarButton == null)
      {
        Creature.InitializeQuickbar();
      }

      InternalSetQuickBarButton(index, data);

      NwPlayer player = ControllingPlayer;

      if (player != null)
      {
        CNWSMessage message = LowLevel.ServerExoApp.GetNWSMessage();
        message?.SendServerToPlayerGuiQuickbar_SetButton(player, index, false.ToInt());
      }
    }

    /// <summary>
    /// Instruct this creature to instantly equip the specified item.
    /// </summary>
    /// <param name="item">The item to equip.</param>
    /// <param name="inventorySlot">The inventory slot to equip the item to.</param>
    /// <returns>True if the item was successfully equipped, otherwise false.</returns>
    /// <exception cref="ArgumentNullException">Item is null.</exception>
    public bool RunEquip(NwItem item, InventorySlot inventorySlot)
    {
      if (item == null)
      {
        throw new ArgumentNullException(nameof(item), "Item must not be null.");
      }

      uint targetSlot = (uint)Math.Pow(2, (uint)inventorySlot);
      return Creature.RunEquip(item, targetSlot).ToBool();
    }

    /// <summary>
    /// Instruct this creature to instantly unequip the specified item.
    /// </summary>
    /// <param name="item">The item to unequip.</param>
    /// <returns>True if the item was successfully unequipped, otherwise false.</returns>
    /// <exception cref="ArgumentNullException">Item is null.</exception>
    public bool RunUnequip(NwItem item)
    {
      if (item == null)
      {
        throw new ArgumentNullException(nameof(item), "Item must not be null.");
      }

      // The module unequip event runs instantly so we have to temporarily change the event script id of the calling script
      // otherwise GetCurrentlyRunningEvent() doesn't return the right id
      EventScriptType previousScriptEvent = VirtualMachine.Instance.CurrentRunningEvent;
      VirtualMachine.Instance.CurrentRunningEvent = EventScriptType.ModuleOnUnequipItem;

      bool retVal = Creature.RunUnequip(item, Invalid, unchecked((byte)-1), unchecked((byte)-1), false.ToInt()).ToBool();

      VirtualMachine.Instance.CurrentRunningEvent = previousScriptEvent;
      return retVal;
    }

    /// <summary>
    /// Gets the creature's highest attack bonus based on its own stats.<br/>
    /// AB vs. Type and +AB on Gauntlets are excluded.
    /// </summary>
    /// <param name="isMelee">TRUE: Get Melee/Unarmed Attack Bonus, FALSE: Get Ranged Attack Bonus</param>
    /// <param name="isTouchAttack">If the attack was a touch attack.</param>
    /// <param name="isOffHand">If the attack was a touch attack.</param>
    /// <param name="includeBaseAttackBonus">If the attack was with the offhand.</param>
    /// <returns>The highest attack bonus.</returns>
    public int GetAttackBonus(bool isMelee = false, bool isTouchAttack = false, bool isOffHand = false, bool includeBaseAttackBonus = true)
    {
      if (isMelee)
      {
        return Creature.m_pStats.GetMeleeAttackBonus(isOffHand.ToInt(), includeBaseAttackBonus.ToInt(), isTouchAttack.ToInt());
      }
      else
      {
        return Creature.m_pStats.GetRangedAttackBonus(includeBaseAttackBonus.ToInt(), isTouchAttack.ToInt());
      }
    }

    private PlayerQuickBarButton InternalGetQuickBarButton(byte index)
    {
      CNWSQuickbarButtonArray quickBarButtons = CNWSQuickbarButtonArray.FromPointer(Creature.m_pQuickbarButton);
      CNWSQuickbarButton button = quickBarButtons[index];

      return new PlayerQuickBarButton(button);
    }

    private void InternalSetQuickBarButton(byte index, PlayerQuickBarButton data)
    {
      CNWSQuickbarButtonArray quickBarButtons = CNWSQuickbarButtonArray.FromPointer(Creature.m_pQuickbarButton);
      CNWSQuickbarButton button = quickBarButtons[index];

      data.ApplyToNativeStructure(button);
    }
  }
}
