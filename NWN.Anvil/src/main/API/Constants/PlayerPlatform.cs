using NWN.Core;

namespace Anvil.API
{
  public enum PlayerPlatform
  {
    Invalid = NWScript.PLAYER_DEVICE_PLATFORM_INVALID,
    WindowsX86 = NWScript.PLAYER_DEVICE_PLATFORM_WINDOWS_X86,
    WindowsX64 = NWScript.PLAYER_DEVICE_PLATFORM_WINDOWS_X64,
    LinuxX86 = NWScript.PLAYER_DEVICE_PLATFORM_LINUX_X86,
    LinuxX64 = NWScript.PLAYER_DEVICE_PLATFORM_LINUX_X64,
    LinuxArm32 = NWScript.PLAYER_DEVICE_PLATFORM_LINUX_ARM32,
    LinuxArm64 = NWScript.PLAYER_DEVICE_PLATFORM_LINUX_ARM64,
    MacX86 = NWScript.PLAYER_DEVICE_PLATFORM_MAC_X86,
    MacX64 = NWScript.PLAYER_DEVICE_PLATFORM_MAC_X64,
    MacArm64 = NWScript.PLAYER_DEVICE_PLATFORM_MAC_ARM64,
    Ios = NWScript.PLAYER_DEVICE_PLATFORM_IOS,
    AndroidArm32 = NWScript.PLAYER_DEVICE_PLATFORM_ANDROID_ARM32,
    AndroidArm64 = NWScript.PLAYER_DEVICE_PLATFORM_ANDROID_ARM64,
    AndroidX64 = NWScript.PLAYER_DEVICE_PLATFORM_ANDROID_X64,
    NintendoSwitch = NWScript.PLAYER_DEVICE_PLATFORM_NINTENDO_SWITCH,
    MicrosoftXboxOne = NWScript.PLAYER_DEVICE_PLATFORM_MICROSOFT_XBOXONE,
    SonyPs4 = NWScript.PLAYER_DEVICE_PLATFORM_SONY_PS4,
  }
}
