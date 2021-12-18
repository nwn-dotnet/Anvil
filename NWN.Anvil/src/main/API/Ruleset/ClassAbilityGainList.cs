using System;
using System.Collections;
using System.Collections.Generic;

namespace Anvil.API
{
  public sealed class ClassAbilityGainList : IReadOnlyDictionary<Ability, sbyte>
  {
    private readonly sbyte[] values;

    internal ClassAbilityGainList(sbyte[] values)
    {
      this.values = values;
    }

    public int Count
    {
      get => values.Length;
    }

    public IEnumerable<Ability> Keys
    {
      get => Enum.GetValues<Ability>();
    }

    public IEnumerable<sbyte> Values
    {
      get => values;
    }

    public sbyte this[Ability key]
    {
      get => values[(int)key];
    }

    public bool ContainsKey(Ability key)
    {
      return (int)key > 0 && (int)key < values.Length;
    }

    public IEnumerator<KeyValuePair<Ability, sbyte>> GetEnumerator()
    {
      for (int i = 0; i < values.Length; i++)
      {
        yield return new KeyValuePair<Ability, sbyte>((Ability)i, values[i]);
      }
    }

    public bool TryGetValue(Ability key, out sbyte value)
    {
      if (ContainsKey(key))
      {
        value = values[(int)key];
        return true;
      }

      value = default;
      return false;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}
