using NWN.Core;

namespace Anvil.API.Events
{
  public static class SpellEvents
  {
    public sealed class OnSpellCast : IEvent
    {
      /// <summary>
      /// Gets the caster of this spell.
      /// </summary>
      public NwGameObject? Caster { get; } = NWScript.OBJECT_SELF.ToNwObject<NwGameObject>();

      /// <summary>
      /// Gets a value indicating whether this spell is considered harmful.
      /// </summary>
      public bool Harmful { get; } = NWScript.GetLastSpellHarmful().ToBool();

      /// <summary>
      /// Gets the item that was used to cast the spell. Returns null if no item was used to cast this spell.
      /// </summary>
      public NwItem? Item { get; } = NWScript.GetSpellCastItem().ToNwObject<NwItem>();

      /// <summary>
      /// Gets the type of metamagic used on the last spell.
      /// </summary>
      public MetaMagic MetaMagicFeat { get; } = (MetaMagic)NWScript.GetMetaMagicFeat();

      /// <summary>
      /// Gets the saving throw DC required to save against the effects of this spell.
      /// </summary>
      public int SaveDC { get; } = NWScript.GetSpellSaveDC();

      /// <summary>
      /// Gets the spell that was cast.
      /// </summary>
      public NwSpell Spell { get; } = NwSpell.FromSpellId(NWScript.GetSpellId())!;

      /// <summary>
      /// Gets the class that the caster cast the spell as.
      /// </summary>
      public NwClass? SpellCastClass { get; } = NwClass.FromClassId(NWScript.GetLastSpellCastClass());

      /// <summary>
      /// Gets the level of the spell that was cast.
      /// </summary>
      public int SpellLevel { get; } = NWScript.GetLastSpellLevel();

      /// <summary>
      /// Gets if this spell was cast spontaneously.
      /// </summary>
      public bool IsSpontaneousCast { get; } = NWScript.GetSpellCastSpontaneously().ToBool();

      /// <summary>
      /// Gets the targeted location of this spell.
      /// </summary>
      public Location? TargetLocation { get; } = NWScript.GetSpellTargetLocation();

      /// <summary>
      /// Gets the object being targeted by this spell, otherwise returns null if the caster targeted the ground.
      /// </summary>
      public NwGameObject? TargetObject { get; } = NWScript.GetSpellTargetObject().ToNwObject<NwGameObject>();

      NwObject? IEvent.Context => Caster;
    }
  }
}
