using System;
using Anvil.Services;

namespace Anvil.API
{
  public sealed class PersistentVariableGuid : PersistentVariable<Guid>
  {
    [Inject]
    private static ObjectStorageService ObjectStorageService { get; set; }

    public override bool HasValue
    {
      get => ObjectStorageService.TryGetObjectStorage(Object, out ObjectStorage objectStorage) && objectStorage.ContainsString(Prefix, Key);
    }

    public override Guid Value
    {
      get
      {
        string stored = ObjectStorageService.GetObjectStorage(Object).GetString(Prefix, Key);
        return string.IsNullOrEmpty(stored) ? Guid.Empty : Guid.Parse(stored);
      }
      set => ObjectStorageService.GetObjectStorage(Object).Set(Prefix, Key, value.ToUUIDString(), true);
    }

    protected override string KeyPrefix
    {
      get => "PERSTR!";
    }

    public override void Delete()
    {
      ObjectStorageService.GetObjectStorage(Object).Remove(Prefix, Key);
    }
  }
}
