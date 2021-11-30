using Anvil.Services;

namespace Anvil.API
{
  public class PersistentVariableString : PersistentVariable<string>
  {
    [Inject]
    private static ObjectStorageService ObjectStorageService { get; set; }

    public sealed override bool HasValue
    {
      get => ObjectStorageService.TryGetObjectStorage(Object, out ObjectStorage objectStorage) && objectStorage.ContainsString(Prefix, Key);
    }

    public sealed override string Value
    {
      get => ObjectStorageService.GetObjectStorage(Object).GetString(Prefix, Key);
      set => ObjectStorageService.GetObjectStorage(Object).Set(Prefix, Key, value, true);
    }

    protected sealed override string KeyPrefix
    {
      get => "PERSTR!";
    }

    public sealed override void Delete()
    {
      ObjectStorageService.GetObjectStorage(Object).Remove(Prefix, Key);
    }

    internal sealed class Internal : PersistentVariableString
    {
      protected override string Prefix
      {
        get => "ANVIL_API";
      }
    }
  }
}
