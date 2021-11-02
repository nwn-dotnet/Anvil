using System;
using NWN.Native.API;

namespace Anvil.API
{
  public abstract unsafe class GffResourceField : IDisposable
  {
    protected CResGFF ResGff { get; private set; }

    protected GffResourceField(CResGFF resGff)
    {
      ResGff = resGff;
    }

    ~GffResourceField()
    {
      Dispose(false);
    }

    public abstract GffResourceFieldType FieldType { get; }

    public virtual GffResourceField this[string key]
    {
      get => throw new InvalidOperationException($"Cannot get child value of {FieldType} with field key.");
    }

    public virtual GffResourceField this[uint index]
    {
      get => throw new InvalidOperationException($"Cannot get child value of {FieldType} with field index.");
    }

    protected virtual string ReadCExoString()
    {
      throw new InvalidOperationException($"Cannot convert {FieldType} to string.");
    }

    public static implicit operator string(GffResourceField field)
    {
      return field.ReadCExoString();
    }

    public override string ToString()
    {
      return ReadCExoString();
    }

    public void Dispose()
    {
      Dispose(true);
    }

    internal static GffResourceField Create(CResGFF resGff, CResStruct resStruct, string fieldId)
    {
      byte* fieldIdPtr = fieldId.GetNullTerminatedString();
      uint index = resGff.GetFieldByLabel(resStruct, fieldIdPtr);
      return Create(resGff, resStruct, index, fieldIdPtr);
    }

    internal static GffResourceField Create(CResGFF resGff, CResStruct resStruct, uint fieldIndex)
    {
      byte* fieldId = resGff.GetFieldStringID(resStruct, fieldIndex);
      if (fieldId == null)
      {
        return null;
      }

      return Create(resGff, resStruct, fieldIndex, fieldId);
    }

    internal static GffResourceField Create(CResGFF resGff, CResStruct resStruct, uint fieldIndex, byte* fieldId)
    {
      GffResourceFieldType fieldType = (GffResourceFieldType)resGff.GetFieldType(resStruct, fieldId, fieldIndex);
      if (!Enum.IsDefined(fieldType)) // User specified struct type.
      {
        fieldType = GffResourceFieldType.Struct;
      }

      switch (fieldType)
      {
        case GffResourceFieldType.Struct:
          CResStruct structField = new CResStruct();
          if (resGff.GetStructFromStruct(structField, resStruct, fieldId).ToBool())
          {
            return new GffResourceFieldStruct(resGff, resStruct);
          }

          break;
        case GffResourceFieldType.List:
          CResList listField = new CResList();
          if (resGff.GetList(listField, resStruct, fieldId).ToBool())
          {
            return new GffResourceFieldList(resGff, listField, resGff.GetListCount(listField));
          }

          break;
        default:
          return new GffResourceFieldValue(resGff, resStruct, fieldId);
      }

      return null;
    }

    private void Dispose(bool disposing)
    {
      ResGff?.Dispose();
      ResGff = null;

      if (disposing)
      {
        GC.SuppressFinalize(this);
      }
    }
  }
}
