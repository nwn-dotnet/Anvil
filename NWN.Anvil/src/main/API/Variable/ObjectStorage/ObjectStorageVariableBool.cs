using Anvil.Services;

namespace Anvil.API
{
  public abstract class ObjectStorageVariableBool : ObjectStorageVariable<bool>
  {
    [Inject]
    private static ObjectStorageService ObjectStorageService { get; set; }

    public sealed override bool HasValue => ObjectStorageService.TryGetObjectStorage(Object, out ObjectStorage objectStorage) && objectStorage.ContainsInt(ObjectStoragePrefix, Key);

    public sealed override bool Value
    {
      get => ObjectStorageService.GetObjectStorage(Object).GetInt(ObjectStoragePrefix, Key).GetValueOrDefault().ToBool();
      set => ObjectStorageService.GetObjectStorage(Object).Set(ObjectStoragePrefix, Key, value.ToInt(), Persist);
    }

    protected sealed override string VariableTypePrefix => "PERINT!";

    public sealed override void Delete()
    {
      ObjectStorageService.GetObjectStorage(Object).Remove(ObjectStoragePrefix, Key);
    }
  }

  public sealed class PersistentVariableBool : ObjectStorageVariableBool
  {
    protected override bool Persist => true;
    protected override string ObjectStoragePrefix => "NWNX_Object";
  }

  internal sealed class InternalVariableBool : ObjectStorageVariableBool
  {
    protected override bool Persist => true;
    protected override string ObjectStoragePrefix => "ANVIL_API";

    internal sealed class Persistent : ObjectStorageVariableBool
    {
      protected override bool Persist => false;
      protected override string ObjectStoragePrefix => "ANVIL_API";
    }
  }
}
