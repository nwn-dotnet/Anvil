using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NWN.Core;
using NWN.Core.NWNX;

namespace NWN.API
{
  [DebuggerDisplay("{" + nameof(Name) + "}")]
  public class NwObject : IEquatable<NwObject>
  {
    internal const uint INVALID = NWScript.OBJECT_INVALID;
    protected readonly uint ObjectId;

    public static implicit operator uint(NwObject obj)
    {
      return obj == null ? INVALID : obj.ObjectId;
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

    /// <summary>
    /// Returns the resource reference used to create this object.
    /// </summary>
    public string ResRef => NWScript.GetResRef(this);

    /// <summary>
    /// Returns true if this is a valid object.
    /// </summary>
    public bool IsValid => NWScript.GetIsObjectValid(this).ToBool();

    /// <summary>
    /// Gets or sets the name of this object.
    /// </summary>
    public string Name
    {
      get => NWScript.GetName(this);
      set => NWScript.SetName(this, value);
    }

    /// <summary>
    /// Gets the original description for this object as defined in the toolset.
    /// </summary>
    public string OriginalDescription => NWScript.GetDescription(this, true.ToInt());

    /// <summary>
    /// Gets or sets the description for this object.
    /// </summary>
    public string Description
    {
      get => NWScript.GetDescription(this);
      set => NWScript.SetDescription(this, value);
    }

    /// <summary>
    /// Gets or sets the tag for this object.
    /// </summary>
    public string Tag
    {
      get => NWScript.GetTag(this);
      set => NWScript.SetTag(this, value);
    }

    /// <summary>
    /// Notifies then awaits for this object to become the current active object for the purpose of implicitly assigned values (e.g. effect creators)<br/>
    /// If the current active object is already this object, then the code runs immediately. Otherwise, it will be run with all other closures.<br/>
    /// This is the async equivalent of AssignCommand in NWScript.
    /// </summary>
    public async Task WaitForObjectContext()
    {
      if (NManager.Instance.ObjectSelf == this)
      {
        return;
      }

      if (!IsValid)
      {
        throw new InvalidOperationException("Cannot wait for the context of an invalid object.");
      }

      TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
      NWScript.AssignCommand(this, () =>
      {
        tcs.SetResult(true);
      });

      await tcs.Task;
    }

    /// <summary>
    /// Inserts the function call aCommand into the Action Queue, ensuring the calling object will perform actions in a particular order.
    /// </summary>
    public async Task AddActionToQueue(ActionDelegate action)
    {
      await WaitForObjectContext();
      NWScript.ActionDoCommand(action);
    }

    /// <summary>
    ///  Clear all the object's actions.
    ///  * No return value, but if an error occurs, the log file will contain
    ///    "ClearAllActions failed.".
    ///  - clearCombatState: if true, this will immediately clear the combat state
    ///    on a creature, which will stop the combat music and allow them to rest,
    ///    engage in dialog, or other actions that they would normally have to wait for.
    /// </summary>
    public async Task ClearActionQueue(bool clearCombatState = false)
    {
      await WaitForObjectContext();
      NWScript.ClearAllActions(clearCombatState.ToInt());
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
      if (ReferenceEquals(null, other))
      {
        return false;
      }

      if (ReferenceEquals(this, other))
      {
        return true;
      }

      return ObjectId == other.ObjectId;
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj))
      {
        return false;
      }

      if (ReferenceEquals(this, obj))
      {
        return true;
      }

      if (obj.GetType() != this.GetType())
      {
        return false;
      }

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