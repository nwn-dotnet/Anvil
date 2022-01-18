using Anvil.Services;

namespace Anvil.API
{
  public abstract class ObjectStorageVariableString : ObjectStorageVariable<string>
  {
    [Inject]
    internal ObjectStorageService ObjectStorageService { private get; init; }

    public sealed override bool HasValue => ObjectStorageService.TryGetObjectStorage(Object, out ObjectStorage objectStorage) && objectStorage.ContainsString(ObjectStoragePrefix, Key);

    public sealed override string Value
    {
      get => ObjectStorageService.GetObjectStorage(Object).GetString(ObjectStoragePrefix, Key);
      set => ObjectStorageService.GetObjectStorage(Object).Set(ObjectStoragePrefix, Key, value, Persist);
    }

    protected sealed override string VariableTypePrefix => "PERSTR!";

    public sealed override void Delete()
    {
      ObjectStorageService.GetObjectStorage(Object).Remove(ObjectStoragePrefix, Key);
    }
  }

  public sealed class PersistentVariableString : ObjectStorageVariableString
  {
    protected override string ObjectStoragePrefix => "NWNX_Object";
    protected override bool Persist => true;
  }

  internal sealed class InternalVariableString : ObjectStorageVariableString
  {
    protected override string ObjectStoragePrefix => "ANVIL_API";
    protected override bool Persist => true;

    internal sealed class Persistent : ObjectStorageVariableString
    {
      protected override string ObjectStoragePrefix => "ANVIL_API";
      protected override bool Persist => false;
    }
  }
}
