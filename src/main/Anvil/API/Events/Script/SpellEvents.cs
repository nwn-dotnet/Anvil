using NWN.Core;

namespace Anvil.API.Events
{
  public static class SpellEvents
  {
    public class OnSpellCast : IEvent
    {
      /// <summary>
      /// Gets the caster of this spell.
      /// </summary>
      public NwGameObject Caster { get; }

      /// <summary>
      /// Gets the spell that was cast.
      /// </summary>
      public Spell Spell { get; }

      /// <summary>
      /// Gets a value indicating whether this spell is considered harmful.
      /// </summary>
      public bool Harmful { get; }

      /// <summary>
      /// Gets the object being targeted by this spell, otherwise returns null if the caster targeted the ground.
      /// </summary>
      public NwGameObject TargetObject { get; }

      /// <summary>
      /// Gets the targeted location of this spell.
      /// </summary>
      public Location TargetLocation { get; }

      /// <summary>
      /// Gets the class that the caster cast the spell as.
      /// </summary>
      public ClassType SpellCastClass { get; }

      /// <summary>
      /// Gets the item that was used to cast the spell. Returns null if no item was used to cast this spell.
      /// </summary>
      public NwItem Item { get; }

      /// <summary>
      /// Gets the saving throw DC required to save against the effects of this spell.
      /// </summary>
      public int SaveDC { get; }

      /// <summary>
      /// Gets the type of metamagic used on the last spell.
      /// </summary>
      public MetaMagic MetaMagicFeat { get; }

      NwObject IEvent.Context
      {
        get => Caster;
      }

      public OnSpellCast()
      {
        Caster = NWScript.OBJECT_SELF.ToNwObject<NwGameObject>();
        Spell = (Spell)NWScript.GetSpellId();
        Harmful = NWScript.GetLastSpellHarmful().ToBool();
        TargetObject = NWScript.GetSpellTargetObject().ToNwObject<NwGameObject>();
        TargetLocation = NWScript.GetSpellTargetLocation();
        SpellCastClass = (ClassType)NWScript.GetLastSpellCastClass();
        Item = NWScript.GetSpellCastItem().ToNwObject<NwItem>();
        SaveDC = NWScript.GetSpellSaveDC();
        MetaMagicFeat = (MetaMagic)NWScript.GetMetaMagicFeat();
      }
    }
  }
}
