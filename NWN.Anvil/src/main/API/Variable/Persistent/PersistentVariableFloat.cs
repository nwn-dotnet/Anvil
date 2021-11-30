using Anvil.Services;

namespace Anvil.API
{
  public class PersistentVariableFloat : PersistentVariable<float>
  {
    [Inject]
    private static ObjectStorageService ObjectStorageService { get; set; }

    public sealed override bool HasValue
    {
      get => ObjectStorageService.TryGetObjectStorage(Object, out ObjectStorage objectStorage) && objectStorage.ContainsFloat(Prefix, Key);
    }

    public sealed override float Value
    {
      get => ObjectStorageService.GetObjectStorage(Object).GetFloat(Prefix, Key).GetValueOrDefault();
      set => ObjectStorageService.GetObjectStorage(Object).Set(Prefix, Key, value, true);
    }

    protected sealed override string KeyPrefix
    {
      get => "PERFLT!";
    }

    public sealed override void Delete()
    {
      ObjectStorageService.GetObjectStorage(Object).Remove(Prefix, Key);
    }

    internal sealed class Internal : PersistentVariableFloat
    {
      protected override string Prefix => "ANVIL_Internal";
    }
  }
}
