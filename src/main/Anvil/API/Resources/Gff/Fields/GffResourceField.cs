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

    public override string ToString()
    {
      return Value().ToString();
    }

    public object Value()
    {
      switch (FieldType)
      {
        case GffResourceFieldType.Byte when TryReadByte(out byte value):
          return value;
        case GffResourceFieldType.Char when TryReadChar(out byte value):
          return value;
        case GffResourceFieldType.Word when TryReadWord(out ushort value):
          return value;
        case GffResourceFieldType.Short when TryReadShort(out short value):
          return value;
        case GffResourceFieldType.DWord when TryReadDWord(out uint value):
          return value;
        case GffResourceFieldType.Int when TryReadInt(out int value):
          return value;
        case GffResourceFieldType.DWord64 when TryReadDWord64(out ulong value):
          return value;
        case GffResourceFieldType.Int64 when TryReadInt64(out long value):
          return value;
        case GffResourceFieldType.Float when TryReadFloat(out float value):
          return value;
        case GffResourceFieldType.Double when TryReadDouble(out double value):
          return value;
        case GffResourceFieldType.CExoString when TryReadCExoString(out string value):
          return value;
        case GffResourceFieldType.CResRef when TryReadCResRef(out string value):
          return value;
        case GffResourceFieldType.CExoLocString when TryReadCExoLocString(out string value):
          return value;
        default:
          throw new InvalidOperationException($"Cannot convert {FieldType} to value.");
      }
    }

    public T Value<T>()
    {
      switch (FieldType)
      {
        case GffResourceFieldType.Byte when typeof(T) == typeof(byte) && TryReadByte(out byte value):
          return (T)(object)value;
        case GffResourceFieldType.Char when typeof(T) == typeof(byte) && TryReadChar(out byte value):
          return (T)(object)value;
        case GffResourceFieldType.Word when typeof(T) == typeof(ushort) && TryReadWord(out ushort value):
          return (T)(object)value;
        case GffResourceFieldType.Short when typeof(T) == typeof(short) && TryReadShort(out short value):
          return (T)(object)value;
        case GffResourceFieldType.DWord when typeof(T) == typeof(uint) && TryReadDWord(out uint value):
          return (T)(object)value;
        case GffResourceFieldType.Int when typeof(T) == typeof(int) && TryReadInt(out int value):
          return (T)(object)value;
        case GffResourceFieldType.DWord64 when typeof(T) == typeof(ulong) && TryReadDWord64(out ulong value):
          return (T)(object)value;
        case GffResourceFieldType.Int64 when typeof(T) == typeof(long) && TryReadInt64(out long value):
          return (T)(object)value;
        case GffResourceFieldType.Float when typeof(T) == typeof(float) && TryReadFloat(out float value):
          return (T)(object)value;
        case GffResourceFieldType.Double when typeof(T) == typeof(double) && TryReadDouble(out double value):
          return (T)(object)value;
        case GffResourceFieldType.CExoString when typeof(T) == typeof(string) && TryReadCExoString(out string value):
          return (T)(object)value;
        case GffResourceFieldType.CResRef when typeof(T) == typeof(string) && TryReadCResRef(out string value):
          return (T)(object)value;
        case GffResourceFieldType.CExoLocString when typeof(T) == typeof(string) && TryReadCExoLocString(out string value):
          return (T)(object)value;
        default:
          throw new InvalidOperationException($"Cannot convert {FieldType} to {typeof(T).Name}");
      }
    }

    public static explicit operator byte(GffResourceField field)
    {
      return field.Value<byte>();
    }

    public static explicit operator ushort(GffResourceField field)
    {
      return field.Value<ushort>();
    }

    public static explicit operator short(GffResourceField field)
    {
      return field.Value<short>();
    }

    public static explicit operator uint(GffResourceField field)
    {
      return field.Value<uint>();
    }

    public static explicit operator int(GffResourceField field)
    {
      return field.Value<int>();
    }

    public static explicit operator ulong(GffResourceField field)
    {
      return field.Value<ulong>();
    }

    public static explicit operator long(GffResourceField field)
    {
      return field.Value<long>();
    }

    public static explicit operator float(GffResourceField field)
    {
      return field.Value<float>();
    }

    public static explicit operator double(GffResourceField field)
    {
      return field.Value<double>();
    }

    public static explicit operator string(GffResourceField field)
    {
      return field.Value<string>();
    }

    public virtual bool TryReadCExoString(out string value)
    {
      value = default;
      return false;
    }

    public virtual bool TryReadCResRef(out string value)
    {
      value = default;
      return false;
    }

    public virtual bool TryReadInt(out int value)
    {
      value = default;
      return false;
    }

    public virtual bool TryReadInt64(out long value)
    {
      value = default;
      return false;
    }

    public virtual bool TryReadByte(out byte value)
    {
      value = default;
      return false;
    }

    public virtual bool TryReadCExoLocString(out string value, int id = 0, Gender gender = Gender.Male)
    {
      value = default;
      return false;
    }

    public virtual bool TryReadChar(out byte value)
    {
      value = default;
      return false;
    }

    public virtual bool TryReadWord(out ushort value)
    {
      value = default;
      return false;
    }

    public virtual bool TryReadDWord(out uint value)
    {
      value = default;
      return false;
    }

    public virtual bool TryReadDWord64(out ulong value)
    {
      value = default;
      return false;
    }

    public virtual bool TryReadFloat(out float value)
    {
      value = default;
      return false;
    }

    public virtual bool TryReadShort(out short value)
    {
      value = default;
      return false;
    }

    public virtual bool TryReadDouble(out double value)
    {
      value = default;
      return false;
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
