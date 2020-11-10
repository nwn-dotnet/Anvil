using NWN.API.Constants;
using NWN.Core;

namespace NWN.API.Events
{
  public static class SpellEvents
  {
    public class OnSpellCast : ScriptEvent<OnSpellCast>
    {
      /// <summary>
      /// Gets the caster of this spell.
      /// </summary>
      public NwGameObject Caster { get; private set; }

      /// <summary>
      /// Gets the spell that was cast.
      /// </summary>
      public Spell Spell { get; private set; }

      /// <summary>
      /// Gets a value indicating whether this spell is considered harmful.
      /// </summary>
      public bool Harmful { get; private set; }

      /// <summary>
      /// Gets the object being targeted by this spell, otherwise returns null if the caster targeted the ground.
      /// </summary>
      public NwGameObject TargetObject { get; private set; }

      /// <summary>
      /// Gets the targeted location of this spell.
      /// </summary>
      public Location TargetLocation { get; private set; }

      /// <summary>
      /// Gets the class that the caster cast the spell as.
      /// </summary>
      public ClassType SpellCastClass { get; private set; }

      /// <summary>
      /// Gets the item that was used to cast the spell. Returns null if no item was used to cast this spell.
      /// </summary>
      public NwItem Item { get; private set; }

      /// <summary>
      /// Gets the saving throw DC required to save against the effects of this spell.
      /// </summary>
      public int SaveDC { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Caster = (NwGameObject)objSelf;
        Spell = (Spell)NWScript.GetSpellId();
        Harmful = NWScript.GetLastSpellHarmful().ToBool();
        TargetObject = NWScript.GetSpellTargetObject().ToNwObject<NwGameObject>();
        TargetLocation = NWScript.GetSpellTargetLocation();
        SpellCastClass = (ClassType)NWScript.GetLastSpellCastClass();
        Item = NWScript.GetSpellCastItem().ToNwObject<NwItem>();
        SaveDC = NWScript.GetSpellSaveDC();
      }
    }
  }
}
