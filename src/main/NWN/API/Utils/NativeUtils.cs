using System;
using System.Numerics;
using System.Runtime.InteropServices;
using NWN.Native.API;
using Vector = NWN.Native.API.Vector;

namespace NWN.API
{
  public static class NativeUtils
  {
    public static unsafe Vector ToNativeVector(this Vector3 vector)
    {
      return new Vector(&vector, false);
    }

    public static Vector3 ToManagedVector(this Vector vector)
    {
      return Marshal.PtrToStructure<Vector3>(vector);
    }

    public static CExoString ToExoString(this string str)
    {
      return new CExoString(str);
    }

    public static CExoLocString ToExoLocString(this string str, int nId = 0, byte gender = 0)
    {
      CExoLocString locString = new CExoLocString();
      locString.AddString(nId, new CExoString(str), gender);
      return locString;
    }

    public static CExoLocString ToExoLocString(this CExoString str, int nId = 0, byte gender = 0)
    {
      CExoLocString locString = new CExoLocString();
      locString.AddString(nId, str, gender);
      return locString;
    }

    public static string ExtractLocString(this CExoLocString locStr, int nID = 0, byte gender = 0)
    {
      CExoString str = new CExoString();
      locStr.GetStringLoc(nID, str, gender);

      return str.ToString();
    }

    public static unsafe T PeekMessage<T>(this CNWSMessage message, int offset) where T : unmanaged
    {
      byte* ptr = message.m_pnReadBuffer + message.m_nReadBufferPtr + offset;
      return Marshal.PtrToStructure<T>((IntPtr)ptr);
    }
  }
}
