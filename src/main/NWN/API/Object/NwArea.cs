using System.Collections.Generic;
using System.Numerics;
using NWN.API.Constants;
using NWN.Core;
using NWNX.API.Constants;

namespace NWN.API
{
  [NativeObjectInfo(0, InternalObjectType.Area)]
  public sealed class NwArea : NwObject
  {
    internal NwArea(uint objectId) : base(objectId) {}

    /// <summary>
    /// Gets the size of this area.
    /// <returns>The number of tiles that the area is wide/high.</returns>
    /// </summary>
    public Vector2 Size => new Vector2(NWScript.GetAreaSize((int) AreaSizeDimension.Width, this), NWScript.GetAreaSize((int) AreaSizeDimension.Height, this));

    /// <summary>
    /// Gets a value indicating whether this area is flagged as either interior (true) or underground (false).
    /// </summary>
    public bool IsInterior => NWScript.GetIsAreaInterior(this).ToBool();

    /// <summary>
    /// Gets a value indicating whether this area is above ground (true), or underground (false).
    /// </summary>
    public bool IsAboveGround => (AreaInfo) NWScript.GetIsAreaAboveGround(this) == AreaInfo.AboveGround;

    /// <summary>
    /// Gets a value indicating whether this area is natural (true), or artificial (false).
    /// </summary>
    public bool IsNatural => (AreaInfo) NWScript.GetIsAreaNatural(this) == AreaInfo.Natural;

    /// <summary>
    /// Gets the tileset (.set) resource name used for this area.
    /// </summary>
    public string Tileset => NWScript.GetTilesetResRef(this);

    /// <summary>
    /// Gets or sets the current weather conditions for this area.
    /// </summary>
    public WeatherType Weather
    {
      get => (WeatherType) NWScript.GetWeather(this);
      set => NWScript.SetWeather(this, (int) value);
    }

    /// <summary>
    /// Gets or sets the current skybox for this area.
    /// </summary>
    public Skybox SkyBox
    {
      get => (Skybox) NWScript.GetSkyBox(this);
      set => NWScript.SetSkyBox((int) value, this);
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
        for (uint areaObj = NWScript.GetFirstObjectInArea(this); areaObj != INVALID; areaObj = NWScript.GetNextObjectInArea(this))
        {
          yield return areaObj.ToNwObject<NwGameObject>();
        }
      }
    }

    /// <summary>
    /// Gets the fog color for this area, at the specified time of day.
    /// </summary>
    public FogColor GetFogColor(FogType fogType)
    {
      return (FogColor) NWScript.GetFogColor((int) fogType, this);
    }

    /// <summary>
    /// Sets the fog color for this area, at the specified time of day.
    /// </summary>
    public void SetFogColor(FogType fogType, FogColor fogColor)
    {
      NWScript.SetFogColor((int) fogType, (int) fogColor, this);
    }

    /// <summary>
    /// Gets the fog amount for this area, at the specified time of day.
    /// </summary>
    public int GetFogAmount(FogType fogType) => NWScript.GetFogAmount((int) fogType, this);

    /// <summary>
    /// Sets the fog amount for this area, at the specified time of day.
    /// </summary>
    public void SetFogAmount(FogType fogType, int fogAmount) => NWScript.SetFogAmount((int) fogType, fogAmount, this);

    /// <summary>
    /// Notifies all clients in this area to recompute static lighting.
    /// This can be used to update the lighting after changing any tile lights
    /// or if placeables with lights have been added/deleted.
    /// </summary>
    public void RecomputeStaticLighting() => NWScript.RecomputeStaticLighting(this);

    /// <summary>
    /// Begins playback of background music in this area.
    /// </summary>
    public void PlayBackgroundMusic() => NWScript.MusicBackgroundPlay(this);

    /// <summary>
    /// Stops playback of any running background music in this area.
    /// </summary>
    public void StopBackgroundMusic() => NWScript.MusicBackgroundStop(this);

    /// <summary>
    /// Begins playback of battle music for this area.
    /// </summary>
    public void PlayBattleMusic() => NWScript.MusicBattlePlay(this);

    /// <summary>
    /// Stops playback of any running battle music in this area.
    /// </summary>
    public void StopBattleMusic() => NWScript.MusicBattleStop(this);

    /// <summary>
    /// Begins playback of ambient sounds in this area.
    /// </summary>
    public void PlayAmbient() => NWScript.AmbientSoundPlay(this);

    /// <summary>
    /// Stops playback of any ambient sounds in this area.
    /// </summary>
    public void StopAmbient() => NWScript.AmbientSoundStop(this);

    /// <summary>
    /// Creates a copy of this area, including everything inside of it (except players).
    /// </summary>
    /// <returns>The new cloned area instance.</returns>
    public NwArea Clone() => NWScript.CopyArea(this).ToNwObject<NwArea>();

    /// <summary>
    /// Creates a new area from the specified resource reference.
    /// </summary>
    /// <param name="resRef">The area resource to create this area from.</param>
    /// <param name="newTag">A new tag for this area. Defaults to the tag set in the toolset.</param>
    /// <param name="newName">A new name for this area. Defaults to the name set in the toolset.</param>
    /// <returns>The created area.</returns>
    public static NwArea Create(string resRef, string newTag = "", string newName = "") => NWScript.CreateArea(resRef, newTag, newName).ToNwObject<NwArea>();

    /// <summary>
    /// Destroys this area and anything within it.
    /// </summary>
    /// <returns>The result of this destroy action.</returns>
    public AreaDestroyResult Destroy() => (AreaDestroyResult) NWScript.DestroyArea(this);

    /// <summary>
    /// Gets the value whether the given tile at x, y, for the given creature in the stated area is visible on the map.
    /// Note that creature needs to be a PC or NPC.
    /// x = The location of the tile on the x axis.
    /// y = The location of the tile on the y axis.
    /// Tile exploration also controls object visibility in areas and the fog of war for interior and underground areas.
    /// </summary>
    public bool GetTileExplored(NwCreature creature, int x, int y)
      => NWScript.GetTileExplored(creature, this, x, y).ToBool();

    /// <summary>
    /// Sets if the given creature has explored tile at x, y of the given area.
    /// Note that creature needs to be a PC or NPC.
    /// x = The location of the tile on the x axis.
    /// y = The location of the tile on the y axis.
    /// Tile exploration also controls object visibility in areas and the fog of war for interior and underground areas.
    /// newState is whether the tile should be explored.
    /// </summary>
    public bool SetTileExplored(NwCreature creature, int x, int y, bool newState)
      => NWScript.SetTileExplored(creature, this, x, y, newState.ToInt()).ToBool();
  }
}
