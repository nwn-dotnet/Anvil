using System;
using Anvil.Services;
using NWN.Core;
using NWN.Native.API;

namespace Anvil.API
{
  [ServiceBinding(typeof(TlkTable))]
  [ServiceBindingOptions(InternalBindingPriority.API)]
  public sealed class TlkTable
  {
    private readonly CTlkTable tlkTable = NWNXLib.TlkTable();

    /// <summary>
    /// Clears the specified TLK override.
    /// </summary>
    /// <param name="strRef">The strref to restore to default.</param>
    public void ClearTlkOverride(uint strRef)
    {
      NWScript.SetTlkOverride((int)strRef);
    }

    /// <summary>
    /// Gets the value of the specified token.
    /// </summary>
    /// <param name="tokenNumber">The token number to query.</param>
    /// <returns>The string representation of the token value.</returns>
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

    public string GetSimpleString(uint strRef)
    {
      return tlkTable.GetSimpleString(strRef).ToString();
    }

    /// <summary>
    /// Sets the value of the specified token.<br/>
    /// </summary>
    /// <remarks>
    /// Custom tokens 0-9 are used by BioWare and should not be used.<br/>
    /// There is a risk if you reuse components that they will have scripts that set the same custom tokens as you set.<br/>
    /// To avoid this, set your custom tokens right before your conversations (do not create new tokens within a conversation, create them all at the beginning of the conversation).<br/>
    /// To use a custom token, place &lt;CUSTOMxxxx&gt; somewhere in your conversation, where xxxx is the value supplied for nCustomTokenNumber. &lt;CUSTOM100&gt; for example.
    /// </remarks>
    /// <param name="tokenNumber">The token number to query.</param>
    /// <param name="tokenValue">The new string representation of the token value.</param>
    public void SetCustomToken(uint tokenNumber, string tokenValue)
    {
      NWScript.SetCustomToken((int)tokenNumber, tokenValue);
    }

    /// <summary>
    /// Overrides the specified strref to return a dfferent value instead of what is in the TLK file.
    /// </summary>
    /// <param name="strRef">The strref to override.</param>
    /// <param name="value">The override value.</param>
    /// <exception cref="ArgumentException">Thrown if value is an empty string or null. Use <see cref="ClearTlkOverride"/> to clear overrides.</exception>
    public void SetTlkOverride(uint strRef, string value)
    {
      if (string.IsNullOrEmpty(value))
      {
        throw new ArgumentException("New value must not be null or empty.", nameof(value));
      }

      tlkTable.SetOverride(strRef, value.ToExoString());
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
