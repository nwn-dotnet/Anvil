using System;
using System.Diagnostics;
using NWN;
using NWNX;

namespace NWM.API
{
  [DebuggerDisplay("{" + nameof(Name) + "}")]
  public class NwObject : IEquatable<NwObject>
  {
    internal const uint INVALID = NWScript.OBJECT_INVALID;
    protected readonly uint ObjectId;

    public static implicit operator uint(NwObject obj)
    {
      return obj == null ? NWScript.OBJECT_INVALID : obj.ObjectId;
    }

    internal NwObject(uint objectId)
    {
      ObjectId = objectId;
    }

    public Guid UUID
    {
      get
      {
        if (this == INVALID)
        {
          return Guid.Empty;
        }

        // TODO - Better Handle UUID conflicts.
        string uid = NWScript.GetObjectUUID(this);
        if (string.IsNullOrEmpty(uid))
        {
          ForceRefreshUUID();
          uid = NWScript.GetObjectUUID(this);
        }

        return Guid.TryParse(uid, out Guid guid) ? guid : Guid.Empty;
      }
    }

    public string ResRef => NWScript.GetResRef(this);
    public bool IsValid => NWScript.GetIsObjectValid(this).ToBool();

    public string Name
    {
      get => NWScript.GetName(this);
      set => NWScript.SetName(this, value);
    }

    public string OriginalDescription => NWScript.GetDescription(this, true.ToInt());

    public string Description
    {
      get => NWScript.GetDescription(this);
      set => NWScript.SetDescription(this, value);
    }

    public string Tag
    {
      get => NWScript.GetTag(this);
      set => NWScript.SetTag(this, value);
    }

    /// <summary>
    /// Assign the specified action to this object
    /// </summary>
    internal void AssignCommand(ActionDelegate action) => NWScript.AssignCommand(this, action);

    protected void ExecuteOnSelf(ActionDelegate action)
    {
      if (this == Internal.ObjectSelf)
      {
        action();
      }
      else
      {
        AssignCommand(action);
      }
    }

    /// <summary>
    /// Inserts the function call aCommand into the Action Queue, ensuring the calling object will perform actions in a particular order.
    /// </summary>
    public void AddActionToQueue(ActionDelegate action)
    {
      ExecuteOnSelf(() => NWScript.ActionDoCommand(action));
    }

    /// <summary>
    ///  Clear all the object's actions.
    ///  * No return value, but if an error occurs, the log file will contain
    ///    "ClearAllActions failed.".
    ///  - clearCombatState: if true, this will immediately clear the combat state
    ///    on a creature, which will stop the combat music and allow them to rest,
    ///    engage in dialog, or other actions that they would normally have to wait for.
    /// </summary>
    public void ClearActionQueue(bool clearCombatState = false)
    {
      ExecuteOnSelf(() => NWScript.ClearAllActions(clearCombatState.ToInt()));
    }

    public LocalBool GetLocalBool(string name)
    {
      return new LocalBool(this, name);
    }

    public LocalInt GetLocalInt(string name)
    {
      return new LocalInt(this, name);
    }

    public LocalFloat GetLocalFloat(string name)
    {
      return new LocalFloat(this, name);
    }

    public LocalString GetLocalString(string name)
    {
      return new LocalString(this, name);
    }

    public LocalLocation GetLocalLocation(string name)
    {
      return new LocalLocation(this, name);
    }

    public LocalObject GetLocalObject(string name)
    {
      return new LocalObject(this, name);
    }

    public LocalUUID GetLocalUUID(string name)
    {
      return new LocalUUID(this, name);
    }

    public bool HasClashingUUID()
    {
      return string.IsNullOrEmpty(NWScript.GetObjectUUID(this));
    }

    public void ForceRefreshUUID()
    {
      NWScript.ForceRefreshObjectUUID(this);
    }

    public string Serialize()
    {
      return ObjectPlugin.Serialize(this);
    }

    public bool Equals(NwObject other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return ObjectId == other.ObjectId;
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != this.GetType()) return false;
      return Equals((NwObject) obj);
    }

    public override int GetHashCode()
    {
      return (int) ObjectId;
    }

    public static bool operator ==(NwObject left, NwObject right)
    {
      return Equals(left, right);
    }

    public static bool operator !=(NwObject left, NwObject right)
    {
      return !Equals(left, right);
    }
  }
}