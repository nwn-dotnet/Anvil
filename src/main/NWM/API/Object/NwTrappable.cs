using NWN;

namespace NWM.API
{
  public abstract class NwTrappable : NwGameObject
  {
    public bool Locked
    {
      get => NWScript.GetLocked(this).ToBool();
      set => NWScript.SetLocked(this, value.ToInt());
    }

    public bool LockKeyRequired
    {
      get => NWScript.GetLockKeyRequired(this).ToBool();
      set => NWScript.SetLockKeyRequired(this, value.ToInt());
    }

    public string LockKeyTag
    {
      get => NWScript.GetLockKeyTag(this);
      set => NWScript.SetLockKeyTag(this, value);
    }

    internal NwTrappable(uint objectId) : base(objectId) {}
  }
}