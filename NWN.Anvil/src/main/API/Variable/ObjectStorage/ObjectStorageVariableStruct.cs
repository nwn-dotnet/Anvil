using Anvil.Services;

namespace Anvil.API
{
  public abstract class ObjectStorageVariableStruct<T> : ObjectStorageVariable<T>
  {
    [Inject]
    internal ObjectStorageService ObjectStorageService { private get; init; } = null!;

    public sealed override bool HasValue => ObjectStorageService.TryGetObjectStorage(Object, out ObjectStorage? objectStorage) && objectStorage.ContainsString(ObjectStoragePrefix, Key);

    public sealed override T? Value
    {
      get
      {
        if (ObjectStorageService.TryGetObjectStorage(Object, out ObjectStorage? objectStorage))
        {
          string? serialized = objectStorage.GetString(ObjectStoragePrefix, Key);
          return !string.IsNullOrEmpty(serialized) ? JsonUtility.FromJson<T>(serialized) : default;
        }

        return default;
      }
      set => ObjectStorageService.GetObjectStorage(Object).Set(ObjectStoragePrefix, Key, JsonUtility.ToJson(value), Persist);
    }

    protected sealed override string VariableTypePrefix => "PERSTR!";

    public sealed override void Delete()
    {
      ObjectStorageService.GetObjectStorage(Object).Remove(ObjectStoragePrefix, Key);
    }
  }

  public sealed class PersistentVariableStruct<T> : ObjectStorageVariableStruct<T>
  {
    protected override string ObjectStoragePrefix => "NWNX_Object";
    protected override bool Persist => true;
  }

  internal sealed class InternalVariableStruct<T> : ObjectStorageVariableStruct<T>
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
