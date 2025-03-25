using System.Text.Json.Serialization;
using Anvil.Internal;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API
{
  /// <summary>
  /// A talk table (tlk) string reference.
  /// </summary>
  [method: JsonConstructor]
  public readonly struct StrRef(uint stringId)
  {
    private const uint CustomTlkOffset = 0x1000000;

    [Inject]
    private static TlkTable TlkTable { get; set; } = null!;

    /// <summary>
    /// Gets the index/key for this StrRef.
    /// </summary>
    [JsonPropertyName("strref")]
    public readonly uint Id = stringId;

    public StrRef(int stringId) : this((uint)stringId) {}

    /// <summary>
    /// Gets the index/key for this StrRef relative to the module's custom talk table. (-16777216)
    /// </summary>
    [JsonIgnore]
    public uint CustomId => Id - CustomTlkOffset;

    /// <summary>
    /// Gets or sets a string override that this StrRef should return instead of the tlk file definition.
    /// </summary>
    [JsonIgnore]
    public string? Override
    {
      get => TlkTable.GetTlkOverride(this);
      set => TlkTable.SetTlkOverride(this, value);
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
    /// Clears the current text override for this StrRef.
    /// </summary>
    public void ClearOverride()
    {
      TlkTable.SetTlkOverride(this, null);
    }

    /// <summary>
    /// Clears the string override for the specified player, optionally restoring the global override.
    /// </summary>
    /// <param name="player">The player to clear the override from.</param>
    /// <param name="restoreGlobal">If true, restores <see cref="Override"/> as the string value.</param>
    public void ClearPlayerOverride(NwPlayer player, bool restoreGlobal = true)
    {
      string? strOverride = null;
      if (restoreGlobal)
      {
        strOverride = Override;
      }

      SetPlayerOverride(player, strOverride);
    }

    /// <summary>
    /// Overrides the string for the specified player only.<br/>
    /// Overrides will not persist through re-logging.
    /// </summary>
    /// <param name="player">The player who should see the different string.</param>
    /// <param name="value">The override string to show.</param>
    public void SetPlayerOverride(NwPlayer player, string? value)
    {
      CNWSMessage message = LowLevel.ServerExoApp.GetNWSMessage();
      message?.SendServerToPlayerSetTlkOverride(player.Player.m_nPlayerID, (int)Id, value.ToExoString());
    }

    /// <summary>
    /// Gets the formatted string associated with this string reference/token number.<br/>
    /// This will parse any tokens (e.g. &lt;CUSTOM0&gt;) as their current set token values.
    /// </summary>
    /// <returns></returns>
    public string? ToParsedString()
    {
      return TlkTable.ResolveParsedStringFromStrRef(this);
    }

    /// <summary>
    /// Gets the raw string associated with this string reference/token number.
    /// </summary>
    /// <returns>The associated string.</returns>
    public override string ToString()
    {
      return TlkTable.ResolveStringFromStrRef(this)!;
    }
  }
}
