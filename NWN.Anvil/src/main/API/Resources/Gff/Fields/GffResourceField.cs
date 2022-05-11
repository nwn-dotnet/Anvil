using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NWN.Native.API;

namespace Anvil.API
{
  /// <summary>
  /// A property/field in a <see cref="GffResource"/>.
  /// </summary>
  public abstract unsafe class GffResourceField
  {
    protected readonly CResGFF ResGff;

    protected GffResourceField(CResGFF resGff)
    {
      ResGff = resGff;
    }

    /// <summary>
    /// Gets the number of child fields.
    /// </summary>
    public virtual int Count => 0;

    /// <summary>
    /// If this field is a struct, gets an enumerable of the key/values pairs.<br/>
    /// Otherwise, returns an empty enumerable.
    /// </summary>
    public virtual IEnumerable<KeyValuePair<string, GffResourceField>> EntrySet { get; } = Enumerable.Empty<KeyValuePair<string, GffResourceField>>();

    /// <summary>
    /// Gets the GFF field type.
    /// </summary>
    public abstract GffResourceFieldType FieldType { get; }

    /// <summary>
    /// Gets whether this field contains child values.
    /// </summary>
    public bool HasChildren => this is IEnumerable;

    /// <summary>
    /// If this field is a struct, gets an enumerable of the struct's keys.<br/>
    /// Otherwise, returns an empty enumerable.
    /// </summary>
    public virtual IEnumerable<string> Keys { get; } = Enumerable.Empty<string>();

    /// <summary>
    /// If this field is an array, gets an enumerable of the array's values.<br/>
    /// If this field is a struct, gets an enumerable of the struct's values.<br/>
    /// Otherwise, returns an empty enumerable.
    /// </summary>
    public virtual IEnumerable<GffResourceField> Values { get; } = Enumerable.Empty<GffResourceField>();

    /// <summary>
    /// Gets the child <see cref="GffResourceField"/> at the specified index.
    /// </summary>
    public virtual GffResourceField this[int index] => throw new InvalidOperationException($"Cannot get child value of {FieldType} with field index.");

    /// <summary>
    /// Gets the child <see cref="GffResourceField"/> with the specified key.
    /// </summary>
    public virtual GffResourceField this[string key] => throw new InvalidOperationException($"Cannot get child value of {FieldType} with field key.");

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

    public static explicit operator string?(GffResourceField field)
    {
      return field.Value<string>();
    }

    /// <summary>
    /// If this field is a <see cref="GffResourceFieldStruct"/>, determines if the specified key exists in the structure.
    /// </summary>
    /// <param name="key">The key to locate in the <see cref="GffResourceFieldStruct"/>./</param>
    /// <returns>True if the field is a <see cref="GffResourceFieldStruct"/> and the key exists. Otherwise, false.</returns>
    public virtual bool ContainsKey(string key)
    {
      return false;
    }

    public sealed override string? ToString()
    {
      return Value()?.ToString();
    }

    /// <summary>
    /// If this field is a <see cref="GffResourceFieldStruct"/>, gets the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key of the value to get./</param>
    /// <param name="value">If the method runs successfully (returns true), this output parameter will contain the associated value.</param>
    /// <returns>True if the field is a <see cref="GffResourceFieldStruct"/> and the key exists. Otherwise, false.</returns>
    public virtual bool TryGetValue(string key, [NotNullWhen(true)] out GffResourceField? value)
    {
      value = default;
      return false;
    }

    /// <summary>
    /// Gets the value of this field.
    /// </summary>
    /// <returns>The value associated with this <see cref="GffResourceField"/>.</returns>
    /// <exception cref="InvalidOperationException">Thrown if this field is not a standard value type.</exception>
    public object? Value()
    {
      if (GetValueInternal(out object? value))
      {
        return value;
      }

      throw new InvalidOperationException($"Cannot convert {FieldType} to value.");
    }

    /// <summary>
    /// Gets the value of this field.
    /// </summary>
    /// <typeparam name="T">The field value type.</typeparam>
    /// <returns>The value associated with this <see cref="GffResourceField"/>.</returns>
    /// <exception cref="InvalidOperationException">Thrown if this field is not a standard value type, or an invalid generic type was specified..</exception>
    public T? Value<T>()
    {
      if (GetValueInternal(out object? value, typeof(T)))
      {
        return (T?)value;
      }
      else
      {
        throw new InvalidOperationException($"Cannot convert {FieldType} to {typeof(T).Name}");
      }
    }

    /// <summary>
    /// Gets the value of this field.
    /// </summary>
    /// <returns>The value associated with this <see cref="GffResourceField"/>. Returns null if the value is a list or struct.</returns>
    public object? ValueOrDefault()
    {
      return GetValueInternal(out object? value) ? value : default;
    }

    /// <summary>
    /// Gets the value of this field.
    /// </summary>
    /// <returns>The value associated with this <see cref="GffResourceField"/>. Returns null if the value is a list or struct, or does not match the specified type.</returns>
    public T? ValueOrDefault<T>()
    {
      return GetValueInternal(out object? value, typeof(T)) ? (T?)value : default;
    }

    internal static GffResourceField? Create(CResGFF resGff, CResStruct resStruct, string fieldId)
    {
      byte* fieldIdPtr = fieldId.GetNullTerminatedString();
      uint index = resGff.GetFieldByLabel(resStruct, fieldIdPtr);
      return Create(resGff, resStruct, index, fieldIdPtr);
    }

    internal static GffResourceField? Create(CResGFF resGff, CResStruct resStruct, uint fieldIndex)
    {
      byte* fieldId = resGff.GetFieldStringID(resStruct, fieldIndex);
      if (fieldId == null)
      {
        return null;
      }

      return Create(resGff, resStruct, fieldIndex, fieldId);
    }

    internal static GffResourceField? Create(CResGFF resGff, CResStruct resStruct, uint fieldIndex, byte* fieldId)
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

    protected virtual bool GetValueInternal(out object? value, Type? requestedType = null)
    {
      value = null;
      return false;
    }
  }
}
