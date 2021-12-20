using Anvil.Services;

namespace Anvil.API
{
  public class PersistentVariableInt : PersistentVariable<int>
  {
    [Inject]
    private static ObjectStorageService ObjectStorageService { get; set; }

    public sealed override bool HasValue => ObjectStorageService.TryGetObjectStorage(Object, out ObjectStorage objectStorage) && objectStorage.ContainsInt(Prefix, Key);

    public sealed override int Value
    {
      get => ObjectStorageService.GetObjectStorage(Object).GetInt(Prefix, Key).GetValueOrDefault();
      set => ObjectStorageService.GetObjectStorage(Object).Set(Prefix, Key, value, true);
    }

    protected sealed override string KeyPrefix => "PERINT!";

    public sealed override void Delete()
    {
      ObjectStorageService.GetObjectStorage(Object).Remove(Prefix, Key);
    }

    internal sealed class Internal : PersistentVariableInt
    {
      protected override string Prefix => "ANVIL_API";
    }
  }
}
