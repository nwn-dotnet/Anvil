using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Triggered when a custom scripted effect receives an interval event, or is applied/removed.
  /// </summary>
  /// <remarks>
  /// This event class should be constructed manually as a part of a script handler.
  /// </remarks>
  /// <example>
  /// <code>
  /// <![CDATA[
  /// public Effect GetAcidIntervalEffect(ScriptHandleFactory scriptHandleFactory)
  /// {
  ///   ScriptCallbackHandle handler = scriptHandleFactory.CreateUniqueHandler(OnInterval);
  ///   return Effect.RunAction(onIntervalHandle: handler, interval: NwTimeSpan.FromRounds(1));
  /// }
  ///
  /// private ScriptHandleResult OnInterval(CallInfo callInfo)
  /// {
  ///   EffectRunScriptEvent eventData = new EffectRunScriptEvent();
  ///   if (eventData.EventType == EffectRunScriptType.OnInterval && eventData.EffectTarget is NwGameObject gameObject)
  ///   {
  ///     _ = ApplyIntervalEffects(eventData.Effect?.Creator, gameObject);
  ///   }
  ///
  ///   return ScriptHandleResult.Handled;
  /// }
  ///
  /// private async Task ApplyIntervalEffects(NwObject? caster, NwGameObject target)
  /// {
  ///   if (caster != null && caster.IsValid)
  ///   {
  ///     await caster.WaitForObjectContext();
  ///   }
  ///
  ///   Effect damageEffect = Effect.Damage(Random.Shared.Roll(6, 2), DamageType.Acid);
  ///   target.ApplyEffect(EffectDuration.Instant, damageEffect);
  /// }
  /// ]]>
  /// </code>
  /// </example>
  public sealed class EffectRunScriptEvent : IEvent
  {
    /// <summary>
    /// Gets the associated effect with this event.
    /// </summary>
    public Effect? Effect { get; } = NWScript.GetLastRunScriptEffect();

    /// <summary>
    /// Gets the target of the effect.
    /// </summary>
    public NwObject? EffectTarget { get; } = NWScript.OBJECT_SELF.ToNwObject();

    /// <summary>
    /// Gets the effect event type.
    /// </summary>
    public EffectRunScriptType EventType { get; } = (EffectRunScriptType)NWScript.GetLastRunScriptEffectScriptType();

    NwObject? IEvent.Context => EffectTarget;
  }
}
