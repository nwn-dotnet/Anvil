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

    protected override string ReadCExoString()
    {
      return TryReadCExoString(out string retVal) ? retVal : null;
    }

    public bool TryReadCExoString(out string value)
    {
      int bSuccess;
      value = ResGff.ReadFieldCExoString(parentStruct, fieldId, &bSuccess).ToString();
      return bSuccess.ToBool();
    }

    public bool TryReadCResRef(out CResRef value)
    {
      int bSuccess;
      value = ResGff.ReadFieldCResRef(parentStruct, fieldId, &bSuccess);
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

    public bool TryReadByte(out byte value)
    {
      int bSuccess;
      value = ResGff.ReadFieldBYTE(parentStruct, fieldId, &bSuccess);
      return bSuccess.ToBool();
    }

    public bool TryReadCExoLocString(out CExoLocString value)
    {
      int bSuccess;
      value = ResGff.ReadFieldCExoLocString(parentStruct, fieldId, &bSuccess);
      return bSuccess.ToBool();
    }

    public bool TryReadChar(out byte value)
    {
      int bSuccess;
      value = ResGff.ReadFieldCHAR(parentStruct, fieldId, &bSuccess);
      return bSuccess.ToBool();
    }

    public bool TryReadWord(out ushort value)
    {
      int bSuccess;
      value = ResGff.ReadFieldWORD(parentStruct, fieldId, &bSuccess);
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

    public bool TryReadShort(out short value)
    {
      int bSuccess;
      value = ResGff.ReadFieldSHORT(parentStruct, fieldId, &bSuccess);
      return bSuccess.ToBool();
    }

    public bool TryReadDouble(out double value)
    {
      int bSuccess;
      value = ResGff.ReadFieldDOUBLE(parentStruct, fieldId, &bSuccess);
      return bSuccess.ToBool();
    }
  }
}
