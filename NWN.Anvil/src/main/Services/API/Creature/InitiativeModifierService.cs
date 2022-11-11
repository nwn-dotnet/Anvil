using System;
using Anvil.API;
using Anvil.Internal;
using NWN.Native.API;
using Feat = Anvil.API.Feat;

namespace Anvil.Services
{
  [ServiceBinding(typeof(InitiativeModifierService))]
  [ServiceBindingOptions(InternalBindingPriority.API, Lazy = true)]
  public sealed unsafe class InitiativeModifierService
  {
    private readonly FunctionHook<InitiativeModifier> initiativeModiferHook;

    public InitiativeModifierService(HookService hookService)
    {
      initiativeModiferHook = hookService.RequestHook<InitiativeModifier>(OnResolveInitiative, FunctionsLinux._ZN12CNWSCreature17ResolveInitiativeEv, HookOrder.Late);
    }

    private delegate void InitiativeModifier(void* pObject);

    /// <summary>
    /// Clears any modifier that is set for the creature's initiative.<br/>
    /// </summary>
    public void ClearInitiativeModifier(NwCreature creature)
    {
      InternalVariables.InitiativeMod(creature).Delete();
    }

    /// <summary>
    /// Gets the modifier that is set for the creature's initiative.<br/>
    /// </summary>
    public int? GetInitiativeModifier(NwCreature creature)
    {
      InternalVariableInt initiativeModifier = InternalVariables.InitiativeMod(creature);
      if (initiativeModifier.HasValue)
      {
        return initiativeModifier.Value;
      }

      return null;
    }

    /// <summary>
    /// Sets the modifier that is set for the creature's initiative.<br/>
    /// </summary>
    public void SetInitiativeModifier(NwCreature creature, int mod)
    {
      InternalVariables.InitiativeMod(creature).Value = mod;
    }

    private void OnResolveInitiative(void* pCreature)
    {
      NwCreature? creature = CNWSCreature.FromPointer(pCreature).ToNwObject<NwCreature>();
      if (creature == null)
      {
        initiativeModiferHook.CallOriginal(pCreature);
        return;
      }

      InternalVariableInt initMod = InternalVariables.InitiativeMod(creature);
      if (initMod.HasNothing)
      {
        initiativeModiferHook.CallOriginal(pCreature);
        return;
      }

      CNWSCreature cCreature = creature.Creature;

      if (cCreature.m_bInitiativeExpired.ToBool())
      {
        CNWSCreatureStats? pStats = cCreature.m_pStats;
        CNWRules rules = NWNXLib.Rules()!;

        ushort diceRoll = rules.RollDice(1, 20);
        int mod = pStats.GetDEXMod(0);
        if (pStats.HasFeat((ushort)Feat.EpicSuperiorInitiative).ToBool())
        {
          mod += rules.GetRulesetIntEntry("EPIC_SUPERIOR_INITIATIVE_BONUS".ToExoString(), 8);
        }

        else if (pStats.HasFeat((ushort)Feat.ImprovedInitiative).ToBool())
        {
          mod += rules.GetRulesetIntEntry("IMPROVED_INITIATIVE_BONUS".ToExoString(), 4);
        }

        if (pStats.HasFeat((ushort)Feat.Blooded).ToBool())
        {
          mod += rules.GetRulesetIntEntry("BLOODED_INITIATIVE_BONUS".ToExoString(), 2);
        }

        if (pStats.HasFeat((ushort)Feat.Thug).ToBool())
        {
          mod += rules.GetRulesetIntEntry("THUG_INITIATIVE_BONUS".ToExoString(), 2);
        }

        // Add creature bonus
        mod += initMod.Value;

        cCreature.m_nInitiativeRoll = unchecked((byte)(diceRoll + mod));
        if (creature.IsPlayerControlled(out NwPlayer? player))
        {
          CNWCCMessageData messageData = new CNWCCMessageData();
          messageData.SetObjectID(0, cCreature.m_idSelf);
          messageData.SetInteger(0, diceRoll);
          messageData.SetInteger(1, mod);
          CNWSMessage? message = LowLevel.ServerExoApp.GetNWSMessage();
          if (message != null)
          {
            message.SendServerToPlayerCCMessage(player.Player.m_nPlayerID, (byte)MessageClientSideMsgMinor.Initiative, messageData, null);
          }
        }

        cCreature.m_bInitiativeExpired = false.ToInt();
      }
    }
  }
}
