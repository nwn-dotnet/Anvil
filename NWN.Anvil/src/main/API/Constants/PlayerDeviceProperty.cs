using NWN.Core;

namespace Anvil.API
{
  public sealed class PlayerDeviceProperty
  {
    public static readonly PlayerDeviceProperty GuiHeight = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GUI_HEIGHT);
    public static readonly PlayerDeviceProperty GuiScale = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GUI_SCALE);
    public static readonly PlayerDeviceProperty GuiWidth = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GUI_WIDTH);

    internal PlayerDeviceProperty(string propertyName)
    {
      PropertyName = propertyName;
    }

    internal string PropertyName { get; }
  }
}
