using System;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed unsafe class DamageData<T> where T : unmanaged
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

    public T GetDamageByType(DamageType damageType)
    {
      return damageType switch
      {
        DamageType.Bludgeoning => Bludgeoning,
        DamageType.Piercing => Pierce,
        DamageType.Slashing => Slash,
        DamageType.Magical => Magical,
        DamageType.Acid => Acid,
        DamageType.Cold => Cold,
        DamageType.Divine => Divine,
        DamageType.Electrical => Electrical,
        DamageType.Fire => Fire,
        DamageType.Negative => Negative,
        DamageType.Positive => Positive,
        DamageType.Sonic => Sonic,
        DamageType.BaseWeapon => Base,
        _ => throw new ArgumentOutOfRangeException(nameof(damageType), damageType, null),
      };
    }

    public void SetDamageByType(DamageType damageType, T value)
    {
      switch (damageType)
      {
        case DamageType.Bludgeoning:
          Bludgeoning = value;
          break;
        case DamageType.Piercing:
          Pierce = value;
          break;
        case DamageType.Slashing:
          Slash = value;
          break;
        case DamageType.Magical:
          Magical = value;
          break;
        case DamageType.Acid:
          Acid = value;
          break;
        case DamageType.Cold:
          Cold = value;
          break;
        case DamageType.Divine:
          Divine = value;
          break;
        case DamageType.Electrical:
          Electrical = value;
          break;
        case DamageType.Fire:
          Fire = value;
          break;
        case DamageType.Negative:
          Negative = value;
          break;
        case DamageType.Positive:
          Positive = value;
          break;
        case DamageType.Sonic:
          Sonic = value;
          break;
        case DamageType.BaseWeapon:
          Base = value;
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(damageType), damageType, null);
      }
    }
  }
}
