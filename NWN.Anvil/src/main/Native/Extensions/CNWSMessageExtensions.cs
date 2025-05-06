using System;
using System.Runtime.InteropServices;
using NWN.Native.API;
using NWNX.NET.Native;

namespace Anvil.Native
{
  public static unsafe class CNWSMessageExtensions
  {
    public static T PeekMessage<T>(this CNWSMessage message, int offset) where T : unmanaged
    {
      byte* ptr = message.m_pnReadBuffer + message.m_nReadBufferPtr + offset;
      return Marshal.PtrToStructure<T>((IntPtr)ptr);
    }

    public static string PeekMessageResRef(this CNWSMessage message, int offset)
    {
      byte* ptr = message.m_pnReadBuffer + message.m_nReadBufferPtr + offset;
      return StringUtils.ReadFixedLengthString(ptr, 16);
    }

    public static string PeekMessageString(this CNWSMessage message, int offset)
    {
      byte* ptr = message.m_pnReadBuffer + message.m_nReadBufferPtr + offset;
      return StringUtils.ReadNullTerminatedString(ptr)!;
    }

    public static void ClearReadMessage(this CNWSMessage message)
    {
      message.m_nReadBufferPtr = message.m_nReadBufferSize;
      message.m_nReadFragmentsBufferPtr = message.m_nReadFragmentsBufferSize;
      message.m_nCurReadBit = message.m_nLastByteBits;
    }
  }
}
