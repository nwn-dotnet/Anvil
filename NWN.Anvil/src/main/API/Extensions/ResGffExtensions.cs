using NWN.Native.API;

namespace Anvil.API
{
  internal static unsafe class ResGffExtensions
  {
    public static bool TryReadByte(this CResGFF resGff, CResStruct resStruct, byte* fieldName, out byte value)
    {
      int bSuccess;
      value = resGff.ReadFieldBYTE(resStruct, fieldName, &bSuccess);
      return bSuccess.ToBool();
    }

    public static bool TryReadCExoLocString(this CResGFF resGff, CResStruct resStruct, byte* fieldName, out CExoLocString value)
    {
      int bSuccess;
      value = resGff.ReadFieldCExoLocString(resStruct, fieldName, &bSuccess);
      return bSuccess.ToBool();
    }

    public static bool TryReadCExoString(this CResGFF resGff, CResStruct resStruct, byte* fieldName, out CExoString value)
    {
      int bSuccess;
      value = resGff.ReadFieldCExoString(resStruct, fieldName, &bSuccess);
      return bSuccess.ToBool();
    }

    public static bool TryReadChar(this CResGFF resGff, CResStruct resStruct, byte* fieldName, out byte value)
    {
      int bSuccess;
      value = resGff.ReadFieldCHAR(resStruct, fieldName, &bSuccess);
      return bSuccess.ToBool();
    }

    public static bool TryReadCResRef(this CResGFF resGff, CResStruct resStruct, byte* fieldName, out CResRef value)
    {
      int bSuccess;
      value = resGff.ReadFieldCResRef(resStruct, fieldName, &bSuccess);
      return bSuccess.ToBool();
    }

    public static bool TryReadDouble(this CResGFF resGff, CResStruct resStruct, byte* fieldName, out double value)
    {
      int bSuccess;
      value = resGff.ReadFieldDOUBLE(resStruct, fieldName, &bSuccess);
      return bSuccess.ToBool();
    }

    public static bool TryReadDWord(this CResGFF resGff, CResStruct resStruct, byte* fieldName, out uint value)
    {
      int bSuccess;
      value = resGff.ReadFieldDWORD(resStruct, fieldName, &bSuccess);
      return bSuccess.ToBool();
    }

    public static bool TryReadDWord64(this CResGFF resGff, CResStruct resStruct, byte* fieldName, out ulong value)
    {
      int bSuccess;
      value = resGff.ReadFieldDWORD64(resStruct, fieldName, &bSuccess);
      return bSuccess.ToBool();
    }

    public static bool TryReadFloat(this CResGFF resGff, CResStruct resStruct, byte* fieldName, out float value)
    {
      int bSuccess;
      value = resGff.ReadFieldFLOAT(resStruct, fieldName, &bSuccess);
      return bSuccess.ToBool();
    }

    public static bool TryReadInt(this CResGFF resGff, CResStruct resStruct, byte* fieldName, out int value)
    {
      int bSuccess;
      value = resGff.ReadFieldINT(resStruct, fieldName, &bSuccess);
      return bSuccess.ToBool();
    }

    public static bool TryReadInt64(this CResGFF resGff, CResStruct resStruct, byte* fieldName, out long value)
    {
      int bSuccess;
      value = resGff.ReadFieldINT64(resStruct, fieldName, &bSuccess);
      return bSuccess.ToBool();
    }

    public static bool TryReadShort(this CResGFF resGff, CResStruct resStruct, byte* fieldName, out short value)
    {
      int bSuccess;
      value = resGff.ReadFieldSHORT(resStruct, fieldName, &bSuccess);
      return bSuccess.ToBool();
    }

    public static bool TryReadWord(this CResGFF resGff, CResStruct resStruct, byte* fieldName, out ushort value)
    {
      int bSuccess;
      value = resGff.ReadFieldWORD(resStruct, fieldName, &bSuccess);
      return bSuccess.ToBool();
    }
  }
}
