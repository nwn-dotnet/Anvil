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
    private readonly FunctionHook<GetInitiativeModifer> initiativeModiferHook;

    public InitiativeModifierService(HookService hookService)
    {
      initiativeModiferHook = hookService.RequestHook<GetInitiativeModifer>(OnResolveInitiative, FunctionsLinux._ZN12CNWSCreature17ResolveInitiativeEv, HookOrder.Late);
    }

    private delegate void GetInitiativeModifer(void* pObject);

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
        return InternalVariables.InitiativeMod(creature);
      }

      return null;
    }

    /// <summary>
    /// Sets the modifier that is set for the creature's initiative.<br/>
    /// </summary>
    public void SetInitiativeModifier(NwCreature creature, Int32 mod)
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

      int? initMod = InternalVariables.InitiativeMod(creature);
      if (initMod == null)
      {
        initiativeModiferHook.CallOriginal(pCreature);
        return;
      }
      CNWSCreature cCreature = creature.Creature;

      if (cCreature.m_bInitiativeExpired == 1)
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
        cCreature.m_bInitiativeExpired = 0;
      }
    }
  }
}
