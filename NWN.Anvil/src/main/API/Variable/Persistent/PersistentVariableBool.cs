using Anvil.Services;

namespace Anvil.API
{
  public sealed class PersistentVariableBool : PersistentVariable<bool>
  {
    [Inject]
    private static ObjectStorageService ObjectStorageService { get; set; }

    public override bool HasValue
    {
      get => ObjectStorageService.TryGetObjectStorage(Object, out ObjectStorage objectStorage) && objectStorage.ContainsInt(Prefix, Key);
    }

    public override bool Value
    {
      get => ObjectStorageService.GetObjectStorage(Object).GetInt(Prefix, Key).GetValueOrDefault().ToBool();
      set => ObjectStorageService.GetObjectStorage(Object).Set(Prefix, Key, value.ToInt(), true);
    }

    protected override string KeyPrefix
    {
      get => "PERINT!";
    }

    public override void Delete()
    {
      ObjectStorageService.GetObjectStorage(Object).Remove(Prefix, Key);
    }
  }
}
