using Anvil.Services;

namespace Anvil.API
{
  public class PersistentVariableBool : PersistentVariable<bool>
  {
    [Inject]
    private static ObjectStorageService ObjectStorageService { get; set; }

    public sealed override bool HasValue
    {
      get => ObjectStorageService.TryGetObjectStorage(Object, out ObjectStorage objectStorage) && objectStorage.ContainsInt(Prefix, Key);
    }

    public sealed override bool Value
    {
      get => ObjectStorageService.GetObjectStorage(Object).GetInt(Prefix, Key).GetValueOrDefault().ToBool();
      set => ObjectStorageService.GetObjectStorage(Object).Set(Prefix, Key, value.ToInt(), true);
    }

    protected sealed override string KeyPrefix
    {
      get => "PERINT!";
    }

    public sealed override void Delete()
    {
      ObjectStorageService.GetObjectStorage(Object).Remove(Prefix, Key);
    }

    internal sealed class Internal : PersistentVariableBool
    {
      protected override string Prefix
      {
        get => "ANVIL_API";
      }
    }
  }
}
