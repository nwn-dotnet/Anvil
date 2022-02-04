using NWN.Native.API;

namespace Anvil.API
{
  public static class NwGameTables
  {
    /// <summary>
    /// Gets the game appearance table (appearance.2da)
    /// </summary>
    public static TwoDimArray<AppearanceTableEntry> AppearanceTable => new TwoDimArray<AppearanceTableEntry>(Arrays.m_pAppearanceTable);

    /// <summary>
    /// Gets the game environment preset table (environment.2da)
    /// </summary>
    public static TwoDimArray<EnvironmentPreset> EnvironmentPresetTable => new TwoDimArray<EnvironmentPreset>("environment.2da");

    private static CTwoDimArrays Arrays => NWNXLib.Rules().m_p2DArrays;
  }
}
