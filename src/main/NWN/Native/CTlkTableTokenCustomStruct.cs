using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace NWN.Native
{
  [StructLayout(LayoutKind.Sequential)]
  internal readonly struct CTlkTableTokenCustomStruct : IEquatable<CTlkTableTokenCustomStruct>
  {
    public readonly uint m_nNumber;
    public readonly CExoStringStruct m_sValue;

    public CTlkTableTokenCustomStruct(uint m_nNumber, CExoStringStruct m_sValue)
    {
      this.m_nNumber = m_nNumber;
      this.m_sValue = m_sValue;
    }

    public bool Equals(CTlkTableTokenCustomStruct other)
    {
      return m_nNumber == other.m_nNumber;
    }

    public override bool Equals(object obj)
    {
      return obj is CTlkTableTokenCustomStruct other && Equals(other);
    }

    public override int GetHashCode()
    {
      return (int)m_nNumber;
    }

    public static bool operator ==(CTlkTableTokenCustomStruct left, CTlkTableTokenCustomStruct right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(CTlkTableTokenCustomStruct left, CTlkTableTokenCustomStruct right)
    {
      return !left.Equals(right);
    }

    private sealed class TokenNumberRelationalComparer : IComparer<CTlkTableTokenCustomStruct>
    {
      public int Compare(CTlkTableTokenCustomStruct x, CTlkTableTokenCustomStruct y)
      {
        return x.m_nNumber.CompareTo(y.m_nNumber);
      }
    }

    public static IComparer<CTlkTableTokenCustomStruct> TokenNumberComparer { get; } = new TokenNumberRelationalComparer();
  }
}
