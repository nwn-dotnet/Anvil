using System;
using NWN.Native.API;

namespace Anvil.API
{
  public sealed unsafe class GffResourceFieldValue : GffResourceField
  {
    private readonly CResStruct parentStruct;
    private readonly byte* fieldId;

    public GffResourceFieldValue(CResGFF resGff, CResStruct parentStruct, byte* fieldId) : base(resGff)
    {
      this.parentStruct = parentStruct;
      this.fieldId = fieldId;
    }

    public override GffResourceFieldType FieldType
    {
      get => (GffResourceFieldType)ResGff.GetFieldType(parentStruct, fieldId);
    }

    public override bool TryReadCExoString(out string value)
    {
      int bSuccess;
      value = ResGff.ReadFieldCExoString(parentStruct, fieldId, &bSuccess).ToString();
      return bSuccess.ToBool();
    }

    public override bool TryReadCResRef(out string value)
    {
      int bSuccess;
      value = ResGff.ReadFieldCResRef(parentStruct, fieldId, &bSuccess).ToString();
      return bSuccess.ToBool();
    }

    public override bool TryReadInt(out int value)
    {
      int bSuccess;
      value = ResGff.ReadFieldINT(parentStruct, fieldId, &bSuccess);
      return bSuccess.ToBool();
    }

    public override bool TryReadInt64(out long value)
    {
      int bSuccess;
      value = ResGff.ReadFieldINT64(parentStruct, fieldId, &bSuccess);
      return bSuccess.ToBool();
    }

    public override bool TryReadByte(out byte value)
    {
      int bSuccess;
      value = ResGff.ReadFieldBYTE(parentStruct, fieldId, &bSuccess);
      return bSuccess.ToBool();
    }

    public override bool TryReadCExoLocString(out string value, int id = 0, Gender gender = Gender.Male)
    {
      int bSuccess;
      value = ResGff.ReadFieldCExoLocString(parentStruct, fieldId, &bSuccess).ExtractLocString(id, (byte)gender);
      return bSuccess.ToBool();
    }

    public override bool TryReadChar(out byte value)
    {
      int bSuccess;
      value = ResGff.ReadFieldCHAR(parentStruct, fieldId, &bSuccess);
      return bSuccess.ToBool();
    }

    public override bool TryReadWord(out ushort value)
    {
      int bSuccess;
      value = ResGff.ReadFieldWORD(parentStruct, fieldId, &bSuccess);
      return bSuccess.ToBool();
    }

    public override bool TryReadDWord(out uint value)
    {
      int bSuccess;
      value = ResGff.ReadFieldDWORD(parentStruct, fieldId, &bSuccess);
      return bSuccess.ToBool();
    }

    public override bool TryReadDWord64(out ulong value)
    {
      int bSuccess;
      value = ResGff.ReadFieldDWORD64(parentStruct, fieldId, &bSuccess);
      return bSuccess.ToBool();
    }

    public override bool TryReadFloat(out float value)
    {
      int bSuccess;
      value = ResGff.ReadFieldFLOAT(parentStruct, fieldId, &bSuccess);
      return bSuccess.ToBool();
    }

    public override bool TryReadShort(out short value)
    {
      int bSuccess;
      value = ResGff.ReadFieldSHORT(parentStruct, fieldId, &bSuccess);
      return bSuccess.ToBool();
    }

    public override bool TryReadDouble(out double value)
    {
      int bSuccess;
      value = ResGff.ReadFieldDOUBLE(parentStruct, fieldId, &bSuccess);
      return bSuccess.ToBool();
    }
  }
}
