using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using Anvil.Native;
using Anvil.Services;
using NLog;
using NWN.Native.API;
using NWNX.NET.Native;
using Vector = NWN.Native.API.Vector;

namespace Anvil.API
{
  public static unsafe class NativeUtils
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private const string DefaultGffVersion = "V3.2";
    private static readonly CExoString DefaultGffVersionExoString = "V3.2".ToExoString();

    [Inject]
    private static ResourceManager ResourceManager { get; set; } = null!;

    public static bool CreateFromResRef(ResRefType resRefType, string resRef, Action<CResGFF, CResStruct> deserializeAction)
    {
      if (string.IsNullOrEmpty(resRef))
      {
        return false;
      }

      if (!ResourceManager.IsValidResource(resRef, resRefType))
      {
        return false;
      }

      CResGFF resGff = new CResGFF((ushort)resRefType, $"{resRefType} ".GetNullTerminatedString(), resRef.ToResRef());
      if (!resGff.m_bLoaded.ToBool())
      {
        Log.Warn($"Unable to load ResRef: {resRef}");
        return false;
      }

      CResStruct resStruct = new CResStruct();
      resGff.GetTopLevelStruct(resStruct).ToBool();
      deserializeAction(resGff, resStruct);

      resStruct.Dispose();
      resGff.Dispose();
      return true;
    }

    public static bool DeserializeGff(byte[] serialized, Func<CResGFF, CResStruct, bool> deserializeAction)
    {
      // GFF header size
      if (serialized.Length < 14 * 4)
      {
        return false;
      }

      CResGFF resGff = new CResGFF();
      CResStruct resStruct = new CResStruct();

      IntPtr dataPtr = Marshal.AllocHGlobal(serialized.Length);
      Marshal.Copy(serialized, 0, dataPtr, serialized.Length);

      void* data = (void*)dataPtr;

      if (!resGff.GetDataFromPointer(data, serialized.Length, true).ToBool())
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
        GC.SuppressFinalize(resGff);
        GC.SuppressFinalize(resStruct);
        return true;
      }

      Marshal.FreeHGlobal(dataPtr);
      return false;
    }

    public static string ExtractLocString(this CExoLocString locStr, int nID = 0, byte gender = 0)
    {
      CExoStringData exoStringData;
      CExoString exoString = CExoString.FromPointer(&exoStringData);

      try
      {
        locStr.GetStringLoc(nID, exoString, gender);
        return exoString.ToString()!;
      }
      finally
      {
        exoString._Destructor();
      }
    }

    public static bool IsValidGff(this CResGFF resGff, string expectedFileType, string expectedVersion = DefaultGffVersion)
    {
      return IsValidGff(resGff, [expectedFileType], [expectedVersion]);
    }

    public static bool IsValidGff(this CResGFF resGff, IEnumerable<string> expectedFileTypes, IEnumerable<string> expectedVersions)
    {
      CExoString sFileType = new CExoString();
      CExoString sFileVersion = new CExoString();
      resGff.GetGFFFileInfo(sFileType, sFileVersion);

      string fileType = sFileType.ToString()!;
      string fileVersion = sFileVersion.ToString()!;

      return expectedVersions.Any(expectedVersion => expectedVersion == fileVersion) &&
        expectedFileTypes.Any(expectedFileType => expectedFileType + " " == fileType);
    }

    public static byte[]? SerializeGff(string fileType, string version, Func<CResGFF, CResStruct, bool> serializeAction)
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

    public static byte[]? SerializeGff(string fileType, Func<CResGFF, CResStruct, bool> serializeAction)
    {
      if (string.IsNullOrEmpty(fileType))
      {
        throw new ArgumentNullException(nameof(fileType), "Type must not be null or empty.");
      }

      return SerializeGff((fileType + " ").ToExoString(), DefaultGffVersionExoString, serializeAction);
    }

    public static Color ToColor(this Vector vector)
    {
      return new Color(vector.x, vector.y, vector.z);
    }

    public static CExoLocString ToExoLocString(this string? str, int nId = 0, byte gender = 0)
    {
      CExoLocString locString = new CExoLocString();
      locString.AddString(nId, str != null ? new CExoString(str) : new CExoString(), gender);
      return locString;
    }

    public static CExoLocString ToExoLocString(this CExoString str, int nId = 0, byte gender = 0)
    {
      CExoLocString locString = new CExoLocString();
      locString.AddString(nId, str, gender);
      return locString;
    }

    public static CExoString ToExoString(this string? str)
    {
      return str != null ? new CExoString(str) : new CExoString();
    }

    public static Vector3 ToManagedVector(this Vector vector)
    {
      return Marshal.PtrToStructure<Vector3>(vector.Pointer);
    }

    public static Vector ToNativeVector(this Vector3 vector)
    {
      return new Vector(vector.X, vector.Y, vector.Z);
    }

    public static Vector ToNativeVector(this Color color)
    {
      return new Vector(color.Red, color.Green, color.Blue);
    }

    public static CResRef ToResRef(this string? str)
    {
      return str != null ? new CResRef(str) : new CResRef();
    }

    public static Vector3 ToVectorOrientation(this float facing)
    {
      float radians = (float)(facing * (Math.PI / 180));
      return new Vector3((float)Math.Cos(radians), (float)Math.Sin(radians), 0.0f);
    }

    private static byte[]? SerializeGff(CExoString fileType, CExoString version, Func<CResGFF, CResStruct, bool> serializeAction)
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
