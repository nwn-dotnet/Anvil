using Anvil.Services;

namespace Anvil.API
{
  /// <summary>
  /// A talk table (tlk) string reference.
  /// </summary>
  public readonly struct StrRef
  {
    private const uint CustomTlkOffset = 0x1000000;

    [Inject]
    private static TlkTable TlkTable { get; set; }

    /// <summary>
    /// Gets the index/key for this StrRef.
    /// </summary>
    public readonly uint Id;

    /// <summary>
    /// Gets the index/key for this StrRef relative to the module's custom talk table. (-16777216)
    /// </summary>
    public uint CustomId => Id - CustomTlkOffset;

    /// <summary>
    /// Gets or sets a string override that this StrRef should return instead of the tlk file definition.
    /// </summary>
    public string Override
    {
      get => TlkTable.GetTlkOverride(this);
      set => TlkTable.SetTlkOverride(this, value);
    }

    public StrRef(uint stringId)
    {
      Id = stringId;
    }

    public StrRef(int stringId) : this((uint)stringId) {}

    public void ClearOverride()
    {
      TlkTable.SetTlkOverride(this, null);
    }

    /// <summary>
    /// Gets a StrRef from the module's custom tlk table.<br/>
    /// Use the index from the custom tlk. This factory method will automatically offset the id value.
    /// </summary>
    /// <param name="stringId">The custom token index.</param>
    /// <returns>The associated StrRef.</returns>
    public static StrRef FromCustomTlk(uint stringId)
    {
      return new StrRef(stringId + CustomTlkOffset);
    }

    /// <summary>
    /// Gets the string associated with this string reference/token number.
    /// </summary>
    /// <returns>The associated string.</returns>
    public override string ToString()
    {
      return TlkTable.ResolveStringFromStrRef(this);
    }
  }
}
