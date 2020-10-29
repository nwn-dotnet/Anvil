using System;
using System.Collections.Generic;
using System.Reflection;
using NWN.API.Constants;
using NWN.Core;
using NWN.Core.NWNX;
using NWNX.API.Constants;

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
      switch ((InternalObjectType) ObjectPlugin.GetInternalObjectType(objectId))
      {
        case InternalObjectType.Invalid:
          return null;
        case InternalObjectType.Creature:
          return NWScript.GetIsPC(objectId) == NWScript.TRUE ? new NwPlayer(objectId) : new NwCreature(objectId);
        case InternalObjectType.Item:
          return new NwItem(objectId);
        case InternalObjectType.Placeable:
          return new NwPlaceable(objectId);
        case InternalObjectType.Module:
          return NwModule.Instance;
        case InternalObjectType.Area:
          return new NwArea(objectId);
        case InternalObjectType.Trigger:
          return new NwTrigger(objectId);
        case InternalObjectType.Door:
          return new NwDoor(objectId);
        case InternalObjectType.Waypoint:
          return new NwWaypoint(objectId);
        case InternalObjectType.Encounter:
          return new NwEncounter(objectId);
        case InternalObjectType.Store:
          return new NwStore(objectId);
        case InternalObjectType.Sound:
          return new NwSound(objectId);
        default:
          return new NwObject(objectId);
      }
    }

    internal static ObjectTypes GetObjectType<T>() where T : NwObject
    {
      return GetNativeObjectInfo(typeof(T)).ObjectType;
    }

    internal static InternalObjectType GetInternalObjectType<T>() where T : NwObject
    {
      return GetNativeObjectInfo(typeof(T)).InternalObjectType;
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
