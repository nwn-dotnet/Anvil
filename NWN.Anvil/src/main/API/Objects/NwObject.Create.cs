using System;
using System.Collections.Generic;
using System.Reflection;
using Anvil.Internal;
using NWN.Core;
using NWN.Native.API;

namespace Anvil.API
{
  public abstract partial class NwObject
  {
    private static readonly Dictionary<Type, NativeObjectInfoAttribute> CachedTypeInfo = new Dictionary<Type, NativeObjectInfoAttribute>();

    /// <summary>
    /// Locates all objects of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of objects to search.</typeparam>
    /// <returns>An enumeration containing all objects of the specified type.</returns>
    public static IEnumerable<T> FindObjectsOfType<T>() where T : NwObject
    {
      for (uint currentArea = NWScript.GetFirstArea(); currentArea != Invalid; currentArea = NWScript.GetNextArea())
      {
        for (uint currentObj = NWScript.GetFirstObjectInArea(currentArea); currentObj != Invalid; currentObj = NWScript.GetNextObjectInArea(currentArea))
        {
          T? obj = currentObj.ToNwObjectSafe<T>();
          if (obj != null)
          {
            yield return obj;
          }
        }
      }
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

        for (i = 0, current = NWScript.GetObjectByTag(tag, i); current != Invalid; i++, current = NWScript.GetObjectByTag(tag, i))
        {
          T? obj = current.ToNwObjectSafe<T>();
          if (obj != null)
          {
            yield return obj;
          }
        }
      }
    }

    internal static NwObject? CreateInternal(Guid uuid)
    {
      return uuid == Guid.Empty ? null : CreateInternal(NWScript.GetObjectByUUID(uuid.ToUUIDString()));
    }

    internal static T? CreateInternal<T>(string template, Location location, bool useAppearAnim, string? newTag) where T : NwGameObject
    {
      ObjectTypes objectType = GetObjectType<T>();
      return NWScript.CreateObject((int)objectType, template, location, useAppearAnim.ToInt(), newTag ?? string.Empty).ToNwObject<T>();
    }

    internal static NwObject? CreateInternal(uint objectId)
    {
      // Not a valid object
      if (objectId == Invalid)
      {
        return null;
      }

      // The module object will never change, so to save performance we return the one we already have instead of finding a new one.
      if (objectId == NwModule.Instance)
      {
        return NwModule.Instance;
      }

      return CreateInternal(LowLevel.ServerExoApp.GetGameObject(objectId));
    }

    internal static NwObject? CreateInternal(ICGameObject? gameObject)
    {
      if (gameObject == null)
      {
        return null;
      }

      return gameObject switch
      {
        CNWSArea area => new NwArea(area),
        CNWSAreaOfEffectObject areaOfEffect => new NwAreaOfEffect(areaOfEffect),
        CNWSCreature creature => new NwCreature(creature),
        CNWSDoor door => new NwDoor(door),
        CNWSEncounter encounter => new NwEncounter(encounter),
        CNWSItem item => new NwItem(item),
        CNWSPlaceable placeable => new NwPlaceable(placeable),
        CNWSSoundObject soundObject => new NwSound(soundObject),
        CNWSStore store => new NwStore(store),
        CNWSTrigger trigger => new NwTrigger(trigger),
        CNWSWaypoint waypoint => new NwWaypoint(waypoint),
        CNWSObject => CreateFromVirtualType(gameObject),
        CGameObject => CreateFromVirtualType(gameObject),
        CNWSModule => NwModule.Instance,
        _ => null,
      };
    }

    internal static ObjectType GetNativeObjectType<T>() where T : NwObject
    {
      return GetNativeObjectInfo(typeof(T)).NativeObjectType;
    }

    internal static ObjectTypes GetObjectType<T>() where T : NwObject
    {
      return GetNativeObjectInfo(typeof(T)).ObjectType;
    }

    private static NwObject? CreateFromVirtualType(ICGameObject gameObject)
    {
      return (ObjectType)gameObject.m_nObjectType switch
      {
        ObjectType.Creature => new NwCreature(gameObject.AsNWSCreature()),
        ObjectType.Item => new NwItem(gameObject.AsNWSItem()),
        ObjectType.Placeable => new NwPlaceable(gameObject.AsNWSPlaceable()),
        ObjectType.Module => NwModule.Instance,
        ObjectType.Area => new NwArea(gameObject.AsNWSArea()),
        ObjectType.Trigger => new NwTrigger(gameObject.AsNWSTrigger()),
        ObjectType.Door => new NwDoor(gameObject.AsNWSDoor()),
        ObjectType.Waypoint => new NwWaypoint(gameObject.AsNWSWaypoint()),
        ObjectType.Encounter => new NwEncounter(gameObject.AsNWSEncounter()),
        ObjectType.Store => new NwStore(gameObject.AsNWSStore()),
        ObjectType.Sound => new NwSound(gameObject.AsNWSSoundObject()),
        ObjectType.AreaOfEffect => new NwAreaOfEffect(gameObject.AsNWSAreaOfEffectObject()),
        _ => null,
      };
    }

    private static NativeObjectInfoAttribute GetNativeObjectInfo(Type type)
    {
      if (!CachedTypeInfo.TryGetValue(type, out NativeObjectInfoAttribute? nativeInfo))
      {
        nativeInfo = type.GetCustomAttribute<NativeObjectInfoAttribute>();
        if (nativeInfo != null)
        {
          CachedTypeInfo[type] = nativeInfo;
        }
      }

      if (nativeInfo == null)
      {
        throw new InvalidOperationException($"Type \"{type.FullName}\" does not have a mapped native object!");
      }

      return nativeInfo;
    }
  }
}
