using System.Collections;
using System.Collections.Generic;
using NWN.Native.API;

namespace Anvil.API
{
  public sealed unsafe class GffResourceFieldStruct : GffResourceField, IReadOnlyDictionary<string, GffResourceField>
  {
    private readonly List<string> keys = new List<string>();
    private readonly List<GffResourceField> values = new List<GffResourceField>();

    private readonly Dictionary<string, GffResourceField> fieldLookup = new Dictionary<string, GffResourceField>();

    public GffResourceFieldStruct(CResGFF resGff, CResStruct resStruct) : base(resGff)
    {
      Count = (int)resGff.GetFieldCount(resStruct);

      for (uint i = 0; i < Count; i++)
      {
        byte* fieldIdPtr = ResGff.GetFieldLabel(resStruct, i);
        string key = StringHelper.ReadNullTerminatedString(fieldIdPtr);
        GffResourceField value = Create(resGff, resStruct, i, fieldIdPtr);

        keys.Add(key);
        values.Add(value);
        fieldLookup.Add(key, value);
      }
    }

    public override GffResourceFieldType FieldType
    {
      get => GffResourceFieldType.Struct;
    }

    public int Count { get; }

    IEnumerable<string> IReadOnlyDictionary<string, GffResourceField>.Keys
    {
      get => keys;
    }

    IEnumerable<GffResourceField> IReadOnlyDictionary<string, GffResourceField>.Values
    {
      get => values;
    }

    public bool ContainsKey(string key)
    {
      return fieldLookup.ContainsKey(key);
    }

    public bool TryGetValue(string key, out GffResourceField value)
    {
      return fieldLookup.TryGetValue(key, out value);
    }

    public override GffResourceField this[string key]
    {
      get => fieldLookup[key];
    }

    public override GffResourceField this[uint index]
    {
      get => fieldLookup[keys[(int)index]];
    }

    public IEnumerator<KeyValuePair<string, GffResourceField>> GetEnumerator()
    {
      for (int i = 0; i < keys.Count; i++)
      {
        yield return new KeyValuePair<string, GffResourceField>(keys[i], values[i]);
      }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}
