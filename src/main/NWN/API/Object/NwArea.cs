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
    /// Gets the size of the area
    /// <returns>The number of tiles that the area is wide/high.</returns>
    /// </summary>
    public Vector2 AreaSize => new Vector2(NWScript.GetAreaSize((int) AreaSizeDimension.Width, this), NWScript.GetAreaSize((int) AreaSizeDimension.Height, this));

    /// <summary>
    /// Returns true if this area is flagged as either interior or underground.
    /// </summary>
    public bool IsInterior => NWScript.GetIsAreaInterior(this).ToBool();

    /// <summary>
    /// Returns true if this area is above ground, or false if it is underground.
    /// </summary>
    public bool IsAboveGround => (AreaInfo) NWScript.GetIsAreaAboveGround(this) == AreaInfo.AboveGround;

    /// <summary>
    /// Returns true if this area is natural, or false if it is artificial.
    /// </summary>
    public bool IsNatural => (AreaInfo) NWScript.GetIsAreaNatural(this) == AreaInfo.Natural;
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
    public int GetFogAmount(FogType fogType)
    {
      return NWScript.GetFogAmount((int) fogType, this);
    }

    /// <summary>
    /// Sets the fog amount for this area, at the specified time of day.
    /// </summary>
    public void SetFogAmount(FogType fogType, int fogAmount)
    {
      NWScript.SetFogAmount((int) fogType, fogAmount, this);
    }

    /// <summary>
    /// All Objects currently in this area.
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
    /// All clients in this area will recompute the static lighting.
    /// This can be used to update the lighting after changing any tile lights
    /// or if placeables with lights have been added/deleted.
    /// </summary>
    public void RecomputeStaticLighting() => NWScript.RecomputeStaticLighting(this);

    public NwArea Clone(string newTag = "", string newName = "") => Create(ResRef, newTag, newName);

    public static NwArea Create(string resRef, string newTag = "", string newName = "") => NWScript.CreateArea(resRef, newTag, newName).ToNwObject<NwArea>();

    /// <summary>
    /// Destroys this area and anything within it.
    /// </summary>
    /// <returns>The result of this destroy action.</returns>
    public AreaDestroyResult Destroy() => (AreaDestroyResult) NWScript.DestroyArea(this);
  }
}