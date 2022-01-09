using Anvil.Services;

namespace Anvil.API
{
  public abstract class ObjectStorageVariableInt : ObjectStorageVariable<int>
  {
    [Inject]
    private static ObjectStorageService ObjectStorageService { get; set; }

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
    protected override bool Persist => true;
    protected override string ObjectStoragePrefix => "NWNX_Object";
  }

  internal sealed class InternalVariableInt : ObjectStorageVariableInt
  {
    protected override bool Persist => true;
    protected override string ObjectStoragePrefix => "ANVIL_API";

    internal sealed class Persistent : ObjectStorageVariableInt
    {
      protected override bool Persist => false;
      protected override string ObjectStoragePrefix => "ANVIL_API";
    }
  }
}
