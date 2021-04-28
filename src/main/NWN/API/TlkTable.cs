using NWN.Native;
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

    public unsafe string GetCustomToken(uint tokenNumber)
    {
      int numTokens = (int)tlkTable.m_nTokensCustom;

      CTlkTableTokenCustomStruct* tokenArray = (CTlkTableTokenCustomStruct*)tlkTable.m_pTokensCustom.Pointer;
      CTlkTableTokenCustomStruct token = new CTlkTableTokenCustomStruct(tokenNumber, default);

      int index = NativeUtils.BinarySearch(tokenArray, 0, numTokens, token, CTlkTableTokenCustomStruct.TokenNumberComparer);
      if (index < 0)
      {
        return null;
      }

      CExoString retVal = tokenArray[index].m_sValue;
      return retVal.ToString();
    }

    public string GetSimpleString(uint strRef)
    {
      return tlkTable.GetSimpleString(strRef).ToString();
    }
  }
}
