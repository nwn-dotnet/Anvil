using System;
using NWN.Services;

namespace NWN.API
{
  public sealed class PersistentVariableGuid : PersistentVariable<Guid>
  {
    public override bool HasValue
    {
      get => ObjectStorageService.TryGetObjectStorage(Object, out ObjectStorage objectStorage) && objectStorage.ContainsInt(Prefix, Name);
    }

    public override void Delete()
    {
      ObjectStorageService.GetObjectStorage(Object).Remove(Prefix, Name);
    }

    public override Guid Value
    {
      get
      {
        string stored = ObjectStorageService.GetObjectStorage(Object).GetString(Prefix, Name);
        return string.IsNullOrEmpty(stored) ? Guid.Empty : Guid.Parse(stored);
      }
      set => ObjectStorageService.GetObjectStorage(Object).Set(Prefix, Name, value.ToUUIDString());
    }
  }
}
