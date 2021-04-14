using System;
using NWN.Native.API;

namespace NWN.API.Events
{
  public unsafe class DamageData<T> where T : unmanaged
  {
    private const int DamageArrayLength = 13;

    private readonly NativeArray<T> source;

    internal DamageData(T* source)
    {
      this.source = new NativeArray<T>((IntPtr)source, DamageArrayLength);
    }

    internal DamageData(NativeArray<T> source)
    {
      this.source = source;
    }

    public T Bludgeoning
    {
      get => source[0];
      set => source[0] = value;
    }

    public T Pierce
    {
      get => source[1];
      set => source[1] = value;
    }

    public T Slash
    {
      get => source[2];
      set => source[2] = value;
    }

    public T Magical
    {
      get => source[3];
      set => source[3] = value;
    }

    public T Acid
    {
      get => source[4];
      set => source[4] = value;
    }

    public T Cold
    {
      get => source[5];
      set => source[5] = value;
    }

    public T Divine
    {
      get => source[6];
      set => source[6] = value;
    }

    public T Electrical
    {
      get => source[7];
      set => source[7] = value;
    }

    public T Fire
    {
      get => source[8];
      set => source[8] = value;
    }

    public T Negative
    {
      get => source[9];
      set => source[9] = value;
    }

    public T Positive
    {
      get => source[10];
      set => source[10] = value;
    }

    public T Sonic
    {
      get => source[11];
      set => source[11] = value;
    }

    public T Base
    {
      get => source[12];
      set => source[12] = value;
    }
  }
}
