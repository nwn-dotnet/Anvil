using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using NWN.Native.API;
using Vector = NWN.Native.API.Vector;

namespace NWN.API
{
  public static class NativeUtils
  {
    private const string DefaultGffVersion = "V2.0";
    private static readonly CExoString DefaultGffVersionExoString = "V2.0".ToExoString();

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

    public static bool IsValidGff(this CResGFF resGff, string expectedFileType, string expectedVersion = DefaultGffVersion)
    {
      return IsValidGff(resGff, expectedFileType.Yield(), expectedVersion);
    }

    public static bool IsValidGff(this CResGFF resGff, IEnumerable<string> expectedFileTypes, string expectedVersion = DefaultGffVersion)
    {
      CExoString sFileType = new CExoString();
      CExoString sFileVersion = new CExoString();
      resGff.GetGFFFileInfo(sFileType, sFileVersion);

      string fileType = sFileType.ToString();
      string fileVersion = sFileVersion.ToString();

      return fileVersion == expectedVersion && expectedFileTypes.Any(expectedFileType => expectedFileType + " " == fileType);
    }

    public static unsafe T PeekMessage<T>(this CNWSMessage message, int offset) where T : unmanaged
    {
      byte* ptr = message.m_pnReadBuffer + message.m_nReadBufferPtr + offset;
      return Marshal.PtrToStructure<T>((IntPtr)ptr);
    }

    public static byte[] SerializeGff(string fileType, string version, Func<CResGFF, CResStruct, bool> serializeAction)
    {
      if (string.IsNullOrEmpty(fileType))
      {
        throw new ArgumentNullException(nameof(fileType), "Type must not be null or empty.");
      }

      if (string.IsNullOrEmpty(version))
      {
        throw new ArgumentNullException(nameof(version), "Version must not be null or empty.");
      }

      return SerializeGff((fileType + " ").ToExoString(), version.ToExoString(), serializeAction);
    }

    public static byte[] SerializeGff(string fileType, Func<CResGFF, CResStruct, bool> serializeAction)
    {
      if (string.IsNullOrEmpty(fileType))
      {
        throw new ArgumentNullException(nameof(fileType), "Type must not be null or empty.");
      }

      return SerializeGff((fileType + " ").ToExoString(), DefaultGffVersionExoString, serializeAction);
    }

    public static unsafe bool DeserializeGff(byte[] serialized, Func<CResGFF, CResStruct, bool> deserializeAction)
    {
      // GFF header size
      if (serialized.Length < 14 * 4)
      {
        return false;
      }

      using CResGFF resGff = new CResGFF();
      using CResStruct resStruct = new CResStruct();

      IntPtr dataPtr = Marshal.AllocHGlobal(serialized.Length);
      Marshal.Copy(serialized, 0, dataPtr, serialized.Length);

      void* data = (void*)dataPtr;

      if (!resGff.GetDataFromPointer(data, serialized.Length).ToBool())
      {
        Marshal.FreeHGlobal(dataPtr);
        return false;
      }

      resGff.InitializeForWriting();
      if (!resGff.GetTopLevelStruct(resStruct).ToBool())
      {
        Marshal.FreeHGlobal(dataPtr);
        return false;
      }

      if (deserializeAction(resGff, resStruct))
      {
        return true;
      }

      Marshal.FreeHGlobal(dataPtr);
      return false;
    }

    private static unsafe byte[] SerializeGff(CExoString fileType, CExoString version, Func<CResGFF, CResStruct, bool> serializeAction)
    {
      void* pData;
      int dataLength;

      using CResGFF resGff = new CResGFF();
      using CResStruct resStruct = new CResStruct();

      if (!resGff.CreateGFFFile(resStruct, fileType, version).ToBool())
      {
        return null;
      }

      if (!serializeAction(resGff, resStruct))
      {
        return null;
      }

      resGff.WriteGFFToPointer(&pData, &dataLength);

      byte[] serialized = new byte[dataLength];

      Marshal.Copy((IntPtr)pData, serialized, 0, dataLength);
      Marshal.FreeHGlobal((IntPtr)pData);

      return serialized;
    }
  }
}
