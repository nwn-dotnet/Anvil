using Anvil.Services;

namespace Anvil.API
{
  public abstract class ObjectStorageVariableString : ObjectStorageVariable<string>
  {
    [Inject]
    private static ObjectStorageService ObjectStorageService { get; set; }

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
    protected override bool Persist => true;
    protected override string ObjectStoragePrefix => "NWNX_Object";
  }

  internal sealed class InternalVariableString : ObjectStorageVariableString
  {
    protected override bool Persist => true;
    protected override string ObjectStoragePrefix => "ANVIL_API";

    internal sealed class Persistent : ObjectStorageVariableString
    {
      protected override bool Persist => false;
      protected override string ObjectStoragePrefix => "ANVIL_API";
    }
  }
}
