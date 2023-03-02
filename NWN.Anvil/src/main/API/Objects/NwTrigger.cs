using System;
using System.Collections.Generic;
using NWN.Core;
using NWN.Native.API;

namespace Anvil.API
{
  /// <summary>
  /// A trigger volume entity.
  /// </summary>
  [NativeObjectInfo(ObjectTypes.Trigger, ObjectType.Trigger)]
  public sealed partial class NwTrigger : NwTrappable
  {
    private readonly CNWSTrigger trigger;

    internal CNWSTrigger Trigger
    {
      get
      {
        AssertObjectValid();
        return trigger;
      }
    }

    internal NwTrigger(CNWSTrigger trigger) : base(trigger)
    {
      this.trigger = trigger;
    }

    public static NwTrigger? Create(string template, Location location, float size = 2.0f, string? newTag = null)
    {
      if (string.IsNullOrEmpty(template))
      {
        return default;
      }

      CNWSArea area = location.Area.Area;
      Vector position = location.Position.ToNativeVector();
      Vector orientation = location.Rotation.ToVectorOrientation().ToNativeVector();

      CNWSTrigger? trigger = null;
      bool result = NativeUtils.CreateFromResRef(ResRefType.UTT, template, (resGff, resStruct) =>
      {
        trigger = new CNWSTrigger();
        GC.SuppressFinalize(trigger);
        trigger.m_sTemplate = template.ToExoString();
        trigger.LoadTrigger(resGff, resStruct);
        trigger.LoadVarTable(resGff, resStruct);

        trigger.SetPosition(position);
        trigger.SetOrientation(orientation);
        trigger.CreateNewGeometry(size, position, area);

        if (!string.IsNullOrEmpty(newTag))
        {
          trigger.m_sTag = newTag.ToExoString();
          NwModule.Instance.Module.AddObjectToLookupTable(trigger.m_sTag, trigger.m_idSelf);
        }

        trigger.AddToArea(area, position.x, position.y, position.z, true.ToInt());
      });

      return result && trigger != null ? trigger.ToNwObject<NwTrigger>() : null;
    }

    public static NwTrigger? Deserialize(byte[] serialized)
    {
      CNWSTrigger? trigger = null;
      bool result = NativeUtils.DeserializeGff(serialized, (resGff, resStruct) =>
      {
        if (!resGff.IsValidGff("UTT"))
        {
          return false;
        }

        trigger = new CNWSTrigger(Invalid);
        if (trigger.LoadTrigger(resGff, resStruct).ToBool())
        {
          trigger.LoadObjectState(resGff, resStruct);
          trigger.m_oidArea = Invalid;
          GC.SuppressFinalize(trigger);
          return true;
        }

        trigger.Dispose();
        return false;
      });

      return result && trigger != null ? trigger.ToNwObject<NwTrigger>() : null;
    }

    public static implicit operator CNWSTrigger?(NwTrigger? trigger)
    {
      return trigger?.Trigger;
    }

    public override NwTrigger Clone(Location location, string? newTag = null, bool copyLocalState = true)
    {
      return CloneInternal<NwTrigger>(location, newTag, copyLocalState);
    }

    /// <summary>
    /// Gets all objects of the given type that are currently in this trigger.
    /// </summary>
    /// <typeparam name="T">The type of objects to return.</typeparam>
    /// <returns>An enumerable containing all objects currently in the trigger.</returns>
    public IEnumerable<T> GetObjectsInTrigger<T>() where T : NwGameObject
    {
      int objType = (int)GetObjectType<T>();
      for (uint obj = NWScript.GetFirstInPersistentObject(this, objType); obj != Invalid; obj = NWScript.GetNextInPersistentObject(this, objType))
      {
        yield return obj.ToNwObject<T>()!;
      }
    }

    /// <summary>
    /// Gets all objects of the given types that are currently in this trigger.
    /// </summary>
    /// <param name="objectTypes">The types of object to return.</param>
    /// <returns>An enumerable containing all objects currently in the trigger.</returns>
    public IEnumerable<NwGameObject> GetObjectsInTrigger(ObjectTypes objectTypes = ObjectTypes.All)
    {
      int objType = (int)objectTypes;
      for (uint obj = NWScript.GetFirstInPersistentObject(this, objType); obj != Invalid; obj = NWScript.GetNextInPersistentObject(this, objType))
      {
        yield return obj.ToNwObject<NwGameObject>()!;
      }
    }

    public override byte[]? Serialize()
    {
      return NativeUtils.SerializeGff("UTT", (resGff, resStruct) =>
      {
        Trigger.SaveObjectState(resGff, resStruct);
        return Trigger.SaveTrigger(resGff, resStruct).ToBool();
      });
    }

    internal override void RemoveFromArea()
    {
      if (IsTrapped)
      {
        Area?.Area.m_pTrapList.Remove(this);
      }

      Trigger.RemoveFromArea();
    }

    private protected override void AddToArea(CNWSArea area, float x, float y, float z)
    {
      Trigger.AddToArea(area, x, y, z, true.ToInt());

      // If the trigger is trapped it needs to be added to the area's trap list for it to be detectable by players.
      if (IsTrapped)
      {
        area.m_pTrapList.Add(this);
      }
    }
  }
}
