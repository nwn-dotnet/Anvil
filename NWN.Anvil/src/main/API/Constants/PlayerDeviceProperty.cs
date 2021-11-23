using NWN.Core;

namespace Anvil.API
{
  public sealed class PlayerDeviceProperty
  {
    public static readonly PlayerDeviceProperty GuiWidth = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GUI_WIDTH);
    public static readonly PlayerDeviceProperty GuiHeight = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GUI_HEIGHT);
    public static readonly PlayerDeviceProperty GuiScale = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GUI_SCALE);

    internal string PropertyName { get; }

    internal PlayerDeviceProperty(string propertyName)
    {
      PropertyName = propertyName;
    }
  }
}
