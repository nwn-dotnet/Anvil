using System;
using NWM.API;
using NWN;

namespace NWM.Core
{
  public partial class EventManager
  {
    public event ClientEventEvent OnClientEnter;
    public delegate void ClientEventEvent(NwPlayer entered);

    [ScriptHandler("client_enter")]
    private void ClientEntered()
    {
      NwPlayer player = NWScript.GetEnteringObject().ToNwObject<NwPlayer>();
      OnClientEnter?.Invoke(player);
    }
  }
}