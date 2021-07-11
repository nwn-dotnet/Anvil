using System;

namespace NWN.API
{
  [Flags]
  public enum PlayerSearch
  {
    None = 0,
    Controlled = 1 << 0,
    Login = 1 << 1,
    All = Controlled | Login,
  }
}
