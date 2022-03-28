using Anvil.Services;

namespace Anvil.API
{
  public abstract class ObjectStorageVariableInt : ObjectStorageVariable<int>
  {
    [Inject]
    internal ObjectStorageService ObjectStorageService { private get; init; }

    public sealed override bool HasValue => ObjectStorageService.TryGetObjectStorage(Object, out ObjectStorage objectStorage) && objectStorage.ContainsInt(ObjectStoragePrefix, Key);

    public sealed override int Value
    {
      get => ObjectStorageService.GetObjectStorage(Object).GetInt(ObjectStoragePrefix, Key).GetValueOrDefault();
      set => ObjectStorageService.GetObjectStorage(Object).Set(ObjectStoragePrefix, Key, value, Persist);
    }

    protected sealed override string VariableTypePrefix => "PERINT!";

    public sealed override void Delete()
    {
      ObjectStorageService.GetObjectStorage(Object).Remove(ObjectStoragePrefix, Key);
    }
  }

  public sealed class PersistentVariableInt : ObjectStorageVariableInt
  {
    protected override string ObjectStoragePrefix => "NWNX_Object";
    protected override bool Persist => true;
  }

  internal sealed class InternalVariableInt : ObjectStorageVariableInt
  {
    protected override string ObjectStoragePrefix => "ANVIL_API";
    protected override bool Persist => false;

    internal sealed class Persistent : ObjectStorageVariableInt
    {
      protected override string ObjectStoragePrefix => "ANVIL_API";
      protected override bool Persist => true;
    }
  }
}
