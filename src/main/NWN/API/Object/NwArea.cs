using System;
using System.Collections.Generic;
using NWN.API.Constants;
using NWN.Core;
using NWN.Native.API;
using NWN.Services;
using AssociateType = NWN.API.Constants.AssociateType;

namespace NWN.API
{
  [NativeObjectInfo(0, ObjectType.Area)]
  public sealed partial class NwArea : NwObject
  {
    internal readonly CNWSArea Area;

    internal NwArea(CNWSArea area) : base(area)
    {
      Area = area;
    }

    public static implicit operator CNWSArea(NwArea area)
    {
      return area?.Area;
    }

    internal override CNWSScriptVarTable ScriptVarTable
    {
      get => Area.m_ScriptVars;
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
    /// Gets the size of this area.
    /// <returns>The number of tiles that the area is wide/high.</returns>
    /// </summary>
    public Vector2Int Size
    {
      get => new Vector2Int(NWScript.GetAreaSize((int)AreaSizeDimension.Width, this), NWScript.GetAreaSize((int)AreaSizeDimension.Height, this));
    }

    /// <summary>
    /// Gets a value indicating whether this area is flagged as either interior (true) or underground (false).
    /// </summary>
    public bool IsInterior
    {
      get => NWScript.GetIsAreaInterior(this).ToBool();
    }

    /// <summary>
    /// Gets a value indicating whether this area is above ground (true), or underground (false).
    /// </summary>
    public bool IsAboveGround
    {
      get => (AreaInfo)NWScript.GetIsAreaAboveGround(this) == AreaInfo.AboveGround;
    }

    /// <summary>
    /// Gets a value indicating whether this area is natural (true), or artificial (false).
    /// </summary>
    public bool IsNatural
    {
      get => (AreaInfo)NWScript.GetIsAreaNatural(this) == AreaInfo.Natural;
    }

    /// <summary>
    /// Gets the tileset (.set) resource name used for this area.
    /// </summary>
    public string Tileset
    {
      get => NWScript.GetTilesetResRef(this);
    }

    /// <summary>
    /// Gets or sets the current weather conditions for this area.
    /// </summary>
    public WeatherType Weather
    {
      get => (WeatherType)NWScript.GetWeather(this);
      set => NWScript.SetWeather(this, (int)value);
    }

    /// <summary>
    /// Gets or sets the current skybox for this area.
    /// </summary>
    public Skybox SkyBox
    {
      get => (Skybox)NWScript.GetSkyBox(this);
      set => NWScript.SetSkyBox((int)value, this);
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
    /// Gets or sets the wind power for this area.<br/>
    /// Set to 0, 1 or 2.
    /// </summary>
    public byte WindPower
    {
      get => Area.m_nWindAmount;
      set => Area.m_nWindAmount = value;
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
          yield return areaObj.ToNwObject<NwGameObject>();
        }
      }
    }

    /// <summary>
    /// Gets the fog color for this area, at the specified time of day.
    /// </summary>
    public FogColor GetFogColor(FogType fogType)
    {
      return (FogColor)NWScript.GetFogColor((int)fogType, this);
    }

    /// <summary>
    /// Sets the fog color for this area, at the specified time of day.
    /// </summary>
    public void SetFogColor(FogType fogType, FogColor fogColor)
    {
      NWScript.SetFogColor((int)fogType, (int)fogColor, this);
    }

    /// <summary>
    /// Gets the fog amount for this area, at the specified time of day.
    /// </summary>
    public int GetFogAmount(FogType fogType)
    {
      return NWScript.GetFogAmount((int)fogType, this);
    }

    /// <summary>
    /// Sets the fog amount for this area, at the specified time of day.
    /// </summary>
    public void SetFogAmount(FogType fogType, int fogAmount)
    {
      NWScript.SetFogAmount((int)fogType, fogAmount, this);
    }

    /// <summary>
    /// Notifies all clients in this area to recompute static lighting.
    /// This can be used to update the lighting after changing any tile lights
    /// or if placeables with lights have been added/deleted.
    /// </summary>
    public void RecomputeStaticLighting()
    {
      NWScript.RecomputeStaticLighting(this);
    }

    /// <summary>
    /// Begins playback of background music in this area.
    /// </summary>
    public void PlayBackgroundMusic()
    {
      NWScript.MusicBackgroundPlay(this);
    }

    /// <summary>
    /// Stops playback of any running background music in this area.
    /// </summary>
    public void StopBackgroundMusic()
    {
      NWScript.MusicBackgroundStop(this);
    }

    /// <summary>
    /// Begins playback of battle music for this area.
    /// </summary>
    public void PlayBattleMusic()
    {
      NWScript.MusicBattlePlay(this);
    }

    /// <summary>
    /// Stops playback of any running battle music in this area.
    /// </summary>
    public void StopBattleMusic()
    {
      NWScript.MusicBattleStop(this);
    }

    /// <summary>
    /// Begins playback of ambient sounds in this area.
    /// </summary>
    public void PlayAmbient()
    {
      NWScript.AmbientSoundPlay(this);
    }

    /// <summary>
    /// Stops playback of any ambient sounds in this area.
    /// </summary>
    public void StopAmbient()
    {
      NWScript.AmbientSoundStop(this);
    }

    /// <summary>
    /// Creates a copy of this area, including everything inside of it (except players).
    /// </summary>
    /// <returns>The new cloned area instance.</returns>
    public NwArea Clone()
    {
      return NWScript.CopyArea(this).ToNwObject<NwArea>();
    }

    /// <summary>
    /// Creates a new area from the specified resource reference.
    /// </summary>
    /// <param name="resRef">The area resource to create this area from.</param>
    /// <param name="newTag">A new tag for this area. Defaults to the tag set in the toolset.</param>
    /// <param name="newName">A new name for this area. Defaults to the name set in the toolset.</param>
    /// <returns>The created area.</returns>
    public static NwArea Create(string resRef, string newTag = "", string newName = "")
    {
      return NWScript.CreateArea(resRef, newTag, newName).ToNwObject<NwArea>();
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
      for (uint currentObj = NWScript.GetFirstObjectInArea(this); currentObj != Invalid; currentObj = NWScript.GetNextObjectInArea(this))
      {
        T obj = currentObj.ToNwObjectSafe<T>();
        if (obj != null)
        {
          yield return obj;
        }
      }
    }

    public byte[] SerializeGIT(ObjectTypes objectFilter = ObjectTypes.All, ICollection<NwGameObject> exclusionList = null, bool exportVarTable = true, bool exportUUID = true, string resRef = null)
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
          CExoArrayListObjectId creatures = new CExoArrayListObjectId();
          CExoArrayListObjectId items = new CExoArrayListObjectId();
          CExoArrayListObjectId doors = new CExoArrayListObjectId();
          CExoArrayListObjectId triggers = new CExoArrayListObjectId();
          CExoArrayListObjectId encounters = new CExoArrayListObjectId();
          CExoArrayListObjectId waypoints = new CExoArrayListObjectId();
          CExoArrayListObjectId sounds = new CExoArrayListObjectId();
          CExoArrayListObjectId placeables = new CExoArrayListObjectId();
          CExoArrayListObjectId stores = new CExoArrayListObjectId();
          CExoArrayListObjectId aoes = new CExoArrayListObjectId();

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

    public unsafe byte[] SerializeARE(string areaName = null, string resRef = null)
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

    /// <inheritdoc cref="Deserialize(string,byte[],byte[],string,string)"/>
    public static NwArea Deserialize(byte[] serializedARE, byte[] serializedGIT, string newTag = "", string newName = "")
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
    public static NwArea Deserialize(string resRef, byte[] serializedARE, byte[] serializedGIT, string newTag = "", string newName = "")
    {
      ResourceManager.WriteTempResource(resRef + ".git", serializedGIT);
      ResourceManager.WriteTempResource(resRef + ".are", serializedARE);

      return Create(resRef, newTag, newName);
    }
  }
}
