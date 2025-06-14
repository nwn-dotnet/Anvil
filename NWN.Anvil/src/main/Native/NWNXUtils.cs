using System;
using System.Runtime.InteropServices;

namespace Anvil.Native
{
  internal static partial class NWNXUtils
  {
    [LibraryImport("NWNX_Core", EntryPoint = "_ZN7NWNXLib5Utils9AsNWSAreaEP11CGameObject")]
    public static partial IntPtr AsNWSArea(IntPtr pObj);

    [LibraryImport("NWNX_Core", EntryPoint = "_ZN7NWNXLib5Utils23AsNWSAreaOfEffectObjectEP11CGameObject")]
    public static partial IntPtr AsNWSAreaOfEffectObject(IntPtr pObj);

    [LibraryImport("NWNX_Core", EntryPoint = "_ZN7NWNXLib5Utils13AsNWSCreatureEP11CGameObject")]
    public static partial IntPtr AsNWSCreature(IntPtr pObj);

    [LibraryImport("NWNX_Core", EntryPoint = "_ZN7NWNXLib5Utils9AsNWSDoorEP11CGameObject")]
    public static partial IntPtr AsNWSDoor(IntPtr pObj);

    [LibraryImport("NWNX_Core", EntryPoint = "_ZN7NWNXLib5Utils14AsNWSEncounterEP11CGameObject")]
    public static partial IntPtr AsNWSEncounter(IntPtr pObj);

    [LibraryImport("NWNX_Core", EntryPoint = "_ZN7NWNXLib5Utils9AsNWSItemEP11CGameObject")]
    public static partial IntPtr AsNWSItem(IntPtr pObj);

    [LibraryImport("NWNX_Core", EntryPoint = "_ZN7NWNXLib5Utils11AsNWSModuleEP11CGameObject")]
    public static partial IntPtr AsNWSModule(IntPtr pObj);

    [LibraryImport("NWNX_Core", EntryPoint = "_ZN7NWNXLib5Utils11AsNWSObjectEP11CGameObject")]
    public static partial IntPtr AsNWSObject(IntPtr pObj);

    [LibraryImport("NWNX_Core", EntryPoint = "_ZN7NWNXLib5Utils14AsNWSPlaceableEP11CGameObject")]
    public static partial IntPtr AsNWSPlaceable(IntPtr pObj);

    [LibraryImport("NWNX_Core", EntryPoint = "_ZN7NWNXLib5Utils16AsNWSSoundObjectEP11CGameObject")]
    public static partial IntPtr AsNWSSoundObject(IntPtr pObj);

    [LibraryImport("NWNX_Core", EntryPoint = "_ZN7NWNXLib5Utils10AsNWSStoreEP11CGameObject")]
    public static partial IntPtr AsNWSStore(IntPtr pObj);

    [LibraryImport("NWNX_Core", EntryPoint = "_ZN7NWNXLib5Utils12AsNWSTriggerEP11CGameObject")]
    public static partial IntPtr AsNWSTrigger(IntPtr pObj);

    [LibraryImport("NWNX_Core", EntryPoint = "_ZN7NWNXLib5Utils13AsNWSWaypointEP11CGameObject")]
    public static partial IntPtr AsNWSWaypoint(IntPtr pObj);

    [LibraryImport("NWNX_Core", EntryPoint = "_ZN7NWNXLib5Utils13GetGameObjectEj")]
    public static partial IntPtr GetGameObject(uint objectId);
  }
}
