using System;
using System.Runtime.CompilerServices;
using NWN.Core;

namespace Anvil.API
{
  public sealed class LocalVariableEnum<T> : LocalVariable<T> where T : struct, Enum
  {
    public LocalVariableEnum()
    {
      if (Unsafe.SizeOf<T>() != Unsafe.SizeOf<int>())
      {
        throw new ArgumentOutOfRangeException(nameof(T), "Specified enum must be backed by a signed int32 (int)");
      }
    }

    public override T Value
    {
      get
      {
        int value = NWScript.GetLocalInt(Object, Name);
        return Unsafe.As<int, T>(ref value);
      }
      set
      {
        int newValue = Unsafe.As<T, int>(ref value);
        NWScript.SetLocalInt(Object, Name, newValue);
      }
    }

    public override void Delete()
    {
      NWScript.DeleteLocalInt(Object, Name);
    }
  }
}
