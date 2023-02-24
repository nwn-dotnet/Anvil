/*
 * Examples for creating and applying effects to objects and locations.
 */

using System;
using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;

namespace NWN.Anvil.Samples
{
  [ServiceBinding(typeof(EffectExamples))]
  public sealed class EffectExamples
  {
    // Some effects can be declared as fields. This allows you to reuse effects, rather than creating them every time.
    private readonly Effect blindnessEffect = Effect.Blindness();
    private readonly Effect bloodVfx = Effect.VisualEffect(VfxType.ComBloodCrtRed);

    public EffectExamples()
    {
      // Make our blindness effect a supernatural effect.
      blindnessEffect.SubType = EffectSubType.Supernatural;

      // Register methods to listen for the player/client enter event.
      NwModule.Instance.OnClientEnter += RatSummonExample;

      // Register methods to listen for the creature damage event.
      NwModule.Instance.OnCreatureDamage += BloodVfxExample;
      NwModule.Instance.OnCreatureDamage += BlindnessExample;
    }

    /// <summary>
    /// Spawns a rat creature summon at the module spawn location when a player connects.
    /// </summary>
    private async void RatSummonExample(ModuleEvents.OnClientEnter eventData)
    {
      // Create the summon effect.
      // Some effects require a "caster" for the effect, and must be made within the caster's context.
      await eventData.Player.ControlledCreature!.WaitForObjectContext();
      Effect summon = Effect.SummonCreature("nw_rat001", VfxType.ImpUnsummon);

      // Spawn the summon at the module's starting location
      Location spawnLocation = NwModule.Instance.StartingLocation;
      spawnLocation.ApplyEffect(EffectDuration.Temporary, summon, TimeSpan.FromMinutes(5));
    }

    /// <summary>
    /// Applies a blood splatter effect to all creatures taking more than 50 damage.
    /// </summary>
    private void BloodVfxExample(OnCreatureDamage eventData)
    {
      // If the target is a creature, and the attack does more than 50 slashing damage...
      if (eventData.Target is NwCreature && eventData.DamageData.Slash > 50)
      {
        // ...Apply our blood effect to the creature
        eventData.Target.ApplyEffect(EffectDuration.Instant, bloodVfx);
      }
    }

    /// <summary>
    /// Applies a 5 second blindness effect to all creatures taking more than 100 damage.
    /// </summary>
    private void BlindnessExample(OnCreatureDamage eventData)
    {
      // If the target is a creature, and the attack does more than 100 bludgeoning damage...
      if (eventData.Target is NwCreature && eventData.DamageData.Bludgeoning > 100)
      {
        // ...Apply our blindness effect for 5 seconds.
        eventData.Target.ApplyEffect(EffectDuration.Temporary, blindnessEffect, TimeSpan.FromSeconds(5));
      }
    }
  }
}
