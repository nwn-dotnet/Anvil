using System;
using NWN.Core;

namespace Anvil.API
{
  [Flags]
  public enum RegexpType
  {
    Ecmascript = NWScript.REGEXP_ECMASCRIPT,
    Basic = NWScript.REGEXP_BASIC,
    Extended = NWScript.REGEXP_EXTENDED,
    Awk = NWScript.REGEXP_AWK,
    Grep = NWScript.REGEXP_GREP,
    Egrep = NWScript.REGEXP_EGREP,
    Icase = NWScript.REGEXP_ICASE,
    Nosubs = NWScript.REGEXP_NOSUBS,
  }
}
