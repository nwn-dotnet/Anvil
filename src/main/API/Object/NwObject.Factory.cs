using System;
using NWM.Internal;
using NWN;
using Object = NWM.Internal.Object;

namespace NWM.API
{
  public partial class NwObject
  {
    private static NwModule cachedModule;

    private static NwModule moduleObj
    {
      get
      {
        if (cachedModule != null)
        {
          return cachedModule;
        }

        cachedModule = new NwModule(NWScript.GetModule());
        return cachedModule;
      }
    }

    public static NwObject Deserialize(string serializedObject)
    {
      return CreateInternal(Object.Deserialize(serializedObject));
    }

    public static T FromUUID<T>(Guid uuid) where T : NwObject
    {
      return (T)FromUUID(uuid);
    }

    public static T FromTag<T>(string tag) where T : NwObject
    {
      return (T) FromTag(tag);
    }

    public static NwObject FromTag(string tag)
    {
      return NWScript.GetObjectByTag(tag).ToNwObject();
    }

    public static NwObject FromUUID(Guid uuid)
    {
      return CreateInternal(NWScript.GetObjectByUUID(uuid.ToString("N")));
    }

    internal static T CreateInternal<T>(ObjectType objectType, string template, Location location, bool useAppearAnim, string newTag) where T : NwObject
    {
      return NWScript.CreateObject((int)objectType, template, location, useAppearAnim.ToInt(), newTag).ToNwObject<T>();
    }

    internal static NwObject CreateInternal(uint objectId)
    {
      switch (NWMInterop.GetObjectType(objectId))
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
          return moduleObj;
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
        default:
          return new NwObject(objectId);
      }
    }
  }
}