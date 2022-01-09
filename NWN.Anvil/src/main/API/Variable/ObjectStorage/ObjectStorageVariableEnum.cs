using System;
using System.Runtime.CompilerServices;
using Anvil.Services;

namespace Anvil.API
{
  public abstract class ObjectStorageVariableEnum<T> : ObjectStorageVariable<T> where T : struct, Enum
  {
    [Inject]
    private ObjectStorageService ObjectStorageService { get; set; }

    protected ObjectStorageVariableEnum()
    {
      if (Unsafe.SizeOf<T>() != Unsafe.SizeOf<int>())
      {
        throw new ArgumentOutOfRangeException(nameof(T), "Specified enum must be backed by a signed int32 (int)");
      }
    }

    public sealed override bool HasValue => ObjectStorageService.TryGetObjectStorage(Object, out ObjectStorage objectStorage) && objectStorage.ContainsInt(ObjectStoragePrefix, Key);

    public sealed override T Value
    {
      get
      {
        int value = ObjectStorageService.GetObjectStorage(Object).GetInt(ObjectStoragePrefix, Key).GetValueOrDefault();
        return Unsafe.As<int, T>(ref value);
      }
      set
      {
        int newValue = Unsafe.As<T, int>(ref value);
        ObjectStorageService.GetObjectStorage(Object).Set(ObjectStoragePrefix, Key, newValue, Persist);
      }
    }

    protected sealed override string VariableTypePrefix => "PERINT!";

    public sealed override void Delete()
    {
      ObjectStorageService.GetObjectStorage(Object).Remove(ObjectStoragePrefix, Key);
    }
  }

  public sealed class PersistentVariableEnum<T> : ObjectStorageVariableEnum<T> where T : struct, Enum
  {
    protected override bool Persist => true;
    protected override string ObjectStoragePrefix => "NWNX_Object";
  }

  internal sealed class InternalVariableEnum<T> : ObjectStorageVariableEnum<T> where T : struct, Enum
  {
    protected override bool Persist => true;
    protected override string ObjectStoragePrefix => "ANVIL_API";

    internal sealed class Persistent : ObjectStorageVariableEnum<T>
    {
      protected override bool Persist => false;
      protected override string ObjectStoragePrefix => "ANVIL_API";
    }
  }
}
