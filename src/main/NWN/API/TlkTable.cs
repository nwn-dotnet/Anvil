using NWN.Native.API;

namespace NWN.API
{
  public sealed class TlkTable
  {
    public static readonly TlkTable Instance = new TlkTable(NWNXLib.TlkTable());

    private readonly CTlkTable tlkTable;

    internal TlkTable(CTlkTable tlkTable)
    {
      this.tlkTable = tlkTable;
    }

    public string GetCustomToken(uint tokenNumber)
    {
      int numTokens = (int)tlkTable.m_nTokensCustom;

      CTlkTableTokenCustomArray tokenArray = CTlkTableTokenCustomArray.FromPointer(tlkTable.m_pTokensCustom);
      CTlkTableTokenCustom token = new CTlkTableTokenCustom { m_nNumber = tokenNumber };

      int index = BinarySearch(tokenArray, 0, numTokens, token);
      if (index < 0)
      {
        return null;
      }

      CExoString retVal = tokenArray[index].m_sValue;
      return retVal.ToString();
    }

    private int BinarySearch(CTlkTableTokenCustomArray array, int index, int length, CTlkTableTokenCustom value)
    {
      int low = index;
      int high = index + length - 1;

      while (low <= high)
      {
        int i = low + ((high - low) >> 1);
        int order = array[i].m_nNumber.CompareTo(value.m_nNumber);

        if (order == 0)
        {
          return i;
        }

        if (order < 0)
        {
          low = i + 1;
        }
        else
        {
          high = i - 1;
        }
      }

      return ~low;
    }

    public string GetSimpleString(uint strRef)
    {
      return tlkTable.GetSimpleString(strRef).ToString();
    }
  }
}
