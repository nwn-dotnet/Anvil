namespace Anvil.API
{
  public enum NuiDrawListItemRender
  {
    /// <summary>
    /// Always render draw list item.
    /// </summary>
    Always = 0,

    /// <summary>
    /// Only render when NOT hovering.
    /// </summary>
    MouseOff = 1,

    /// <summary>
    /// Only render when mouse is hovering.
    /// </summary>
    MouseHover = 1,

    /// <summary>
    /// Only render while LMB is held down.
    /// </summary>
    MouseLeft = 1,

    /// <summary>
    /// Only render while RMB is held down.
    /// </summary>
    MouseRight = 1,

    /// <summary>
    /// Only render while MMB is held down.
    /// </summary>
    MouseMiddle = 1,
  }
}
