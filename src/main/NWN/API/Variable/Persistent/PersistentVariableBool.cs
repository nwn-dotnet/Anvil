using NWN.Services;

namespace NWN.API
{
  public sealed class PersistentVariableBool : PersistentVariable<bool>
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

    public override bool Value
    {
      get => ObjectStorageService.GetObjectStorage(Object).GetInt(Prefix, Name).GetValueOrDefault().ToBool();
      set => ObjectStorageService.GetObjectStorage(Object).Set(Prefix, Name, value.ToInt());
    }
  }
}
