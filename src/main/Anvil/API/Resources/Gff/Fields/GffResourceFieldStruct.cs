using System.Collections;
using System.Collections.Generic;
using NWN.Native.API;

namespace Anvil.API
{
  /// <summary>
  /// A <see cref="GffResourceField"/> containing a structure of key/value pairs.
  /// </summary>
  public sealed unsafe class GffResourceFieldStruct : GffResourceField, IReadOnlyDictionary<string, GffResourceField>
  {
    private readonly List<string> keys = new List<string>();
    private readonly List<GffResourceField> values = new List<GffResourceField>();

    private readonly Dictionary<string, GffResourceField> fieldLookup = new Dictionary<string, GffResourceField>();

    public override GffResourceFieldType FieldType
    {
      get => GffResourceFieldType.Struct;
    }

    internal GffResourceFieldStruct(CResGFF resGff, CResStruct resStruct) : base(resGff)
    {
      int fieldCount = (int)resGff.GetFieldCount(resStruct);
      List<KeyValuePair<string, GffResourceField>> entrySet = new List<KeyValuePair<string, GffResourceField>>();

      for (uint i = 0; i < fieldCount; i++)
      {
        byte* fieldIdPtr = ResGff.GetFieldLabel(resStruct, i);
        string key = StringHelper.ReadNullTerminatedString(fieldIdPtr);
        GffResourceField value = Create(resGff, resStruct, i, fieldIdPtr);

        keys.Add(key);
        values.Add(value);
        fieldLookup.Add(key, value);
        entrySet.Add(new KeyValuePair<string, GffResourceField>(key, value));
      }

      EntrySet = entrySet;
    }

    public override IEnumerable<KeyValuePair<string, GffResourceField>> EntrySet { get; }

    public override int Count
    {
      get => keys.Count;
    }

    public override IEnumerable<string> Keys
    {
      get => keys;
    }

    public override IEnumerable<GffResourceField> Values
    {
      get => values;
    }

    public override GffResourceField this[int index]
    {
      get => fieldLookup[keys[index]];
    }

    public override GffResourceField this[string key]
    {
      get => fieldLookup[key];
    }

    public override bool ContainsKey(string key)
    {
      return fieldLookup.ContainsKey(key);
    }

    public override bool TryGetValue(string key, out GffResourceField value)
    {
      return fieldLookup.TryGetValue(key, out value);
    }

    public IEnumerator<KeyValuePair<string, GffResourceField>> GetEnumerator()
    {
      return EntrySet.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}
