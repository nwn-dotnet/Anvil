using System;
using Anvil.Services;

namespace Anvil.API
{
  public abstract class ObjectStorageVariableGuid : ObjectStorageVariable<Guid>
  {
    [Inject]
    internal ObjectStorageService ObjectStorageService { private get; init; }

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
    protected override string ObjectStoragePrefix => "NWNX_Object";
    protected override bool Persist => true;
  }

  internal sealed class InternalVariableGuid : ObjectStorageVariableGuid
  {
    protected override string ObjectStoragePrefix => "ANVIL_API";
    protected override bool Persist => true;

    internal sealed class Persistent : ObjectStorageVariableGuid
    {
      protected override string ObjectStoragePrefix => "ANVIL_API";
      protected override bool Persist => false;
    }
  }
}
