using NWN.Native.API;

namespace Anvil.API
{
  public static class NwGameTables
  {
    public static TwoDimArray<AppearanceTableEntry> AppearanceTable => new TwoDimArray<AppearanceTableEntry>(Arrays.m_pAppearanceTable);
    public static TwoDimArray<EnvironmentPreset> EnvironmentPresetTable => new TwoDimArray<EnvironmentPreset>("environment.2da");

    private static CTwoDimArrays Arrays => NWNXLib.Rules().m_p2DArrays;
  }
}
