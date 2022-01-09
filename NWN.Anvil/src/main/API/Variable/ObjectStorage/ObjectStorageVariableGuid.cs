using System;
using Anvil.Services;

namespace Anvil.API
{
  public abstract class ObjectStorageVariableGuid : ObjectStorageVariable<Guid>
  {
    [Inject]
    private static ObjectStorageService ObjectStorageService { get; set; }

    public sealed override bool HasValue => ObjectStorageService.TryGetObjectStorage(Object, out ObjectStorage objectStorage) && objectStorage.ContainsString(ObjectStoragePrefix, Key);

    public sealed override Guid Value
    {
      get
      {
        string stored = ObjectStorageService.GetObjectStorage(Object).GetString(ObjectStoragePrefix, Key);
        return string.IsNullOrEmpty(stored) ? Guid.Empty : Guid.Parse(stored);
      }
      set => ObjectStorageService.GetObjectStorage(Object).Set(ObjectStoragePrefix, Key, value.ToUUIDString(), true);
    }

    protected sealed override string VariableTypePrefix => "PERSTR!";

    public sealed override void Delete()
    {
      ObjectStorageService.GetObjectStorage(Object).Remove(ObjectStoragePrefix, Key);
    }
  }

  public sealed class PersistentVariableGuid : ObjectStorageVariableGuid
  {
    protected override bool Persist => true;
    protected override string ObjectStoragePrefix => "NWNX_Object";
  }

  internal sealed class InternalVariableGuid : ObjectStorageVariableGuid
  {
    protected override bool Persist => true;
    protected override string ObjectStoragePrefix => "ANVIL_API";

    internal sealed class Persistent : ObjectStorageVariableGuid
    {
      protected override bool Persist => false;
      protected override string ObjectStoragePrefix => "ANVIL_API";
    }
  }
}
