using Anvil.Services;

namespace Anvil.API
{
  public sealed class PersistentVariableFloat : PersistentVariable<float>
  {
    [Inject]
    private static ObjectStorageService ObjectStorageService { get; set; }

    public override bool HasValue
    {
      get => ObjectStorageService.TryGetObjectStorage(Object, out ObjectStorage objectStorage) && objectStorage.ContainsFloat(Prefix, Key);
    }

    public override float Value
    {
      get => ObjectStorageService.GetObjectStorage(Object).GetFloat(Prefix, Key).GetValueOrDefault();
      set => ObjectStorageService.GetObjectStorage(Object).Set(Prefix, Key, value, true);
    }

    protected override string KeyPrefix
    {
      get => "PERFLT!";
    }

    public override void Delete()
    {
      ObjectStorageService.GetObjectStorage(Object).Remove(Prefix, Key);
    }
  }
}
