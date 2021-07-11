using NWN.Services;

namespace NWN.API
{
  public sealed class PersistentVariableString : PersistentVariable<string>
  {
    [Inject]
    private static ObjectStorageService ObjectStorageService { get; set; }

    public override bool HasValue
    {
      get => ObjectStorageService.TryGetObjectStorage(Object, out ObjectStorage objectStorage) && objectStorage.ContainsInt(Prefix, Name);
    }

    public override void Delete()
    {
      ObjectStorageService.GetObjectStorage(Object).Remove(Prefix, Name);
    }

    public override string Value
    {
      get => ObjectStorageService.GetObjectStorage(Object).GetString(Prefix, Name);
      set => ObjectStorageService.GetObjectStorage(Object).Set(Prefix, Name, value);
    }
  }
}
