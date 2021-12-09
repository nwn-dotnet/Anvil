using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// Represents a NUI scriptable window container.
  /// </summary>
  public sealed class NuiWindow
  {
    [JsonConstructor]
    public NuiWindow(NuiLayout root, NuiProperty<string> title)
    {
      Title = title;
      Root = root;
    }

    /// <summary>
    /// Gets or sets whether the window border should be rendered.
    /// </summary>
    [JsonProperty("border")]
    public NuiProperty<bool> Border { get; set; } = true;

    /// <summary>
    /// Gets or sets whether this window can be closed.<br/>
    /// You must provide a way to close the window if you set this to false.
    /// </summary>
    [JsonProperty("closable")]
    public NuiProperty<bool> Closable { get; set; } = true;

    /// <summary>
    /// Gets or sets whether this window is collapsed.<br/>
    /// Use a static value to force the popup into a collapsed/unfolded state.
    /// </summary>
    [JsonProperty("collapsed")]
    public NuiProperty<bool> Collapsed { get; set; }

    /// <summary>
    /// Gets or sets the geometry and bounds of this window.<br/>
    /// Set x and y to -1.0 to center the window.
    /// </summary>
    [JsonProperty("geometry")]
    public NuiProperty<NuiRect> Geometry { get; set; } = new NuiRect(-1, -1, 0, 0);

    /// <summary>
    /// Gets or sets the element ID for this window.
    /// </summary>
    [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets whether this window can be resized.
    /// </summary>
    [JsonProperty("resizable")]
    public NuiProperty<bool> Resizable { get; set; } = true;

    /// <summary>
    /// Gets or sets the root parent layout containing the window content.
    /// </summary>
    [JsonProperty("root")]
    public NuiLayout Root { get; set; }

    /// <summary>
    /// Gets or sets the title of this window.
    /// </summary>
    [JsonProperty("title")]
    public NuiProperty<string> Title { get; set; }

    /// <summary>
    /// Gets or sets whether the background should be rendered.
    /// </summary>
    [JsonProperty("transparent")]
    public NuiProperty<bool> Transparent { get; set; } = false;

    /// <summary>
    /// Gets the current serialized version of this window.
    /// </summary>
    [JsonProperty("version")]
    public int Version { get; private set; } = 1;
  }
}
