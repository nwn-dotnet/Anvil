using System;
using NWN;

namespace NWM.API.NWNX
{
  public static class Player
  {
    private const string PLUGIN_NAME = "NWNX_Player";

    // Force display placeable examine window for player
    // If used on a placeable in a different area than the player, the portait will not be shown.
    public static void ForcePlaceableExamineWindow(NwObject player, NwObject placeable)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "ForcePlaceableExamineWindow");
      NWN.Internal.NativeFunctions.nwnxPushObject(placeable);
      NWN.Internal.NativeFunctions.nwnxPushObject(player);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    // Force opens the target object's inventory for the player.
    // A few notes about this function:
    // - If the placeable is in a different area than the player, the portrait will not be shown
    // - The placeable's open/close animations will be played
    // - Clicking the 'close' button will cause the player to walk to the placeable;
    //     If the placeable is in a different area, the player will just walk to the edge
    //     of the current area and stop. This action can be cancelled manually.
    // - Walking will close the placeable automatically.
    public static void ForcePlaceableInventoryWindow(NwObject player, NwObject placeable)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "ForcePlaceableInventoryWindow");
      NWN.Internal.NativeFunctions.nwnxPushObject(placeable);
      NWN.Internal.NativeFunctions.nwnxPushObject(player);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    // Starts displaying a timing bar.
    // Will run a script at the end of the timing bar, if specified.
    public static void StartGuiTimingBar(NwObject player, float seconds, string script = "", TimingBarType type = TimingBarType.Custom)
    {
      if (NWScript.GetLocalInt(player, "NWNX_PLAYER_GUI_TIMING_ACTIVE") == 1)
        return;

      NWN.Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "StartGuiTimingBar");
      NWN.Internal.NativeFunctions.nwnxPushInt((int) type);
      NWN.Internal.NativeFunctions.nwnxPushFloat(seconds);
      NWN.Internal.NativeFunctions.nwnxPushObject(player);

      NWN.Internal.NativeFunctions.nwnxCallFunction();

      int id = NWScript.GetLocalInt(player, "NWNX_PLAYER_GUI_TIMING_ID") + 1;
      NWScript.SetLocalInt(player, "NWNX_PLAYER_GUI_TIMING_ACTIVE", id);
      NWScript.SetLocalInt(player, "NWNX_PLAYER_GUI_TIMING_ID", id);

      NWScript.DelayCommand(seconds, () => StopGuiTimingBar(player, script, id));
    }

    // Stops displaying a timing bar.
    // Runs a script if specified.
    public static void StopGuiTimingBar(NwObject creature, string script, int id)
    {
      int activeId = NWScript.GetLocalInt(creature, "NWNX_PLAYER_GUI_TIMING_ACTIVE");
      // Either the timing event was never started, or it already finished.
      if (activeId == 0)
        return;

      // If id != -1, we ended up here through DelayCommand. Make sure it's for the right ID
      if (id != -1 && id != activeId)
        return;

      NWScript.DeleteLocalInt(creature, "NWNX_PLAYER_GUI_TIMING_ACTIVE");

      NWN.Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "StopGuiTimingBar");
      NWN.Internal.NativeFunctions.nwnxPushObject(creature);
      NWN.Internal.NativeFunctions.nwnxCallFunction();

      if (!string.IsNullOrWhiteSpace(script))
      {
        NWScript.ExecuteScript(script, creature);
      }
    }

    // Stops displaying a timing bar.
    // Runs a script if specified.
    public static void StopGuiTimingBar(NwObject player, string script)
    {
      StopGuiTimingBar(player, script, -1);
    }

    // Sets whether the player should always walk when given movement commands.
    // If true, clicking on the ground or using WASD will trigger walking instead of running.
    public static void SetAlwaysWalk(NwObject player, int bWalk)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "SetAlwaysWalk");
      NWN.Internal.NativeFunctions.nwnxPushInt(bWalk);
      NWN.Internal.NativeFunctions.nwnxPushObject(player);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    // Gets the player's quickbar slot info
    public static QuickBarSlot GetQuickBarSlot(NwObject player, int slot)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "GetQuickBarSlot");
      QuickBarSlot qbs = new QuickBarSlot();
      NWN.Internal.NativeFunctions.nwnxPushInt(slot);
      NWN.Internal.NativeFunctions.nwnxPushObject(player);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      qbs.Associate = NWN.Internal.NativeFunctions.nwnxPopObject().ToNwObjectSafe<NwCreature>();
      qbs.AssociateType = NWN.Internal.NativeFunctions.nwnxPopInt();
      qbs.DomainLevel = NWN.Internal.NativeFunctions.nwnxPopInt();
      qbs.MetaType = NWN.Internal.NativeFunctions.nwnxPopInt();
      qbs.INTParam1 = NWN.Internal.NativeFunctions.nwnxPopInt();
      qbs.ToolTip = NWN.Internal.NativeFunctions.nwnxPopString();
      qbs.CommandLine = NWN.Internal.NativeFunctions.nwnxPopString();
      qbs.CommandLabel = NWN.Internal.NativeFunctions.nwnxPopString();
      qbs.Resref = NWN.Internal.NativeFunctions.nwnxPopString();
      qbs.MultiClass = NWN.Internal.NativeFunctions.nwnxPopInt();
      qbs.ObjectType = (QuickBarSlotType) NWN.Internal.NativeFunctions.nwnxPopInt();
      qbs.SecondaryItem = NWN.Internal.NativeFunctions.nwnxPopObject().ToNwObjectSafe<NwItem>();
      qbs.Item = NWN.Internal.NativeFunctions.nwnxPopObject().ToNwObjectSafe<NwItem>();

      return qbs;
    }

    // Sets a player's quickbar slot
    public static void SetQuickBarSlot(NwObject player, int slot, QuickBarSlot qbs)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "SetQuickBarSlot");
      NWN.Internal.NativeFunctions.nwnxPushObject(qbs.Item);
      NWN.Internal.NativeFunctions.nwnxPushObject(qbs.SecondaryItem);
      NWN.Internal.NativeFunctions.nwnxPushInt((int) qbs.ObjectType);
      NWN.Internal.NativeFunctions.nwnxPushInt(qbs.MultiClass);
      NWN.Internal.NativeFunctions.nwnxPushString(qbs.Resref);
      NWN.Internal.NativeFunctions.nwnxPushString(qbs.CommandLabel);
      NWN.Internal.NativeFunctions.nwnxPushString(qbs.CommandLine);
      NWN.Internal.NativeFunctions.nwnxPushString(qbs.ToolTip);
      NWN.Internal.NativeFunctions.nwnxPushInt(qbs.INTParam1);
      NWN.Internal.NativeFunctions.nwnxPushInt(qbs.MetaType);
      NWN.Internal.NativeFunctions.nwnxPushInt(qbs.DomainLevel);
      NWN.Internal.NativeFunctions.nwnxPushInt(qbs.AssociateType);
      NWN.Internal.NativeFunctions.nwnxPushObject(qbs.Associate);

      NWN.Internal.NativeFunctions.nwnxPushInt(slot);
      NWN.Internal.NativeFunctions.nwnxPushObject(player);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }


    // Get the name of the .bic file associated with the player's character.
    public static string GetBicFileName(NwObject player)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "GetBicFileName");
      NWN.Internal.NativeFunctions.nwnxPushObject(player);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.nwnxPopString();
    }

    // Plays the VFX at the target position in current area for the given player only
    public static void ShowVisualEffect(NwObject player, int effectId, Vector position)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "ShowVisualEffect");
      NWN.Internal.NativeFunctions.nwnxPushFloat(position.x);
      NWN.Internal.NativeFunctions.nwnxPushFloat(position.y);
      NWN.Internal.NativeFunctions.nwnxPushFloat(position.z);
      NWN.Internal.NativeFunctions.nwnxPushInt(effectId);
      NWN.Internal.NativeFunctions.nwnxPushObject(player);

      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    // Changes the nighttime music track for the given player only
    public static void MusicBackgroundChangeTimeToggle(NwObject player, int track, bool nNight)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "ChangeBackgroundMusic");
      NWN.Internal.NativeFunctions.nwnxPushInt(track);
      NWN.Internal.NativeFunctions.nwnxPushInt(nNight ? 1 : 0); // bool day = false
      NWN.Internal.NativeFunctions.nwnxPushObject(player);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    // Toggle the background music for the given player only
    public static void MusicBackgroundToggle(NwObject player, bool nOn)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "PlayBackgroundMusic");
      NWN.Internal.NativeFunctions.nwnxPushInt(nOn ? 1 : 0); // bool play = false
      NWN.Internal.NativeFunctions.nwnxPushObject(player);

      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    // Changes the battle music track for the given player only
    public static void MusicBattleChange(NwObject player, int track)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "ChangeBattleMusic");
      NWN.Internal.NativeFunctions.nwnxPushInt(track);
      NWN.Internal.NativeFunctions.nwnxPushObject(player);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    // Toggle the background music for the given player only
    public static void MusicBattleToggle(NwObject player, bool nOn)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "PlayBattleMusic");
      NWN.Internal.NativeFunctions.nwnxPushInt(nOn ? 1 : 0);
      NWN.Internal.NativeFunctions.nwnxPushObject(player);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    // Play a sound at the location of target for the given player only
    // If target is OBJECT_INVALID the sound will play at the location of the player
    public static void PlaySound(NwObject player, string sound, NwObject target)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "PlaySound");
      NWN.Internal.NativeFunctions.nwnxPushObject(target);
      NWN.Internal.NativeFunctions.nwnxPushString(sound);
      NWN.Internal.NativeFunctions.nwnxPushObject(player);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    // Toggle a placeable's usable flag for the given player only
    public static void SetPlaceableUseable(NwObject player, NwObject placeable, bool isUseable)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "SetPlaceableUsable");
      NWN.Internal.NativeFunctions.nwnxPushInt(isUseable ? 1 : 0);
      NWN.Internal.NativeFunctions.nwnxPushObject(placeable);
      NWN.Internal.NativeFunctions.nwnxPushObject(player);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    // Override player's rest duration
    // Duration is in milliseconds, 1000 = 1 second
    // Minimum duration of 10ms
    // -1 clears the override
    public static void SetRestDuration(NwObject player, int duration)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "SetRestDuration");
      NWN.Internal.NativeFunctions.nwnxPushInt(duration);
      NWN.Internal.NativeFunctions.nwnxPushObject(player);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    // Apply visualeffect to target that only player can see
    // Note: Only works with instant effects: VFX_COM_*, VFX_FNF_*, VFX_IMP_*
    public static void ApplyInstantVisualEffectToObject(NwObject player, NwObject target, int visualeffect)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "ApplyInstantVisualEffectToObject");
      NWN.Internal.NativeFunctions.nwnxPushInt(visualeffect);
      NWN.Internal.NativeFunctions.nwnxPushObject(target);
      NWN.Internal.NativeFunctions.nwnxPushObject(player);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    // Refreshes the players character sheet
    // Note: You may need to use DelayCommand if you're manipulating values
    // through nwnx and forcing a UI refresh, 0.5s seemed to be fine
    public static void UpdateCharacterSheet(NwObject player)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "UpdateCharacterSheet");
      NWN.Internal.NativeFunctions.nwnxPushObject(player);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    // Allows player to open target's inventory
    // Target must be a creature or another player
    // Note: only works if player and target are in the same area
    public static void OpenInventory(NwObject player, NwObject target, bool open = true)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "OpenInventory");
      NWN.Internal.NativeFunctions.nwnxPushInt(open ? 1 : 0);
      NWN.Internal.NativeFunctions.nwnxPushObject(target);
      NWN.Internal.NativeFunctions.nwnxPushObject(player);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    // Get player's area exploration state
    public static string GetAreaExplorationState(NwObject player, NwObject area)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "GetAreaExplorationState");
      NWN.Internal.NativeFunctions.nwnxPushObject(area);
      NWN.Internal.NativeFunctions.nwnxPushObject(player);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.nwnxPopString();
    }

    // Set player's area exploration state (str is an encoded string obtained with NWNX_Player_GetAreaExplorationState)
    public static void SetAreaExplorationState(NwObject player, NwObject area, string str)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "SetAreaExplorationState");
      NWN.Internal.NativeFunctions.nwnxPushString(str);
      NWN.Internal.NativeFunctions.nwnxPushObject(area);
      NWN.Internal.NativeFunctions.nwnxPushObject(player);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    // Override oPlayer's rest animation to nAnimation
    //
    // NOTE: nAnimation does not take ANIMATION_LOOPING_* or ANIMATION_FIREFORGET_* constants
    //       Use NWNX_Consts_TranslateNWScriptAnimation() in nwnx_consts.nss to get their NWNX equivalent
    //       -1 to clear the override
    public static void SetRestAnimation(NwObject oPlayer, int nAnimation)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "SetRestAnimation");
      NWN.Internal.NativeFunctions.nwnxPushInt(nAnimation);
      NWN.Internal.NativeFunctions.nwnxPushObject(oPlayer);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    // Override a visual transform on the given object that only oPlayer will see.
    // - oObject can be any valid Creature, Placeable, Item or Door.
    // - nTransform is one of OBJECT_VISUAL_TRANSFORM_* or -1 to remove the override
    // - fValue depends on the transformation to apply.
    public static void SetObjectVisualTransformOverride(NwObject oPlayer, NwObject oObject, int nTransform, float fValue)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "SetObjectVisualTransformOverride");
      NWN.Internal.NativeFunctions.nwnxPushFloat(fValue);
      NWN.Internal.NativeFunctions.nwnxPushInt(nTransform);
      NWN.Internal.NativeFunctions.nwnxPushObject(oObject);
      NWN.Internal.NativeFunctions.nwnxPushObject(oPlayer);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    // Apply a looping visualeffect to target that only player can see
    // visualeffect: VFX_DUR_*, call again to remove an applied effect
    //               -1 to remove all effects
    //
    // Note: Only really works with looping effects: VFX_DUR_*
    //       Other types *kind* of work, they'll play when reentering the area and the object is in view
    //       or when they come back in view range.
    public static void ApplyLoopingVisualEffectToObject(NwObject player, NwObject target, int visualeffect)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "ApplyLoopingVisualEffectToObject");
      NWN.Internal.NativeFunctions.nwnxPushInt(visualeffect);
      NWN.Internal.NativeFunctions.nwnxPushObject(target);
      NWN.Internal.NativeFunctions.nwnxPushObject(player);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    // Override the name of placeable for player only
    // "" to clear the override
    public static void SetPlaceableNameOverride(NwObject player, NwObject placeable, string name)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "SetPlaceableNameOverride");
      NWN.Internal.NativeFunctions.nwnxPushString(name);
      NWN.Internal.NativeFunctions.nwnxPushObject(placeable);
      NWN.Internal.NativeFunctions.nwnxPushObject(player);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    // Gets whether a quest has been completed by a player
    // Returns -1 if they don't have the journal entry
    public static int GetQuestCompleted(NwObject player, string sQuestName)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "GetQuestCompleted");
      NWN.Internal.NativeFunctions.nwnxPushString(sQuestName);
      NWN.Internal.NativeFunctions.nwnxPushObject(player);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.nwnxPopInt();
    }

    // This will require storing the PC's cd key or community name (depending on how you store in your vault)
    // and bic_filename along with routinely updating their location in some persistent method like OnRest,
    // OnAreaEnter and OnClentExit.
    //
    // Place waypoints on module load representing where a PC should start
    public static void SetPersistentLocation(string sCDKeyOrCommunityName, string sBicFileName, NwObject oWP, bool bFirstConnectOnly = true)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "SetPersistentLocation");
      NWN.Internal.NativeFunctions.nwnxPushInt(bFirstConnectOnly ? 1 : 0);
      NWN.Internal.NativeFunctions.nwnxPushObject(oWP);
      NWN.Internal.NativeFunctions.nwnxPushString(sBicFileName);
      NWN.Internal.NativeFunctions.nwnxPushString(sCDKeyOrCommunityName);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    // Force an item name to be updated.
    // This is a workaround for bug that occurs when updating item names in open containers.
    public static void UpdateItemName(NwObject oPlayer, NwObject oItem)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "UpdateItemName");
      NWN.Internal.NativeFunctions.nwnxPushObject(oItem);
      NWN.Internal.NativeFunctions.nwnxPushObject(oPlayer);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    // Possesses a creature by temporarily making them a familiar
    // This command allows a PC to possess an NPC by temporarily adding them as a familiar. It will work
    // if the player already has an existing familiar. The creatures must be in the same area. Unpossession can be
    // done with the regular @nwn{UnpossessFamiliar} commands.
    // The possessed creature will send automap data back to the possessor.
    // If you wish to prevent this you may wish to use NWNX_Player_GetAreaExplorationState() and
    // NWNX_Player_SetAreaExplorationState() before and after the possession.
    // The possessing creature will be left wherever they were when beginning the possession. You may wish
    // to use @nwn{EffectCutsceneImmobilize} and @nwn{EffectCutsceneGhost} to hide them.
    public static bool PossessCreature(NwObject oPossessor, NwObject oPossessed, bool bMindImmune = true, bool bCreateDefaultQB = false)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "PossessCreature");
      NWN.Internal.NativeFunctions.nwnxPushInt(bCreateDefaultQB ? 1 : 0);
      NWN.Internal.NativeFunctions.nwnxPushInt(bMindImmune ? 1 : 0);
      NWN.Internal.NativeFunctions.nwnxPushObject(oPossessed);
      NWN.Internal.NativeFunctions.nwnxPushObject(oPossessor);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return Convert.ToBoolean(NWN.Internal.NativeFunctions.nwnxPopInt());
    }
  }
}