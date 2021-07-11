using NWN.Services;

namespace NWN.API
{
  public sealed class PersistentVariableString : PersistentVariable<string>
  {
    [Inject]
    private static ObjectStorageService ObjectStorageService { get; set; }

    protected override string KeyPrefix
    {
      get => "PERSTR!";
    }

    public override bool HasValue
    {
      get => ObjectStorageService.TryGetObjectStorage(Object, out ObjectStorage objectStorage) && objectStorage.ContainsString(Prefix, Key);
    }

    public override void Delete()
    {
      ObjectStorageService.GetObjectStorage(Object).Remove(Prefix, Key);
    }

    public override string Value
    {
      get => ObjectStorageService.GetObjectStorage(Object).GetString(Prefix, Key);
      set => ObjectStorageService.GetObjectStorage(Object).Set(Prefix, Key, value, true);
    }
  }
}
