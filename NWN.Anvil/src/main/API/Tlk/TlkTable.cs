using System;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API
{
  [ServiceBinding(typeof(TlkTable))]
  [ServiceBindingOptions(InternalBindingPriority.API)]
  public sealed class TlkTable
  {
    private readonly CTlkTable tlkTable = NWNXLib.TlkTable();

    [Obsolete("Use StrRef.ClearOverride instead.")]
    public void ClearTlkOverride(uint strId)
    {
      StrRef strRef = new StrRef(strId);
      strRef.ClearOverride();
    }

    [Obsolete("Use StrTokenCustom.Value instead.")]
    public string GetCustomToken(uint tokenNumber)
    {
      return new StrTokenCustom((int)tokenNumber).Value;
    }

    [Obsolete("Use StrRef.ToString() instead.")]
    public string GetSimpleString(uint strRef)
    {
      return new StrRef(strRef).ToString();
    }

    [Obsolete("Use StrToken.Value instead.")]
    public void SetCustomToken(uint tokenNumber, string tokenValue)
    {
      StrTokenCustom _ = new StrTokenCustom((int)tokenNumber)
      {
        Value = tokenValue,
      };
    }

    [Obsolete("Use StrRef.Override instead.")]
    public void SetTlkOverride(uint strId, string value)
    {
      StrRef _ = new StrRef(strId)
      {
        Override = value,
      };
    }

    internal string? GetCustomToken(StrTokenCustom customToken)
    {
      int numTokens = (int)tlkTable.m_nTokensCustom;

      CTlkTableTokenCustomArray tokenArray = CTlkTableTokenCustomArray.FromPointer(tlkTable.m_pTokensCustom);
      CTlkTableTokenCustom token = new CTlkTableTokenCustom { m_nNumber = (uint)customToken.TokenNumber };

      int index = BinarySearch(tokenArray, 0, numTokens, token);
      if (index < 0)
      {
        return null;
      }

      CExoString retVal = tokenArray[index].m_sValue;
      return retVal.ToString();
    }

    internal string? GetTlkOverride(StrRef strRef)
    {
      if (tlkTable.m_overrides.TryGetValue(strRef.Id, out CExoString retVal))
      {
        return retVal.ToString();
      }

      return null;
    }

    internal string? ResolveParsedStringFromStrRef(StrRef strRef)
    {
      CExoString rawString = tlkTable.GetSimpleString(strRef.Id);
      if (rawString != null)
      {
        tlkTable.ParseStr(rawString);
      }

      return rawString?.ToString();
    }

    internal string ResolveStringFromStrRef(StrRef strRef)
    {
      return tlkTable.GetSimpleString(strRef.Id).ToString();
    }

    internal void SetCustomToken(StrTokenCustom customToken, string value)
    {
      tlkTable.SetCustomToken(customToken.TokenNumber, value.ToExoString());
    }

    internal void SetTlkOverride(StrRef strRef, string? value)
    {
      tlkTable.SetOverride(strRef.Id, value.ToExoString());
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
  }
}
