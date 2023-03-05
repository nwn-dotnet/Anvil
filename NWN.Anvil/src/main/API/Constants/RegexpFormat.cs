using System;
using NWN.Core;

namespace Anvil.API
{
  [Flags]
  public enum RegexpFormat
  {
    Default = NWScript.REGEXP_FORMAT_DEFAULT,
    Sed = NWScript.REGEXP_FORMAT_SED,
    NoCopy = NWScript.REGEXP_FORMAT_NO_COPY,
    FirstOnly = NWScript.REGEXP_FORMAT_FIRST_ONLY,
  }
}
