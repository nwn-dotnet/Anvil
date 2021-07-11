using Anvil.Services;

namespace NWN.API
{
  public sealed class PersistentVariableInt : PersistentVariable<int>
  {
    [Inject]
    private static ObjectStorageService ObjectStorageService { get; set; }

    protected override string KeyPrefix
    {
      get => "PERINT!";
    }

    public override bool HasValue
    {
      get => ObjectStorageService.TryGetObjectStorage(Object, out ObjectStorage objectStorage) && objectStorage.ContainsInt(Prefix, Key);
    }

    public override void Delete()
    {
      ObjectStorageService.GetObjectStorage(Object).Remove(Prefix, Key);
    }

    public override int Value
    {
      get => ObjectStorageService.GetObjectStorage(Object).GetInt(Prefix, Key).GetValueOrDefault();
      set => ObjectStorageService.GetObjectStorage(Object).Set(Prefix, Key, value, true);
    }
  }
}
