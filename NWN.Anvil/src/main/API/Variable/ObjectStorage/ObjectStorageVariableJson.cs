using System.Text.Json;
using Anvil.Services;

namespace Anvil.API
{
  public abstract class ObjectStorageVariableJson : ObjectStorageVariable<JsonElement>
  {
    [Inject]
    internal ObjectStorageService ObjectStorageService { private get; init; }

    public sealed override bool HasValue => ObjectStorageService.TryGetObjectStorage(Object, out ObjectStorage objectStorage) && objectStorage.ContainsString(ObjectStoragePrefix, Key);

    public sealed override JsonElement Value
    {
      get => JsonSerializer.Deserialize<JsonElement>(ObjectStorageService.GetObjectStorage(Object).GetString(ObjectStoragePrefix, Key));
      set => ObjectStorageService.GetObjectStorage(Object).Set(ObjectStoragePrefix, Key, JsonSerializer.Serialize(value), Persist);
    }

    protected sealed override string VariableTypePrefix => "PERSTR!";

    public sealed override void Delete()
    {
      ObjectStorageService.GetObjectStorage(Object).Remove(ObjectStoragePrefix, Key);
    }
  }

  public sealed class PersistentVariableJson : ObjectStorageVariableJson
  {
    protected override string ObjectStoragePrefix => "NWNX_Object";
    protected override bool Persist => true;
  }

  internal sealed class InternalVariableJson : ObjectStorageVariableJson
  {
    protected override string ObjectStoragePrefix => "ANVIL_API";
    protected override bool Persist => true;

    internal sealed class Persistent : ObjectStorageVariableJson
    {
      protected override string ObjectStoragePrefix => "ANVIL_API";
      protected override bool Persist => false;
    }
  }
}
