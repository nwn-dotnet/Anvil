using System;
using NWN;
using Object = NWM.Internal.Object;

namespace NWM.API
{
  public partial class NwObject : IEquatable<NwObject>
  {
    protected const uint INVALID = NWScript.OBJECT_INVALID;
    protected readonly uint ObjectId;

    public static implicit operator uint(NwObject obj)
    {
      return obj == null ? NWScript.OBJECT_INVALID : obj.ObjectId;
    }

    protected NwObject(uint objectId)
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

    public virtual string Name
    {
      get => NWScript.GetName(this);
      set => NWScript.SetName(this, value);
    }

    public string Tag
    {
      get => NWScript.GetTag(this);
      set => NWScript.SetTag(this, value);
    }

    public void AssignCommand(ActionDelegate action)
    {
      NWScript.AssignCommand(this, action);
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
      return Object.Serialize(this);
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