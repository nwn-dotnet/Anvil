using System;
using NWN.Core;

namespace Anvil.API
{
  [Flags]
  public enum ObjectUiDiscovery
  {
    Default = NWScript.OBJECT_UI_DISCOVERY_DEFAULT,
    None = NWScript.OBJECT_UI_DISCOVERY_NONE,
    HiliteMouseover = NWScript.OBJECT_UI_DISCOVERY_HILITE_MOUSEOVER,
    HiliteTab = NWScript.OBJECT_UI_DISCOVERY_HILITE_TAB,
    TextbubbleMouseover = NWScript.OBJECT_UI_DISCOVERY_TEXTBUBBLE_MOUSEOVER,
    TextbubbleTab = NWScript.OBJECT_UI_DISCOVERY_TEXTBUBBLE_TAB,
  }
}
