using NWN.Core;

namespace NWN.API.Constants
{
  public enum Action
  {
    MoveToPoint = NWScript.ACTION_MOVETOPOINT,
    PickupItem = NWScript.ACTION_PICKUPITEM,
    DropItem = NWScript.ACTION_DROPITEM,
    AttackObject = NWScript.ACTION_ATTACKOBJECT,
    CastSpell = NWScript.ACTION_CASTSPELL,
    OpenDoor = NWScript.ACTION_OPENDOOR,
    CloseDoor = NWScript.ACTION_CLOSEDOOR,
    DialogObject = NWScript.ACTION_DIALOGOBJECT,
    DisableTrap = NWScript.ACTION_DISABLETRAP,
    RecoverTrap = NWScript.ACTION_RECOVERTRAP,
    FlagTrap = NWScript.ACTION_FLAGTRAP,
    ExamineTrap = NWScript.ACTION_EXAMINETRAP,
    SetTrap = NWScript.ACTION_SETTRAP,
    OpenLock = NWScript.ACTION_OPENLOCK,
    Lock = NWScript.ACTION_LOCK,
    UseObject = NWScript.ACTION_USEOBJECT,
    AnimalEmpathy = NWScript.ACTION_ANIMALEMPATHY,
    Rest = NWScript.ACTION_REST,
    Taunt = NWScript.ACTION_TAUNT,
    ItemCastSpell = NWScript.ACTION_ITEMCASTSPELL,
    CounterSpell = NWScript.ACTION_COUNTERSPELL,
    Heal = NWScript.ACTION_HEAL,
    Pickpocket = NWScript.ACTION_PICKPOCKET,
    Follow = NWScript.ACTION_FOLLOW,
    Wait = NWScript.ACTION_WAIT,
    Sit = NWScript.ACTION_SIT,
    SmiteGood = NWScript.ACTION_SMITEGOOD,
    KiDamage = NWScript.ACTION_KIDAMAGE,
    RandomWalk = NWScript.ACTION_RANDOMWALK,
    Invalid = NWScript.ACTION_INVALID,
  }
}
