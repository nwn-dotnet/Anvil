using System;
using NWN.Native.API;

namespace Anvil.API
{
  /// <summary>
  /// A regular <see cref="GffResourceField"/> containing a primitive value.
  /// </summary>
  public sealed unsafe class GffResourceFieldValue : GffResourceField
  {
    private readonly byte* fieldId;
    private readonly CResStruct parentStruct;

    internal GffResourceFieldValue(CResGFF resGff, CResStruct parentStruct, byte* fieldId) : base(resGff)
    {
      this.parentStruct = parentStruct;
      this.fieldId = fieldId;
    }

    public override GffResourceFieldType FieldType => (GffResourceFieldType)ResGff.GetFieldType(parentStruct, fieldId);

    public bool TryReadByte(out byte value)
    {
      int bSuccess;
      value = ResGff.ReadFieldBYTE(parentStruct, fieldId, &bSuccess);
      return bSuccess.ToBool();
    }

    public bool TryReadCExoLocString(out string value, int id = 0, Gender gender = Gender.Male)
    {
      int bSuccess;
      value = ResGff.ReadFieldCExoLocString(parentStruct, fieldId, &bSuccess).ExtractLocString(id, (byte)gender);
      return bSuccess.ToBool();
    }

    public bool TryReadCExoString(out string value)
    {
      int bSuccess;
      value = ResGff.ReadFieldCExoString(parentStruct, fieldId, &bSuccess).ToString();
      return bSuccess.ToBool();
    }

    public bool TryReadChar(out byte value)
    {
      int bSuccess;
      value = ResGff.ReadFieldCHAR(parentStruct, fieldId, &bSuccess);
      return bSuccess.ToBool();
    }

    public bool TryReadCResRef(out string value)
    {
      int bSuccess;
      value = ResGff.ReadFieldCResRef(parentStruct, fieldId, &bSuccess).ToString();
      return bSuccess.ToBool();
    }

    public bool TryReadDouble(out double value)
    {
      int bSuccess;
      value = ResGff.ReadFieldDOUBLE(parentStruct, fieldId, &bSuccess);
      return bSuccess.ToBool();
    }

    public bool TryReadDWord(out uint value)
    {
      int bSuccess;
      value = ResGff.ReadFieldDWORD(parentStruct, fieldId, &bSuccess);
      return bSuccess.ToBool();
    }

    public bool TryReadDWord64(out ulong value)
    {
      int bSuccess;
      value = ResGff.ReadFieldDWORD64(parentStruct, fieldId, &bSuccess);
      return bSuccess.ToBool();
    }

    public bool TryReadFloat(out float value)
    {
      int bSuccess;
      value = ResGff.ReadFieldFLOAT(parentStruct, fieldId, &bSuccess);
      return bSuccess.ToBool();
    }

    public bool TryReadInt(out int value)
    {
      int bSuccess;
      value = ResGff.ReadFieldINT(parentStruct, fieldId, &bSuccess);
      return bSuccess.ToBool();
    }

    public bool TryReadInt64(out long value)
    {
      int bSuccess;
      value = ResGff.ReadFieldINT64(parentStruct, fieldId, &bSuccess);
      return bSuccess.ToBool();
    }

    public bool TryReadShort(out short value)
    {
      int bSuccess;
      value = ResGff.ReadFieldSHORT(parentStruct, fieldId, &bSuccess);
      return bSuccess.ToBool();
    }

    public bool TryReadWord(out ushort value)
    {
      int bSuccess;
      value = ResGff.ReadFieldWORD(parentStruct, fieldId, &bSuccess);
      return bSuccess.ToBool();
    }

    protected override bool GetValueInternal(out object value, Type requestedType = null)
    {
      switch (FieldType)
      {
        case GffResourceFieldType.Byte when (requestedType == null || requestedType == typeof(byte)) && TryReadByte(out byte readValue):
          value = readValue;
          return true;
        case GffResourceFieldType.Char when (requestedType == null || requestedType == typeof(byte)) && TryReadChar(out byte readValue):
          value = readValue;
          return true;
        case GffResourceFieldType.Word when (requestedType == null || requestedType == typeof(ushort)) && TryReadWord(out ushort readValue):
          value = readValue;
          return true;
        case GffResourceFieldType.Short when (requestedType == null || requestedType == typeof(short)) && TryReadShort(out short readValue):
          value = readValue;
          return true;
        case GffResourceFieldType.DWord when (requestedType == null || requestedType == typeof(uint)) && TryReadDWord(out uint readValue):
          value = readValue;
          return true;
        case GffResourceFieldType.Int when (requestedType == null || requestedType == typeof(int)) && TryReadInt(out int readValue):
          value = readValue;
          return true;
        case GffResourceFieldType.DWord64 when (requestedType == null || requestedType == typeof(ulong)) && TryReadDWord64(out ulong readValue):
          value = readValue;
          return true;
        case GffResourceFieldType.Int64 when (requestedType == null || requestedType == typeof(long)) && TryReadInt64(out long readValue):
          value = readValue;
          return true;
        case GffResourceFieldType.Float when (requestedType == null || requestedType == typeof(float)) && TryReadFloat(out float readValue):
          value = readValue;
          return true;
        case GffResourceFieldType.Double when (requestedType == null || requestedType == typeof(double)) && TryReadDouble(out double readValue):
          value = readValue;
          return true;
        case GffResourceFieldType.CExoString when (requestedType == null || requestedType == typeof(string)) && TryReadCExoString(out string readValue):
          value = readValue;
          return true;
        case GffResourceFieldType.CResRef when (requestedType == null || requestedType == typeof(string)) && TryReadCResRef(out string readValue):
          value = readValue;
          return true;
        case GffResourceFieldType.CExoLocString when (requestedType == null || requestedType == typeof(string)) && TryReadCExoLocString(out string readValue):
          value = readValue;
          return true;
        default:
          value = null;
          return false;
      }
    }
  }
}
