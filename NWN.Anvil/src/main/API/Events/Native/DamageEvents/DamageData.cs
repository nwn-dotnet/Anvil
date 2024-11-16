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

    public T Custom1
    {
      get => source[13];
      set => source[13] = value;
    }

    public T Custom2
    {
      get => source[14];
      set => source[14] = value;
    }

    public T Custom3
    {
      get => source[15];
      set => source[15] = value;
    }

    public T Custom4
    {
      get => source[16];
      set => source[16] = value;
    }

    public T Custom5
    {
      get => source[17];
      set => source[17] = value;
    }

    public T Custom6
    {
      get => source[18];
      set => source[18] = value;
    }

    public T Custom7
    {
      get => source[19];
      set => source[19] = value;
    }

    public T Custom8
    {
      get => source[20];
      set => source[20] = value;
    }

    public T Custom9
    {
      get => source[21];
      set => source[21] = value;
    }

    public T Custom10
    {
      get => source[22];
      set => source[22] = value;
    }

    public T Custom11
    {
      get => source[23];
      set => source[23] = value;
    }

    public T Custom12
    {
      get => source[24];
      set => source[24] = value;
    }

    public T Custom13
    {
      get => source[25];
      set => source[25] = value;
    }

    public T Custom14
    {
      get => source[26];
      set => source[26] = value;
    }

    public T Custom15
    {
      get => source[27];
      set => source[27] = value;
    }

    public T Custom16
    {
      get => source[28];
      set => source[28] = value;
    }

    public T Custom17
    {
      get => source[29];
      set => source[29] = value;
    }

    public T Custom18
    {
      get => source[30];
      set => source[30] = value;
    }

    public T Custom19
    {
      get => source[31];
      set => source[31] = value;
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
