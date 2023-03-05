using NWN.Core;

namespace Anvil.API
{
  public sealed class PlayerDeviceProperty
  {
    public static readonly PlayerDeviceProperty GuiHeight = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GUI_HEIGHT);
    public static readonly PlayerDeviceProperty GuiScale = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GUI_SCALE);
    public static readonly PlayerDeviceProperty GuiWidth = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GUI_WIDTH);
    public static readonly PlayerDeviceProperty GraphicsAntialiasingMode = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GRAPHICS_ANTIALIASING_MODE);
    public static readonly PlayerDeviceProperty GraphicsAnisotropicFiltering = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GRAPHICS_ANISOTROPIC_FILTERING);
    public static readonly PlayerDeviceProperty GraphicsGamma = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GRAPHICS_GAMMA);
    public static readonly PlayerDeviceProperty GraphicsTextureAnimations = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GRAPHICS_TEXTURE_ANIMATIONS);
    public static readonly PlayerDeviceProperty GraphicsSkyboxes = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GRAPHICS_SKYBOXES);
    public static readonly PlayerDeviceProperty GraphicsCreatureWind = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GRAPHICS_CREATURE_WIND);
    public static readonly PlayerDeviceProperty GraphicsSecondStoryTiles = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GRAPHICS_SECOND_STORY_TILES);
    public static readonly PlayerDeviceProperty GraphicsTileBorders = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GRAPHICS_TILE_BORDERS);
    public static readonly PlayerDeviceProperty GraphicsSpellTargetingEffect = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GRAPHICS_SPELL_TARGETING_EFFECT);
    public static readonly PlayerDeviceProperty GraphicsTexturesPack = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GRAPHICS_TEXTURES_PACK);
    public static readonly PlayerDeviceProperty GraphicsGrass = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GRAPHICS_GRASS);
    public static readonly PlayerDeviceProperty GraphicsGrassRenderDistance = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GRAPHICS_GRASS_RENDER_DISTANCE);
    public static readonly PlayerDeviceProperty GraphicsShinyWater = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GRAPHICS_SHINY_WATER);
    public static readonly PlayerDeviceProperty GraphicsLightingMaxLights = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GRAPHICS_LIGHTING_MAX_LIGHTS);
    public static readonly PlayerDeviceProperty GraphicsLightingEnhanced = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GRAPHICS_LIGHTING_ENHANCED);
    public static readonly PlayerDeviceProperty GraphicsShadowsEnvironment = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GRAPHICS_SHADOWS_ENVIRONMENT);
    public static readonly PlayerDeviceProperty GraphicsShadowsCreatures = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GRAPHICS_SHADOWS_CREATURES);
    public static readonly PlayerDeviceProperty GraphicsShadowsMaxCastingLights = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GRAPHICS_SHADOWS_MAX_CASTING_LIGHTS);
    public static readonly PlayerDeviceProperty GraphicsEffectsHighQuality = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GRAPHICS_EFFECTS_HIGH_QUALITY);
    public static readonly PlayerDeviceProperty GraphicsEffectsCreatureEnvironmentMapping = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GRAPHICS_EFFECTS_CREATURE_ENVIRONMENT_MAPPING);
    public static readonly PlayerDeviceProperty GraphicsKeyholing = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GRAPHICS_KEYHOLING);
    public static readonly PlayerDeviceProperty GraphicsKeyholingWithTooltip = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GRAPHICS_KEYHOLING_WITH_TOOLTIP);
    public static readonly PlayerDeviceProperty GraphicsKeyholingDisablesCameraCollisions = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GRAPHICS_KEYHOLING_DISABLES_CAMERA_COLLISIONS);
    public static readonly PlayerDeviceProperty GraphicsFboSsao = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GRAPHICS_FBO_SSAO);
    public static readonly PlayerDeviceProperty GraphicsFboHighContrast = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GRAPHICS_FBO_HIGH_CONTRAST);
    public static readonly PlayerDeviceProperty GraphicsFboVibrance = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GRAPHICS_FBO_VIBRANCE);
    public static readonly PlayerDeviceProperty GraphicsFboToon = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GRAPHICS_FBO_TOON);
    public static readonly PlayerDeviceProperty GraphicsFboDof = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GRAPHICS_FBO_DOF);
    public static readonly PlayerDeviceProperty GraphicsLOD = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GRAPHICS_LOD);
    public static readonly PlayerDeviceProperty GraphicsRenderCloaks = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GRAPHICS_RENDER_CLOAKS);
    public static readonly PlayerDeviceProperty GraphicsGeneratePLTWithShaders = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GRAPHICS_GENERATE_PLT_WITH_SHADERS);
    public static readonly PlayerDeviceProperty GraphicsHilite = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GRAPHICS_HILITE);
    public static readonly PlayerDeviceProperty GraphicsHiliteGlow = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GRAPHICS_HILITE_GLOW);
    public static readonly PlayerDeviceProperty InputKeyboardShiftWalkInverted = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_INPUT_KEYBOARD_SHIFT_WALK_INVERTED);
    public static readonly PlayerDeviceProperty InputMouseHardwarePointer = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_INPUT_MOUSE_HARDWARE_POINTER);
    public static readonly PlayerDeviceProperty UiScale = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_UI_SCALE);
    public static readonly PlayerDeviceProperty UiLargeFont = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_UI_LARGE_FONT);
    public static readonly PlayerDeviceProperty UiTooltipDelay = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_UI_TOOLTIP_DELAY);
    public static readonly PlayerDeviceProperty UiMouseoverFeedback = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_UI_MOUSEOVER_FEEDBACK);
    public static readonly PlayerDeviceProperty UiTextBubble = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_UI_TEXT_BUBBLE);
    public static readonly PlayerDeviceProperty UiTargetingFeedback = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_UI_TARGETING_FEEDBACK);
    public static readonly PlayerDeviceProperty UiFloatingTextFeedback = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_UI_FLOATING_TEXT_FEEDBACK);
    public static readonly PlayerDeviceProperty UiFloatingTextFeedbackDamageTotalsOnly = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_UI_FLOATING_TEXT_FEEDBACK_DAMAGE_TOTALS_ONLY);
    public static readonly PlayerDeviceProperty UiHideQuickchatTextInChatWindow = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_UI_HIDE_QUICKCHAT_TEXT_IN_CHAT_WINDOW);
    public static readonly PlayerDeviceProperty UiConfirmSelfcastSpells = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_UI_CONFIRM_SELFCAST_SPELLS);
    public static readonly PlayerDeviceProperty UiConfirmSelfcastFeats = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_UI_CONFIRM_SELFCAST_FEATS);
    public static readonly PlayerDeviceProperty UiConfirmSelfcastItems = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_UI_CONFIRM_SELFCAST_ITEMS);
    public static readonly PlayerDeviceProperty UiChatPanePrimaryHeight = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_UI_CHAT_PANE_PRIMARY_HEIGHT);
    public static readonly PlayerDeviceProperty UiChatPaneSecondaryHeight = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_UI_CHAT_PANE_SECONDARY_HEIGHT);
    public static readonly PlayerDeviceProperty UiChatSwearFilter = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_UI_CHAT_SWEAR_FILTER);
    public static readonly PlayerDeviceProperty UiPartyInvitePopup = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_UI_PARTY_INVITE_POPUP);
    public static readonly PlayerDeviceProperty UiSpellbookSortSpells = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_UI_SPELLBOOK_SORT_SPELLS);
    public static readonly PlayerDeviceProperty UiRadialSpellcastingAlwaysSubradial = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_UI_RADIAL_SPELLCASTING_ALWAYS_SUBRADIAL);
    public static readonly PlayerDeviceProperty UiRadialClassAbilitiesAlwaysSubradial = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_UI_RADIAL_CLASS_ABILITIES_ALWAYS_SUBRADIAL);
    public static readonly PlayerDeviceProperty CameraMode = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_CAMERA_MODE);
    public static readonly PlayerDeviceProperty CameraEdgeTurning = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_CAMERA_EDGE_TURNING);
    public static readonly PlayerDeviceProperty CameraDialogZoom = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_CAMERA_DIALOG_ZOOM);
    public static readonly PlayerDeviceProperty GameGore = new PlayerDeviceProperty(NWScript.PLAYER_DEVICE_PROPERTY_GAME_GORE);

    internal PlayerDeviceProperty(string propertyName)
    {
      PropertyName = propertyName;
    }

    internal string PropertyName { get; }
  }
}
