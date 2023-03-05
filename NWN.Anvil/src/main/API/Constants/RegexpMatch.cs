using System;
using NWN.Core;

namespace Anvil.API
{
  [Flags]
  public enum RegexpMatch
  {
    NotBol = NWScript.REGEXP_MATCH_NOT_BOL,
    NotEol = NWScript.REGEXP_MATCH_NOT_EOL,
    NotBow = NWScript.REGEXP_MATCH_NOT_BOW,
    NotEow = NWScript.REGEXP_MATCH_NOT_EOW,
    Any = NWScript.REGEXP_MATCH_ANY,
    NotNull = NWScript.REGEXP_MATCH_NOT_NULL,
    Continuous = NWScript.REGEXP_MATCH_CONTINUOUS,
    PrevAvail = NWScript.REGEXP_MATCH_PREV_AVAIL,
  }
}
