using Anvil.Services;

namespace Anvil.API
{
  /// <summary>
  /// A talk table (tlk) string reference.
  /// </summary>
  public readonly struct StrRef
  {
    [Inject]
    private static TlkTable TlkTable { get; set; }

    public readonly uint TokenNumber;

    public StrRef(uint tokenNumber)
    {
      TokenNumber = tokenNumber;
    }

    public StrRef(int tokenNumber)
    {
      TokenNumber = (uint)tokenNumber;
    }

    /// <summary>
    /// Gets the string associated with this string reference/token number.
    /// </summary>
    /// <returns>The associated string.</returns>
    public override string ToString()
    {
      return TlkTable.GetSimpleString(TokenNumber);
    }
  }
}
