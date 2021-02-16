using System;
using System.Collections.Generic;
using System.Reflection;
using NWN.API.Constants;
using NWN.Core;
using NWN.Core.NWNX;
using NWN.Native.API;

namespace NWN.API
{
  public partial class NwObject
  {
    private static readonly Dictionary<Type, NativeObjectInfoAttribute> CachedTypeInfo = new Dictionary<Type, NativeObjectInfoAttribute>();

    public static T Deserialize<T>(string serializedObject) where T : NwObject
    {
      return (T) Deserialize(serializedObject);
    }

    public static NwObject Deserialize(string serializedObject)
    {
      return CreateInternal(ObjectPlugin.Deserialize(serializedObject));
    }

    /// <summary>
    /// Locates all objects of that have the specified tag.
    /// </summary>
    /// <param name="tags">The tag/s of the objects to locate.</param>
    /// <returns>An enumeration containing all objects with the specified tags.</returns>
    public static IEnumerable<NwObject> FindObjectsWithTag(params string[] tags)
    {
      return FindObjectsWithTag<NwObject>(tags);
    }

    /// <summary>
    /// Locates all objects of the specified type that have the specified tag.
    /// </summary>
    /// <param name="tags">The tag/s of the objects to locate.</param>
    /// <typeparam name="T">The type of objects to search.</typeparam>
    /// <returns>An enumeration containing all objects with the specified tags.</returns>
    public static IEnumerable<T> FindObjectsWithTag<T>(params string[] tags) where T : NwObject
    {
      foreach (string tag in tags)
      {
        int i;
        uint current;

        for (i = 0, current = NWScript.GetObjectByTag(tag, i); current != INVALID; i++, current = NWScript.GetObjectByTag(tag, i))
        {
          T obj = current.ToNwObjectSafe<T>();
          if (obj != null)
          {
            yield return obj;
          }
        }
      }
    }

    /// <summary>
    /// Locates all objects of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of objects to search.</typeparam>
    /// <returns>An enumeration containing all objects of the specified type.</returns>
    public static IEnumerable<T> FindObjectsOfType<T>() where T : NwObject
    {
      for (uint currentArea = NWScript.GetFirstArea(); currentArea != INVALID; currentArea = NWScript.GetNextArea())
      {
        for (uint currentObj = NWScript.GetFirstObjectInArea(currentArea); currentObj != INVALID; currentObj = NWScript.GetNextObjectInArea(currentArea))
        {
          T obj = currentObj.ToNwObjectSafe<T>();
          if (obj != null)
          {
            yield return obj;
          }
        }
      }
    }

    internal static NwObject CreateInternal(Guid uuid)
    {
      return uuid == Guid.Empty ? null : CreateInternal(NWScript.GetObjectByUUID(uuid.ToUUIDString()));
    }

    internal static T CreateInternal<T>(string template, Location location, bool useAppearAnim, string newTag) where T : NwObject
    {
      ObjectTypes objectType = GetObjectType<T>();
      return NWScript.CreateObject((int) objectType, template, location, useAppearAnim.ToInt(), newTag).ToNwObject<T>();
    }

    internal static NwObject CreateInternal(uint objectId)
    {
      // Not a valid object
      if (objectId == NwObject.INVALID)
      {
        return null;
      }

      // The module object will never change, so to save performance we return the one we already have instead of finding a new one.
      if (objectId == NwModule.Instance)
      {
        return NwModule.Instance;
      }

      return ConstructManagedObject(objectId);
    }

    private static NwObject ConstructManagedObject(uint objectId)
    {
      ICGameObject gameObject = LowLevel.ServerExoApp.GetGameObject(objectId);
      if (gameObject == null)
      {
        return null;
      }

      return (ObjectType)gameObject.m_nObjectType switch
      {
        ObjectType.Creature => NWScript.GetIsPC(objectId) == NWScript.TRUE
          ? new NwPlayer(objectId, gameObject.AsNWSCreature(), LowLevel.ServerExoApp.GetClientObjectByObjectId(objectId), gameObject.AsNWSPlayerTURD())
          : new NwCreature(objectId, gameObject.AsNWSCreature()),
        ObjectType.Item => new NwItem(objectId, gameObject.AsNWSItem()),
        ObjectType.Placeable => new NwPlaceable(objectId, gameObject.AsNWSPlaceable()),
        ObjectType.Module => NwModule.Instance,
        ObjectType.Area => new NwArea(objectId, gameObject.AsNWSArea()),
        ObjectType.Trigger => new NwTrigger(objectId, gameObject.AsNWSTrigger()),
        ObjectType.Door => new NwDoor(objectId, gameObject.AsNWSDoor()),
        ObjectType.Waypoint => new NwWaypoint(objectId, gameObject.AsNWSWaypoint()),
        ObjectType.Encounter => new NwEncounter(objectId, gameObject.AsNWSEncounter()),
        ObjectType.Store => new NwStore(objectId, gameObject.AsNWSStore()),
        ObjectType.Sound => new NwSound(objectId, gameObject.AsNWSSoundObject()),
        ObjectType.AreaOfEffect => new NwAreaOfEffect(objectId, gameObject.AsNWSAreaOfEffectObject()),
        _ => new NwObject(objectId),
      };
    }

    internal static ObjectTypes GetObjectType<T>() where T : NwObject
    {
      return GetNativeObjectInfo(typeof(T)).ObjectType;
    }

    internal static ObjectType GetNativeObjectType<T>() where T : NwObject
    {
      return GetNativeObjectInfo(typeof(T)).NativeObjectType;
    }

    private static NativeObjectInfoAttribute GetNativeObjectInfo(Type type)
    {
      if (!CachedTypeInfo.TryGetValue(type, out NativeObjectInfoAttribute nativeInfo))
      {
        nativeInfo = type.GetCustomAttribute<NativeObjectInfoAttribute>();
        CachedTypeInfo[type] = nativeInfo;
      }

      if (nativeInfo == null)
      {
        throw new InvalidOperationException($"Type \"{type.FullName}\" does not have a mapped native object!");
      }

      return nativeInfo;
    }
  }
}
