using System;
using Anvil.API;
using Anvil.Native;
using NLog;
using NWN.Native.API;
using AssociateType = Anvil.API.AssociateType;
using EffectSubType = Anvil.API.EffectSubType;
using ImmunityType = Anvil.API.ImmunityType;
using RacialType = Anvil.API.RacialType;

namespace Anvil.Services
{
  [ServiceBinding(typeof(PlayerPossessionService))]
  internal sealed unsafe class PlayerPossessionService
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly FunctionHook<Functions.CNWSCreature.UnsummonMyself> unsummonMyselfHook;
    private readonly FunctionHook<Functions.CNWSCreature.PossessFamiliar> possessFamiliarHook;
    private readonly FunctionHook<Functions.CNWSCreature.UnpossessFamiliar> unpossessFamiliarHook;

    public PlayerPossessionService(HookService hookService)
    {
      Log.Info($"Initialising optional service {nameof(PlayerPossessionService)}");
      unsummonMyselfHook = hookService.RequestHook<Functions.CNWSCreature.UnsummonMyself>(OnUnsummonMyself, HookOrder.Late);
      possessFamiliarHook = hookService.RequestHook<Functions.CNWSCreature.PossessFamiliar>(OnPossessFamiliar, HookOrder.Late);
      unpossessFamiliarHook = hookService.RequestHook<Functions.CNWSCreature.UnpossessFamiliar>(OnUnpossessFamiliar, HookOrder.Early);
    }

    public void PossessCreature(NwPlayer player, NwCreature creature, bool mindImmunity, bool createQuickBar)
    {
      CheckValidPossession(player, creature);

      NwCreature playerLoginCreature = player.LoginCreature!;
      CNWSCreature cPlayerLoginCreature = playerLoginCreature.Creature;
      CNWSCreature cTargetCreature = creature.Creature;

      InternalVariables.PossessedAssociateType(creature).Value = creature.AssociateType;

      // If they already have a familiar we temporarily remove it as an associate
      // then we add the possessed creature as a familiar. We then add the regular familiar back.
      // This is because PossessFamiliar looks for the first associate of type familiar.
      NwCreature? existingFamiliar = playerLoginCreature.GetAssociate(AssociateType.Familiar);
      if (existingFamiliar != null)
      {
        cPlayerLoginCreature.RemoveAssociate(existingFamiliar);
      }

      cPlayerLoginCreature.AddAssociate(creature, (ushort)AssociateType.Familiar);
      cPlayerLoginCreature.PossessFamiliar();

      if (existingFamiliar != null)
      {
        cPlayerLoginCreature.AddAssociate(existingFamiliar, (ushort)AssociateType.Familiar);
      }

      if (createQuickBar)
      {
        cTargetCreature.CreateDefaultQuickButtons();
      }

      InternalVariables.PossessedObject(player).Value = (int)creature.ObjectId;
      InternalVariables.PossessedByObject(creature).Value = (int)playerLoginCreature.ObjectId;

      if (!mindImmunity)
      {
        RemoveImmunityEffect(playerLoginCreature);
      }
    }

    private void OnUnsummonMyself(void* pCreature)
    {
      CNWSCreature cCreature = CNWSCreature.FromPointer(pCreature);
      NwCreature creature = cCreature.ToNwObject<NwCreature>()!;

      InternalVariableInt possessedByVar = InternalVariables.PossessedByObject(creature);
      NwCreature? possessedBy = ((uint)possessedByVar.Value).ToNwObject<NwCreature>();

      if (possessedByVar.HasNothing || possessedBy == null)
      {
        unsummonMyselfHook.CallOriginal(pCreature);
        return;
      }

      if (creature.Area != null)
      {
        possessedBy.Creature.UnpossessFamiliar();
        return;
      }

      unsummonMyselfHook.CallOriginal(pCreature);
      RemoveImmunityEffect(possessedBy);
    }

    private void OnPossessFamiliar(void* pCreature)
    {
      NwCreature creature = CNWSCreature.FromPointer(pCreature).ToNwObject<NwCreature>()!;
      if (InternalVariables.PossessedObject(creature).HasNothing)
      {
        possessFamiliarHook.CallOriginal(pCreature);
      }
    }

    private void OnUnpossessFamiliar(void* pCreature)
    {
      unpossessFamiliarHook.CallOriginal(pCreature);
      CNWSCreature cPossessedBy = CNWSCreature.FromPointer(pCreature);
      NwCreature possessedBy = cPossessedBy.ToNwObject<NwCreature>()!;

      InternalVariableInt possessedCreatureVar = InternalVariables.PossessedObject(possessedBy);
      NwCreature? possessedCreature = possessedCreatureVar.HasValue ? ((uint)possessedCreatureVar.Value).ToNwObject<NwCreature>() : null;

      if (possessedCreature == null)
      {
        return;
      }

      InternalVariableInt possessedByVar = InternalVariables.PossessedByObject(possessedCreature);
      cPossessedBy.RemoveAssociate(possessedCreature);

      possessedCreatureVar.Delete();
      possessedByVar.Delete();

      InternalVariableEnum<AssociateType> associateTypeVar = InternalVariables.PossessedAssociateType(possessedCreature);
      if (associateTypeVar.HasValue && associateTypeVar.Value != AssociateType.None)
      {
        cPossessedBy.AddAssociate(possessedCreature, (ushort)associateTypeVar.Value);
        associateTypeVar.Delete();
      }
    }

    private void CheckValidPossession(NwPlayer player, NwCreature creature)
    {
      if (!player.IsDM)
      {
        throw new InvalidOperationException("Player must be a DM to possess the creature.");
      }

      if (creature.Master != null)
      {
        throw new InvalidOperationException("Cannot possess creature as it is already being possessed.");
      }

      if (creature.IsPlayerControlled || creature.IsLoginPlayerCharacter)
      {
        throw new InvalidOperationException("Cannot possess creature as it is a player character.");
      }

      if (player.LoginCreature != player.ControlledCreature)
      {
        throw new InvalidOperationException("Cannot possess creature as the player is already possessing another creature.");
      }
    }

    private void RemoveImmunityEffect(NwCreature creature)
    {
      foreach (Effect effect in creature.ActiveEffects)
      {
        if (effect.EffectType == EffectType.Immunity &&
          effect.SubType == EffectSubType.Magical &&
          effect.Creator == creature &&
          Math.Abs(effect.DurationRemaining - 4.0f) < float.Epsilon &&
          effect.CasterLevel == -1 &&
          effect.IntParams[0] == (int)ImmunityType.MindSpells &&
          effect.IntParams[1] == (int)RacialType.Invalid)
        {
          creature.RemoveEffect(effect);
          break;
        }
      }
    }
  }
}
