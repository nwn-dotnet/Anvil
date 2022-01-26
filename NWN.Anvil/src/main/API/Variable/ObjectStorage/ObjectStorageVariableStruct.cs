using System.Text.Json;
using Anvil.Services;

namespace Anvil.API
{
  public abstract class ObjectStorageVariableStruct<T> : ObjectStorageVariable<T>
  {
    [Inject]
    internal ObjectStorageService ObjectStorageService { private get; init; }

    public sealed override bool HasValue => ObjectStorageService.TryGetObjectStorage(Object, out ObjectStorage objectStorage) && objectStorage.ContainsString(ObjectStoragePrefix, Key);

    public sealed override T Value
    {
      get => HasValue ? JsonSerializer.Deserialize<T>(ObjectStorageService.GetObjectStorage(Object).GetString(ObjectStoragePrefix, Key)) : default;
      set => ObjectStorageService.GetObjectStorage(Object).Set(ObjectStoragePrefix, Key, JsonSerializer.Serialize(value), Persist);
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
