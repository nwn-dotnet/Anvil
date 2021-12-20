using System;

namespace Anvil.API
{
  [Flags]
  public enum EquipmentSlots : uint
  {
    None = NWN.Native.API.EquipmentSlot.None,
    Head = NWN.Native.API.EquipmentSlot.Head,
    Chest = NWN.Native.API.EquipmentSlot.Chest,
    Boots = NWN.Native.API.EquipmentSlot.Boots,
    Arms = NWN.Native.API.EquipmentSlot.Arms,
    RightHand = NWN.Native.API.EquipmentSlot.RightHand,
    LeftHand = NWN.Native.API.EquipmentSlot.LeftHand,
    Cloak = NWN.Native.API.EquipmentSlot.Cloak,
    LeftRing = NWN.Native.API.EquipmentSlot.LeftRing,
    RightRing = NWN.Native.API.EquipmentSlot.RightRing,
    Neck = NWN.Native.API.EquipmentSlot.Neck,
    Belt = NWN.Native.API.EquipmentSlot.Belt,
    Arrows = NWN.Native.API.EquipmentSlot.Arrows,
    Bullets = NWN.Native.API.EquipmentSlot.Bullets,
    Bolts = NWN.Native.API.EquipmentSlot.Bolts,
    CreatureWeaponLeft = NWN.Native.API.EquipmentSlot.CreatureWeaponLeft,
    CreatureWeaponRight = NWN.Native.API.EquipmentSlot.CreatureWeaponRight,
    CreatureWeaponBite = NWN.Native.API.EquipmentSlot.CreatureWeaponBite,
    CreatureArmour = NWN.Native.API.EquipmentSlot.CreatureArmour,
    Rings = LeftRing | RightRing,
  }
}
