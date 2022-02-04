namespace Anvil.API
{
  /// <summary>
  /// An environment preset defined in environment.2da
  /// </summary>
  public sealed class EnvironmentPreset : ITwoDimArrayEntry
  {
    public DayNightMode DayNightMode { get; set; }

    public float FogClipDistance { get; set; } = 45f;

    public string Label { get; set; }

    public int? LightningChance { get; set; }

    public int? Main1Color1 { get; set; }

    public int? Main1Color2 { get; set; }

    public int? Main1Color3 { get; set; }

    public int? Main1Color4 { get; set; }

    public int? Main2Color1 { get; set; }

    public int? Main2Color2 { get; set; }

    public int? Main2Color3 { get; set; }

    public int? Main2Color4 { get; set; }

    public Color MoonAmbientColor { get; set; }

    public Color MoonDiffuseColor { get; set; }

    public int? MoonFogAmount { get; set; }

    public Color MoonFogColor { get; set; }

    public bool? MoonShadows { get; set; }

    public int? RainChance { get; set; }

    public int RowIndex { get; init; }

    public int? SecondaryColor1 { get; set; }

    public int? SecondaryColor2 { get; set; }

    public int? SecondaryColor3 { get; set; }

    public int? SecondaryColor4 { get; set; }

    public float? ShadowAlpha { get; set; }

    public int? SnowChance { get; set; }

    public StrRef? StrRef { get; set; }

    public Color SunAmbientColor { get; set; }

    public Color SunDiffuseColor { get; set; }

    public int? SunFogAmount { get; set; }

    public Color SunFogColor { get; set; }

    public bool? SunShadows { get; set; }

    public int? WindPower { get; set; }

    void ITwoDimArrayEntry.InterpretEntry(TwoDimArrayEntry entry)
    {
      Label = entry.GetString("LABEL");
      StrRef = entry.GetStrRef("STRREF");
      DayNightMode = entry.GetString("DAYNIGHT") switch
      {
        "cycle" => DayNightMode.EnableDayNightCycle,
        "day" => DayNightMode.AlwaysDay,
        "night" => DayNightMode.AlwaysNight,
        _ => DayNightMode.EnableDayNightCycle,
      };

      SunAmbientColor = new Color((byte)entry.GetInt("LIGHT_AMB_RED").GetValueOrDefault(0), (byte)entry.GetInt("LIGHT_AMB_GREEN").GetValueOrDefault(0), (byte)entry.GetInt("LIGHT_AMB_BLUE").GetValueOrDefault(0));
      SunDiffuseColor = new Color((byte)entry.GetInt("LIGHT_DIFF_RED").GetValueOrDefault(0), (byte)entry.GetInt("LIGHT_DIFF_GREEN").GetValueOrDefault(0), (byte)entry.GetInt("LIGHT_DIFF_BLUE").GetValueOrDefault(0));
      SunFogColor = new Color((byte)entry.GetInt("LIGHT_FOG_RED").GetValueOrDefault(0), (byte)entry.GetInt("LIGHT_FOG_GREEN").GetValueOrDefault(0), (byte)entry.GetInt("LIGHT_FOG_BLUE").GetValueOrDefault(0));
      SunFogAmount = entry.GetInt("LIGHT_FOG");
      SunShadows = entry.GetBool("LIGHT_SHADOWS");
      MoonAmbientColor = new Color((byte)entry.GetInt("DARK_AMB_RED").GetValueOrDefault(0), (byte)entry.GetInt("DARK_AMB_GREEN").GetValueOrDefault(0), (byte)entry.GetInt("DARK_AMB_BLUE").GetValueOrDefault(0));
      MoonDiffuseColor = new Color((byte)entry.GetInt("DARK_DIFF_RED").GetValueOrDefault(0), (byte)entry.GetInt("DARK_DIFF_GREEN").GetValueOrDefault(0), (byte)entry.GetInt("DARK_DIFF_BLUE").GetValueOrDefault(0));
      MoonFogColor = new Color((byte)entry.GetInt("DARK_FOG_RED").GetValueOrDefault(0), (byte)entry.GetInt("DARK_FOG_GREEN").GetValueOrDefault(0), (byte)entry.GetInt("DARK_FOG_BLUE").GetValueOrDefault(0));
      MoonFogAmount = entry.GetInt("DARK_FOG");
      MoonShadows = entry.GetBool("DARK_SHADOWS");
      Main1Color1 = entry.GetInt("MAIN1_COLOR1");
      Main1Color2 = entry.GetInt("MAIN1_COLOR2");
      Main1Color3 = entry.GetInt("MAIN1_COLOR3");
      Main1Color4 = entry.GetInt("MAIN1_COLOR4");
      Main2Color1 = entry.GetInt("MAIN2_COLOR1");
      Main2Color2 = entry.GetInt("MAIN2_COLOR2");
      Main2Color3 = entry.GetInt("MAIN2_COLOR3");
      Main2Color4 = entry.GetInt("MAIN2_COLOR4");
      SecondaryColor1 = entry.GetInt("SECONDARY_COLOR1");
      SecondaryColor2 = entry.GetInt("SECONDARY_COLOR2");
      SecondaryColor3 = entry.GetInt("SECONDARY_COLOR3");
      SecondaryColor4 = entry.GetInt("SECONDARY_COLOR4");
      WindPower = entry.GetInt("WIND");
      SnowChance = entry.GetInt("SNOW");
      RainChance = entry.GetInt("RAIN");
      LightningChance = entry.GetInt("LIGHTNING");
      ShadowAlpha = entry.GetFloat("SHADOW_ALPHA");
    }
  }
}
