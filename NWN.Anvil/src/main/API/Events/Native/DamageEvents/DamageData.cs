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

    public T Acid
    {
      get => source[4];
      set => source[4] = value;
    }

    public T Base
    {
      get => source[12];
      set => source[12] = value;
    }

    public T Bludgeoning
    {
      get => source[0];
      set => source[0] = value;
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

    public T Magical
    {
      get => source[3];
      set => source[3] = value;
    }

    public T Negative
    {
      get => source[9];
      set => source[9] = value;
    }

    public T Pierce
    {
      get => source[1];
      set => source[1] = value;
    }

    public T Positive
    {
      get => source[10];
      set => source[10] = value;
    }

    public T Slash
    {
      get => source[2];
      set => source[2] = value;
    }

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
        DamageType.Custom1 => Custom1,
        DamageType.Custom2 => Custom2,
        DamageType.Custom3 => Custom3,
        DamageType.Custom4 => Custom4,
        DamageType.Custom5 => Custom5,
        DamageType.Custom6 => Custom6,
        DamageType.Custom7 => Custom7,
        DamageType.Custom8 => Custom8,
        DamageType.Custom9 => Custom9,
        DamageType.Custom10 => Custom10,
        DamageType.Custom11 => Custom11,
        DamageType.Custom12 => Custom12,
        DamageType.Custom13 => Custom13,
        DamageType.Custom14 => Custom14,
        DamageType.Custom15 => Custom15,
        DamageType.Custom16 => Custom16,
        DamageType.Custom17 => Custom17,
        DamageType.Custom18 => Custom18,
        DamageType.Custom19 => Custom19,
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
