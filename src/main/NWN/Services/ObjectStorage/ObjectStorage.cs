using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NWN.API;

namespace NWN.Services
{
  public sealed class ObjectStorage
  {
    private const string IntStorageKey = "INTMAP";
    private const string FloatStorageKey = "FLTMAP";
    private const string StringStorageKey = "STRMAP";

    private Dictionary<string, ObjectStorageValue<int>> intMap = new Dictionary<string, ObjectStorageValue<int>>();
    private Dictionary<string, ObjectStorageValue<float>> floatMap = new Dictionary<string, ObjectStorageValue<float>>();
    private Dictionary<string, ObjectStorageValue<string>> stringMap = new Dictionary<string, ObjectStorageValue<string>>();

    /// <summary>
    /// Gets the stored integer with the specified key.
    /// </summary>
    /// <param name="prefix">The storage prefix/group.</param>
    /// <param name="key">The storage key.</param>
    /// <returns>The integer value stored with the specified key, otherwise null if the key has no value populated.</returns>
    public int? GetInt(string prefix, string key)
    {
      string fullKey = prefix + "!" + key;
      return intMap.TryGetValue(fullKey, out ObjectStorageValue<int> value) ? value.Value : null;
    }

    /// <summary>
    /// Gets the stored float with the specified key.
    /// </summary>
    /// <param name="prefix">The storage prefix/group.</param>
    /// <param name="key">The storage key.</param>
    /// <returns>The float value stored with the specified key, otherwise null if the key has no value populated.</returns>
    public float? GetFloat(string prefix, string key)
    {
      string fullKey = prefix + "!" + key;
      return floatMap.TryGetValue(fullKey, out ObjectStorageValue<float> value) ? value.Value : null;
    }

    /// <summary>
    /// Gets the stored string with the specified key.
    /// </summary>
    /// <param name="prefix">The storage prefix/group.</param>
    /// <param name="key">The storage key.</param>
    /// <returns>The string stored with the specified key, otherwise null if the key has no value populated.</returns>
    public string GetString(string prefix, string key)
    {
      string fullKey = prefix + "!" + key;
      return stringMap.TryGetValue(fullKey, out ObjectStorageValue<string> value) ? value.Value : null;
    }

    /// <summary>
    /// Gets if the specified prefix + key combination has an integer value assigned.
    /// </summary>
    /// <param name="prefix">The storage prefix/group.</param>
    /// <param name="key">The storage key.</param>
    /// <returns>True if a value exists, otherwise false.</returns>
    public bool ContainsInt(string prefix, string key)
    {
      string fullKey = prefix + "!" + key;
      return intMap.ContainsKey(fullKey);
    }

    /// <summary>
    /// Gets if the specified prefix + key combination has a float value assigned.
    /// </summary>
    /// <param name="prefix">The storage prefix/group.</param>
    /// <param name="key">The storage key.</param>
    /// <returns>True if a value exists, otherwise false.</returns>
    public bool ContainsFloat(string prefix, string key)
    {
      string fullKey = prefix + "!" + key;
      return floatMap.ContainsKey(fullKey);
    }

    /// <summary>
    /// Gets if the specified prefix + key combination has a string value assigned.
    /// </summary>
    /// <param name="prefix">The storage prefix/group.</param>
    /// <param name="key">The storage key.</param>
    /// <returns>True if a value exists, otherwise false.</returns>
    public bool ContainsString(string prefix, string key)
    {
      string fullKey = prefix + "!" + key;
      return stringMap.ContainsKey(fullKey);
    }

    /// <summary>
    /// Stores the specified value using the specified unique key.
    /// </summary>
    /// <param name="prefix">The storage prefix/group.</param>
    /// <param name="key">The storage key.</param>
    /// <param name="value">The value to store.</param>
    /// <param name="persist">Set to true to include this value when the object is serialized.</param>
    public void Set(string prefix, string key, int value, bool persist = false)
    {
      string fullKey = prefix + "!" + key;
      ObjectStorageValue<int> data = new ObjectStorageValue<int> { Value = value, Persist = persist };
      intMap[fullKey] = data;
    }

    /// <inheritdoc cref="Set(string,string,int,bool)"/>
    public void Set(string prefix, string key, float value, bool persist = false)
    {
      string fullKey = prefix + "!" + key;
      ObjectStorageValue<float> data = new ObjectStorageValue<float> { Value = value, Persist = persist };
      floatMap[fullKey] = data;
    }

    /// <inheritdoc cref="Set(string,string,int,bool)"/>
    public void Set(string prefix, string key, string value, bool persist = false)
    {
      string fullKey = prefix + "!" + key;
      ObjectStorageValue<string> data = new ObjectStorageValue<string> { Value = value, Persist = persist };
      stringMap[fullKey] = data;
    }

    /// <summary>
    /// Removes an
    /// </summary>
    /// <param name="prefix">The storage prefix/group containing the key to be removed.</param>
    /// <param name="key">The storage key to be removed.</param>
    /// <returns>True if an entry was removed, otherwise false.</returns>
    public bool Remove(string prefix, string key)
    {
      string fullKey = prefix + "!" + key;

      return intMap.Remove(fullKey) ||
        floatMap.Remove(fullKey) ||
        stringMap.Remove(fullKey);
    }

    internal string Serialize(bool persistOnly = true)
    {
      List<KeyValuePair<string, ObjectStorageValue<int>>> intData = GetValuesToSerialize(intMap, persistOnly);
      List<KeyValuePair<string, ObjectStorageValue<float>>> floatData = GetValuesToSerialize(floatMap, persistOnly);
      List<KeyValuePair<string, ObjectStorageValue<string>>> stringData = GetValuesToSerialize(stringMap, persistOnly);

      if (intData.Count == 0 && floatData.Count == 0 && stringData.Count == 0)
      {
        return null;
      }

      StringBuilder stringBuilder = new StringBuilder();

      SerializeSimpleData(IntStorageKey, intData);
      SerializeSimpleData(FloatStorageKey, floatData);

      void SerializeSimpleData<T>(string storageKey, IReadOnlyCollection<KeyValuePair<string, ObjectStorageValue<T>>> data)
      {
        if (data.Count != 0)
        {
          stringBuilder.Append($"[{storageKey}:{data.Count}]");
          foreach ((string key, ObjectStorageValue<T> value) in data)
          {
            if (!persistOnly || value.Persist)
            {
              stringBuilder.Append($"<{key.Length}>{key} = {value.Value};");
            }
          }
        }
      }

      if (stringData.Count > 0)
      {
        stringBuilder.Append($"[{StringStorageKey}:{stringData.Count}]");
        foreach ((string key, ObjectStorageValue<string> value) in stringData)
        {
          if (!persistOnly || value.Persist)
          {
            stringBuilder.Append($"<{key.Length}>{key} = <{value.Value.Length}>{value.Value};");
          }
        }
      }

      return stringBuilder.ToString();
    }

    internal void Deserialize(string serialized, bool persist = true)
    {
      intMap.Clear();
      floatMap.Clear();
      stringMap.Clear();

      if (string.IsNullOrEmpty(serialized))
      {
        return;
      }

      using StringReader stringReader = new StringReader(serialized);

      int next;

      while ((next = stringReader.Read()) != -1)
      {
        char c = (char)next;
        if (c != '[')
        {
          throw new InvalidOperationException($"Expected a storage open character '[', but read character '{c}'");
        }

        string storageName = stringReader.ReadUntilChar(':');
        if (ReadAndAssertEndOfData(stringReader) != ':')
        {
          throw new InvalidOperationException($"Unknown storage type '{storageName}'");
        }

        string entryCountString = stringReader.ReadUntilChar(']');
        int entryCount = int.Parse(entryCountString);
        if (ReadAndAssertEndOfData(stringReader) != ']')
        {
          throw new InvalidOperationException($"Unknown storage type '{storageName}'");
        }

        switch (storageName)
        {
          case IntStorageKey:
            ReadStorage(intMap, entryCount, false, int.Parse);
            break;
          case FloatStorageKey:
            ReadStorage(floatMap, entryCount, false, float.Parse);
            break;
          case StringStorageKey:
            ReadStorage(stringMap, entryCount, true, value => value);
            break;
          default:
            throw new InvalidOperationException($"Unknown storage type '{storageName}'");
        }
      }

      void ReadStorage<T>(Dictionary<string, ObjectStorageValue<T>> store, int entryCount, bool valueHasLength, Func<string, T> parseValue)
      {
        for (int i = 0; i < entryCount; i++)
        {
          int keyLength = ReadLength(stringReader);
          string key = stringReader.ReadBlock(keyLength);
          stringReader.Skip(" = ".Length);

          string value;
          if (valueHasLength)
          {
            int valueLength = ReadLength(stringReader);
            value = stringReader.ReadBlock(valueLength);
          }
          else
          {
            value = stringReader.ReadUntilChar(';');
          }

          char endOfValue = ReadAndAssertEndOfData(stringReader);
          if (endOfValue != ';')
          {
            throw new InvalidOperationException($"Expected end of value token ';' but read character {endOfValue}");
          }

          store[key] = new ObjectStorageValue<T>
          {
            Persist = persist,
            Value = parseValue(value),
          };
        }
      }
    }

    internal ObjectStorage Clone()
    {
      ObjectStorage clone = new ObjectStorage
      {
        intMap = new Dictionary<string, ObjectStorageValue<int>>(intMap),
        floatMap = new Dictionary<string, ObjectStorageValue<float>>(floatMap),
        stringMap = new Dictionary<string, ObjectStorageValue<string>>(stringMap),
      };

      return clone;
    }

    private int ReadLength(StringReader stringReader)
    {
      char c = ReadAndAssertEndOfData(stringReader);
      if (c != '<')
      {
        throw new InvalidOperationException($"Expected a length open character '<', but read character '{c}'");
      }

      string length = stringReader.ReadUntilChar('>');

      c = ReadAndAssertEndOfData(stringReader);
      if (c != '>')
      {
        throw new InvalidOperationException($"Expected a length close character '>', but read character '{c}'");
      }

      return int.Parse(length);
    }

    private char ReadAndAssertEndOfData(StringReader stringReader)
    {
      int next = stringReader.Read();
      if (next == -1)
      {
        throw new InvalidOperationException($"Unexpected end of data.");
      }

      return (char)next;
    }

    private List<KeyValuePair<string, ObjectStorageValue<T>>> GetValuesToSerialize<T>(Dictionary<string, ObjectStorageValue<T>> source, bool persistOnly)
    {
      return persistOnly ? source.Where(pair => pair.Value.Persist).ToList() : source.ToList();
    }
  }
}
