using System;

namespace Anvil.API
{
  [Flags]
  public enum AreaFlags
  {
    Interior = 0x0001,
    UnderGround = 0x0002,
    Natural = 0x0004,
  }
}
