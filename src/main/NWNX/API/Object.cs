using System.Numerics;
using NWN.API;
using NWN.API.Constants;
using NWN.Core.NWNX;

namespace NWNX.API
{
  public static class Object
  {
    static Object() => 
      PluginUtils.AssertPluginExists<ObjectPlugin>();

    public static int GetLocalVariableCount(this NwObject obj) => 
      ObjectPlugin.GetLocalVariableCount(obj);

    public static LocalVariable GetLocalVariable(this NwObject obj, int index) => 
      ObjectPlugin.GetLocalVariable(obj, index);

    public static NwObject StringToObject(string id) =>
      ObjectPlugin.StringToObject(id).ToNwObject();

    public static int GetCurrentHitPoints(this NwObject obj) =>
      ObjectPlugin.GetCurrentHitPoints(obj);

    public static void SetCurrentHitPoints(this NwObject obj, int hp) =>
      ObjectPlugin.SetCurrentHitPoints(obj, hp);

    public static void Serialize(this NwObject obj) =>
      ObjectPlugin.Serialize(obj);

    public static NwObject Deserialize(string id) =>
      ObjectPlugin.Deserialize(id).ToNwObject();

    public static string GetDialogResref(this NwGameObject obj) =>
      ObjectPlugin.GetDialogResref(obj);

    public static void SetDialogResref(this NwGameObject obj, string dialog) =>
      ObjectPlugin.SetDialogResref(obj, dialog);

    public static void SetAppearance(this NwPlaceable obj, int appearance) =>
      ObjectPlugin.SetAppearance(obj, appearance);

    public static void GetAppearance(this NwPlaceable obj) =>
      ObjectPlugin.GetAppearance(obj);

    public static bool GetHasVisualEffect(this NwObject obj, VfxType vfx) =>
      ObjectPlugin.GetHasVisualEffect(obj, (int)vfx).ToBool();

    public static int GetDamageImmunity(this NwObject obj, DamageType type) =>
      ObjectPlugin.GetDamageImmunity(obj, (int)type);

    public static bool GetAutoRemoveKey(this NwDoor obj) =>
      ObjectPlugin.GetAutoRemoveKey(obj).ToBool();

    public static void SetAutoRemoveKey(this NwDoor obj, bool removeKey) =>
      ObjectPlugin.SetAutoRemoveKey(obj, removeKey.ToInt());

    public static string GetTriggerGeometry(this NwTrigger obj) =>
      ObjectPlugin.GetTriggerGeometry(obj);

    public static void SetTriggerGeometry(this NwTrigger obj, string value) =>
      ObjectPlugin.SetTriggerGeometry(obj, value);

    public static void AddIconEffect(this NwPlayer obj, int icon, float duration = default) =>
      ObjectPlugin.AddIconEffect(obj, icon, duration);

    public static void RemoveIconEffect(this NwPlayer obj, int icon) =>
      ObjectPlugin.RemoveIconEffect(obj, icon);

    public static void Export(this NwGameObject obj, string fileName) =>
      ObjectPlugin.Export(fileName, obj);

    public static void AddToArea(this NwGameObject gameObject, Location location) =>
      ObjectPlugin.AddToArea(gameObject, location.Area, location.Position);

    public static void AddToArea(this NwGameObject gameObject, NwArea area, Vector3 position) =>
      ObjectPlugin.AddToArea(gameObject, area, position);

    public static void AcquireItem(this NwGameObject gameObject, NwItem item) =>
      ObjectPlugin.AcquireItem(gameObject, item);

    public static bool CheckFit(this NwGameObject gameObject, NwItem item) => 
      CheckFit(gameObject, item.BaseItemType);

    public static bool CheckFit(this NwGameObject gameObject, BaseItemType baseItemType) => 
      ObjectPlugin.CheckFit(gameObject, (int) baseItemType).ToBool();

    public static string GetPersistentString(this NwObject obj, string key) => 
      ObjectPlugin.GetString(obj, key);

    public static void SetPersistentString(this NwObject obj, string key, string value) =>
      ObjectPlugin.SetString(obj, key, value, true.ToInt());

    public static bool GetIsStatic(this NwPlaceable placeable) => 
      ObjectPlugin.GetPlaceableIsStatic(placeable).ToBool();

    public static void SetIsStatic(this NwPlaceable placeable, bool value) =>
      ObjectPlugin.SetPlaceableIsStatic(placeable, value.ToInt());

    public static bool GetPositionIsInTrigger(this NwObject obj, Location location) => 
      ObjectPlugin.GetPositionIsInTrigger(obj, location.Position).ToBool();

    public static Constants.LocalVariableType GetInternalObjectType(this NwObject obj) => 
      (Constants.LocalVariableType)ObjectPlugin.GetInternalObjectType(obj);

    public static void AcquireItem(this NwObject obj, NwItem item) =>
      ObjectPlugin.AcquireItem(obj, item);

    public static void SetFacing(this NwObject obj, float value) =>
      ObjectPlugin.SetFacing(obj, value);

    public static void ClearSpellEffectsOnOthers(this NwObject obj) =>
      ObjectPlugin.ClearSpellEffectsOnOthers(obj);

    public static string PeekUUID(this NwObject obj) => 
      ObjectPlugin.PeekUUID(obj);

    public static bool GetDoorHasVisibleModel(this NwDoor obj) => 
      ObjectPlugin.GetDoorHasVisibleModel(obj).ToBool();

    public static bool GetIsDestroyable(this NwObject obj) => 
      ObjectPlugin.GetIsDestroyable(obj).ToBool();
  }
}
