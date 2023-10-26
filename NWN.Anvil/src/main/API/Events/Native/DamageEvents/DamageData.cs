using System;
using System.Numerics;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed unsafe class DamageData<T> where T : unmanaged
  {
    private const int DamageArrayLength = 32;
    private readonly NativeArray<T> source;

    internal DamageData(T* source)
    {
      this.source = new NativeArray<T>((IntPtr)source, DamageArrayLength);
    }

    internal DamageData(NativeArray<T> source)
    {
      this.source = source;
    }

    [Obsolete("Use Get/SetDamageByType instead.")]
    public T Acid
    {
      get => source[4];
      set => source[4] = value;
    }

    [Obsolete("Use Get/SetDamageByType instead.")]
    public T Base
    {
      get => source[12];
      set => source[12] = value;
    }

    [Obsolete("Use Get/SetDamageByType instead.")]
    public T Bludgeoning
    {
      get => source[0];
      set => source[0] = value;
    }

    [Obsolete("Use Get/SetDamageByType instead.")]
    public T Cold
    {
      get => source[5];
      set => source[5] = value;
    }

    [Obsolete("Use Get/SetDamageByType instead.")]
    public T Divine
    {
      get => source[6];
      set => source[6] = value;
    }

    [Obsolete("Use Get/SetDamageByType instead.")]
    public T Electrical
    {
      get => source[7];
      set => source[7] = value;
    }

    [Obsolete("Use Get/SetDamageByType instead.")]
    public T Fire
    {
      get => source[8];
      set => source[8] = value;
    }

    [Obsolete("Use Get/SetDamageByType instead.")]
    public T Magical
    {
      get => source[3];
      set => source[3] = value;
    }

    [Obsolete("Use Get/SetDamageByType instead.")]
    public T Negative
    {
      get => source[9];
      set => source[9] = value;
    }

    [Obsolete("Use Get/SetDamageByType instead.")]
    public T Pierce
    {
      get => source[1];
      set => source[1] = value;
    }

    [Obsolete("Use Get/SetDamageByType instead.")]
    public T Positive
    {
      get => source[10];
      set => source[10] = value;
    }

    [Obsolete("Use Get/SetDamageByType instead.")]
    public T Slash
    {
      get => source[2];
      set => source[2] = value;
    }

    [Obsolete("Use Get/SetDamageByType instead.")]
    public T Sonic
    {
      get => source[11];
      set => source[11] = value;
    }

    public T GetDamageByType(DamageType damageType)
    {
      int index = BitOperations.Log2((uint)damageType);
      return source[index];
    }

    public void SetDamageByType(DamageType damageType, T value)
    {
      int index = BitOperations.Log2((uint)damageType);
      source[index] = value;
    }
  }
}
