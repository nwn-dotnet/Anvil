using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NWN.API.Constants;
using NWN.Core;
using NWNX.API;
using NWNX.API.Constants;
using Action = NWN.API.Constants.Action;

namespace NWN.API
{
  [NativeObjectInfo(ObjectType.Creature, InternalObjectType.Creature)]
  public class NwCreature : NwGameObject
  {
    internal NwCreature(uint objectId) : base(objectId) {}

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
      get => (Phenotype) NWScript.GetPhenoType(this);
      set => NWScript.SetPhenoType((int) value, this);
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
      get => (RacialType) NWScript.GetRacialType(this);
    }

    /// <summary>
    /// Gets the gender of this creature.
    /// </summary>
    public Gender Gender
    {
      get => (Gender) NWScript.GetGender(this);
    }

    /// <summary>
    /// Gets the associate type of this creature, otherwise returns <see cref="NWN.API.Constants.AssociateType.None"/> if this creature is not an associate of anyone.
    /// </summary>
    public AssociateType AssociateType
    {
      get => (AssociateType) NWScript.GetAssociateType(this);
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
    /// Gets the Base Attack Bonus for this creature.
    /// </summary>
    public int BaseAttackBonus
    {
      get => NWScript.GetBaseAttackBonus(this);
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
      get => (CreatureSize) NWScript.GetCreatureSize(this);
    }

    /// <summary>
    /// Gets the calculated challenge rating for this creature.
    /// </summary>
    public float ChallengeRating
    {
      get => NWScript.GetChallengeRating(this);
    }

    /// <summary>
    /// Gets the movement rate of this creature.
    /// </summary>
    public MovementRate MovementRate
    {
      get => (MovementRate) NWScript.GetMovementRate(this);
    }

    /// <summary>
    /// Gets this creature's Law/Chaos Alignment.
    /// </summary>
    public Alignment LawChaosAlignment
    {
      get => (Alignment) NWScript.GetAlignmentLawChaos(this);
    }

    /// <summary>
    /// Gets this creature's Good/Evil Alignment.
    /// </summary>
    public Alignment GoodEvilAlignment
    {
      get => (Alignment) NWScript.GetAlignmentGoodEvil(this);
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
      get => (AssociateCommand) NWScript.GetLastAssociateCommand(this);
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
      get => (Action) NWScript.GetCurrentAction(this);
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
    /// Gets this creature's classes.
    /// </summary>
    public IReadOnlyList<ClassType> Classes
    {
      get
      {
        const int maxClasses = 3;

        List<ClassType> classes = new List<ClassType>(maxClasses);
        for (int i = 1; i <= maxClasses; i++)
        {
          ClassType classType = (ClassType) NWScript.GetClassByPosition(i, this);
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
        NWScript.AdjustAlignment(this, (int) alignment, shift, false.ToInt());
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
        NWScript.AdjustAlignment(this, (int) alignment, shift, false.ToInt());
      }
    }

    /// <summary>
    /// Gets the amount of gold carried by this creature.<br/>
    /// See <see cref="GiveGold"/>, <see cref="TakeGold"/> for adding/removing gold from this creature.
    /// </summary>
    public int Gold
    {
      get => NWScript.GetGold(this);
    }

    /// <summary>
    /// Forces this creature to join the specified standard faction. This will NOT work on players.
    /// </summary>
    /// <param name="newFaction">The NPCs new faction.</param>
    public void ChangeToStandardFaction(StandardFaction newFaction)
      => NWScript.ChangeToStandardFaction(this, (int) newFaction);

    /// <summary>
    /// Gets whether this creature has a specific immunity.
    /// </summary>
    /// <param name="immunityType">The immunity type to check.</param>
    /// <param name="verses">If specified, the race and alignment of verses will be considered when determining immunities.</param>
    /// <returns>True if the creature has the specified immunity, otherwise false.</returns>
    public bool IsImmuneTo(ImmunityType immunityType, NwGameObject verses = null)
      => NWScript.GetIsImmune(this, (int) immunityType, verses).ToBool();

    /// <summary>
    /// Gives gold to this creature.
    /// </summary>
    /// <param name="amount">The amount of gold to give.</param>
    public void GiveGold(int amount)
      => NWScript.GiveGoldToCreature(this, amount);

    /// <summary>
    /// Takes gold away from this creature.
    /// </summary>
    /// <param name="amount">The amount of gold to take.</param>
    public async Task TakeGold(int amount)
    {
      await WaitForObjectContext();
      NWScript.TakeGoldFromCreature(amount, this, true.ToInt());
    }

    /// <summary>
    /// Gives gold to this creature by taking it from another.
    /// </summary>
    /// <param name="amount">The amount of gold to take.</param>
    /// <param name="target">The target creature to take the gold from.</param>
    public async Task TakeGoldFrom(int amount, NwCreature target)
    {
      await WaitForObjectContext();
      NWScript.TakeGoldFromCreature(amount, target, false.ToInt());
    }

    /// <summary>
    /// Creates a creature at the specified location.
    /// </summary>
    /// <param name="template">The creature resref template from the toolset palette.</param>
    /// <param name="location">The location where this creature will spawn.</param>
    /// <param name="useAppearAnim">If true, plays EffectAppear when created.</param>
    /// <param name="newTag">The new tag to assign this creature. Leave uninitialized/as null to use the template's tag.</param>
    public static NwCreature Create(string template, Location location, bool useAppearAnim = false, string newTag = "")
      => CreateInternal<NwCreature>(template, location, useAppearAnim, newTag);

    /// <summary>
    /// Gets the item that is equipped in the specified inventory slot.
    /// </summary>
    /// <param name="slot">The inventory slot to check.</param>
    /// <returns>The item in the inventory slot, otherwise null if it is unpopulated.</returns>
    public NwItem GetItemInSlot(InventorySlot slot)
      => NWScript.GetItemInSlot((int) slot, this).ToNwObject<NwItem>();

    /// <summary>
    /// Attempts to perform a melee touch attack on target. This is not a creature action, and assumes that this creature is already within range of the target.
    /// </summary>
    /// <param name="target">The target of this touch attack.</param>
    public async Task<TouchAttackResult> TouchAttackMelee(NwGameObject target)
    {
      await WaitForObjectContext();
      return (TouchAttackResult) NWScript.TouchAttackMelee(target);
    }

    /// <summary>
    /// Attempts to perform a ranged touch attack on the target. This is not a creature action, and simply does a roll to see if the target was hit.
    /// </summary>
    /// <param name="target">The target of this touch attack.</param>
    /// <param name="displayFeedback">If true, displays combat feedback in the chat window.</param>
    public async Task<TouchAttackResult> TouchAttackRanged(NwGameObject target, bool displayFeedback)
    {
      await WaitForObjectContext();
      return (TouchAttackResult) NWScript.TouchAttackRanged(target, displayFeedback.ToInt());
    }

    /// <summary>
    /// Adjusts the alignment of this creature and associated party members by the specified value.<br/>
    /// Use the <see cref="LawChaosValue"/> and <see cref="GoodEvilValue"/> setters to only affect this creature.
    /// </summary>
    /// <param name="alignment">The alignment to shift towards.</param>
    /// <param name="shift">The amount of alignment shift.</param>
    public void AdjustPartyAlignment(Alignment alignment, int shift)
      => NWScript.AdjustAlignment(this, (int) alignment, shift);

    /// <summary>
    /// Gets the specified ability score from this creature.
    /// </summary>
    /// <param name="ability">The type of ability.</param>
    /// <param name="baseOnly">If true, will return the creature's base ability score without bonuses or penalties.</param>
    public int GetAbilityScore(Ability ability, bool baseOnly = false)
      => NWScript.GetAbilityScore(this, (int) ability, baseOnly.ToInt());

    /// <summary>
    /// Gets the DC to save against for a spell (10 + spell level + relevant ability bonus).
    /// </summary>
    public async Task<int> GetSpellSaveDC()
    {
      await WaitForObjectContext();
      return NWScript.GetSpellSaveDC();
    }

    /// <summary>
    /// Returns the last target this creature tried to attack. Reset at the end of combat.
    /// </summary>
    public async Task<NwGameObject> GetAttemptedAttackTarget()
    {
      await WaitForObjectContext();
      return NWScript.GetAttemptedAttackTarget().ToNwObject<NwGameObject>();
    }

    /// <summary>
    /// Gets the target object of this creature's last spell.
    /// </summary>
    public async Task<NwGameObject> GetLastSpellTargetObject()
    {
      await WaitForObjectContext();
      return NWScript.GetSpellTargetObject().ToNwObject<NwGameObject>();
    }

    /// <summary>
    /// Gets the target location of this creature's last spell.
    /// </summary>
    public async Task<Location> GetLastSpellTargetLocation()
    {
      await WaitForObjectContext();
      return NWScript.GetSpellTargetLocation();
    }

    /// <summary>
    /// Gets the target this creature attempted to cast a spell at. Reset at the end of combat.
    /// </summary>
    public async Task<NwGameObject> GetAttemptedSpellTarget()
    {
      await WaitForObjectContext();
      return NWScript.GetAttemptedSpellTarget().ToNwObject<NwGameObject>();
    }

    /// <summary>
    ///  Determine the number of levels this creature holds in the specified <see cref="ClassType"/>.
    /// </summary>
    public int GetLevelByClass(ClassType classType)
      => NWScript.GetLevelByClass((int) classType, this);

    /// <summary>
    /// Gets if this creature has the specified spell available to cast.
    /// </summary>
    /// <param name="spell">The spell to check.</param>
    /// <returns>True if this creature can immediately cast the spell.</returns>
    public bool HasSpellUse(Spell spell)
      => NWScript.GetHasSpell((int) spell, this) > 0;

    /// <summary>
    /// Gets the number of ranks this creature has in the specified skill.
    /// </summary>
    /// <param name="skill">The skill to check.</param>
    /// <param name="ranksOnly">If true, returns the base amount of skill ranks without any ability modifiers.</param>
    /// <returns>-1 if the creature does not have this skill, 0 if untrained, otherwise the number of skill ranks.</returns>
    public int GetSkillRank(Skill skill, bool ranksOnly = false)
      => NWScript.GetSkillRank((int) skill, this, ranksOnly.ToInt());

    /// <summary>
    /// Returns true if this creature has the skill specified, and is useable.
    /// </summary>
    /// <param name="skill">The skill to check.</param>
    /// <returns>True if the creature has this skill.</returns>
    public bool HasSkill(Skill skill)
      => NWScript.GetHasSkill((int) skill, this).ToBool();

    /// <summary>
    /// Returns true if 1d20 + skill rank is greater than, or equal to difficultyClass.
    /// </summary>
    /// <param name="skill">The type of skill check.</param>
    /// <param name="difficultyClass">The DC of this skill check.</param>
    public bool DoSkillCheck(Skill skill, int difficultyClass)
      => NWScript.GetIsSkillSuccessful(this, (int) skill, difficultyClass).ToBool();

    /// <summary>
    /// Returns true if this creature knows the specified <see cref="Feat"/>, and can use it.<br/>
    /// Use <see cref="Creature.KnowsFeat"/> to simply check if a creature knows <see cref="Feat"/>, but may or may not have uses remaining.
    /// </summary>
    public bool HasFeatPrepared(Feat feat)
      => NWScript.GetHasFeat((int) feat, this).ToBool();

    /// <summary>
    /// Determines whether this creature has the specified talent.
    /// </summary>
    /// <param name="talent">The talent to check.</param>
    /// <returns>True if this creature has talent, otherwise false.</returns>
    public bool HasTalent(Talent talent)
      => NWScript.GetCreatureHasTalent(talent, this).ToBool();

    /// <summary>
    /// Applies the specified effect to this creature.
    /// </summary>
    /// <param name="durationType">The duration type to apply with this effect.</param>
    /// <param name="effect">The effect to apply.</param>
    /// <param name="duration">If duration type is <see cref="EffectDuration.Temporary"/>, the duration of this effect in seconds.</param>
    public void ApplyEffect(EffectDuration durationType, Effect effect, float duration = 0f)
    {
      NWScript.ApplyEffectToObject((int) durationType, effect, this, duration);
    }

    /// <summary>
    /// Removes the specified effect from this creature.
    /// </summary>
    /// <param name="effect">The existing effect instance.</param>
    public void RemoveEffect(Effect effect)
      => NWScript.RemoveEffect(this, effect);

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
    /// Causes the calling creature to start attacking the target using whichever weapon is current equipped.
    /// </summary>
    /// <param name="target">The target object to attack.</param>
    /// <param name="passive">If TRUE, the attacker will not move to attack the target. If we have a melee weapon equipped, we will just stand still.</param>
    public async Task ActionAttackTarget(NwGameObject target, bool passive = false)
    {
      await WaitForObjectContext();
      NWScript.ActionAttack(target, passive.ToInt());
    }

    /// <summary>
    /// Commands this creature to walk/run to the specified destination. If the location is invalid or a path cannot be found to it, the command does nothing.
    /// </summary>
    /// <param name="destination">The location to move towards.</param>
    /// <param name="run">If this is TRUE, the creature will run rather than walk.</param>
    public async Task ActionMoveToLocation(Location destination, bool run = false)
    {
      await WaitForObjectContext();
      NWScript.ActionMoveToLocation(destination, run.ToInt());
    }

    /// <summary>
    /// Commands this creature to move to a certain distance from the target object.
    /// If there is no path to the object, this command will do nothing.
    /// </summary>
    /// <param name="target">The object we wish the creature to move to.</param>
    /// <param name="run">If this is TRUE, the action subject will run rather than walk.</param>
    /// <param name="range">This is the desired distance between the creature and the target object.</param>
    public async Task ActionMoveToObject(NwObject target, bool run = false, float range = 1.0f)
    {
      await WaitForObjectContext();
      NWScript.ActionMoveToObject(target, run.ToInt(), range);
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
    /// Instructs this creature to move to a certain distance away from the specified location
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
      NWScript.ActionCastFakeSpellAtLocation((int) spell, location, (int) pathType);
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
      NWScript.ActionCastFakeSpellAtObject((int) spell, target, (int) pathType);
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
    /// <param name="verses">If set, finds the most effective weapon for attacking this object.</param>
    /// <param name="offhand">Determines if an off-hand weapon is equipped.</param>
    public async Task ActionEquipMostDamagingMelee(NwGameObject verses = null, bool offhand = false)
    {
      await WaitForObjectContext();
      NWScript.ActionEquipMostDamagingMelee();
    }

    /// <summary>
    /// Creates a copy of this creature.
    /// </summary>
    /// <param name="location">The location to place the new creature. Defaults to the current creature's location.</param>
    /// <param name="newTag">A new tag to assign to the creature.</param>
    /// <returns>The cloned creature.</returns>
    public NwCreature Clone(Location location = null, string newTag = null)
    {
      if (location == null)
      {
        location = Location;
      }

      return NWScript.CopyObject(this, location, sNewTag: newTag ?? string.Empty).ToNwObject<NwCreature>();
    }

    /// <summary>
    /// Moves the specified item to this creature's inventory.
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
    /// Get the item possessed by this creature with the tag itemTag.
    /// </summary>
    public NwItem FindItemWithTag(string itemTag)
      => NWScript.GetItemPossessedBy(this, itemTag).ToNwObject<NwItem>();

    /// <summary>
    /// Commands the creature to equip the specified item into the given inventory slot.<br/>
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
      NWScript.ActionEquipItem(item, (int) slot);
    }

    /// <summary>
    /// Commands this creature to unequip the specified item from whatever slot it is currently in.
    /// </summary>
    public async Task ActionUnequipItem(NwItem item)
    {
      await WaitForObjectContext();
      NWScript.ActionUnequipItem(item);
    }

    /// <summary>
    /// Commands this creature to walk over, and pick up the specified item on the ground.
    /// </summary>
    /// <param name="item">The item to pick up.</param>
    public async Task ActionPickUpItem(NwItem item)
    {
      await WaitForObjectContext();
      NWScript.ActionPickUpItem(item);
    }

    /// <summary>
    /// Commands this creature to begin placing down an item at its feet.
    /// </summary>
    /// <param name="item">The item to drop.</param>
    public async Task ActionPutDownItem(NwItem item)
    {
      await WaitForObjectContext();
      NWScript.ActionPutDownItem(item);
    }

    /// <summary>
    /// Commands the creature to sit in the specified placeable.
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
    /// Gets a value indicating whether this creature considers the target as a friend.
    /// </summary>
    /// <param name="target">The target creature.</param>
    /// <returns>true if target is an enemy, otherwise false.</returns>
    public bool IsEnemy(NwCreature target)
      => NWScript.GetIsEnemy(target, this).ToBool();

    /// <summary>
    /// Gets a value indicating whether this creature considers the target as neutral.
    /// </summary>
    /// <param name="target">The target creature.</param>
    /// <returns>true if this creature considers the target as neutral, otherwise false.</returns>
    public bool IsNeutral(NwCreature target)
      => NWScript.GetIsNeutral(target, this).ToBool();

    /// <summary>
    /// Gets a value indicating whether this creature considers the target as a enemy.
    /// </summary>
    /// <param name="target">The target creature.</param>
    /// <returns>true if target is a friend, otherwise false.</returns>
    public bool IsFriend(NwCreature target)
      => NWScript.GetIsFriend(target, this).ToBool();

    /// <summary>
    /// Returns this creature's spell school specialization in the specified class.<br/>
    /// Unless custom content is used, only Wizards have spell schools.
    /// </summary>
    /// <param name="classType">The class to query for specialized spell schools.</param>
    /// <returns>The creature's selected spell specialization.</returns>
    public SpellSchool GetSpecialization(ClassType classType = ClassType.Wizard)
      => (SpellSchool) NWScript.GetSpecialization(this, (int) classType);

    /// <summary>
    /// Returns this creature's domains in the specified class. Unless custom content is used, only clerics have domains.
    /// </summary>
    /// <param name="classType">The class with domains.</param>
    /// <returns>An enumeration of this creature's domains.</returns>
    public IEnumerable<Domain> GetClassDomains(ClassType classType = ClassType.Cleric)
    {
      const int error = (int) Domain.Error;
      int classT = (int) classType;

      int i;
      int current;

      for (i = 1, current = NWScript.GetDomain(this, i, classT); current != error; i++, current = NWScript.GetDomain(this, i, classT))
      {
        yield return (Domain) current;
      }
    }

    /// <summary>
    /// Instructs this creature to enable/disable the specified action mode (parry, power attack, expertise, etc).
    /// </summary>
    /// <param name="actionMode">The action mode to toggle.</param>
    /// <param name="status">The new state of the action mode.</param>
    public void SetActionMode(ActionMode actionMode, bool status)
      => NWScript.SetActionMode(this, (int) actionMode, status.ToInt());

    /// <summary>
    /// Instantly gives this creature the benefits of a rest (restored hitpoints, spells, feats, etc...)
    /// </summary>
    public void ForceRest()
      => NWScript.ForceRest(this);
  }
}
