using Anvil.Services;

namespace Anvil.API
{
  public abstract class ObjectStorageVariableBool : ObjectStorageVariable<bool>
  {
    [Inject]
    internal ObjectStorageService ObjectStorageService { private get; init; }

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
    protected override string ObjectStoragePrefix => "NWNX_Object";
    protected override bool Persist => true;
  }

  internal sealed class InternalVariableBool : ObjectStorageVariableBool
  {
    protected override string ObjectStoragePrefix => "ANVIL_API";
    protected override bool Persist => true;

    internal sealed class Persistent : ObjectStorageVariableBool
    {
      protected override string ObjectStoragePrefix => "ANVIL_API";
      protected override bool Persist => false;
    }
  }
}
