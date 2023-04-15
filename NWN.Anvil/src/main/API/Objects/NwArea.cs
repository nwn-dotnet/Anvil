using System;
using System.Collections.Generic;
using System.Numerics;
using Anvil.Services;
using NWN.Core;
using NWN.Native.API;
using Vector = NWN.Native.API.Vector;

namespace Anvil.API
{
  /// <summary>
  /// An environment/game level.
  /// </summary>
  [ObjectType(0)]
  public sealed partial class NwArea : NwObject
  {
    private readonly CNWSArea area;

    internal CNWSArea Area
    {
      get
      {
        AssertObjectValid();
        return area;
      }
    }

    internal NwArea(CNWSArea area) : base(area)
    {
      this.area = area;
    }

    /// <summary>
    /// Sets the daytime ambient track for this area.<br/>
    /// See "ambientsound.2da" for track numbers.
    /// </summary>
    public int AmbientDayTrack
    {
      set => NWScript.AmbientSoundChangeDay(this, value);
    }

    /// <summary>
    /// Sets the daytime ambient track volume for this area.
    /// </summary>
    public int AmbientDayVolume
    {
      set => NWScript.AmbientSoundSetDayVolume(this, value);
    }

    /// <summary>
    /// Sets the night ambient track for this area.<br/>
    /// See "ambientsound.2da" for track numbers.
    /// </summary>
    public int AmbientNightTrack
    {
      set => NWScript.AmbientSoundChangeNight(this, value);
    }

    /// <summary>
    /// Sets the night ambient track volume for this area.
    /// </summary>
    public int AmbientNightVolume
    {
      set => NWScript.AmbientSoundSetNightVolume(this, value);
    }

    /// <summary>
    /// Gets or sets area terrain metadata flags.
    /// </summary>
    public AreaFlags AreaFlags
    {
      get => (AreaFlags)Area.m_nFlags;
      set => Area.m_nFlags = (uint)value;
    }

    /// <summary>
    /// Gets or sets the day/night mode to use for this area.
    /// </summary>
    public DayNightMode DayNightMode
    {
      get => Area.m_bUseDayNightCycle.ToBool() ? DayNightMode.EnableDayNightCycle : Area.m_bIsNight.ToBool() ? DayNightMode.AlwaysNight : DayNightMode.AlwaysDay;
      set
      {
        switch (value)
        {
          case DayNightMode.EnableDayNightCycle:
            Area.m_bUseDayNightCycle = true.ToInt();
            break;
          case DayNightMode.AlwaysNight:
            Area.SetIsNight(true.ToInt());
            Area.m_bUseDayNightCycle = false.ToInt();
            break;
          case DayNightMode.AlwaysDay:
            Area.m_bUseDayNightCycle = false.ToInt();
            Area.SetIsNight(false.ToInt());
            break;
        }
      }
    }

    /// <summary>
    /// Gets or sets the fog clip distance in the area.
    /// </summary>
    public float FogClipDistance
    {
      get => Area.m_fFogClipDistance;
      set => Area.m_fFogClipDistance = value;
    }

    /// <summary>
    /// Gets or sets whether this area is considered above ground and not under ground.
    /// </summary>
    public bool IsAboveGround
    {
      get => !IsUnderGround;
      set => IsUnderGround = !value;
    }

    /// <summary>
    /// Gets a value indicating whether battle music is currently playing in the area.
    /// </summary>
    public bool IsBattleMusicPlaying => Area.m_pAmbientSound.m_bBattlePlaying.ToBool();

    /// <summary>
    /// Gets or sets whether this area is considered an as exterior, and not an interior.
    /// </summary>
    public bool IsExterior
    {
      get => !IsInterior;
      set => IsInterior = !value;
    }

    /// <summary>
    /// Gets or sets whether this area is considered an as interior, and not an exterior.
    /// </summary>
    /// <remarks>Unlike the equivalent NwScript function <see cref="NWScript.GetIsAreaInterior"/>, this function will only return true if the <see cref="API.AreaFlags.Interior"/> flag is set.<br/>
    /// An area that is simply underground is not considered an interior.<br/><br/>
    /// Use IsInterior || IsUnderGround together to replicate the existing function.
    /// </remarks>
    public bool IsInterior
    {
      get => IsAreaFlagSet(AreaFlags.Interior);
      set => SetAreaFlag(AreaFlags.Interior, value);
    }

    /// <summary>
    /// Gets a value indicating whether ambient music is currently playing in the area.
    /// </summary>
    public bool IsMusicPlaying => Area.m_pAmbientSound.m_bMusicPlaying.ToBool();

    /// <summary>
    /// Gets or sets whether this area is considered natural and non-urban.
    /// </summary>
    public bool IsNatural
    {
      get => IsAreaFlagSet(AreaFlags.Natural);
      set => SetAreaFlag(AreaFlags.Natural, value);
    }

    /// <summary>
    /// Gets or sets whether this area is considered under ground and not above ground.
    /// </summary>
    public bool IsUnderGround
    {
      get => IsAreaFlagSet(AreaFlags.UnderGround);
      set => SetAreaFlag(AreaFlags.UnderGround, value);
    }

    /// <summary>
    /// Gets or sets whether this area is considered urban and not natural.
    /// </summary>
    public bool IsUrban
    {
      get => !IsNatural;
      set => IsNatural = !value;
    }

    /// <summary>
    /// Gets the last object that entered this area.
    /// </summary>
    public NwGameObject? LastEntered => Area.m_oidLastEntered.ToNwObject<NwGameObject>();

    /// <summary>
    /// Gets the last object that left this area.
    /// </summary>
    public NwGameObject? LastLeft => Area.m_oidLastLeft.ToNwObject<NwGameObject>();

    /// <summary>
    /// Gets or sets the percentage value (0-100) that lightning may occur.
    /// </summary>
    public int LightningChance
    {
      get => Area.m_nChanceOfLightning;
      set => Area.m_nChanceOfLightning = (byte)value;
    }

    /// <summary>
    /// Gets or sets the listen modifier for this area.
    /// </summary>
    public int ListenModifier
    {
      get => Area.m_nAreaListenModifier;
      set => Area.m_nAreaListenModifier = value;
    }

    /// <summary>
    /// Gets or sets the load screen for this area.
    /// </summary>
    public LoadScreenTableEntry LoadScreen
    {
      get => NwGameTables.LoadScreenTable[Area.m_nLoadScreenID];
      set => Area.m_nLoadScreenID = (ushort)value.RowIndex;
    }

    /// <summary>
    /// Gets or sets the area ambient color during night time.
    /// </summary>
    public Color MoonAmbientColor
    {
      get => Color.FromRGBA(Area.m_nMoonAmbientColor);
      set => Area.m_nMoonAmbientColor = value.ToUnsignedRGBA();
    }

    /// <summary>
    /// Gets or sets the area diffuse color during night time.
    /// </summary>
    public Color MoonDiffuseColor
    {
      get => Color.FromRGBA(Area.m_nMoonDiffuseColor);
      set => Area.m_nMoonDiffuseColor = value.ToUnsignedRGBA();
    }

    /// <summary>
    /// Gets or sets the fog density during night time.
    /// </summary>
    public int MoonFogAmount
    {
      get => Area.m_nMoonFogAmount;
      set => Area.m_nMoonFogAmount = (byte)value;
    }

    /// <summary>
    /// Gets or sets the area fog color during night time.
    /// </summary>
    public Color MoonFogColor
    {
      get => Color.FromRGBA(Area.m_nMoonFogColor);
      set => Area.m_nMoonFogColor = value.ToUnsignedRGBA();
    }

    /// <summary>
    /// Gets or sets whether shadows are cast during night time.
    /// </summary>
    public bool MoonShadows
    {
      get => Area.m_bMoonShadows.ToBool();
      set => Area.m_bMoonShadows = value.ToInt();
    }

    /// <summary>
    /// Gets or sets the daytime background track index for this area.<br/>
    /// See "Resources > Sounds and Music > Music" in the toolset for track numbers.
    /// </summary>
    public int MusicBackgroundDayTrack
    {
      get => NWScript.MusicBackgroundGetDayTrack(this);
      set => NWScript.MusicBackgroundChangeDay(this, value);
    }

    /// <summary>
    /// Gets or sets the nighttime background track index for this area.<br/>
    /// Refer to Resources > Sounds and Music > Music in the toolset for track numbers.
    /// </summary>
    public int MusicBackgroundNightTrack
    {
      get => NWScript.MusicBackgroundGetNightTrack(this);
      set => NWScript.MusicBackgroundChangeNight(this, value);
    }

    /// <summary>
    /// Gets or sets the combat track index for this area.<br/>
    /// Refer to Resources > Sounds and Music > Music in the toolset for track numbers.
    /// </summary>
    public int MusicBattleTrack
    {
      get => NWScript.MusicBackgroundGetBattleTrack(this);
      set => NWScript.MusicBattleChange(this, value);
    }

    /// <summary>
    /// Gets all Objects currently in this area.
    /// </summary>
    public IEnumerable<NwGameObject> Objects
    {
      get
      {
        for (uint areaObj = NWScript.GetFirstObjectInArea(this); areaObj != Invalid; areaObj = NWScript.GetNextObjectInArea(this))
        {
          yield return areaObj.ToNwObject<NwGameObject>()!;
        }
      }
    }

    /// <summary>
    /// Gets the number of players in this area.
    /// </summary>
    public int PlayerCount => Area.m_nPlayersInArea;

    /// <summary>
    /// Gets or sets the PvP setting for this area.
    /// </summary>
    public PVPSetting PVPSetting
    {
      get => (PVPSetting)Area.m_nPVPSetting;
      set => Area.m_nPVPSetting = (byte)value;
    }

    /// <summary>
    /// Gets or sets the percentage value (0-100) that rain may occur.
    /// </summary>
    public int RainChance
    {
      get => Area.m_nChanceOfRain;
      set => Area.m_nChanceOfRain = (byte)value;
    }

    /// <summary>
    /// Gets or sets whether resting is allowed in this area.
    /// </summary>
    public bool RestingAllowed
    {
      get => !Area.m_bNoRestingAllowed.ToBool();
      set => Area.m_bNoRestingAllowed = (!value).ToInt();
    }

    /// <summary>
    /// Gets or sets the shadow opacity for this area (0-100).
    /// </summary>
    public byte ShadowOpacity
    {
      get => Area.m_nShadowOpacity;
      set => Area.m_nShadowOpacity = value;
    }

    /// <summary>
    /// Gets the size of this area.
    /// <returns>The number of tiles that the area is wide/high.</returns>
    /// </summary>
    public Vector2Int Size => new Vector2Int(NWScript.GetAreaSize((int)AreaSizeDimension.Width, this), NWScript.GetAreaSize((int)AreaSizeDimension.Height, this));

    /// <summary>
    /// Gets or sets the current skybox for this area.
    /// </summary>
    public Skybox SkyBox
    {
      get => (Skybox)NWScript.GetSkyBox(this);
      set => NWScript.SetSkyBox((int)value, this);
    }

    /// <summary>
    /// Gets or sets the percentage value (0-100) that snow may occur.
    /// </summary>
    public int SnowChance
    {
      get => Area.m_nChanceOfSnow;
      set => Area.m_nChanceOfSnow = (byte)value;
    }

    /// <summary>
    /// Gets or sets the spot modifier for this area.
    /// </summary>
    public int SpotModifier
    {
      get => Area.m_nAreaSpotModifier;
      set => Area.m_nAreaSpotModifier = value;
    }

    /// <summary>
    /// Gets or sets the area ambient color during day time.
    /// </summary>
    public Color SunAmbientColor
    {
      get => Color.FromRGBA(Area.m_nSunAmbientColor);
      set => Area.m_nSunAmbientColor = value.ToUnsignedRGBA();
    }

    /// <summary>
    /// Gets or sets the area diffuse color during day time.
    /// </summary>
    public Color SunDiffuseColor
    {
      get => Color.FromRGBA(Area.m_nSunDiffuseColor);
      set => Area.m_nSunDiffuseColor = value.ToUnsignedRGBA();
    }

    /// <summary>
    /// Gets or sets the fog density during day time.
    /// </summary>
    public int SunFogAmount
    {
      get => Area.m_nSunFogAmount;
      set => Area.m_nSunFogAmount = (byte)value;
    }

    /// <summary>
    /// Gets or sets the area fog color during day time.
    /// </summary>
    public Color SunFogColor
    {
      get => Color.FromRGBA(Area.m_nSunFogColor);
      set => Area.m_nSunFogColor = value.ToUnsignedRGBA();
    }

    /// <summary>
    /// Gets or sets whether shadows are cast during day time.
    /// </summary>
    public bool SunShadows
    {
      get => Area.m_bSunShadows.ToBool();
      set => Area.m_bSunShadows = value.ToInt();
    }

    /// <summary>
    /// Gets the tileset (.set) resource name used for this area.
    /// </summary>
    public string Tileset => NWScript.GetTilesetResRef(this);

    /// <summary>
    /// Gets or sets the current weather conditions for this area.
    /// </summary>
    public WeatherType Weather
    {
      get => (WeatherType)NWScript.GetWeather(this);
      set => NWScript.SetWeather(this, (int)value);
    }

    /// <summary>
    /// Gets or sets the wind power for this area.<br/>
    /// Set to 0, 1 or 2.
    /// </summary>
    public byte WindPower
    {
      get => Area.m_nWindAmount;
      set => Area.m_nWindAmount = value;
    }

    internal override CNWSScriptVarTable ScriptVarTable => Area.m_ScriptVars;

    /// <summary>
    /// Creates a new area from the specified resource reference.
    /// </summary>
    /// <param name="resRef">The area resource to create this area from.</param>
    /// <param name="newTag">A new tag for this area. Defaults to the tag set in the toolset.</param>
    /// <param name="newName">A new name for this area. Defaults to the name set in the toolset.</param>
    /// <returns>The created area.</returns>
    public static NwArea? Create(string resRef, string newTag = "", string newName = "")
    {
      return NWScript.CreateArea(resRef, newTag, newName).ToNwObject<NwArea>();
    }

    /// <inheritdoc cref="Deserialize(string,byte[],byte[],string,string)"/>
    public static NwArea? Deserialize(byte[] serializedARE, byte[] serializedGIT, string newTag = "", string newName = "")
    {
      string resourceName = ResourceNameGenerator.Create();
      return Deserialize(resourceName, serializedARE, serializedGIT, newTag, newName);
    }

    /// <summary>
    /// Creates an area from the specified serialized area data.
    /// </summary>
    /// <param name="resRef">The base resref name to use (e.g. area001). Overrides previous areas with the same resref (excl. development folder areas).</param>
    /// <param name="serializedARE">The serialized static area information (.are).</param>
    /// <param name="serializedGIT">The serialized dynamic area information (.git).</param>
    /// <param name="newTag">A new tag for this area. Defaults to the tag set in the toolset.</param>
    /// <param name="newName">A new name for this area. Defaults to the name set in the toolset.</param>
    /// <returns>The created area.</returns>
    public static NwArea? Deserialize(string resRef, byte[] serializedARE, byte[] serializedGIT, string newTag = "", string newName = "")
    {
      ResourceManager.WriteTempResource(resRef + ".git", serializedGIT);
      ResourceManager.WriteTempResource(resRef + ".are", serializedARE);

      return Create(resRef, newTag, newName);
    }

    public static implicit operator CNWSArea?(NwArea? area)
    {
      return area?.Area;
    }

    public void ApplyEnvironmentPreset(EnvironmentPreset preset)
    {
      DayNightMode = preset.DayNightMode;
      SunAmbientColor = preset.SunAmbientColor;
      SunDiffuseColor = preset.SunDiffuseColor;
      SunFogColor = preset.SunFogColor;
      SunFogAmount = preset.SunFogAmount.GetValueOrDefault(0);
      SunShadows = preset.SunShadows.GetValueOrDefault(false);
      MoonAmbientColor = preset.MoonAmbientColor;
      MoonDiffuseColor = preset.MoonDiffuseColor;
      MoonFogColor = preset.MoonFogColor;
      MoonFogAmount = preset.MoonFogAmount.GetValueOrDefault(0);
      MoonShadows = preset.MoonShadows.GetValueOrDefault(false);
      WindPower = (byte)preset.WindPower.GetValueOrDefault(0);
      SnowChance = preset.SnowChance.GetValueOrDefault(0);
      RainChance = preset.RainChance.GetValueOrDefault(0);
      LightningChance = preset.LightningChance.GetValueOrDefault(0);
      FogClipDistance = preset.FogClipDistance;
      ShadowOpacity = (byte)Math.Round(preset.ShadowAlpha.GetValueOrDefault(0.5f) * 10, MidpointRounding.ToZero);
    }

    /// <summary>
    /// Creates a copy of this area, including everything inside of it (except players).
    /// </summary>
    /// <returns>The new cloned area instance.</returns>
    public NwArea? Clone()
    {
      return NWScript.CopyArea(this).ToNwObject<NwArea>();
    }

    public EnvironmentPreset CreateEnvironmentPreset()
    {
      return new EnvironmentPreset
      {
        DayNightMode = DayNightMode,
        SunAmbientColor = SunAmbientColor,
        SunDiffuseColor = SunDiffuseColor,
        SunFogColor = SunFogColor,
        SunFogAmount = SunFogAmount,
        SunShadows = SunShadows,
        MoonAmbientColor = MoonAmbientColor,
        MoonDiffuseColor = MoonDiffuseColor,
        MoonFogColor = MoonFogColor,
        MoonFogAmount = MoonFogAmount,
        MoonShadows = MoonShadows,
        WindPower = WindPower,
        SnowChance = SnowChance,
        RainChance = RainChance,
        LightningChance = LightningChance,
        FogClipDistance = FogClipDistance,
        ShadowAlpha = ShadowOpacity / 10f,
      };
    }

    /// <summary>
    /// Destroys this area and anything within it.
    /// </summary>
    /// <returns>The result of this destroy action.</returns>
    public AreaDestroyResult Destroy()
    {
      return (AreaDestroyResult)NWScript.DestroyArea(this);
    }

    /// <summary>
    /// Locates all objects of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of objects to search.</typeparam>
    /// <returns>An enumeration containing all objects of the specified type.</returns>
    public IEnumerable<T> FindObjectsOfTypeInArea<T>() where T : NwObject
    {
      int typeFilter = (int)GetObjectFilter<T>();

      for (uint currentObj = NWScript.GetFirstObjectInArea(this, typeFilter); currentObj != Invalid; currentObj = NWScript.GetNextObjectInArea(this, typeFilter))
      {
        T? obj = currentObj.ToNwObjectSafe<T>();
        if (obj != null)
        {
          yield return obj;
        }
      }
    }

    /// <summary>
    /// Gets the fog amount for this area, at the specified time of day.
    /// </summary>
    [Obsolete("Use SunFogAmount or MoonFogAmount instead.")]
    public int GetFogAmount(FogType fogType)
    {
      return NWScript.GetFogAmount((int)fogType, this);
    }

    /// <summary>
    /// Gets the fog color for this area, at the specified time of day.
    /// </summary>
    [Obsolete("Use SunFogColor or MoonFogColor instead.")]
    public FogColor GetFogColor(FogType fogType)
    {
      return (FogColor)NWScript.GetFogColor((int)fogType, this);
    }

    /// <summary>
    /// Gets the tile info at the specified position in the area.
    /// </summary>
    /// <param name="tileX">The x coordinate of the tile to get info.</param>
    /// <param name="tileY">The y coordinate of the tile to get info.</param>
    /// <returns>A structure containing the associated tile info.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the tile coordinates are larger than the area size.</exception>
    public TileInfo? GetTileInfo(uint tileX, uint tileY)
    {
      Vector2Int max = Size;
      if (tileX >= max.X || tileY >= max.Y)
      {
        throw new ArgumentOutOfRangeException(null, "Tile index must be smaller than the area size.");
      }

      CNWTile tile = Area.GetTile(new Vector(tileX, tileY, 0f));

      return tile != null ? new TileInfo(tile) : null;
    }

    public override Guid? PeekUUID()
    {
      CNWSUUID uid = Area.m_pUUID;
      if (!uid.CanCarryUUID())
      {
        return null;
      }

      CExoString uidString = uid.m_uuid;
      return uidString != null ? Guid.Parse(uidString.ToString()) : null;
    }

    /// <summary>
    /// Begins playback of ambient sounds in this area.
    /// </summary>
    public void PlayAmbient()
    {
      NWScript.AmbientSoundPlay(this);
    }

    /// <summary>
    /// Begins playback of background music in this area.
    /// </summary>
    public void PlayBackgroundMusic()
    {
      NWScript.MusicBackgroundPlay(this);
    }

    /// <summary>
    /// Begins playback of battle music for this area.
    /// </summary>
    public void PlayBattleMusic()
    {
      NWScript.MusicBattlePlay(this);
    }

    /// <summary>
    /// Notifies all clients in this area to recompute static lighting.<br/>
    /// This can be used to update the lighting after changing any tile lights or if placeables with lights have been added/deleted.
    /// </summary>
    public void RecomputeStaticLighting()
    {
      NWScript.RecomputeStaticLighting(this);
    }

    /// <summary>
    /// Notifies all clients in this area to recalculate grass.<br/>
    /// This can be used to update the grass of an area after changing a tile with SetTile() that will have or used to have grass.
    /// </summary>
    public void ReloadAreaGrass()
    {
      NWScript.ReloadAreaGrass(this);
    }

    /// <summary>
    /// Notifies all clients in this area to reload the inaccesible border tiles.<br/>
    /// This can be used to update the edge tiles after changing a tile with SetTile().
    /// </summary>
    public void ReloadAreaBorder()
    {
      NWScript.ReloadAreaBorder(this);
    }

    public unsafe byte[]? SerializeARE(string? areaName = null, string? resRef = null)
    {
      areaName ??= Name;
      resRef ??= ResRef;

      if (string.IsNullOrEmpty(resRef))
      {
        throw new ArgumentOutOfRangeException(nameof(resRef), "The new ResRef must not be empty.");
      }

      if (resRef.Length > 16)
      {
        throw new ArgumentOutOfRangeException(nameof(resRef), "The new ResRef must smaller than 17 characters.");
      }

      return NativeUtils.SerializeGff("ARE", "V3.2", (resGff, resStruct) =>
      {
        // Important Stuff
        resGff.WriteFieldCExoLocString(resStruct, areaName.ToExoLocString(), "Name".GetNullTerminatedString());
        resGff.WriteFieldCExoString(resStruct, new CExoString(Tag), "Tag".GetNullTerminatedString());
        resGff.WriteFieldCExoString(resStruct, new CExoString(ResRef), "ResRef".GetNullTerminatedString());
        resGff.WriteFieldINT(resStruct, Size.X, "Width".GetNullTerminatedString());
        resGff.WriteFieldINT(resStruct, Size.Y, "Height".GetNullTerminatedString());
        resGff.WriteFieldCResRef(resStruct, Area.m_refTileSet, "Tileset".GetNullTerminatedString());

        // Less Important Stuff
        resGff.WriteFieldINT(resStruct, Area.m_nChanceOfLightning, "ChanceLightning".GetNullTerminatedString());
        resGff.WriteFieldINT(resStruct, Area.m_nChanceOfRain, "ChanceRain".GetNullTerminatedString());
        resGff.WriteFieldINT(resStruct, Area.m_nChanceOfSnow, "ChanceSnow".GetNullTerminatedString());
        resGff.WriteFieldBYTE(resStruct, (byte)Area.m_bUseDayNightCycle, "DayNightCycle".GetNullTerminatedString());
        resGff.WriteFieldDWORD(resStruct, Area.m_nFlags, "Flags".GetNullTerminatedString());
        resGff.WriteFieldFLOAT(resStruct, Area.m_fFogClipDistance, "FogClipDist".GetNullTerminatedString());
        resGff.WriteFieldBYTE(resStruct, (byte)Area.m_bIsNight, "IsNight".GetNullTerminatedString());
        resGff.WriteFieldBYTE(resStruct, Area.m_nLightingScheme, "LightingScheme".GetNullTerminatedString());
        resGff.WriteFieldWORD(resStruct, Area.m_nLoadScreenID, "LoadScreenID".GetNullTerminatedString());
        resGff.WriteFieldINT(resStruct, Area.m_nAreaListenModifier, "ModListenCheck".GetNullTerminatedString());
        resGff.WriteFieldINT(resStruct, Area.m_nAreaSpotModifier, "ModSpotCheck".GetNullTerminatedString());
        resGff.WriteFieldDWORD(resStruct, Area.m_nMoonAmbientColor, "MoonAmbientColor".GetNullTerminatedString());
        resGff.WriteFieldDWORD(resStruct, Area.m_nMoonDiffuseColor, "MoonDiffuseColor".GetNullTerminatedString());
        resGff.WriteFieldBYTE(resStruct, Area.m_nMoonFogAmount, "MoonFogAmount".GetNullTerminatedString());
        resGff.WriteFieldDWORD(resStruct, Area.m_nMoonFogColor, "MoonFogColor".GetNullTerminatedString());
        resGff.WriteFieldBYTE(resStruct, (byte)Area.m_bMoonShadows, "MoonShadows".GetNullTerminatedString());
        resGff.WriteFieldBYTE(resStruct, (byte)Area.m_bNoRestingAllowed, "NoRest".GetNullTerminatedString());
        resGff.WriteFieldCResRef(resStruct, new CResRef(Area.m_sScripts[0]), "OnHeartbeat".GetNullTerminatedString());
        resGff.WriteFieldCResRef(resStruct, new CResRef(Area.m_sScripts[1]), "OnUserDefined".GetNullTerminatedString());
        resGff.WriteFieldCResRef(resStruct, new CResRef(Area.m_sScripts[2]), "OnEnter".GetNullTerminatedString());
        resGff.WriteFieldCResRef(resStruct, new CResRef(Area.m_sScripts[3]), "OnExit".GetNullTerminatedString());
        resGff.WriteFieldBYTE(resStruct, Area.m_nPVPSetting, "PlayerVsPlayer".GetNullTerminatedString());
        resGff.WriteFieldBYTE(resStruct, Area.m_nShadowOpacity, "ShadowOpacity".GetNullTerminatedString());
        resGff.WriteFieldBYTE(resStruct, Area.m_nSkyBox, "SkyBox".GetNullTerminatedString());
        resGff.WriteFieldDWORD(resStruct, Area.m_nSunAmbientColor, "SunAmbientColor".GetNullTerminatedString());
        resGff.WriteFieldDWORD(resStruct, Area.m_nSunDiffuseColor, "SunDiffuseColor".GetNullTerminatedString());
        resGff.WriteFieldBYTE(resStruct, Area.m_nSunFogAmount, "SunFogAmount".GetNullTerminatedString());
        resGff.WriteFieldDWORD(resStruct, Area.m_nSunFogColor, "SunFogColor".GetNullTerminatedString());
        resGff.WriteFieldBYTE(resStruct, (byte)Area.m_bSunShadows, "SunShadows".GetNullTerminatedString());
        resGff.WriteFieldINT(resStruct, Area.m_nWindAmount, "WindPower".GetNullTerminatedString());

        // Tile Stuff
        using CResList resList = new CResList();
        resGff.AddList(resList, resStruct, "Tile_List".GetNullTerminatedString());
        int tileCount = Area.m_nWidth * Area.m_nHeight;
        CNWSTileArray tiles = CNWSTileArray.FromPointer(Area.m_pTile);

        for (int i = 0; i < tileCount; i++)
        {
          CNWTile tile = tiles[i];
          resGff.AddListElement(resStruct, resList, 1);
          resGff.WriteFieldINT(resStruct, tile.m_nID, "Tile_ID".GetNullTerminatedString());
          resGff.WriteFieldINT(resStruct, tile.m_nOrientation, "Tile_Orientation".GetNullTerminatedString());
          resGff.WriteFieldINT(resStruct, tile.m_nHeight, "Tile_Height".GetNullTerminatedString());

          resGff.WriteFieldBYTE(resStruct, tile.m_nMainLight1Color == byte.MaxValue ? byte.MinValue : tile.m_nMainLight1Color, "Tile_MainLight1".GetNullTerminatedString());
          resGff.WriteFieldBYTE(resStruct, tile.m_nMainLight2Color == byte.MaxValue ? byte.MinValue : tile.m_nMainLight2Color, "Tile_MainLight2".GetNullTerminatedString());

          resGff.WriteFieldBYTE(resStruct, tile.m_nSourceLight1Color == byte.MaxValue ? byte.MinValue : tile.m_nSourceLight1Color, "Tile_SrcLight1".GetNullTerminatedString());
          resGff.WriteFieldBYTE(resStruct, tile.m_nSourceLight2Color == byte.MaxValue ? byte.MinValue : tile.m_nSourceLight2Color, "Tile_SrcLight2".GetNullTerminatedString());

          resGff.WriteFieldBYTE(resStruct, tile.m_nAnimLoop1, "Tile_AnimLoop1".GetNullTerminatedString());
          resGff.WriteFieldBYTE(resStruct, tile.m_nAnimLoop2, "Tile_AnimLoop2".GetNullTerminatedString());
          resGff.WriteFieldBYTE(resStruct, tile.m_nAnimLoop3, "Tile_AnimLoop3".GetNullTerminatedString());
        }

        return true;
      });
    }

    public byte[]? SerializeGIT(ObjectTypes objectFilter = ObjectTypes.All, ICollection<NwGameObject>? exclusionList = null, bool exportVarTable = true, bool exportUUID = true, string? resRef = null)
    {
      resRef ??= ResRef;

      if (string.IsNullOrEmpty(resRef))
      {
        throw new ArgumentOutOfRangeException(nameof(resRef), "The new ResRef must not be empty.");
      }

      if (resRef.Length > 16)
      {
        throw new ArgumentOutOfRangeException(nameof(resRef), "The new ResRef must smaller than 17 characters.");
      }

      List<NwCreature> addedCreatures = new List<NwCreature>();

      try
      {
        return NativeUtils.SerializeGff("GIT", "V3.2", (resGff, resStruct) =>
        {
          CExoArrayListUInt32 creatures = new CExoArrayListUInt32();
          CExoArrayListUInt32 items = new CExoArrayListUInt32();
          CExoArrayListUInt32 doors = new CExoArrayListUInt32();
          CExoArrayListUInt32 triggers = new CExoArrayListUInt32();
          CExoArrayListUInt32 encounters = new CExoArrayListUInt32();
          CExoArrayListUInt32 waypoints = new CExoArrayListUInt32();
          CExoArrayListUInt32 sounds = new CExoArrayListUInt32();
          CExoArrayListUInt32 placeables = new CExoArrayListUInt32();
          CExoArrayListUInt32 stores = new CExoArrayListUInt32();
          CExoArrayListUInt32 aoes = new CExoArrayListUInt32();

          foreach (NwGameObject gameObject in Objects)
          {
            if (exclusionList != null && exclusionList.Contains(gameObject))
            {
              continue;
            }

            if (gameObject is NwCreature creature)
            {
              if (creature.ControllingPlayer != null || creature.AssociateType != AssociateType.None)
              {
                continue;
              }

              // Temporarily set pCreature's areaID to OBJECT_INVALID
              // When loading the creatures from the GIT, if the creature's areaID is the same as pArea's it
              // won't call AddToArea() which leaves all the creatures in limbo.
              creature.Creature.m_oidArea = Invalid;
              creatures.Add(creature);
              addedCreatures.Add(creature);
            }
            else if (gameObject is NwItem item)
            {
              items.Add(item);
            }
            else if (gameObject is NwDoor door)
            {
              doors.Add(door);
            }
            else if (gameObject is NwTrigger trigger)
            {
              triggers.Add(trigger);
            }
            else if (gameObject is NwEncounter encounter)
            {
              encounters.Add(encounter);
            }
            else if (gameObject is NwWaypoint waypoint)
            {
              waypoints.Add(waypoint);
            }
            else if (gameObject is NwSound sound)
            {
              sounds.Add(sound);
            }
            else if (gameObject is NwPlaceable placeable)
            {
              placeables.Add(placeable);
            }
            else if (gameObject is NwStore store)
            {
              stores.Add(store);
            }
            else if (gameObject is NwAreaOfEffect areaOfEffect)
            {
              aoes.Add(areaOfEffect);
            }
          }

          if (objectFilter.HasFlag(ObjectTypes.Creature))
          {
            Area.SaveCreatures(resGff, resStruct, creatures, false.ToInt());
          }

          if (objectFilter.HasFlag(ObjectTypes.Item))
          {
            Area.SaveItems(resGff, resStruct, items, false.ToInt());
          }

          if (objectFilter.HasFlag(ObjectTypes.Trigger))
          {
            Area.SaveTriggers(resGff, resStruct, triggers, false.ToInt());
          }

          if (objectFilter.HasFlag(ObjectTypes.Door))
          {
            Area.SaveDoors(resGff, resStruct, doors, false.ToInt());
          }

          if (objectFilter.HasFlag(ObjectTypes.AreaOfEffect))
          {
            Area.SaveAreaEffects(resGff, resStruct, aoes, false.ToInt());
          }

          if (objectFilter.HasFlag(ObjectTypes.Waypoint))
          {
            Area.SaveWaypoints(resGff, resStruct, waypoints, false.ToInt());
          }

          if (objectFilter.HasFlag(ObjectTypes.Placeable))
          {
            Area.SavePlaceables(resGff, resStruct, placeables, false.ToInt());
          }

          if (objectFilter.HasFlag(ObjectTypes.Store))
          {
            Area.SaveStores(resGff, resStruct, stores, false.ToInt());
          }

          if (objectFilter.HasFlag(ObjectTypes.Encounter))
          {
            Area.SaveEncounters(resGff, resStruct, encounters, false.ToInt());
          }

          Area.SaveStores(resGff, resStruct, sounds, false.ToInt());
          Area.SaveProperties(resGff, resStruct);

          if (exportVarTable)
          {
            Area.m_ScriptVars.SaveVarTable(resGff, resStruct);
          }

          if (exportUUID)
          {
            Area.m_pUUID.SaveToGff(resGff, resStruct);
          }

          return true;
        });
      }
      finally
      {
        // Restore the areaIDs of all creatures
        foreach (NwCreature creature in addedCreatures)
        {
          creature.Creature.m_oidArea = this;
        }
      }
    }

    /// <summary>
    /// Gets a light color in this area.
    /// </summary>
    /// <param name="colorType">The color type to get.</param>
    public int GetAreaLightColor(AreaLightColor colorType)
    {
      return NWScript.GetAreaLightColor((int)colorType, this);
    }

    /// <summary>
    /// Sets a light color in this area.
    /// </summary>
    /// <param name="colorType">The type of color to set.</param>
    /// <param name="color">The new color to set.</param>
    /// <param name="fadeTime">The time to fade the new color.</param>
    public void SetAreaLightColor(AreaLightColor colorType, int color, TimeSpan fadeTime = default)
    {
      NWScript.SetAreaLightColor((int)colorType, color, this, (float)fadeTime.TotalSeconds);
    }

    /// <summary>
    /// Gets the light direction for the specified light type.
    /// </summary>
    /// <param name="lightType">The type of light to get the direction from.</param>
    /// <returns>A <see cref="Vector3"/> representing the light direction.</returns>
    public Vector3 GetAreaLightDirection(AreaLightDirection lightType)
    {
      return NWScript.GetAreaLightDirection((int)lightType, this);
    }

    /// <summary>
    /// Sets the light direction for the specified light type.
    /// </summary>
    /// <param name="lightType">The type of light to get the direction from.</param>
    /// <param name="direction">The new direction for the light.</param>
    /// <param name="fadeTime">The time to fade in the new light direction.</param>
    /// <returns>A <see cref="Vector3"/> representing the light direction.</returns>
    public void SetAreaLightDirection(AreaLightDirection lightType, Vector3 direction, TimeSpan fadeTime = default)
    {
      NWScript.SetAreaLightDirection((int)lightType, direction, this, (float)fadeTime.TotalSeconds);
    }

    /// <summary>
    /// Sets the detailed wind data for this area.
    /// </summary>
    /// <remarks>
    /// The predefined values in the toolset are:<br/>
    /// NONE:  direction=(1.0, 1.0, 0.0), magnitude=0.0, yaw=0.0,   pitch=0.0<br/>
    /// LIGHT: direction=(1.0, 1.0, 0.0), magnitude=1.0, yaw=100.0, pitch=3.0<br/>
    /// HEAVY: direction=(1.0, 1.0, 0.0), magnitude=2.0, yaw=150.0, pitch=5.0
    /// </remarks>
    /// <param name="direction">The direction of the wind.</param>
    /// <param name="magnitude">The magnitude/intensity of the wind.</param>
    /// <param name="yaw">The yaw value of the wind.</param>
    /// <param name="pitch">The pitch value of the wind.</param>
    public void SetAreaWind(Vector3 direction, float magnitude, float yaw, float pitch)
    {
      NWScript.SetAreaWind(this, direction, magnitude, yaw, pitch);
    }

    /// <summary>
    /// Sets the fog amount for this area, at the specified time of day.
    /// </summary>
    public void SetFogAmount(FogType fogType, int fogAmount)
    {
      NWScript.SetFogAmount((int)fogType, fogAmount, this);
    }

    /// <summary>
    /// Sets the fog color for this area, at the specified time of day.
    /// </summary>
    public void SetFogColor(FogType fogType, FogColor fogColor, TimeSpan fadeTime)
    {
      NWScript.SetFogColor((int)fogType, (int)fogColor, this, (float)fadeTime.TotalSeconds);
    }

    /// <summary>
    /// Stops playback of any ambient sounds in this area.
    /// </summary>
    public void StopAmbient()
    {
      NWScript.AmbientSoundStop(this);
    }

    /// <summary>
    /// Stops playback of any running background music in this area.
    /// </summary>
    public void StopBackgroundMusic()
    {
      NWScript.MusicBackgroundStop(this);
    }

    /// <summary>
    /// Stops playback of any running battle music in this area.
    /// </summary>
    public void StopBattleMusic()
    {
      NWScript.MusicBattleStop(this);
    }

    private bool IsAreaFlagSet(AreaFlags flag)
    {
      return (AreaFlags & flag) == flag;
    }

    private void SetAreaFlag(AreaFlags flag, bool state)
    {
      if (state)
      {
        AreaFlags |= flag;
      }
      else
      {
        AreaFlags &= ~flag;
      }
    }
  }
}
