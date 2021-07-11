using System;
using NWN.Core;

namespace NWN.API.Constants
{
  [Flags]
  public enum MetaMagic
  {
    None = NWScript.METAMAGIC_NONE,
    Empower = NWScript.METAMAGIC_EMPOWER,
    Extend = NWScript.METAMAGIC_EXTEND,
    Maximize = NWScript.METAMAGIC_MAXIMIZE,
    Quicken = NWScript.METAMAGIC_QUICKEN,
    Silent = NWScript.METAMAGIC_SILENT,
    Still = NWScript.METAMAGIC_STILL,
    Any = NWScript.METAMAGIC_ANY,
  }
}
