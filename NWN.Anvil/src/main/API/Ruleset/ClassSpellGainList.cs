using System.Collections;
using System.Collections.Generic;
using NWN.Native.API;

namespace Anvil.API
{
  public sealed class ClassSpellGainList(NativeArray<byte> array) : IReadOnlyList<byte>
  {
    public int Count => array.Length;

    public byte this[int index] => GetGainAmount(index);

    public IEnumerator<byte> GetEnumerator()
    {
      return array.GetEnumerator();
    }

    public byte GetGainAmount(int spellLevel)
    {
      if (spellLevel > 9)
      {
        return 0;
      }

      return spellLevel > 0 && spellLevel < array.Length ? array[spellLevel] : (byte)0;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}
