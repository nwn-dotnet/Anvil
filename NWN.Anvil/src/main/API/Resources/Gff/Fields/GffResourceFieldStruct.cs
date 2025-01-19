using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NWN.Native.API;
using NWNX.NET.Native;

namespace Anvil.API
{
  /// <summary>
  /// A <see cref="GffResourceField"/> containing a structure of key/value pairs.
  /// </summary>
  public sealed unsafe class GffResourceFieldStruct : GffResourceField, IReadOnlyDictionary<string, GffResourceField>
  {
    private readonly Dictionary<string, GffResourceField> fieldLookup = new Dictionary<string, GffResourceField>();
    private readonly List<string> keys = [];
    private readonly List<GffResourceField> values = [];

    internal GffResourceFieldStruct(CResGFF resGff, CResStruct resStruct) : base(resGff)
    {
      int fieldCount = (int)resGff.GetFieldCount(resStruct);
      List<KeyValuePair<string, GffResourceField>> entrySet = [];

      for (uint i = 0; i < fieldCount; i++)
      {
        byte* fieldIdPtr = ResGff.GetFieldLabel(resStruct, i);
        string? key = StringUtils.ReadNullTerminatedString(fieldIdPtr);
        GffResourceField? value = Create(resGff, resStruct, i, fieldIdPtr);

        if (key == null || value == null)
        {
          continue;
        }

        keys.Add(key);
        values.Add(value);
        fieldLookup.Add(key, value);
        entrySet.Add(new KeyValuePair<string, GffResourceField>(key, value));
      }

      EntrySet = entrySet;
    }

    public override int Count => keys.Count;

    public override IEnumerable<KeyValuePair<string, GffResourceField>> EntrySet { get; }

    public override GffResourceFieldType FieldType => GffResourceFieldType.Struct;

    public override IEnumerable<string> Keys => keys;

    public override IEnumerable<GffResourceField> Values => values;

    public override GffResourceField this[int index] => fieldLookup[keys[index]];

    public override GffResourceField this[string key] => fieldLookup[key];

    public override bool ContainsKey(string key)
    {
      return fieldLookup.ContainsKey(key);
    }

    public IEnumerator<KeyValuePair<string, GffResourceField>> GetEnumerator()
    {
      return EntrySet.GetEnumerator();
    }

    public override bool TryGetValue(string key, [NotNullWhen(true)] out GffResourceField? value)
    {
      return fieldLookup.TryGetValue(key, out value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}
