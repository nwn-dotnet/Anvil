using Anvil.Services;

namespace Anvil.API
{
  public readonly struct StrTokenCustom
  {
    [Inject]
    private static TlkTable TlkTable { get; set; } = null!;

    public readonly int TokenNumber;

    public StrTokenCustom(int tokenNumber)
    {
      TokenNumber = tokenNumber;
    }

    /// <summary>
    /// Gets or sets the string value of this token.<br/>
    /// </summary>
    /// <remarks>
    /// Custom tokens 0-9 are used by BioWare and should not be modified.<br/>
    /// There is a risk if you reuse components that they will have scripts that set the same custom tokens as you set.<br/>
    /// To avoid this, set your custom tokens right before your conversations (do not create new tokens within a conversation, create them all at the beginning of the conversation).<br/>
    /// To use a custom token, place &lt;CUSTOMxxxx&gt; somewhere in your conversation, where xxxx is the value supplied for nCustomTokenNumber. &lt;CUSTOM100&gt; for example.
    /// </remarks>
    public string? Value
    {
      get => TlkTable.GetCustomToken(this);
      set => TlkTable.SetCustomToken(this, value);
    }
  }
}
