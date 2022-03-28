using Anvil.Services;

namespace Anvil.API
{
  public abstract class ObjectStorageVariableFloat : ObjectStorageVariable<float>
  {
    [Inject]
    internal ObjectStorageService ObjectStorageService { private get; init; }

    public sealed override bool HasValue => ObjectStorageService.TryGetObjectStorage(Object, out ObjectStorage objectStorage) && objectStorage.ContainsFloat(ObjectStoragePrefix, Key);

    public sealed override float Value
    {
      get => ObjectStorageService.GetObjectStorage(Object).GetFloat(ObjectStoragePrefix, Key).GetValueOrDefault();
      set => ObjectStorageService.GetObjectStorage(Object).Set(ObjectStoragePrefix, Key, value, Persist);
    }

    protected sealed override string VariableTypePrefix => "PERFLT!";

    public sealed override void Delete()
    {
      ObjectStorageService.GetObjectStorage(Object).Remove(ObjectStoragePrefix, Key);
    }
  }

  public sealed class PersistentVariableFloat : ObjectStorageVariableFloat
  {
    protected override string ObjectStoragePrefix => "NWNX_Object";
    protected override bool Persist => true;
  }

  internal sealed class InternalVariableFloat : ObjectStorageVariableFloat
  {
    protected override string ObjectStoragePrefix => "ANVIL_API";
    protected override bool Persist => false;

    internal sealed class Persistent : ObjectStorageVariableFloat
    {
      protected override string ObjectStoragePrefix => "ANVIL_API";
      protected override bool Persist => true;
    }
  }
}
