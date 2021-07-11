namespace Anvil.API
{
  public enum AddPropertyPolicy
  {
    // Removes any property of the same type, subtype, and durationtype before adding.
    ReplaceExisting = 0,

    // Itemproperty won't be added if any property with same type, subtype and durationtype already exists.
    KeepExisting = 1,

    // Adds itemproperty in any case - do not Use with OnHit or OnHitSpellCast props!
    IgnoreExisting = 2,
  }
}
