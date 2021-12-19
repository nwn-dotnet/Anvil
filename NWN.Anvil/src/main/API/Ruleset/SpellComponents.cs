using System;

namespace Anvil.API
{
  [Flags]
  public enum SpellComponents
  {
    None = 0,
    Verbal = 1 << 0,
    Somatic = 1 << 1,
    VerbalSomatic = Verbal | Somatic,
  }
}
